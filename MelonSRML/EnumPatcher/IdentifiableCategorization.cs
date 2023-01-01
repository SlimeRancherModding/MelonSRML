using System;
using System.Collections.Generic;
using MelonSRML.Utils;

namespace MelonSRML.EnumPatcher
{
    [AttributeUsage(AttributeTargets.Field)]
    public class IdentifiableCategorization : Attribute
    {
        public static List<IdentifiableType> doNotCategorize = new List<IdentifiableType>();

        internal Rule rules;
        internal bool shouldRegister = true;
        internal bool usingShouldRegister = false;

        public IdentifiableCategorization(Rule rules) => this.rules = rules;
        public IdentifiableCategorization(Rule rules, bool shouldRegister)
        {
            this.rules = rules;
            this.shouldRegister = true;
            usingShouldRegister = true;
        }

        [Flags]
        public enum Rule
        {
            NONE = 1,
            VEGGIE = 2,
            FRUIT = 4,
            NECTAR = 8,
            ELDER = 32,
            PLORT = 64,
            GORDO = 128,
            GADGET_DECORATION = 256,
            GADGET_UTILITIES = 512,
            GADGET_CURIOS = 1024,
            GADGET_WARP = 2048,
            LIQUID = 4096,
            WATER = 8192,
            LARGO = 16384,
            SLIME = 32768,
            TARR = 65536,
            VACCABLE = 131072,
            RESOURCE = 262144,
            ECHO = 524288,
            ORNAMENTS = 1048576,
            MEAT = 2097152
        }
    }

    public static class RuleExtensions
    {
        public static bool HasFlagFast(this IdentifiableCategorization.Rule value, IdentifiableCategorization.Rule flag)
        {
            return (value & flag) != 0;
        }

        public static void SetRule(this IdentifiableType @this, IdentifiableCategorization.Rule rule)
        {
            if (IdentifiableCategorization.doNotCategorize.Contains(@this))
                return;

            var identifiableTypeGroupList = IdentifiableTypeResolver.AllTypeGroupsList;
            foreach (var s in rule.ToString().Split(','))
            {
                var ruleSet = EnumUtils.Parse<IdentifiableCategorization.Rule>(s);
                {
                    switch (ruleSet)
                    {
                        case IdentifiableCategorization.Rule.NONE:
                            break;
                        case IdentifiableCategorization.Rule.VEGGIE:
                        {
                            identifiableTypeGroupList.Find(x => x.name.Equals("VeggieGroup")).AddIfNotContaining(@this);
                            break;
                        }
                        case IdentifiableCategorization.Rule.FRUIT:
                            identifiableTypeGroupList.Find(x => x.name.Equals("FruitGroup")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.NECTAR:
                            identifiableTypeGroupList.Find(x => x.name.Equals("NectarGroup")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.ELDER:
                            identifiableTypeGroupList.Find(x => x.name.Equals("ElderGroup")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.PLORT:
                            identifiableTypeGroupList.Find(x => x.name.Equals("PlortGroup")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.GORDO:
                            identifiableTypeGroupList.Find(x => x.name.Equals("GordoGroup")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.GADGET_DECORATION:
                            identifiableTypeGroupList.Find(x => x.name.Equals("GadgetDecorGroup")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.GADGET_UTILITIES:
                            identifiableTypeGroupList.Find(x => x.name.Equals("GadgetUtilitiesGroup")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.GADGET_CURIOS:
                            identifiableTypeGroupList.Find(x => x.name.Equals("GadgetCurios")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.GADGET_WARP:
                            identifiableTypeGroupList.Find(x => x.name.Equals("GadgetCurios")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.LIQUID:
                            identifiableTypeGroupList.Find(x => x.name.Equals("LiquidGroup")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.WATER:
                            identifiableTypeGroupList.Find(x => x.name.Equals("WaterGroup")).AddIfNotContaining(@this);
                            @this.SetRule(IdentifiableCategorization.Rule.LIQUID);
                            break;
                        case IdentifiableCategorization.Rule.LARGO:
                            identifiableTypeGroupList.Find(x => x.name.Equals("LargoGroup")).AddIfNotContaining(@this);
                            @this.SetRule(IdentifiableCategorization.Rule.SLIME);
                            break;
                        case IdentifiableCategorization.Rule.SLIME:
                            identifiableTypeGroupList.Find(x => x.name.Equals("SlimesGroup")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.TARR:
                            identifiableTypeGroupList.Find(x => x.name.Equals("TarrGroup")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.VACCABLE:
                            if (rule.HasFlagFast(IdentifiableCategorization.Rule.SLIME))
                                identifiableTypeGroupList.Find(x => x.name.Equals("VaccableBaseSlimeGroup")).AddIfNotContaining(@this);
                            else
                                identifiableTypeGroupList.Find(x => x.name.Equals("VaccableNonLiquids")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.RESOURCE: 
                            break;
                        case IdentifiableCategorization.Rule.ECHO:
                            identifiableTypeGroupList.Find(x => x.name.Equals("EchoGroup")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.ORNAMENTS:
                            identifiableTypeGroupList.Find(x => x.name.Equals("OrnamentGroup")).AddIfNotContaining(@this);
                            break;
                        case IdentifiableCategorization.Rule.MEAT:
                            identifiableTypeGroupList.Find(x => x.name.Equals("MeatGroup")).AddIfNotContaining(@this);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}
