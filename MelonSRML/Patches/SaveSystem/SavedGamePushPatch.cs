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
            foreach (var pediaEntry in PediaRegistry.moddedPediaEntries)
            {
                if (__instance.pediaEntryLookup.ContainsKey(pediaEntry.name))
                    __instance.pediaEntryLookup.Add(pediaEntry.name, pediaEntry);
            }
            List<string> idsToRemove = __instance.gameState.pedia.unlockedIds._items
                .Where(pediaUnlockedId => !__instance.pediaEntryLookup.ContainsKey(pediaUnlockedId)).ToList();

            idsToRemove.ForEach(x => __instance.gameState.pedia.unlockedIds.Remove(x));
           
        }
    }
}