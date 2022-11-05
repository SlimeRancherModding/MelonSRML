using System.Collections.Generic;
using System.Linq;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;

namespace MelonSRML.Patches.SaveSystem
{
    [HarmonyLib.HarmonyPatch(typeof(SavedGame), nameof(SavedGame.Push), typeof(GameModel))]
    public static class SavedGamePush
    {
        public static void Prefix(SavedGame __instance)
        {
            List<string> idsToRemove = __instance.gameState.pedia.unlockedIds._items
                .Where(pediaUnlockedId => !__instance.pediaEntryLookup.ContainsKey(pediaUnlockedId)).ToList();

            foreach (string variable in idsToRemove)
                __instance.gameState.pedia.unlockedIds.Remove(variable);
        }
    }
}