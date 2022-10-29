using MelonLoader;
using MelonSRML.Patches;
using MelonSRML.SR2.Slime;
using Il2CppSystem;
using UnhollowerRuntimeLib;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace MelonSRML
{
    public class EntryPoint : MelonPlugin
    {
        internal static List<SRMLMelonMod> registeredMods = new List<SRMLMelonMod>();

        internal static bool interruptMenuLoad = false;
        internal static bool interruptGameLoad = false;
        internal static LoadingError error;

        internal static Transform prefabParent;

        public override void OnInitializeMelon()
        {
            SystemContext.IsModded = true; 

            ClassInjector.RegisterTypeInIl2Cpp<ModdedSlimeSubbehavior>();
            CustomSlimeSubbehaviorPatches.moddedType = Il2CppType.Of<ModdedSlimeSubbehavior>();
        }

        public override void OnPreModsLoaded()
        {
            OnMelonRegistered.Subscribe(x =>
            {
                if (x is SRMLMelonMod mod)
                    registeredMods.Add(mod);
            });
        }
    }
}
