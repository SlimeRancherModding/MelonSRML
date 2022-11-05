using MelonSRML.Patches;
using MelonSRML.SR2.Slime;
using System.Collections.Generic;
using System.Reflection;
using MelonSRML.EnumPatcher;
using UnhollowerRuntimeLib;

namespace MelonSRML
{
    
    public class EntryPoint : MelonPlugin
    {
        internal static List<SRMLMelonMod> registeredMods = new List<SRMLMelonMod>();

        internal static bool interruptMenuLoad = false;
        internal static bool interruptGameLoad = false;
        internal static LoadingError error;

        internal static Transform prefabParent;
        public static Assembly execAssembly = Assembly.GetExecutingAssembly();

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
                if (x is not SRMLMelonMod mod) return;

                registeredMods.Add(mod);
                EnumHolderResolver.RegisterAllEnums(mod);
            });
        }
    }
}
