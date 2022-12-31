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
    public static class IdentifiableTypeResolver
    {
        internal static List<IdentifiableTypeGroup> AllTypeGroupsList = null;
        public static void RegisterAllIdentifiables(SRMLMelonMod mod = null)
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
                    foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(info => Il2CppType.From(info.FieldType) != null))
                    {
                        
                        var fieldFieldType = field.FieldType;
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
                            Error($"The mSRML IdentifiableTypeHolder can't create identifiabletype for mod: {mod?.Info.Name}, name of field: {field.Name}");
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
                        var cast = scriptableObject.Cast<IdentifiableType>();
                        
                        if (identifiableTypeHolder.shouldCategorize)
                        {
                            foreach (var att in field.GetCustomAttributes())
                                if (att is IdentifiableCategorizationAttribute attribute)
                                {
                                    cast.SetRule(attribute.rules);
                                }
                        }
                        else
                        {
                            IdentifiableCategorizationAttribute.doNotCategorize.Add(cast);
                        }
                        
                    }
                }
            }
        }
    }
}

