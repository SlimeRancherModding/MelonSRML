using System;
using System.Collections.Generic;
using Il2CppMonomiPark.SlimeRancher.Script.Util;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace MelonSRML.SR2
{
    public static class TranslationPatcher
    {
        internal static Dictionary<string, Dictionary<string, string>> addedTranslations = new Dictionary<string, Dictionary<string, string>>();

        public static LocalizedString AddTranslation(string table, string key, string localized)
        {
            StringTable stringTable = LocalizationUtil.GetTable(table);
            if (stringTable == null)
                throw new NullReferenceException("Table is null");
            
            if (!addedTranslations.TryGetValue(table, out var patched))
            {
                var dictionary = new Dictionary<string, string>();
                patched = dictionary;
                addedTranslations.Add(table, dictionary);
            }
            if (patched.ContainsKey(key))
            {
                MelonLogger.Msg($"Translation Key {key} for table {table} is already taken by another mod! Overwriting");
                patched[key] = localized;
                var localizedStringTableEntry = stringTable.GetEntry(key);
                localizedStringTableEntry.Value = localized;
                return new LocalizedString(stringTable.SharedData.TableCollectionName, localizedStringTableEntry.SharedEntry.Id);
            }
            patched.TryAdd(key, localized);

            StringTableEntry stringTableEntry = stringTable.AddEntry(key, localized);
            return new LocalizedString(stringTable.SharedData.TableCollectionName, stringTableEntry.SharedEntry.Id);
        }
    }
}