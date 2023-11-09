using System;
using System.Collections.Generic;
using Il2CppMonomiPark.SlimeRancher.UI.Pedia;
using MelonSRML.Patches;
using System.Linq;
using MelonSRML.Utils;
using UnityEngine.Localization;
using Il2Cpp;
using Il2CppSystem.Security;

namespace MelonSRML.SR2
{
    public static class PediaRegistry
    {
        internal static HashSet<PediaEntry> addedPedias = new HashSet<PediaEntry>();

        public static string CreateIdentifiableKey(string prefix, IdentifiableType identifiableType)
        { return "m." + prefix + "." + identifiableType.localizationSuffix; }

        public static string CreateIdentifiablePageKey(string prefix, int pageNumber, IdentifiableType identifiableType)
        { return "m." + prefix + "." + identifiableType.localizationSuffix + ".page." + pageNumber.ToString(); }

        public static string CreateFixedKey(string prefix, string textId)
        { return "m." + prefix + "." + textId; }

        public static string CreateFixedPageKey(string prefix, int pageNumber, string textId)
        { return "m." + prefix + "." + textId + ".page." + pageNumber.ToString(); }

        public static IdentifiablePediaEntry CreateIdentifiableEntry(IdentifiableType identifiableType, string pediaEntryName, PediaTemplate pediaTemplate,
            LocalizedString pediaTitle, LocalizedString pediaIntro, LocalizedString actionButtonLabel, LocalizedString infoButtonLabel, bool unlockedInitially = false)
        {
            if (SRLookup.Get<IdentifiablePediaEntry>(pediaEntryName))
                return null;

            IdentifiablePediaEntry identifiablePediaEntry = ScriptableObject.CreateInstance<IdentifiablePediaEntry>();

            identifiablePediaEntry.hideFlags |= HideFlags.HideAndDontSave;
            identifiablePediaEntry.name = pediaEntryName;
            identifiablePediaEntry.identifiableType = identifiableType;
            identifiablePediaEntry.template = pediaTemplate;
            identifiablePediaEntry.title = pediaTitle;
            identifiablePediaEntry.description = pediaIntro;
            identifiablePediaEntry.isUnlockedInitially = unlockedInitially;
            identifiablePediaEntry.actionButtonLabel = actionButtonLabel;
            identifiablePediaEntry.infoButtonLabel = infoButtonLabel;

            return identifiablePediaEntry;
        }

        public static FixedPediaEntry CreateFixedEntry(string pediaEntryName, string pediaTextId, Sprite pediaIcon, PediaTemplate pediaTemplate,
            LocalizedString pediaTitle, LocalizedString pediaIntro, LocalizedString actionButtonLabel, LocalizedString infoButtonLabel, bool unlockedInitially = false)
        {
            if (SRLookup.Get<FixedPediaEntry>(pediaEntryName))
                return null;

            FixedPediaEntry fixedPediaEntry = ScriptableObject.CreateInstance<FixedPediaEntry>();

            fixedPediaEntry.hideFlags |= HideFlags.HideAndDontSave;
            fixedPediaEntry.name = pediaEntryName;
            fixedPediaEntry.template = pediaTemplate;
            fixedPediaEntry.title = pediaTitle;
            fixedPediaEntry.description = pediaIntro;
            fixedPediaEntry.icon = pediaIcon;
            fixedPediaEntry.textId = pediaTextId;
            fixedPediaEntry.isUnlockedInitially = unlockedInitially;
            fixedPediaEntry.actionButtonLabel = actionButtonLabel;
            fixedPediaEntry.infoButtonLabel = infoButtonLabel;

            return fixedPediaEntry;
        }

