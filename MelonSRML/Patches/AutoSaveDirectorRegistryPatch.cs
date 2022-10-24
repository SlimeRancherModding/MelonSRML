using HarmonyLib;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(AutoSaveDirector), "Awake")]
    internal static class AutoSaveDirectorRegistryPatch
    {
        public static void Prefix(AutoSaveDirector __instance) => EntryPoint.preRegister?.Invoke(__instance);
    }
}
