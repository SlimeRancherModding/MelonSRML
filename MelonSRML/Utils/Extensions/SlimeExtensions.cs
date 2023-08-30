using Il2CppSystem.Linq;
using MelonSRML.SR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelonSRML.Utils.Extensions
{
    public static class SlimeExtensions
    {
        public static SlimeAppearance GetAppearanceById(this SlimeDefinitions defs, IdentifiableType id) => defs.GetSlimeByIdentifiableId(id)?.AppearancesDefault[0];

        public static SlimeDefinition GetDefinitionByAppearance(this SlimeDefinitions defs, SlimeAppearance appearance) => defs.Slimes.First(x => x.Appearances.Contains(appearance));

        public static SlimeAppearanceStructure Clone(this SlimeAppearanceStructure structure)
        {
            SlimeAppearanceStructure cloned = new SlimeAppearanceStructure(structure);
            cloned.Element.name = cloned.Element.name.Replace("(Clone)", string.Empty);
            return cloned;
        }

        public static GameObject GetPrefab(this SlimeDefinition def) => def.prefab;

        public static SlimeDiet LoadLargoDiet(this SlimeDefinition def)
        {
            if (!def.IsLargo || def.BaseSlimes.Length == 0) return null;
            SlimeDiet diet = new SlimeDiet();

            diet.AdditionalFoodIdents = new IdentifiableType[0];
            diet.FavoriteIdents = new IdentifiableType[0];
            diet.MajorFoodGroups = new SlimeEat.FoodGroup[0];
            diet.MajorFoodIdentifiableTypeGroups = new IdentifiableTypeGroup[0];
            diet.ProduceIdents = new IdentifiableType[0];

            foreach (SlimeDefinition baseDef in def.BaseSlimes)
            {
                SlimeDiet baseDiet = baseDef.Diet;
                if (baseDiet.AdditionalFoodIdents != null) diet.AdditionalFoodIdents = diet.AdditionalFoodIdents.Union(baseDiet.AdditionalFoodIdents).ToArray();
                if (baseDiet.FavoriteIdents != null) diet.FavoriteIdents = diet.FavoriteIdents.Union(baseDiet.FavoriteIdents).ToArray();
                if (baseDiet.MajorFoodGroups != null) diet.MajorFoodGroups = diet.MajorFoodGroups.Union(baseDiet.MajorFoodGroups).ToArray();
                if (baseDiet.MajorFoodIdentifiableTypeGroups != null) diet.MajorFoodIdentifiableTypeGroups = diet.MajorFoodIdentifiableTypeGroups.Union(baseDiet.MajorFoodIdentifiableTypeGroups).ToArray();
                if (baseDiet.ProduceIdents != null) diet.ProduceIdents = diet.ProduceIdents.Concat(baseDiet.ProduceIdents).ToArray();
            }
            diet.FavoriteProductionCount = 2;

            def.Diet = diet;
            diet.RefreshEatMap(GameContext.Instance.SlimeDefinitions, def);
            return diet;
        }

        internal static Dictionary<IdentifiableType, List<SlimeDiet.EatMapEntry>> extraEatEntries = new Dictionary<IdentifiableType, List<SlimeDiet.EatMapEntry>>();

        public static void AddEatMapEntry(this SlimeDefinition def, SlimeDiet.EatMapEntry entry)
        {
            def.Diet.EatMap.Add(entry);
            if (!extraEatEntries.ContainsKey(def)) extraEatEntries[def] = new List<SlimeDiet.EatMapEntry>();
            extraEatEntries[def].Add(entry);
        }

        public static void RefreshEatmaps(this SlimeDefinitions defs)
        {
            foreach (SlimeDefinition def in defs.Slimes) def.Diet?.RefreshEatMap(defs, def);
        }
    }
}
