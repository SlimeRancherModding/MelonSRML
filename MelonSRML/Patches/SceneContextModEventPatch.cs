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
            MSRModLoader.CurrentLoadingStep = MSRModLoader.Step.OnSceneContext;
            foreach (SRMLMelonMod mod in EntryPoint.registeredMods)
            {
                try
                {
                    mod.OnSceneContext(__instance);
                }
                catch (Exception e)
                {
                    LoadingError.CreateLoadingError(mod, MSRModLoader.CurrentLoadingStep, e);
                    EntryPoint.interruptGameLoad = true;

                    break;
                }
            }
        }
    }
}
