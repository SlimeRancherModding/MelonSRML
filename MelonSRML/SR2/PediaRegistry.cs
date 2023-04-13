using System;
using System.Collections.Generic;
using MelonSRML.Utils;
using UnityEngine.Localization;

namespace MelonSRML.SR2
{
    public static class PediaRegistry
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
        
    } 
}

