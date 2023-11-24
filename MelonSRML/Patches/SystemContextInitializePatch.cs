using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.SceneManagement;
using System;
using Il2CppMonomiPark.SlimeRancher.UI.Popup;
using MelonSRML.SR2;
using MelonSRML.Utils;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(SystemContext), "Start")]
    internal static class SystemContextInitializePatch
    {
        public static void Prefix(SystemContext __instance)
        {
            __instance.SceneLoader.OnSceneGroupLoadedDelegate += new Action<SceneGroup, Il2CppSystem.Action<SceneLoadErrorData>>((x,y) =>
            {
                if ((x.name == "MainMenuFromBoot" && EntryPoint.interruptMenuLoad) || (x._isGameplay && EntryPoint.interruptGameLoad))
                {
                    LoadingError e = EntryPoint.error;
                    GameContext.Instance.UITemplates.CreatePositivePopupPrompt(ScriptableObjectUtils.CreateScriptable(new Action<PositivePopupPromptConfig>(config =>
                    {
                        config._activateActionsDelay = 0;
                        config._expirationDuration = 0;
                        config._expires = false;
                        config._message = TranslationPatcher.AddTranslation("UI", "m.srml_load_error.error", $"{e.LoadingStep} error from '{e.ModName}': {e.Exception.Message}");
                        config._shouldDimBackground = true;
                        config._title =   TranslationPatcher.AddTranslation("UI", "m.srml_load_error.title", "mSRML Error"); ;
                        config._positiveButtonText = TranslationPatcher.AddTranslation("UI", "m.srml_load_error.quit", "EXIT");

                    })), new Action(Application.Quit));
                    
                    Error(e.Exception);
                }
            });

            if (EntryPoint.interruptMenuLoad)
                return;
            MSRModLoader.CurrentLoadingStep = MSRModLoader.Step.OnSystemContext;

            foreach (SRMLMelonMod mod in EntryPoint.registeredMods)
            {
                try
                {
                    mod.OnSystemContext(__instance);
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
