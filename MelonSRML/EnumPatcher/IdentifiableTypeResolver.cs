using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Il2CppMonomiPark.SlimeRancher;
using MelonSRML.SR2;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;

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
            foreach (IdentifiableType t in moddedTypes)
            {
                if (IdentifiableCategorization.doNotCategorize.Contains(t))
                    continue;

                // TODO: auto-categorize
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
                    if (!type.GetCustomAttributes(true).Any((x) => x is IdentifiableTypeHolderAttribute)) 
                        continue;
                    IdentifiableTypeHolderAttribute identifiableTypeHolder = type.GetCustomAttribute<IdentifiableTypeHolderAttribute>();

                    foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(info => Il2CppType.From(info.FieldType) != null))
                    {
                        Type fieldFieldType = field.FieldType;
                        bool shouldRegister = identifiableTypeHolder.shouldRegister;

                        if (field.GetValue(null) != null)
                            return;
                        if (fieldFieldType.IsSubclassOf(typeof(ScriptableObject)) == false) 
                            continue;
                        if (fieldFieldType.BaseType == null) 
                            continue;
                        Il2CppObjectBase scriptableObject;
                        switch (fieldFieldType.BaseType.Name)
                        {
                            case nameof(ScriptableObject) when fieldFieldType.Name.Equals(nameof(IdentifiableType)):
                            case nameof(IdentifiableType):
                            {
                                var identifiableType = ScriptableObject.CreateInstance(Il2CppType.From(fieldFieldType)).Cast<IdentifiableType>();
                                identifiableType.name = field.Name;
                                var scriptableObjectReferenceId = identifiableType.ReferenceId;
                                scriptableObject = identifiableType;
                                break;
                            }
                            default:
                            {
                                scriptableObject = null;
                                break;
                            }
                        }

                        if (scriptableObject is null)
                        {
                            Error($"Error when trying to create IdentifiableType '{field.Name}' for mod '{mod?.Info.Name}'");
                            continue;
                        }
                        if (!fieldFieldType.Name.Equals(nameof(IdentifiableType)))
                        {
                            try
                            {
                                scriptableObject = (Il2CppObjectBase)EntryPoint.TryCast.MakeGenericMethod(fieldFieldType).Invoke(scriptableObject, Array.Empty<object>());

                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                        }
                        if (scriptableObject is null) continue;
                        
                        field.SetValue(null, scriptableObject);
                        IdentifiableType cast = scriptableObject.Cast<IdentifiableType>();

                        foreach (var att in field.GetCustomAttributes())
                            if (att is IdentifiableCategorization attribute)
                            {
                                if (attribute.usingShouldRegister)
                                    shouldRegister = attribute.shouldRegister;
                                if (IGNORE_REGISTER.Any(x => attribute.rules.HasFlag(x)))
                                    shouldRegister = false;

                                cast.SetRule(attribute.rules);
                                IdentifiableCategorization.doNotCategorize.Add(cast);
                            }

                        if (shouldRegister)
                            dir.identifiableTypes.AddIfNotContaining(cast);
                    }
                }
            }
        }
    }
}

