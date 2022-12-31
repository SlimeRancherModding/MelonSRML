using HarmonyLib;
using System;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(GameContext), "Start")]
    internal static class GameContextModEventPatch
    {
        public static void Prefix(GameContext __instance)
        {
          

            if (EntryPoint.interruptMenuLoad)
                return;
            MSRModLoader.CurrentLoadingStep = MSRModLoader.Step.OnGameContext;
            foreach (SRMLMelonMod mod in EntryPoint.registeredMods)
            {
                
                try
                {
                    mod.OnGameContext(__instance);
                }
                catch (Exception e)
                {
                    LoadingError.CreateLoadingError(mod, MSRModLoader.CurrentLoadingStep, e);
                    EntryPoint.interruptMenuLoad = true;

                    break;
                }
            }
        }
    }
}
