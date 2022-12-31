using System;
using System.Collections.Generic;
using MelonSRML.Utils;

namespace MelonSRML.EnumPatcher
{
    [AttributeUsage(AttributeTargets.Field)]
    public class IdentifiableCategorizationAttribute : Attribute
    {
        
        public static List<IdentifiableType> doNotCategorize = new List<IdentifiableType>(); 
        public IdentifiableCategorizationAttribute(Rule rules) => this.rules = rules;

        
        
        public Rule rules;
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
            ORNAMENTS =1048576,
            


        }        
        
    }

    public static class RuleExtensions
    {
        public static bool HasFlagFast(this IdentifiableCategorizationAttribute.Rule value, IdentifiableCategorizationAttribute.Rule flag)
        {
            return (value & flag) != 0;
        }

        public static void SetRule(this IdentifiableType @this, IdentifiableCategorizationAttribute.Rule rule)
        {
            var identifiableTypeGroupList = IdentifiableTypeResolver.AllTypeGroupsList;
            foreach (var s in rule.ToString().Split(','))
            {
                var ruleSet = EnumUtils.Parse<IdentifiableCategorizationAttribute.Rule>(s);
                {
                    switch (ruleSet)
                    {
                        case IdentifiableCategorizationAttribute.Rule.NONE:
                            break;
                        case IdentifiableCategorizationAttribute.Rule.VEGGIE:
                        {
                            identifiableTypeGroupList.Find(x => x.name.Equals("VeggieGroup")).IfDontContainsAdd(@this);
                            break;
                        }
                        case IdentifiableCategorizationAttribute.Rule.FRUIT:
                            identifiableTypeGroupList.Find(x => x.name.Equals("FruitGroup")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.NECTAR:
                            identifiableTypeGroupList.Find(x => x.name.Equals("NectarGroup")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.ELDER:
                            identifiableTypeGroupList.Find(x => x.name.Equals("ElderGroup")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.PLORT:
                            identifiableTypeGroupList.Find(x => x.name.Equals("PlortGroup")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.GORDO:
                            identifiableTypeGroupList.Find(x => x.name.Equals("GordoGroup")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.GADGET_DECORATION:
                            identifiableTypeGroupList.Find(x => x.name.Equals("GadgetDecorGroup")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.GADGET_UTILITIES:
                            identifiableTypeGroupList.Find(x => x.name.Equals("GadgetUtilitiesGroup")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.GADGET_CURIOS:
                            identifiableTypeGroupList.Find(x => x.name.Equals("GadgetCurios")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.GADGET_WARP:
                            identifiableTypeGroupList.Find(x => x.name.Equals("GadgetCurios")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.LIQUID:
                            identifiableTypeGroupList.Find(x => x.name.Equals("LiquidGroup")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.WATER:
                            identifiableTypeGroupList.Find(x => x.name.Equals("WaterGroup")).IfDontContainsAdd(@this);
                            @this.SetRule(IdentifiableCategorizationAttribute.Rule.LIQUID);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.LARGO:
                            identifiableTypeGroupList.Find(x => x.name.Equals("LargoGroup")).IfDontContainsAdd(@this);
                            @this.SetRule(IdentifiableCategorizationAttribute.Rule.SLIME);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.SLIME:
                            identifiableTypeGroupList.Find(x => x.name.Equals("SlimesGroup")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.TARR:
                            identifiableTypeGroupList.Find(x => x.name.Equals("TarrGroup")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.VACCABLE:
                            if (rule.HasFlagFast(IdentifiableCategorizationAttribute.Rule.SLIME))
                            {
                                identifiableTypeGroupList.Find(x => x.name.Equals("VaccableBaseSlimeGroup")).IfDontContainsAdd(@this);
                            }
                            else
                            {
                                identifiableTypeGroupList.Find(x => x.name.Equals("VaccableNonLiquids")).IfDontContainsAdd(@this);
                            }
                            break;
                        case IdentifiableCategorizationAttribute.Rule.RESOURCE: 
                            break;
                        case IdentifiableCategorizationAttribute.Rule.ECHO:
                            identifiableTypeGroupList.Find(x => x.name.Equals("EchoGroup")).IfDontContainsAdd(@this);
                            break;
                        case IdentifiableCategorizationAttribute.Rule.ORNAMENTS:
                            identifiableTypeGroupList.Find(x => x.name.Equals("OrnamentGroup")).IfDontContainsAdd(@this);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}
