using HarmonyLib;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(GameContext), "Start")]
    internal static class GameContextModEventPatch
    {
        public static void Prefix(GameContext __instance) => EntryPoint.onGameContext?.Invoke(__instance);
    }
}
