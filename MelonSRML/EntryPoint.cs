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
using Il2CppMonomiPark.SlimeRancher.Damage;
using MelonSRML.Console;
using MelonSRML.EnumPatcher;
using MelonSRML.SR2.Ranch;
using MelonSRML.SR2.Translation;
using MelonSRML.SR2;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using System.Linq;
using UnityEngine.Localization;
using Il2CppMonomiPark.SlimeRancher.Script.Util;

namespace MelonSRML
{
    internal class EntryPoint : MelonPlugin
    {
        internal static List<SRMLMelonMod> registeredMods = new List<SRMLMelonMod>();

        internal static bool interruptMenuLoad = false;
        internal static bool interruptGameLoad = false;
        internal static LoadingError error;

        internal static Transform prefabParent;

        internal static Damage KillObject;
        internal static MethodInfo TryCast = AccessTools.Method(typeof(Il2CppObjectBase), nameof(Il2CppObjectBase.TryCast));

        public override void OnInitializeMelon()
        {
            
            if (KillObject == null)
            {
                KillObject = new Damage
                {
                    DamageSource = ScriptableObject.CreateInstance<DamageSourceDefinition>()
                };
                KillObject.DamageSource.hideFlags |= HideFlags.HideAndDontSave;
                KillObject.DamageSource._logMessage = "RemoveCommand.Execute";
            }
            
            /*
            This currently doesn't work
            ClassInjector.RegisterTypeInIl2Cpp<ModdedSlimeSubbehavior>();
            CustomSlimeSubbehaviorPatches.moddedType = Il2CppType.Of<ModdedSlimeSubbehavior>();
            */

            ClassInjector.RegisterTypeInIl2Cpp<ModdedPlotUpgrader>();
            Console.Console.Init();


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

        public override void OnApplicationQuit()
        {
            KeyBindManager.Pull();
        }
    }
}

