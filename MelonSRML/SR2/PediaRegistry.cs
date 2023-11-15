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
        internal static HashSet<PediaEntry> addedPedias = new HashSet<PediaEntry>();

        public static string CreateIdentifiableKey(string prefix, IdentifiableType identifiableType)
        { return "m." + prefix + "." + identifiableType.localizationSuffix; }

        /*public static string CreateIdentifiablePageKey(string prefix, int pageNumber, IdentifiableType identifiableType)
        { return "m." + prefix + "." + identifiableType.localizationSuffix + ".page." + pageNumber.ToString(); }*/

        public static string CreateFixedKey(string prefix, string textId)
        { return "m." + prefix + "." + textId; }

        /*public static string CreateFixedPageKey(string prefix, int pageNumber, string textId)
        { return "m." + prefix + "." + textId + ".page." + pageNumber.ToString(); }*/

        public static IdentifiablePediaEntry CreateIdentifiableEntry(IdentifiableType identifiableType, string pediaEntryName, PediaTemplate pediaTemplate,
            LocalizedString pediaTitle, LocalizedString pediaIntro, PediaEntry.PediaPagesEntry[] pediaPageEntries, bool unlockedInitially = false)
        {
            if (SRLookup.Get<IdentifiablePediaEntry>(pediaEntryName))
                return null;

            IdentifiablePediaEntry identifiablePediaEntry = ScriptableObject.CreateInstance<IdentifiablePediaEntry>();

            identifiablePediaEntry.hideFlags |= HideFlags.HideAndDontSave;
            identifiablePediaEntry.name = pediaEntryName;
            identifiablePediaEntry._title = pediaTitle;
            identifiablePediaEntry._description = pediaIntro;
            identifiablePediaEntry._identifiableType = identifiableType;

            identifiablePediaEntry._template = pediaTemplate;
            identifiablePediaEntry._pageEntries = pediaPageEntries;
            identifiablePediaEntry._unlockInfoProvider = SceneContext.Instance.PediaDirector.Cast<IUnlockInfoProvider>();
            identifiablePediaEntry._isUnlockedInitially = unlockedInitially;

            return identifiablePediaEntry;
        }

        public static FixedPediaEntry CreateFixedEntry(string pediaEntryName, string pediaTextId, Sprite pediaIcon, PediaTemplate pediaTemplate,
            LocalizedString pediaTitle, LocalizedString pediaIntro, PediaEntry.PediaPagesEntry[] pediaPageEntries, bool unlockedInitially = false)
        {
            if (SRLookup.Get<FixedPediaEntry>(pediaEntryName))
                return null;

            FixedPediaEntry fixedPediaEntry = ScriptableObject.CreateInstance<FixedPediaEntry>();

            fixedPediaEntry.hideFlags |= HideFlags.HideAndDontSave;
            fixedPediaEntry.name = pediaEntryName;
            fixedPediaEntry._title = pediaTitle;
            fixedPediaEntry._description = pediaIntro;

            fixedPediaEntry._icon = pediaIcon;
            fixedPediaEntry._textId = pediaTextId;
            fixedPediaEntry._template = pediaTemplate;
            fixedPediaEntry._pageEntries = pediaPageEntries;
            fixedPediaEntry._unlockInfoProvider = SceneContext.Instance.PediaDirector.Cast<IUnlockInfoProvider>();
            fixedPediaEntry._isUnlockedInitially = unlockedInitially;

            return fixedPediaEntry;
        }

        // Pages are now non-existent, lol
        public static void AddIdentifiableSection(IdentifiablePediaEntry identifiablePediaEntry, string pediaText, bool isHowToUse = false)
        {
            List<PediaEntry.PediaPagesEntry> pediaPagesEntries = identifiablePediaEntry._pageEntries.ToList();

            if (pediaPagesEntries.IsNull())
                pediaPagesEntries = new List<PediaEntry.PediaPagesEntry>();

            if (!isHowToUse)
            {
                pediaPagesEntries.Add(new PediaEntry.PediaPagesEntry()
                {
                    PediaPage = SRLookup.Get<PediaPage>("Description"),
                    Text = TranslationPatcher.AddTranslation("PediaPage", CreateIdentifiableKey("desc", identifiablePediaEntry.IdentifiableType), pediaText),
                    TextGamepad = new LocalizedString(),
                    TextPS4 = new LocalizedString()
                });
            }
            else
            {
                pediaPagesEntries.Add(new PediaEntry.PediaPagesEntry()
                {
                    PediaPage = SRLookup.Get<PediaPage>("OnTheRanchResource"),
                    Text = TranslationPatcher.AddTranslation("PediaPage", CreateIdentifiableKey("how_to_use", identifiablePediaEntry.IdentifiableType), pediaText),
                    TextGamepad = new LocalizedString(),
                    TextPS4 = new LocalizedString()
                });
            }

            identifiablePediaEntry._pageEntries = pediaPagesEntries.ToArray();
        }

        public static void AddSlimepediaSection(IdentifiablePediaEntry identifiablePediaEntry, string pediaText, bool isRisks = false, bool isPlortonomics = false)
        {
            List<PediaEntry.PediaPagesEntry> pediaPagesEntries = identifiablePediaEntry._pageEntries.ToList();

            if (pediaPagesEntries.IsNull())
                pediaPagesEntries = new List<PediaEntry.PediaPagesEntry>();

            if (isRisks && !isPlortonomics)
            {
                pediaPagesEntries.Add(new PediaEntry.PediaPagesEntry()
                {
                    PediaPage = SRLookup.Get<PediaPage>("Rancher Risks"),
                    Text = TranslationPatcher.AddTranslation("PediaPage", CreateIdentifiableKey("risks", identifiablePediaEntry.IdentifiableType), pediaText),
                    TextGamepad = new LocalizedString(),
                    TextPS4 = new LocalizedString()
                });
            }
            else if (!isRisks && isPlortonomics)
            {
                pediaPagesEntries.Add(new PediaEntry.PediaPagesEntry()
                {
                    PediaPage = SRLookup.Get<PediaPage>("Plortonomics"),
                    Text = TranslationPatcher.AddTranslation("PediaPage", CreateIdentifiableKey("plortonomics", identifiablePediaEntry.IdentifiableType), pediaText),
                    TextGamepad = new LocalizedString(),
                    TextPS4 = new LocalizedString()
                });
            }
            else
            {
                pediaPagesEntries.Add(new PediaEntry.PediaPagesEntry()
                {
                    PediaPage = SRLookup.Get<PediaPage>("Slimeology"),
                    Text = TranslationPatcher.AddTranslation("PediaPage", CreateIdentifiableKey("slimeology", identifiablePediaEntry.IdentifiableType), pediaText),
                    TextGamepad = new LocalizedString(),
                    TextPS4 = new LocalizedString()
                });
            }

            identifiablePediaEntry._pageEntries = pediaPagesEntries.ToArray();
        }

        public static void AddTutorialSection(FixedPediaEntry fixedPediaEntry, string pediaText)
        {
            List<PediaEntry.PediaPagesEntry> pediaPagesEntries = fixedPediaEntry._pageEntries.ToList();

            if (pediaPagesEntries.IsNull())
                pediaPagesEntries = new List<PediaEntry.PediaPagesEntry>();

            pediaPagesEntries.Add(new PediaEntry.PediaPagesEntry()
            {
                PediaPage = SRLookup.Get<PediaPage>("Instructions"),
                Text = TranslationPatcher.AddTranslation("PediaPage", CreateFixedKey("instructions", fixedPediaEntry._textId), pediaText),
                TextGamepad = new LocalizedString(),
                TextPS4 = new LocalizedString()
            });

            fixedPediaEntry._pageEntries = pediaPagesEntries.ToArray();
        }

        public static PediaEntry AddIdentifiablePedia(IdentifiableType identifiableType, string pediaEntryName, string pediaCategory, string pediaIntro, bool useHighlightedTemplate = false, bool unlockedInitially = false)
        {
            if (SRLookup.Get<IdentifiablePediaEntry>(pediaEntryName))
                return null;

            PediaEntryCategory pediaEntryCategory = SRSingleton<SceneContext>.Instance.PediaDirector._pediaConfiguration.Categories.ToArray().First(x => x.name == pediaCategory);
            PediaEntryCategory basePediaEntryCategory = SRSingleton<SceneContext>.Instance.PediaDirector._pediaConfiguration.Categories.ToArray().First(x => x.name == "Resources");
            PediaEntry pediaEntry = basePediaEntryCategory.Items.ToArray().First();

            LocalizedString intro = TranslationPatcher.AddTranslation("Pedia", CreateIdentifiableKey("intro", identifiableType), pediaIntro);
            IdentifiablePediaEntry identifiablePediaEntry = CreateIdentifiableEntry(identifiableType, pediaEntryName, pediaEntry._template, 
                identifiableType.localizedName, intro, null, unlockedInitially);

            if (useHighlightedTemplate)
                identifiablePediaEntry._template = UnityEngine.Object.Instantiate(SRLookup.Get<PediaTemplate>("HighlightedResourcePediaTemplate"));

            if (!pediaEntryCategory._items.ToArray().FirstOrDefault(x => x == identifiablePediaEntry))
                pediaEntryCategory._items = pediaEntryCategory._items.ToArray().AddToArray(identifiablePediaEntry);
            if (!addedPedias.Contains(identifiablePediaEntry))
                addedPedias.Add(identifiablePediaEntry);

            return identifiablePediaEntry;
        }

        public static PediaEntry AddSlimepedia(IdentifiableType identifiableType, string pediaEntryName, string pediaIntro, bool unlockedInitially = false)
        {
            if (SRLookup.Get<IdentifiablePediaEntry>(pediaEntryName))
                return null;

            PediaEntryCategory basePediaEntryCategory = SRSingleton<SceneContext>.Instance.PediaDirector._pediaConfiguration.Categories.ToArray().First(x => x.name == "Slimes");
            PediaEntry pediaEntry = basePediaEntryCategory.Items.ToArray().First();

            LocalizedString intro = TranslationPatcher.AddTranslation("Pedia", CreateIdentifiableKey("intro", identifiableType), pediaIntro);
            IdentifiablePediaEntry identifiablePediaEntry = CreateIdentifiableEntry(identifiableType, pediaEntryName, pediaEntry._template,
                identifiableType.localizedName, intro, null, unlockedInitially);

            if (!basePediaEntryCategory.Items.ToArray().FirstOrDefault(x => x == identifiablePediaEntry))
                basePediaEntryCategory._items = basePediaEntryCategory._items.ToArray().AddToArray(identifiablePediaEntry);
            if (!addedPedias.Contains(identifiablePediaEntry))
                addedPedias.Add(identifiablePediaEntry);

            return identifiablePediaEntry;
        }

        public static PediaEntry AddTutorialPedia(string pediaEntryName, Sprite pediaIcon, string pediaTitle, string pediaIntro, bool unlockedInitially = true)
        {
            if (SRLookup.Get<FixedPediaEntry>(pediaEntryName))
                return null;

            PediaEntryCategory basePediaEntryCategory = SRSingleton<SceneContext>.Instance.PediaDirector._pediaConfiguration.Categories.ToArray().First(x => x.name == "Tutorials");
            PediaEntry pediaEntry = basePediaEntryCategory.Items.ToArray().First();

            string pediaTextId = pediaEntryName.ToLower().Replace(" ", "_");
            LocalizedString title = TranslationPatcher.AddTranslation("Pedia", "t." + pediaTextId, pediaTitle);
            LocalizedString intro = TranslationPatcher.AddTranslation("Pedia", CreateFixedKey("intro", pediaTextId), pediaIntro);
            FixedPediaEntry tutorialPediaEntry = CreateFixedEntry(pediaEntryName, pediaTextId, pediaIcon, pediaEntry._template, title, intro, null, unlockedInitially);

            if (!basePediaEntryCategory.Items.ToArray().FirstOrDefault(x => x == tutorialPediaEntry))
                basePediaEntryCategory._items = basePediaEntryCategory._items.ToArray().AddToArray(tutorialPediaEntry);
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

