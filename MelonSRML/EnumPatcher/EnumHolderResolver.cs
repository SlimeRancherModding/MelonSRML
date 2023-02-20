using System;
using System.Linq;
using System.Reflection;
using Il2CppInterop.Runtime;

namespace MelonSRML.EnumPatcher
{
    public class EnumHolderResolver
    {
        public static void RegisterAllEnums(SRMLMelonMod mod = null)
        {
            Assembly melonAssembly = mod is not null ? mod.MelonAssembly.Assembly : Melon<EntryPoint>.Instance.MelonAssembly.Assembly;
            
            foreach (Module module in melonAssembly.Modules)
            {
                foreach (Type type in module.GetTypes())
                {
                    if (!type.GetCustomAttributes(true).Any((x) => x is EnumHolderAttribute)) continue;
                    foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (!field.FieldType.IsEnum) continue;

                        if ((int)field.GetValue(null) != 0) continue;
                        var newVal = EnumPatcher.GetFirstFreeValue(field.FieldType);
                        EnumPatcher.AddEnumValue(field.FieldType, newVal, field.Name);
                        field.SetValue(null, newVal);
                    }
                }
            }
        }
    }
}