using HarmonyLib;
using System;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(SceneContext), "Start")]
    internal static class SceneContextModEventPatch
    {
        public static void Prefix(SceneContext __instance)
        {
            if (EntryPoint.interruptGameLoad)
                return;

            foreach (var mod in EntryPoint.registeredMods)
            {
                try
                {
                    mod.OnSceneContext(__instance);
                }
                catch (Exception e)
                {
                    LoadingError.CreateLoadingError(mod, LoadingError.Step.OnSceneContext, e);
                    EntryPoint.interruptGameLoad = true;

                    break;
                }
            }
        }
    }
}
