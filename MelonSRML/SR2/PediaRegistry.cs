using System;
using System.Collections.Generic;
using Il2CppMonomiPark.SlimeRancher.UI.Pedia;
using MelonSRML.Patches;
using System.Linq;
using MelonSRML.Utils;
using UnityEngine.Localization;
using Il2Cpp;
using Il2CppSystem.Security;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using HarmonyLib;
using MelonSRML.Utils.Extensions;

namespace MelonSRML.SR2
{
    public static class PediaRegistry
    {
        internal static HashSet<PediaEntry> pediasToPatch = new HashSet<PediaEntry>();

        public static string CreatePediaKey(string prefix, string suffix)
        { return "m." + prefix + "." + suffix; }

        public static string CreateIdentifiableKey(IdentifiableType identifiableType, string prefix)
        { return "m." + prefix + "." + identifiableType._pediaPersistenceSuffix; }

        /*public static string CreateIdentifiablePageKey(string prefix, int pageNumber, IdentifiableType identifiableType)
        { return "m." + prefix + "." + identifiableType.localizationSuffix + ".page." + pageNumber.ToString(); }*/

        /*public static string CreateFixedPageKey(string prefix, int pageNumber, string textId)
        { return "m." + prefix + "." + textId + ".page." + pageNumber.ToString(); }*/

        public static void RegisterPediaEntry(PediaEntry pediaEntry)
        {
            if (!pediasToPatch.Contains(pediaEntry))
                pediasToPatch.Add(pediaEntry);
        }

        public static PediaDetailSection CreatePediaSection(Sprite icon, string name)
        {
            if (SRLookup.Get<PediaDetailSection>(name))
                return null;

            PediaDetailSection pediaDetailSection = ScriptableObject.CreateInstance<PediaDetailSection>();
            pediaDetailSection.hideFlags |= HideFlags.HideAndDontSave;
            pediaDetailSection.name = name;

            pediaDetailSection._icon = icon;
            pediaDetailSection._title = TranslationPatcher.AddTranslation("UI", "l." + name.ToLower().Replace(" ", "_"), name);

            return pediaDetailSection;
        }

        public static IdentifiablePediaEntry CreateIdentifiableEntry(IdentifiableType identifiableType, PediaHighlightSet highlightSet, 
            LocalizedString intro, PediaEntryDetail[] entryDetails, bool isUnlockedInitially = false)
        {
            if (SRLookup.Get<IdentifiablePediaEntry>(identifiableType?.name))
                return null;

            IdentifiablePediaEntry identifiablePediaEntry = ScriptableObject.CreateInstance<IdentifiablePediaEntry>();
            identifiablePediaEntry.hideFlags |= HideFlags.HideAndDontSave;
            identifiablePediaEntry.name = identifiableType.name;

            identifiablePediaEntry._title = identifiableType.localizedName;
            identifiablePediaEntry._description = intro;
            identifiablePediaEntry._identifiableType = identifiableType;

            identifiablePediaEntry._details = entryDetails;
            identifiablePediaEntry._highlightSet = highlightSet;
            // identifiablePediaEntry._unlockInfoProvider = SceneContext.Instance.PediaDirector.Cast<IUnlockInfoProvider>();
            identifiablePediaEntry._isUnlockedInitially = isUnlockedInitially;

            RegisterPediaEntry(identifiablePediaEntry);
            return identifiablePediaEntry;
        }

