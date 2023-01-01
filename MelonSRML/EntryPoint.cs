﻿global using UnityEngine;
global using MelonLoader;
global using static MelonLoader.MelonLogger;
using MelonSRML.Patches;
using MelonSRML.SR2.Slime;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonSRML.EnumPatcher;
using MelonSRML.SR2.Ranch;

namespace MelonSRML
{
    internal class EntryPoint : MelonPlugin
    {
        internal static List<SRMLMelonMod> registeredMods = new List<SRMLMelonMod>();

        internal static bool interruptMenuLoad = false;
        internal static bool interruptGameLoad = false;
        internal static LoadingError error;

        internal static Transform prefabParent;
        public static Assembly execAssembly = Assembly.GetExecutingAssembly();
        internal static MethodInfo TryCast = AccessTools.Method(typeof(Il2CppObjectBase), nameof(Il2CppObjectBase.TryCast));

        public override void OnInitializeMelon()
        {
            /*
             This currently doesn't work
            ClassInjector.RegisterTypeInIl2Cpp<ModdedSlimeSubbehavior>();
            CustomSlimeSubbehaviorPatches.moddedType = Il2CppType.Of<ModdedSlimeSubbehavior>();
            */
            
            ClassInjector.RegisterTypeInIl2Cpp<ModdedPlotUpgrader>();
            
            
            HarmonyInstance.PatchAll();
            SystemContext.IsModded = true;
        }

        public override void OnPreModsLoaded()
        {
            OnMelonRegistered.Subscribe(x =>
            {
                if (x is not SRMLMelonMod mod) return;

                registeredMods.Add(mod);
                EnumHolderResolver.RegisterAllEnums(mod);
            });
        }
    }
}

