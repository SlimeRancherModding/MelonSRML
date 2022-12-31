using HarmonyLib;
using System;
using MelonSRML.EnumPatcher;
using MelonSRML.SR2;
using MelonSRML.Utils;
using UnityEngine.Localization;

namespace MelonSRML.Patches
{
    
    [HarmonyPatch(typeof(AutoSaveDirector), "Awake")]
    internal static class AutoSaveDirectorRegistryPatch
    {
        public static void Prefix(AutoSaveDirector __instance)
        {
          
            
            if (EntryPoint.interruptMenuLoad)
                return;
            MSRModLoader.CurrentLoadingStep = MSRModLoader.Step.PreRegister;
            foreach (SRMLMelonMod mod in EntryPoint.registeredMods)
            {
                IdentifiableTypeResolver.RegisterAllIdentifiables(mod);
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

           
        }
    }
}
