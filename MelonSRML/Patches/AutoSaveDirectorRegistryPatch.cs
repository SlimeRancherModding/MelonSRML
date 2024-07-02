using HarmonyLib;
using System;
using MelonSRML.EnumPatcher;

namespace MelonSRML.Patches
{
/*    [HarmonyPatch(typeof(AutoSaveDirector), "Awake")]
    internal static class AutoSaveDirectorRegistryPatch
    {
        public static void Prefix(AutoSaveDirector __instance)
        {
            if (EntryPoint.interruptMenuLoad)
                return;

            MSRModLoader.CurrentLoadingStep = MSRModLoader.Step.PreRegister;
            foreach (SRMLMelonMod mod in EntryPoint.registeredMods)
            {
                IdentifiableTypeResolver.RegisterAllIdentifiables(__instance, mod);
                try
                {
                    mod.PreRegister(__instance);
                }
                catch (Exception e)
                {
                    LoadingError.CreateLoadingError(mod, MSRModLoader.CurrentLoadingStep, e);
                    EntryPoint.interruptMenuLoad = true;

                    break;
                }
            }
            IdentifiableTypeResolver.CategorizeAllIdentifiables();
        }
    }*/
}
