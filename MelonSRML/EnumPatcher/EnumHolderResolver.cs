using System;
using System.Linq;
using System.Reflection;
using UnhollowerRuntimeLib;

namespace MelonSRML.EnumPatcher
{
    public class EnumHolderResolver
    {
        public static void RegisterAllEnums(SRMLMelonMod mod = null)
        {

            Assembly melonAssemblyAssembly = null;
            if (mod is not null)
            {
                melonAssemblyAssembly = mod.MelonAssembly.Assembly;
            }
            else
            {
                melonAssemblyAssembly = Melon<EntryPoint>.Instance.MelonAssembly.Assembly;
            }
            
            foreach (Module module in melonAssemblyAssembly.Modules)
            {
                foreach (Type type in module.GetTypes())
                {
                    if (!type.GetCustomAttributes(true).Any((x) => x is EnumHolderAttribute)) continue;
                    foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (!field.FieldType.IsEnum) continue;

                        if ((int)field.GetValue(null) != 0) continue;
                        Il2CppSystem.Type il2cppType = Il2CppType.From(field.FieldType, false);
                        if (il2cppType is not null)
                            EnumPatcher.AddEnumValueInIL2CPP(il2cppType, EnumPatcher.GetFirstFreeValueInIL2CPP(il2cppType), field.Name);

                        var newVal = EnumPatcher.GetFirstFreeValue(field.FieldType);
                        EnumPatcher.AddEnumValue(field.FieldType, newVal, field.Name);

                        field.SetValue(null, newVal);
                    }
                }
            }
        }
    }
}