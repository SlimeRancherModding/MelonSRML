using HarmonyLib;
using MelonSRML.EnumPatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(LookupDirector), nameof(LookupDirector.Awake))]
    internal static class LookupDirectorAwakePatch
    {
        public static void Prefix(LookupDirector __instance)
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
    }
}
