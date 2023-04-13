using System.Collections.Generic;
using System.Linq;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using MelonSRML.SR2;

namespace MelonSRML.Patches.SaveSystem
{
    [HarmonyLib.HarmonyPatch(typeof(SavedGame), nameof(SavedGame.Push), typeof(GameModel))]
    internal static class SavedGamePushPatch
    {
        public static void Prefix(SavedGame __instance)
        {
            foreach (var pediaEntry in PediaRegistry.addedPedias.Values)
            {
                if (!__instance.pediaEntryLookup.ContainsKey(pediaEntry.GetPersistenceId()))
                    __instance.pediaEntryLookup.Add(pediaEntry.GetPersistenceId(), pediaEntry);
            }

            foreach (var pediaEntry in PediaRegistry.addedFixedPedias)
            {
                if (!__instance.pediaEntryLookup.ContainsKey(pediaEntry.GetPersistenceId()))
                    __instance.pediaEntryLookup.Add(pediaEntry.GetPersistenceId(), pediaEntry);
            }

            List<string> idsToRemove = __instance.gameState.pedia.unlockedIds.ToArray()
                .Where(pediaUnlockedId => !__instance.pediaEntryLookup.ContainsKey(pediaUnlockedId)).ToList();

            idsToRemove.ForEach(x => __instance.gameState.pedia.unlockedIds.Remove(x));
           
        }
    }
}