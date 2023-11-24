using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.UI.MainMenu.Model;
using MelonSRML.SR2;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(LoadGameBehaviorModel), nameof(LoadGameBehaviorModel.Image), MethodType.Getter)]
    internal static class LoadGameBehaviorModelImagePatch
    {
        private static Sprite iconDot;
        public static bool Prefix(LoadGameBehaviorModel __instance, ref Sprite __result)
        {
            iconDot ??= SRLookup.Get<Sprite>("iconDot");
            if (__instance.GameDataSummary.IconId is not null)
                return true;

            __result = iconDot;
            return false;
        }
    }
}