        public static void AddIdentifiablePage(string pediaEntryName, int pageNumber, string pediaText, bool isHowToUse = false)
        {
            IdentifiablePediaEntry identifiablePediaEntry = SRLookup.Get<IdentifiablePediaEntry>(pediaEntryName);

            if (!isHowToUse)
                TranslationPatcher.AddTranslation("PediaPage", CreateIdentifiablePageKey("desc", pageNumber, identifiablePediaEntry.identifiableType), pediaText);
            else if (isHowToUse)
                TranslationPatcher.AddTranslation("PediaPage", CreateIdentifiablePageKey("how_to_use", pageNumber, identifiablePediaEntry.identifiableType), pediaText);
        }

        public static void AddSlimepediaPage(string pediaEntryName, int pageNumber, string pediaText, bool isRisks = false, bool isPlortonomics = false)
        {
            IdentifiablePediaEntry identifiablePediaEntry = SRLookup.Get<IdentifiablePediaEntry>(pediaEntryName);

            if (isRisks && !isPlortonomics)
                TranslationPatcher.AddTranslation("PediaPage", CreateIdentifiablePageKey("risks", pageNumber, identifiablePediaEntry.identifiableType), pediaText);
            else if (!isRisks && isPlortonomics)
                TranslationPatcher.AddTranslation("PediaPage", CreateIdentifiablePageKey("plortonomics", pageNumber, identifiablePediaEntry.identifiableType), pediaText);
            else if (!isRisks && !isPlortonomics)
                TranslationPatcher.AddTranslation("PediaPage", CreateIdentifiablePageKey("slimeology", pageNumber, identifiablePediaEntry.identifiableType), pediaText);
        }

        public static void AddTutorialPage(string pediaEntryName, int pageNumber, string pediaText)
        {
            FixedPediaEntry fixedPediaEntry = SRLookup.Get<FixedPediaEntry>(pediaEntryName);
            TranslationPatcher.AddTranslation("PediaPage", CreateFixedPageKey("instructions", pageNumber, fixedPediaEntry.textId), pediaText);
        }

        public static PediaEntry AddIdentifiablePedia(IdentifiableType identifiableType, string pediaCategory, string pediaEntryName, string pediaIntro, bool useHighlightedTemplate = false, bool unlockedInitially = false)
        {
            if (SRLookup.Get<IdentifiablePediaEntry>(pediaEntryName))
                return null;

            PediaEntryCategory pediaEntryCategory = SRSingleton<SceneContext>.Instance.PediaDirector.entryCategories.items.ToArray().First(x => x.name == pediaCategory);
            PediaEntryCategory basePediaEntryCategory = SRSingleton<SceneContext>.Instance.PediaDirector.entryCategories.items.ToArray().First(x => x.name == "Resources");
            PediaEntry pediaEntry = basePediaEntryCategory.items.ToArray().First();
            IdentifiablePediaEntry identifiablePediaEntry = ScriptableObject.CreateInstance<IdentifiablePediaEntry>();

            LocalizedString intro = TranslationPatcher.AddTranslation("Pedia", CreateIdentifiableKey("intro", identifiableType), pediaIntro);

            identifiablePediaEntry.hideFlags |= HideFlags.HideAndDontSave;
            identifiablePediaEntry.name = pediaEntryName;
            identifiablePediaEntry.identifiableType = identifiableType;
            if (!useHighlightedTemplate)
                identifiablePediaEntry.template = pediaEntry.template;
            else
                identifiablePediaEntry.template = UnityEngine.Object.Instantiate(SRLookup.Get<PediaTemplate>("HighlightedResourcePediaTemplate"));
            identifiablePediaEntry.title = identifiableType.localizedName;
            identifiablePediaEntry.description = intro;
            identifiablePediaEntry.isUnlockedInitially = unlockedInitially;
            identifiablePediaEntry.actionButtonLabel = pediaEntry.actionButtonLabel;
            identifiablePediaEntry.infoButtonLabel = pediaEntry.infoButtonLabel;

            if (!pediaEntryCategory.items.Contains(identifiablePediaEntry))
                pediaEntryCategory.items.Add(identifiablePediaEntry);
            if (!addedPedias.Contains(identifiablePediaEntry))
                addedPedias.Add(identifiablePediaEntry);

            return identifiablePediaEntry;
        }

