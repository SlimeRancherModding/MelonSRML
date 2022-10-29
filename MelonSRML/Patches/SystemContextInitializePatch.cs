using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.SceneManagement;
using Il2CppMonomiPark.SlimeRancher.Script.Util;
using System;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;
using Il2CppMonomiPark.SlimeRancher.UI.Popup;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(SystemContext), "Start")]
    internal static class SystemContextInitializePatch
    {
        public static void Prefix(SystemContext __instance)
        {
            __instance.SceneLoader.onSceneGroupLoadedDelegate += new Action<SceneGroup, Il2CppSystem.Action<SceneLoadErrorData>>((x,y) =>
            {
                if ((x.name == "MainMenuFromBoot" && EntryPoint.interruptMenuLoad) || (x.isGameplay && EntryPoint.interruptGameLoad))
                {
                    LoadingError e = EntryPoint.error;
                    
                    // TODO: make better once a proper translation handler is made.
                    StringTableEntry title = LocalizationUtil.GetTable("UI").AddEntry("m.srml_load_error.title", "mSRML Error");
                    LocalizedString titleString = new LocalizedString(LocalizationUtil.GetTable("UI").SharedData.TableCollectionNameGuid, title.SharedEntry.Id);

                    StringTableEntry error = LocalizationUtil.GetTable("UI").AddEntry("m.srml_load_error.error", 
                        $"{e.LoadingStep} error from '{e.ModName}': {e.Exception.Message}");
                    LocalizedString errorString = new LocalizedString(LocalizationUtil.GetTable("UI").SharedData.TableCollectionNameGuid, error.SharedEntry.Id);

                    StringTableEntry exit = LocalizationUtil.GetTable("UI").AddEntry("m.srml_load_error.quit", "EXIT");
                    LocalizedString exitString = new LocalizedString(LocalizationUtil.GetTable("UI").SharedData.TableCollectionNameGuid, exit.SharedEntry.Id);

                    GameContext.Instance.UITemplates.CreatePositivePopupPrompt(new PositivePopupPromptConfig()
                    {
                        activateActionsDelay = 0,
                        expirationDuration = 0,
                        expires = false,
                        message = errorString,
                        shouldDimBackground = true,
                        title = titleString,
                        positiveButtonText = exitString
                    }, new Action(() => Application.Quit()));
                }
            });

            if (EntryPoint.interruptMenuLoad)
                return;

            foreach (SRMLMelonMod mod in EntryPoint.registeredMods)
            {
                try
                {
                    mod.OnSystemContext(__instance);
                }
                catch (Exception e)
                {
                    LoadingError.CreateLoadingError(mod, LoadingError.Step.OnSystemContext, e);
                    EntryPoint.interruptMenuLoad = true;

                    break;
                }
            }
        }
    }
}
