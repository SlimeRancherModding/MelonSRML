using HarmonyLib;
using UnityEngine;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(SystemContext), "Start")]
    internal static class SystemContextInitializePatch
    {
        public static void Prefix(SystemContext __instance) => EntryPoint.onSystemContext(__instance);
    }
}
