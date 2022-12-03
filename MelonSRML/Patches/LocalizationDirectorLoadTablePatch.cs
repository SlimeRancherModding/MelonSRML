using System.Collections;
using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.UI.Localization;
using MelonSRML.SR2;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(LocalizationDirector), nameof(LocalizationDirector.LoadTables))]
    internal static class LocalizationDirectorLoadTablePatch
    {
        public static void Postfix(LocalizationDirector __instance)
        {
            MelonCoroutines.Start(LoadTable(__instance));
        }    
        private static IEnumerator LoadTable(LocalizationDirector director)
        {
            WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(0.01f);
            yield return waitForSecondsRealtime;
            foreach (var stringTable in director.Tables)
            {
                if (TranslationPatcher.addedTranslations.TryGetValue(stringTable.Key, out var dict))
                {
                    foreach (var keyvalue in dict)
                    {
                        stringTable.Value.AddEntry(keyvalue.Key, keyvalue.Value);

                    }
                }

            }

        }
    }
}

