using MelonLoader;
using MelonSRML.Patches;
using MelonSRML.SR2.Slime;
using Il2CppSystem;
using UnhollowerRuntimeLib;
using UnityEngine;
using System;

namespace MelonSRML
{
    public class EntryPoint : MelonPlugin
    {
        internal delegate void PreRegister(AutoSaveDirector autoSaveDirector);
        internal delegate void OnSystemContext(SystemContext systemContext);
        internal delegate void OnGameContext(GameContext gameContext);
        internal delegate void OnSceneContext(SceneContext sceneContext);

        internal static PreRegister preRegister;
        internal static OnSystemContext onSystemContext;
        internal static OnGameContext onGameContext;
        internal static OnSceneContext onSceneContext;

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
                if (x is SRMLMelonMod)
                {
                    SRMLMelonMod mod = (SRMLMelonMod)x;
                    preRegister += mod.PreRegister;
                    onSystemContext += mod.OnSystemContext;
                    onGameContext += mod.OnGameContext;
                    onSceneContext += mod.OnSceneContext;
                }
            });
        }
    }
}