        public static FixedPediaEntry CreateFixedEntry(Sprite icon, string name, string persistenceSuffix, PediaHighlightSet highlightSet, 
            LocalizedString title, LocalizedString intro, PediaEntryDetail[] entryDetails, bool isUnlockedInitially = false)
        {
            if (SRLookup.Get<FixedPediaEntry>(name))
                return null;

            FixedPediaEntry fixedPediaEntry = ScriptableObject.CreateInstance<FixedPediaEntry>();
            fixedPediaEntry.hideFlags |= HideFlags.HideAndDontSave;
            fixedPediaEntry.name = name;

            fixedPediaEntry._icon = icon;
            fixedPediaEntry._title = title;
            fixedPediaEntry._description = intro;
            fixedPediaEntry._persistenceSuffix = persistenceSuffix;

            fixedPediaEntry._details = entryDetails;
            fixedPediaEntry._highlightSet = highlightSet;
            // fixedPediaEntry._unlockInfoProvider = SceneContext.Instance.PediaDirector.Cast<IUnlockInfoProvider>();
            fixedPediaEntry._isUnlockedInitially = isUnlockedInitially;

            RegisterPediaEntry(fixedPediaEntry);
            return fixedPediaEntry;
        }

        public static void AddPediaToCategory(PediaEntry pediaEntry, PediaCategory pediaCategory)
        {
            if (!pediaCategory)
                return;

            if (!pediaCategory._items.Contains(pediaEntry))
                pediaCategory._items = pediaCategory._items.AddItem(pediaEntry).ToArray();

            LookupDirector director = SRSingleton<GameContext>.Instance.LookupDirector;
            if (!director._categories[director._categories.IndexOf(pediaCategory.GetRuntimeCategory())].Contains(pediaEntry))
                director.AddPediaEntryToCategory(pediaEntry, pediaCategory);
        }

        public static void AddSectionToPedia(PediaEntry pediaEntry, PediaDetailSection pediaDetailSection, LocalizedString textTranslation)
        {
            if (pediaEntry.IsNull())
                return;

            if (pediaDetailSection.IsNull())
                return;

/*            FixedPediaEntry fixedPediaEntry = pediaEntry.TryCast<FixedPediaEntry>();
            IdentifiablePediaEntry identifiablePediaEntry = pediaEntry.TryCast<IdentifiablePediaEntry>();

            string suffix = fixedPediaEntry ? fixedPediaEntry.PersistenceSuffix : 
                (identifiablePediaEntry ? identifiablePediaEntry.PersistenceSuffix : null);

            if (suffix.IsNull())
                return;*/

            List<PediaEntryDetail> entryDetails = pediaEntry._details?.ToList();
            if (entryDetails.IsNull())
                entryDetails = [];

            entryDetails.Add(new()
            {
                Section = pediaDetailSection,
                Text = textTranslation,
                TextGamepad = textTranslation,
                TextPS4 = textTranslation
            });

            pediaEntry._details = entryDetails.ToArray();
        }

        /*        public static PediaCategory CreatePediaCategory(Sprite icon, string name)
        {
            if (SRLookup.Get<PediaCategory>(name))
                return null;

            // Pedia Category

            PediaCategory pediaCategory = ScriptableObject.CreateInstance<PediaCategory>();
            pediaCategory.hideFlags |= HideFlags.HideAndDontSave;
            pediaCategory.name = name;

            pediaCategory._icon = icon;
            pediaCategory._items = Array.Empty<PediaEntry>();
            pediaCategory._title = TranslationPatcher.AddTranslation("UI", "b." + name.ToLower().Replace(" ", "_"), name);
            pediaCategory._lockedEntry = SRLookup.Get<FixedPediaEntry>("Locked");

            PediaConfiguration pediaConfiguration = SRLookup.Get<PediaConfiguration>("PediaConfiguration");
            if (!pediaConfiguration._categories.Contains(pediaCategory))
                pediaConfiguration._categories = pediaConfiguration._categories.AddItem(pediaCategory).ToArray();

            // Pedia Category Button

*//*            PediaCategoryButton baseCategoryButton = SRLookup.Get<PediaCategoryButton>("Slimes");

            PediaCategoryButton pediaCategoryButton = ScriptableObject.CreateInstance<PediaCategoryButton>();
            pediaCategoryButton.name = name;

            pediaCategoryButton._icon = UnityEngine.Object.Instantiate(baseCategoryButton._icon);
            pediaCategoryButton._icon.name = "Image";
            pediaCategoryButton._icon.sprite = icon;

            pediaCategoryButton._labelString.StringReference = pediaCategory._title;

            pediaCategoryButton._scaleSpring = UnityEngine.Object.Instantiate(baseCategoryButton._scaleSpring);
            pediaCategoryButton._scaleSpring.name = name;

            pediaCategoryButton._wobbleSpring = UnityEngine.Object.Instantiate(baseCategoryButton._wobbleSpring);
            pediaCategoryButton._wobbleSpring.name = name;

            PediaHomeScreen pediaHomeScreen = SRLookup.Get<PediaHomeScreen>("Home");
            if (!pediaHomeScreen._buttons.Contains(pediaCategoryButton))
                pediaHomeScreen._buttons = pediaHomeScreen._buttons.AddItem(pediaCategoryButton).ToArray();*//*

            return pediaCategory;
        }*/
    }

