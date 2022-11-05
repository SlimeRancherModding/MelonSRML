using System.Linq;
using System.Reflection;
using UnhollowerRuntimeLib;

namespace MelonSRML.RelatedEnumPatch;

public class EnumHolderResolver
{
    public static void RegisterAllEnums(SRMLMelonMod mod)
    {
        foreach (var module in mod.MelonAssembly.Assembly.Modules)
        {
            foreach (var type in module.GetTypes())
            {
                if (!type.GetCustomAttributes(true).Any((x) => x is EnumHolderAttribute)) continue;
                foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!field.FieldType.IsEnum) continue;

                    if ((int)field.GetValue(null) != 0) continue;
                    var from = Il2CppType.From(field.FieldType, false);
                    if (from is not null)
                        EnumPatcher.AddEnumValueInIL2CPP(from, EnumPatcher.GetFirstFreeValueInIL2CPP(from), field.Name);
                    

                    var newVal = EnumPatcher.GetFirstFreeValue(field.FieldType);
                    EnumPatcher.AddEnumValue(field.FieldType, newVal, field.Name);

                    field.SetValue(null, newVal);


                }
            }
        }
    }
}