        public static PediaEntry AddSlimepedia(IdentifiableType identifiableType, string pediaEntryName, string pediaIntro, bool unlockedInitially = false)
        {
            if (SRLookup.Get<IdentifiablePediaEntry>(pediaEntryName))
                return null;

            PediaEntryCategory basePediaEntryCategory = SRSingleton<SceneContext>.Instance.PediaDirector.entryCategories.items.ToArray().First(x => x.name == "Slimes");
            PediaEntry pediaEntry = basePediaEntryCategory.items.ToArray().First();
            IdentifiablePediaEntry identifiablePediaEntry = ScriptableObject.CreateInstance<IdentifiablePediaEntry>();

            LocalizedString intro = TranslationPatcher.AddTranslation("Pedia", CreateIdentifiableKey("intro", identifiableType), pediaIntro);

            identifiablePediaEntry.hideFlags |= HideFlags.HideAndDontSave;
            identifiablePediaEntry.name = pediaEntryName;
            identifiablePediaEntry.identifiableType = identifiableType;
            identifiablePediaEntry.template = pediaEntry.template;
            identifiablePediaEntry.title = identifiableType.localizedName;
            identifiablePediaEntry.description = intro;
            identifiablePediaEntry.isUnlockedInitially = unlockedInitially;
            identifiablePediaEntry.actionButtonLabel = pediaEntry.actionButtonLabel;
            identifiablePediaEntry.infoButtonLabel = pediaEntry.infoButtonLabel;

            if (!basePediaEntryCategory.items.Contains(identifiablePediaEntry))
                basePediaEntryCategory.items.Add(identifiablePediaEntry);
            if (!addedPedias.Contains(identifiablePediaEntry))
                addedPedias.Add(identifiablePediaEntry);

            return identifiablePediaEntry;
        }

        public static PediaEntry AddTutorialPedia(string pediaEntryName, Sprite pediaIcon, string pediaTitle, string pediaDescription, bool unlockedInitially = true)
        {
            if (SRLookup.Get<FixedPediaEntry>(pediaEntryName))
                return null;

            PediaEntryCategory basePediaEntryCategory = SRSingleton<SceneContext>.Instance.PediaDirector.entryCategories.items.ToArray().First(x => x.name == "Tutorials");
            PediaEntry pediaEntry = basePediaEntryCategory.items.ToArray().First();
            FixedPediaEntry tutorialPediaEntry = ScriptableObject.CreateInstance<FixedPediaEntry>();

            LocalizedString title = TranslationPatcher.AddTranslation("Pedia", "m." + pediaEntryName.ToLower().Replace(" ", "_"), pediaTitle);
            LocalizedString desc = TranslationPatcher.AddTranslation("Pedia", CreateFixedKey("desc", pediaEntryName.ToLower().Replace(" ", "_")), pediaDescription);

            tutorialPediaEntry.hideFlags |= HideFlags.HideAndDontSave;
            tutorialPediaEntry.name = pediaEntryName;
            tutorialPediaEntry.template = pediaEntry.template;
            tutorialPediaEntry.title = title;
            tutorialPediaEntry.description = desc;
            tutorialPediaEntry.icon = pediaIcon;
            tutorialPediaEntry.textId = pediaEntryName.ToLower().Replace(" ", "_");
            tutorialPediaEntry.isUnlockedInitially = unlockedInitially;
            tutorialPediaEntry.actionButtonLabel = pediaEntry.actionButtonLabel;
            tutorialPediaEntry.infoButtonLabel = pediaEntry.infoButtonLabel;

            if (!basePediaEntryCategory.items.Contains(tutorialPediaEntry))
                basePediaEntryCategory.items.Add(tutorialPediaEntry);
            if (!addedPedias.Contains(tutorialPediaEntry))
                addedPedias.Add(tutorialPediaEntry);

            return tutorialPediaEntry;
        }
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

