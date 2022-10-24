using HarmonyLib;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(SceneContext), "Start")]
    internal static class SceneContextModEventPatch
    {
        public static void Prefix(SceneContext __instance) => EntryPoint.onSceneContext(__instance);
    }
}
