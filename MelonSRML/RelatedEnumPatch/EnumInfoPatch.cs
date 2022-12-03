using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Enum = Il2CppSystem.Enum;

namespace MelonSRML.RelatedEnumPatch;

[HarmonyPatch(typeof(Il2CppSystem.Enum), nameof(Il2CppSystem.Enum.GetCachedValuesAndNames))]
public class EnumInfoInIl2CPP
{
    public static void Postfix(Il2CppSystem.RuntimeType enumType, ref Il2CppSystem.Enum.ValuesAndNames __result)
    {
        ulong[] il2CppStructArray = __result.Values;
        string[] il2CppStringArray = __result.Names;
        FixEnum(enumType, ref il2CppStructArray, ref il2CppStringArray);
        __result = new Enum.ValuesAndNames(il2CppStructArray, il2CppStringArray);
    }
    static void FixEnum(Il2CppSystem.Object type, ref ulong[] oldValues, ref string[] oldNames)
    {
        var enumType = type.Cast<Il2CppSystem.Type>();
        if (enumType is null) return;
        if (EnumPatcher.TryGetRawPatchInIL2CPP(enumType, out var patch))
        {

            patch.GetArrays(out var toBePatchedNames,out var toBePatchedValues);
            Array.Resize(ref toBePatchedNames, toBePatchedNames.Length + oldNames.Length);
            Array.Resize(ref toBePatchedValues, toBePatchedValues.Length + oldValues.Length);
            Array.Copy(oldNames, 0, toBePatchedNames, toBePatchedNames.Length - oldNames.Length, oldNames.Length);
            Array.Copy(oldValues, 0, toBePatchedValues, toBePatchedValues.Length - oldValues.Length, oldValues.Length);
            oldValues = toBePatchedValues;
            oldNames = toBePatchedNames;

            Array.Sort<ulong, string>(oldValues, oldNames, Comparer<ulong>.Default);
        }
    }
}

[HarmonyPatch]
public static class EnumInfoPatch
{
    static MethodBase TargetMethod()
    {
        return AccessTools.Method(Type.GetType("System.Enum"), "GetCachedValuesAndNames");

    }
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        using (var enumerator = instructions.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                var v = enumerator.Current;
                if (v != null && v.operand is MethodInfo me && me.Name=="Sort")
                {
                    yield return v;
                    enumerator.MoveNext();
                    v = enumerator.Current;
                    var labels = v.labels;
                    v.labels = new List<Label>();
                    yield return new CodeInstruction(OpCodes.Ldarg_0) { labels = labels};
                    yield return new CodeInstruction(OpCodes.Ldloca, 1);
                    yield return new CodeInstruction(OpCodes.Ldloca, 2);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(EnumInfoPatch), "FixEnum"));
                    yield return v;
                }
                else
                {
                    yield return v;
                }
            }
        }
    }

    static void FixEnum(object type, ref ulong[] oldValues, ref string[] oldNames)
    {
        if (type is not Type enumType) return;
        if (EnumPatcher.TryGetRawPatch(enumType, out var patch))
        {

            patch.GetArrays(out var toBePatchedNames,out var toBePatchedValues);
            Array.Resize(ref toBePatchedNames, toBePatchedNames.Length + oldNames.Length);
            Array.Resize(ref toBePatchedValues, toBePatchedValues.Length + oldValues.Length);
            Array.Copy(oldNames, 0, toBePatchedNames, toBePatchedNames.Length - oldNames.Length, oldNames.Length);
            Array.Copy(oldValues, 0, toBePatchedValues, toBePatchedValues.Length - oldValues.Length, oldValues.Length);
            oldValues = toBePatchedValues;
            oldNames = toBePatchedNames;

            Array.Sort<ulong, string>(oldValues, oldNames, Comparer<ulong>.Default);
        }
        
    }
    
}