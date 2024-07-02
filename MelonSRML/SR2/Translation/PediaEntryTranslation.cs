using Il2CppMonomiPark.SlimeRancher.Pedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Localization;

namespace MelonSRML.SR2.Translation
{
    // I kept these just in case
/*    public class PediaEntryTranslation
    {
        private const string TITLE_PREFIX = "t.";

        private const string INTRO_PREFIX = "m.intro.";

        private const string DESCRIPTION_PREFIX = "m.desc.";

        public PediaEntry PediaEntry { get; protected set; }

        public LocalizedString Title => PediaEntry.Title;

        public LocalizedString Intro => PediaEntry.Description;

        public LocalizedString Description { get; protected set; }

        public string TitleKey => TITLE_PREFIX + PersistenceSuffix;

        public string IntroKey => INTRO_PREFIX + PersistenceSuffix;

        public string DescriptionKey => DESCRIPTION_PREFIX + PersistenceSuffix;

        public string PersistenceSuffix { get; protected set; }

        public PediaEntryTranslation(PediaEntry pediaEntry)
        {
            PediaEntry = pediaEntry;
            PersistenceSuffix = PediaEntry.PersistenceSuffix;
        }

        public PediaEntryTranslation AddTitleTranslation(string title)
        {
            PediaEntry._title = TranslationPatcher.AddTranslation("Pedia", TitleKey, title);
            return this;
        }

        public PediaEntryTranslation AddIntroTranslation(string intro)
        {
            PediaEntry._description = TranslationPatcher.AddTranslation("Pedia", IntroKey, intro);
            return this;
        }

        public PediaEntryTranslation AddDescriptionTranslation(string description)
        {
            Description = TranslationPatcher.AddTranslation("PediaPage", DescriptionKey, description);
            return this;
        }

        public PediaEntryTranslation AddTranslation(string key, string text, out LocalizedString localizedString)
        {
            localizedString = TranslationPatcher.AddTranslation("PediaPage", key, text);
            return this;
        }

        public PediaEntryTranslation AddTranslation(string table, string key, string text, out LocalizedString localizedString)
        {
            localizedString = TranslationPatcher.AddTranslation(table, key, text);
            return this;
        }
    }*/
}
