using System;
using System.Collections.Generic;
using MelonSRML.Utils;
using MelonSRML.Utils.Extensions;
using UnityEngine.Localization;

namespace MelonSRML.SR2
{
    public static class FoodGroupRegistry
    {
        internal static Dictionary<SlimeEat.FoodGroup, List<IdentifiableType>> addedFoodGroups = new();

        public static (IdentifiableTypeGroup, LocalizedString) AddNewFoodGroup(SlimeEat.FoodGroup foodGroup, string localized, Sprite icon, params IdentifiableType[] identifiableTypes)
        {
            if (MSRModLoader.CurrentLoadingStep != MSRModLoader.Step.OnGameContext)
            {
                throw new Exception("Can't register foodgroups outside of the OnGameContext Step");
            }
            if (SRLookup.Get<IdentifiableTypeGroup>(foodGroup.ToString().ToLower().FirstCharToUpper()+"Group") is not null)
            {
                throw new Exception($"This foodgroup is already registered: {foodGroup}");
            }

            var localizedString = TranslationPatcher.AddTranslation("UI", $"m.foodgroup.{foodGroup.ToString().ToLower()}", localized);
            return (ScriptableObjectUtils.CreateScriptable<IdentifiableTypeGroup>(group =>
            {
                group.name = foodGroup.ToString().ToLower().FirstCharToUpper() + "Group";
                group._icon = icon;
                group._localizedName = localizedString;
                foreach (var identifiableType in identifiableTypes)
                {
                    if (!addedFoodGroups.TryGetValue(foodGroup, out var list))
                    {
                        var value = new List<IdentifiableType>();
                        addedFoodGroups.Add(foodGroup, value);
                        list = value;

                    }

                    list.Add(identifiableType);

                    group._memberTypes.Add(identifiableType);
                }

            }), localizedString);
        }
        public static void AddToExistingGroup(SlimeEat.FoodGroup foodGroup, params IdentifiableType[] identifiableTypes)
        {
            var identifiableTypeGroup = SRLookup.Get<IdentifiableTypeGroup>(foodGroup.ToString().ToLower().FirstCharToUpper()+"Group");
            if (identifiableTypeGroup is null)
                throw new Exception($"This group is not registered: {foodGroup}");
            foreach (var identifiableType in identifiableTypes)
            {
                if (!addedFoodGroups.TryGetValue(foodGroup, out var list))
                {
                    var value = new List<IdentifiableType>();
                    addedFoodGroups.Add(foodGroup, value);
                    list = value;

                }
                list.Add(identifiableType); 
                identifiableTypeGroup._memberTypes.Add(identifiableType);
            }
        }
        
    }
}

