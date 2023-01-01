using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime;
using MelonSRML.Utils;

namespace MelonSRML.EnumPatcher
{
    public static class EnumPatcher
    {
        private static Dictionary<Type, EnumPatch> patches = new Dictionary<Type, EnumPatch>();
        private static Dictionary<int, EnumPatch> IL2CPPpatches = new Dictionary<int, EnumPatch>();
        
        internal static bool TryAsNumber(this object value, Type type, out object result)
        {
            if (type.IsSubclassOf(typeof(IConvertible)))
                throw new ArgumentException("The type must inherit the IConvertible interface", "type");
            result = null;
            if (type.IsInstanceOfType(value))
            {
                result = value;
                return true;
            }
            if (value is IConvertible convertible)
            {
                if (type.IsEnum)
                {
                    result = Enum.ToObject(type, convertible);
                    return true;
                }
                var format = NumberFormatInfo.CurrentInfo;
                result = convertible.ToType(type, format);
                return true;
            }
            return false;
        }
        public static object GetFirstFreeValue(Type enumType)
        {
            if (enumType == null) throw new ArgumentNullException("enumType");
            if (!enumType.IsEnum) throw new Exception($"{enumType} is not a valid Enum!");

            var vals = Enum.GetValues(enumType);
            long l = 0;
            for (ulong i = 0; i <= ulong.MaxValue; i++)
            {
                if (!i.TryAsNumber(enumType, out var v))
                    break;
                for (; l < vals.Length; l++)
                    if (Convert.ToUInt64(vals.GetValue(l)).Equals(Convert.ToUInt32(v)))
                        goto skip;
                return v;
                skip:;
                
            }
            for (long i = -1; i >= long.MinValue; i--)
            {
                if (!i.TryAsNumber(enumType, out var v))
                    break;
                for (; l < vals.Length; l++)
                    if (Convert.ToUInt64(vals.GetValue(l)).Equals(Convert.ToUInt32(v)))
                        goto skip;
                return v;
                skip:;
            }
            throw new Exception("No unused values in enum " + enumType.FullName);
        }
        
        
        public static TEnum GetFirstFreeValue<TEnum>() => (TEnum)GetFirstFreeValue(typeof(TEnum));
        public static void AddEnumValue(Type enumType, object value, string name)
        {
            if (enumType == null) throw new ArgumentNullException("enumType");
            if (!enumType.IsEnum) throw new Exception($"{enumType} is not a valid Enum!");
            if (AlreadyHasName(enumType, name) || EnumUtils.HasEnumValue(enumType, name)) throw new Exception($"The enum ({enumType.FullName}) already has a value with the name \"{name}\"");

            value = (ulong)Convert.ToInt64(value, CultureInfo.InvariantCulture);
            if (!patches.TryGetValue(enumType, out var patch))
            {
                patch = new EnumPatch();
                patches.Add(enumType, patch);
            }

            var from = Il2CppType.From(enumType, false);
            if (from != null)
            {
                if (!IL2CPPpatches.TryGetValue(from.GetHashCode(), out var Il2CPPpatch))
                {
                    Il2CPPpatch = new EnumPatch();
                    IL2CPPpatches.Add(from.GetHashCode(), Il2CPPpatch);
                } 
                Il2CPPpatch.AddValue((ulong)value, name);
            }


            patch.AddValue((ulong)value, name);
        }
        public static void AddEnumValue<T>(object value, string name) => AddEnumValue(typeof(T), value, name);
        public static object AddEnumValue(Type enumType, string name)
        {
            var newVal = GetFirstFreeValue(enumType);
            AddEnumValue(enumType, newVal, name);
            return newVal;
        }
        internal static bool AlreadyHasName(Type enumType, string name)
        {
            if (TryGetRawPatch(enumType, out EnumPatch patch))
                return patch.HasName(name);
            return false;
        }
        internal static bool TryGetRawPatch(Type enumType, out EnumPatch patch)
        {
            return patches.TryGetValue(enumType, out patch);
        }

        internal static bool TryGetRawPatchInIL2CPP(Il2CppSystem.Type enumType, out EnumPatch patch)
        {
            return IL2CPPpatches.TryGetValue(enumType.GetHashCode(), out patch);
        }
        

        
        public class EnumPatch 
        {
            private Dictionary<ulong, List<string>> values = new Dictionary<ulong, List<string>>();

            public void AddValue(ulong enumValue, string name)
            {
                if (values.ContainsKey(enumValue))
                    values[enumValue].Add(name);
                else
                    values.Add(enumValue, new List<string> { name });
            }
            
            

            public List<KeyValuePair<ulong, string>> GetPairs()
            {
                return (from pair in values from value in pair.Value select new KeyValuePair<ulong, string>(pair.Key, value)).ToList();
            }

            public bool HasName(string name)
            {
                return this.values.Values.SelectMany(l => l).Any(enumName => name.Equals(enumName));
            }
        }
    }
}