    /*public static class PediaRegistry
    {
        internal static List<PediaEntry> moddedPediaEntries = new List<PediaEntry>();
        private static PediaEntry defaultEntry;
        private static DietGroupHighlight dietGroupHighlight;
        public enum TemplateId
        {
            NONE = 0,
            SLIME = 1,
            TUTORIAL = 2,
            RESOURCE = 3,
            DESCRIPTION = 4,
        }

        public enum CategoryId
        {
            NONE = 0,
            TUTORIALS = 1,
            SCIENCE = 2,
            RESOURCES = 3,
            SLIMES = 4,
            WORLD = 5,
            RANCH = 6,
            
        }
        public struct ModdedPediaEntry
        {
            public TemplateId TemplateId;
            public CategoryId CategoryId;
            public Func<bool> IsAvailable;
            public Func<bool> IsHidden;
            public bool IsUnlockedInitially;
            public LocalizedString Description;
            public LocalizedString Title;
        }

        public static void RegisterPediaEntry(PediaEntry toRegister) => moddedPediaEntries.Add(toRegister);

        public static IdentifiablePediaEntry CreatePediaEntryForIdentifiable(IdentifiableType identifiableType, ModdedPediaEntry moddedPediaEntry)
        {
            defaultEntry ??= Resources.FindObjectsOfTypeAll<FixedPediaEntry>()[0];
            var identifiablePediaEntry = ScriptableObjectUtils.CreateScriptable<IdentifiablePediaEntry>();
            SetPediaTemplate(identifiablePediaEntry, moddedPediaEntry.TemplateId);
            SetPediaCategory(identifiablePediaEntry, moddedPediaEntry.CategoryId);
            identifiablePediaEntry.name = identifiableType.ValidatableName;
            identifiablePediaEntry.identifiableType = identifiableType;
            identifiablePediaEntry.title = moddedPediaEntry.Title;
            identifiablePediaEntry.description = moddedPediaEntry.Description;
            identifiablePediaEntry.isAvailable = moddedPediaEntry.IsAvailable;
            identifiablePediaEntry.isHidden = moddedPediaEntry.IsHidden;
            identifiablePediaEntry.isUnlockedInitially = moddedPediaEntry.IsUnlockedInitially;
            identifiablePediaEntry.actionButtonLabel = defaultEntry.actionButtonLabel;
            identifiablePediaEntry.infoButtonLabel = defaultEntry.infoButtonLabel;
            identifiablePediaEntry._UnavailableIcon_k__BackingField = defaultEntry.UnavailableIcon;
            RegisterPediaEntry(identifiablePediaEntry);
            return identifiablePediaEntry;
        }
        public static FixedPediaEntry CreatePediaEntryForNonIdentifiable(string textId, Sprite icon, ModdedPediaEntry moddedPediaEntry)
        {
            defaultEntry ??= Resources.FindObjectsOfTypeAll<FixedPediaEntry>()[0];
            var fixedPediaEntry = ScriptableObjectUtils.CreateScriptable<FixedPediaEntry>();
            SetPediaTemplate(fixedPediaEntry, moddedPediaEntry.TemplateId);
            SetPediaCategory(fixedPediaEntry, moddedPediaEntry.CategoryId);
            fixedPediaEntry.textId = textId;
            fixedPediaEntry.icon = icon;
            fixedPediaEntry.name = textId;
            fixedPediaEntry.title = moddedPediaEntry.Title;
            fixedPediaEntry.description = moddedPediaEntry.Description;
            fixedPediaEntry.isAvailable = moddedPediaEntry.IsAvailable;
            fixedPediaEntry.isHidden = moddedPediaEntry.IsHidden;
            fixedPediaEntry.isUnlockedInitially = moddedPediaEntry.IsUnlockedInitially;
            fixedPediaEntry.actionButtonLabel = defaultEntry.actionButtonLabel;
            fixedPediaEntry.infoButtonLabel = defaultEntry.infoButtonLabel;
            fixedPediaEntry._UnavailableIcon_k__BackingField = defaultEntry.UnavailableIcon;
            RegisterPediaEntry(fixedPediaEntry);
            return fixedPediaEntry;
        }

        public static void SetPediaCategory(PediaEntry pediaEntry, CategoryId categoryId)
        {
            switch (categoryId)
            {
                case CategoryId.NONE:
                    break;
                case CategoryId.TUTORIALS:
                    SRLookup.Get<Il2CppMonomiPark.SlimeRancher.UI.Pedia.PediaEntryCategory>("Tutorials").items.Add(pediaEntry);
                    break;
                case CategoryId.SCIENCE:
                    SRLookup.Get<Il2CppMonomiPark.SlimeRancher.UI.Pedia.PediaEntryCategory>("Science").items.Add(pediaEntry);
                    break;
                case CategoryId.RESOURCES:
                    SRLookup.Get<Il2CppMonomiPark.SlimeRancher.UI.Pedia.PediaEntryCategory>("Resources").items.Add(pediaEntry);
                    break;
                case CategoryId.SLIMES:
                    SRLookup.Get<Il2CppMonomiPark.SlimeRancher.UI.Pedia.PediaEntryCategory>("Slimes").items.Add(pediaEntry);
                    break;
                case CategoryId.WORLD:
                    SRLookup.Get<Il2CppMonomiPark.SlimeRancher.UI.Pedia.PediaEntryCategory>("World").items.Add(pediaEntry);
                    break;
                case CategoryId.RANCH:
                    SRLookup.Get<Il2CppMonomiPark.SlimeRancher.UI.Pedia.PediaEntryCategory>("Ranch").items.Add(pediaEntry);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(categoryId), categoryId, null);
            }
        }

        public static void SetPediaTemplate(PediaEntry pediaEntry, TemplateId templateId)
        {
            switch (templateId)
            {
                case TemplateId.NONE:
                    break;
                case TemplateId.SLIME:
                    pediaEntry.template = SRLookup.Get<PediaTemplate>("SlimePediaTemplate");
                    break;
                case TemplateId.TUTORIAL:
                    pediaEntry.template = SRLookup.Get<PediaTemplate>("TutorialPediaTemplate");
                    break;
                case TemplateId.RESOURCE:
                    pediaEntry.template = SRLookup.Get<PediaTemplate>("HighlightedResourcePediaTemplate");
                    break;
                case TemplateId.DESCRIPTION:
                    pediaEntry.template = SRLookup.Get<PediaTemplate>("DescriptionPediaTemplate");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(templateId), templateId, null);
            }
        }

        public static void AddFoodGroupOverride(DietGroupHighlight.SlimeDietOverrideEntry entry)
        {
            dietGroupHighlight ??= SRLookup.Get<DietGroupHighlight>("DietHighlight");
            dietGroupHighlight.slimeDietOverrideEntries.Add(entry);
        }
        
    }*/
}

