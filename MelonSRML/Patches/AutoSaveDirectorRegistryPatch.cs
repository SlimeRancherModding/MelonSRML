using HarmonyLib;
using System;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(AutoSaveDirector), "Awake")]
    internal static class AutoSaveDirectorRegistryPatch
    {
        public static void Prefix(AutoSaveDirector __instance)
        {
            if (EntryPoint.interruptMenuLoad)
                return;

            foreach (SRMLMelonMod mod in EntryPoint.registeredMods)
            {
                try
                {
                    mod.PreRegister(__instance);
                }
                catch (Exception e)
                {
                    LoadingError.CreateLoadingError(mod, LoadingError.Step.PreRegister, e);
                    EntryPoint.interruptMenuLoad = true;

                    break;
                }
            }
        }
    }
}
