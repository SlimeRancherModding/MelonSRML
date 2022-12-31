using System.Collections.Generic;
using System.Linq;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;

namespace MelonSRML.Patches.SaveSystem;

[HarmonyLib.HarmonyPatch(typeof(SavedGame), nameof(SavedGame.Push), typeof(GameModel))]
public static class SavedGamePush
{
    public static void Prefix(SavedGame __instance)
    {
        var stringsToDelete = __instance.gameState.pedia.unlockedIds._items.Where(pediaUnlockedId => !__instance.pediaEntryLookup.ContainsKey(pediaUnlockedId)).ToList();

        foreach (var variable in stringsToDelete)
        {
            __instance.gameState.pedia.unlockedIds.Remove(variable);
        }
    }
}