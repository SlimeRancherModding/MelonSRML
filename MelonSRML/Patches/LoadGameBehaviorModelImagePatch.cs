using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.UI.MainMenu.Model;
using JetBrains.Annotations;
using MelonSRML.SR2;

namespace MelonSRML.Patches;

[HarmonyPatch(typeof(LoadGameBehaviorModel),
    nameof(LoadGameBehaviorModel.Image), MethodType.Getter)]
public static class LoadGameBehaviorModelImagePatch
{
    public static bool Prefix(LoadGameBehaviorModel __instance, ref Sprite __result)
    {
        if (__instance.GameDataSummary.iconId is not null) return true;
        __result =  SRLookup.Get<Sprite>("iconDot");
        return false;

    }
}