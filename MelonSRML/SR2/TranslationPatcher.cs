using System.Collections.Generic;
using Il2CppMonomiPark.SlimeRancher.Script.Util;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace MelonSRML.SR2
{
    public static class TranslationPatcher
    {
        public static Dictionary<string, Dictionary<string, string>> addedTranslations = new Dictionary<string, Dictionary<string, string>>();

        public static LocalizedString AddTranslation(string table, string key, string localized)
        {
            if (!addedTranslations.TryGetValue(table, out var patched))
            {
                var dictionary = new Dictionary<string, string> {};
                patched = dictionary;
                addedTranslations.Add(table, dictionary);
            }
            patched.Add(key, localized);

            StringTable stringTable = LocalizationUtil.GetTable(table);
            StringTableEntry stringTableEntry = stringTable.AddEntry(key, localized);
            return new LocalizedString(stringTable.SharedData.TableCollectionName, stringTableEntry.SharedEntry.Id);
        }
    }
}

