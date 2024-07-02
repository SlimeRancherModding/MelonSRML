using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;
using Il2CppMonomiPark.SlimeRancher.Weather;

namespace MelonSRML.Patches.SaveSystem
{
    [HarmonyPatch(typeof(SavedGame), nameof(SavedGame.Push), typeof(GameModel))]
    internal static class SavedGamePushPatch
    {
        [HarmonyPrefix]
        [HarmonyPriority(Priority.Last)]
        public static void LatePrefix(SavedGame __instance)
        {
            List<string> idsToRemove = __instance.gameState.Pedia.UnlockedIds.ToArray()
                .Where(pediaUnlockedId => !__instance.pediaEntryLookup.ContainsKey(pediaUnlockedId)).ToList();

            idsToRemove.ForEach(x => __instance.gameState.Pedia.UnlockedIds.Remove(x));
            RemoveUnavailableWeather(__instance);
        }

        public static void Prefix(SavedGame __instance)
        {
            // Adding to `pediaEntryLookup` is no longer needed due to the 0.5.0 update.
            // PEDIA
/*            foreach (var pediaEntry in PediaRegistry.pediasToPatch)
                if (pediaEntry)
                    __instance.pediaEntryLookup.TryAdd(pediaEntry.PersistenceId, pediaEntry);*/

            // WEATHER
            var stateTranslation = __instance._weatherStateTranslation;
            var patternTranslation = __instance._weatherPatternTranslation;

            foreach (var weatherState in SR2.WeatherRegistry.weatherStatesToPatch)
            {
                if (weatherState)
                {
                    stateTranslation.RawLookupDictionary.TryAdd(weatherState.Guid, weatherState.TryCast<IWeatherState>());
                    stateTranslation.ReverseLookupTable._indexTable = stateTranslation.ReverseLookupTable._indexTable.ToArray().AddToArray(weatherState.Guid);

                    stateTranslation.InstanceLookupTable._primaryIndex = stateTranslation.InstanceLookupTable._primaryIndex.ToArray().AddToArray(weatherState.Guid);
                    stateTranslation.InstanceLookupTable._reverseIndex.TryAdd(weatherState.Guid, stateTranslation.InstanceLookupTable._primaryIndex.Length - 1);
                }
            }

            foreach (var weatherPattern in SR2.WeatherRegistry.weatherPatternsToPatch)
            {
                if (weatherPattern)
                {
                    patternTranslation.RawLookupDictionary.TryAdd(weatherPattern.Guid, weatherPattern);
                    patternTranslation.ReverseLookupTable._indexTable = patternTranslation.ReverseLookupTable._indexTable.ToArray().AddToArray(weatherPattern.Guid);

                    patternTranslation.InstanceLookupTable._primaryIndex = patternTranslation.InstanceLookupTable._primaryIndex.ToArray().AddToArray(weatherPattern.Guid);
                    patternTranslation.InstanceLookupTable._reverseIndex.TryAdd(weatherPattern.Guid, patternTranslation.InstanceLookupTable._primaryIndex.Length - 1);
                }
            }
        }

        private static void RemoveUnavailableWeather(SavedGame __instance)
        {
            PersistenceIdReverseLookupTable<IWeatherState> idToStateTable = __instance._weatherStateTranslation.ReverseLookupTable;
            PersistenceIdReverseLookupTable<WeatherPatternDefinition> idToPatternTable = __instance._weatherPatternTranslation.ReverseLookupTable;
            PersistenceIdLookupTable<IWeatherState> stateLookup = __instance._weatherStateTranslation.InstanceLookupTable;
            PersistenceIdLookupTable<WeatherPatternDefinition> patternLookup = __instance._weatherPatternTranslation.InstanceLookupTable;
            WeatherV01 weatherV01 = __instance.gameState.Weather;

            List<string> stateTable = idToStateTable._indexTable.ToList();
            List<string> patternTable = idToPatternTable._indexTable.ToList();

            var stateDict = __instance._weatherStateTranslation.RawLookupDictionary;
            var stateIndexes = idToStateTable._indexTable.Where(index => !stateDict.ContainsKey(index));
            foreach (var index in stateIndexes)
                stateTable.Remove(index);

            var patternDict = __instance._weatherPatternTranslation.RawLookupDictionary;
            var patternIndexes = idToPatternTable._indexTable.Where(index => !patternDict.ContainsKey(index));
            foreach (var index in patternIndexes)
                patternTable.Remove(index);

            foreach (var entry in weatherV01.Entries)
            {
                var entryStateIds = entry.StateCompletionTimeIDs.ToArray().Where(id => !stateLookup._reverseIndex.ContainsValue(id));
                foreach (var id in entryStateIds)
                    entry.StateCompletionTimeIDs.Remove(id);

                var entryPatternIds = entry.PatternCompletionTimeIDs.ToArray().Where(id => !patternLookup._reverseIndex.ContainsValue(id));
                foreach (var id in entryPatternIds)
                    entry.PatternCompletionTimeIDs.Remove(id);

                var forecastEntries = entry.Forecast.ToArray().Where(id => !stateLookup._reverseIndex.ContainsValue(id.StateID) || !patternLookup._reverseIndex.ContainsValue(id.PatternID));
                foreach (var forecastEntry in forecastEntries)
                    entry.Forecast.Remove(forecastEntry);
            }

            idToStateTable._indexTable = stateTable.ToArray();
            idToPatternTable._indexTable = patternTable.ToArray();
            __instance.gameState.WeatherIndex.StateIndexTable = stateTable.ToArray();
            __instance.gameState.WeatherIndex.PatternIndexTable = patternTable.ToArray();
        }
    }
}