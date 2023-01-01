using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Il2CppInterop.Runtime;
using Il2CppMonomiPark.SlimeRancher;
using MelonSRML.SR2;

namespace MelonSRML.EnumPatcher
{
    internal static class IdentifiableTypeResolver
    {
        internal static List<IdentifiableTypeGroup> AllTypeGroupsList = null;
        internal static List<IdentifiableType> moddedTypes = new List<IdentifiableType>();

        internal static readonly IdentifiableCategorization.Rule[] IGNORE_REGISTER = new IdentifiableCategorization.Rule[]
        {
            IdentifiableCategorization.Rule.GADGET_CURIOS,
            IdentifiableCategorization.Rule.GADGET_UTILITIES,
            IdentifiableCategorization.Rule.GADGET_WARP,
            IdentifiableCategorization.Rule.GORDO,
        };

        public static void CategorizeAllIdentifiables()
        {
            foreach (var t in moddedTypes.Where(t => IdentifiableCategorization.doNotCategorize.Contains(t)))
            {
                continue;
            }
        }

        public static void RegisterAllIdentifiables(AutoSaveDirector dir, SRMLMelonMod mod = null)
        {
            if (AllTypeGroupsList == null)
            {
                AllTypeGroupsList = new List<IdentifiableTypeGroup>();
                foreach (var VARIABLE in SRLookup.Get<IdentifiableTypeGroupList>("All Type Groups List").items)
                {
                    if (VARIABLE.memberGroups.Count == 0)
                    {
                        AllTypeGroupsList.Add(VARIABLE);
                    }
                }
            }
            Assembly melonAssemblyAssembly = mod is not null ? mod.MelonAssembly.Assembly : Melon<EntryPoint>.Instance.MelonAssembly.Assembly;
            
            
            foreach (Module module in melonAssemblyAssembly.Modules)
            {
                foreach (Type type in module.GetTypes())
                {
                    if (!type.GetCustomAttributes(true).Any((x) => x is IdentifiableTypeHolderAttribute)) continue;
                    IdentifiableTypeHolderAttribute identifiableTypeHolder = type.GetCustomAttribute<IdentifiableTypeHolderAttribute>();
                    foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(info => Il2CppType.From(info.FieldType, false) != null))
                    {
                        bool shouldRegister = identifiableTypeHolder.shouldRegister;
                        
                        if (field.GetValue(null) != null)
                            continue;
                        
                        if (!typeof(IdentifiableType).IsAssignableFrom(field.FieldType)) continue;
                        var identifiableType = ScriptableObject.CreateInstance(Il2CppType.From(field.FieldType)).Cast<IdentifiableType>();
                        identifiableType.name = field.Name;
                        var scriptableObjectReferenceId = identifiableType.ReferenceId;
                        if (identifiableType is null)
                        {
                            Error($"Error when trying to create IdentifiableType '{field.Name}' for mod '{mod?.Info.Name}'");
                            continue;
                        }
                        if (field.FieldType == typeof(IdentifiableType))
                        {
                            field.SetValue(null, identifiableType);
                        }
                        else
                        {
                            var invoke = EntryPoint.TryCast.MakeGenericMethod(field.FieldType).Invoke(identifiableType, Array.Empty<object>());
                            
                            field.SetValue(null, invoke);
                        }

                        foreach (var att in field.GetCustomAttributes())
                            if (att is IdentifiableCategorization attribute)
                            {
                                if (attribute.usingShouldRegister)
                                    shouldRegister = attribute.shouldRegister;
                                if (IGNORE_REGISTER.Any(x => attribute.rules.HasFlag(x)))
                                    shouldRegister = false;

                                identifiableType.SetRule(attribute.rules);
                                IdentifiableCategorization.doNotCategorize.Add(identifiableType);
                            }

                        if (shouldRegister)
                            dir.identifiableTypes.AddIfNotContaining(identifiableType);
                    }
                    
                }
            }
        }
    }
}

