using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace MelonSRML.EnumPatcher
{
    /// <summary>
    /// Allows adding values to any Enum
    /// </summary>
    public static class EnumPatcher
    {
        private static FieldInfo cache;
        private static FieldInfo IL2CPPcache;

        private static Dictionary<Type, EnumPatch> patches = new Dictionary<Type, EnumPatch>();
        private static Dictionary<int, EnumPatch> IL2CPPpatches = new Dictionary<int, EnumPatch>();

        static EnumPatcher()
        {
            cache = AccessTools.Field(AccessTools.TypeByName("System.RuntimeType"), "GenericCache");
            IL2CPPcache = AccessTools.Field(typeof(Il2CppSystem.RuntimeType), "GenericCache");
        }

        internal static bool TryGetRawPatch(Type enumType, out EnumPatch patch) =>  patches.TryGetValue(enumType, out patch);
        internal static bool TryGetRawPatchInIL2CPP(Il2CppSystem.Type enumType, out EnumPatch patch) => IL2CPPpatches.TryGetValue(enumType.GetHashCode(), out patch);
        
        public static object GetFirstFreeValue(Type enumType)
        {
            var allValues = Enum.GetValues(enumType);
            for (var i = 0; i < allValues.Length - 1; i++)
            {
                if ((int)allValues.GetValue(i + 1) - (int)allValues.GetValue(i)>1)
                {
                    return Enum.ToObject(enumType, (int) allValues.GetValue(i) + 1);
                }
            }
            return Enum.ToObject(enumType,(int)allValues.GetValue(allValues.Length - 1) + 1);
        }
        public static Il2CppSystem.Object GetFirstFreeValueInIL2CPP(Il2CppSystem.Type enumType)
        {
            var allValues = Il2CppSystem.Enum.GetValues(enumType);
            for (var i = 0; i < allValues.Length - 1; i++)
            {
                if ((int)Il2CppSystem.Convert.ToInt32(allValues.GetValue(i + 1)) - (int)Il2CppSystem.Convert.ToInt32(allValues.GetValue(i)) >1)
                {
                    return Il2CppSystem.Enum.ToObject(enumType, Il2CppSystem.Convert.ToInt32(allValues.GetValue(i)) + 1);
                }
            }

            return Il2CppSystem.Enum.ToObject(enumType,
                Il2CppSystem.Convert.ToInt32(allValues.GetValue(allValues.Length - 1)) + 1);

            /*Msg(enumType.Name);
            var allValues = Il2CppSystem.Enum.GetValues(enumType);
            for (int i = 0; i < allValues.Length; i++)
            {
                var value = Il2CppSystem.Convert.ToInt32(allValues.GetValue(i +1));
                var allValue = Il2CppSystem.Convert.ToInt32(i);
                if (value - allValue > 1)
                {
                    
                    return Il2CppSystem.Enum.ToObject(enumType, Convert.ToInt32(allValues.GetValue(i)) + 1);
                }
            }

            return Il2CppSystem.Enum.ToObject(enumType, Convert.ToInt32(allValues.GetValue(allValues.Length - 1)) + 1);
            */
        }
        public static void ClearEnumCache(Type enumType)
        {
            cache.SetValue(enumType, null);
        }

        public static void ClearEnumCacheInIL2CPP(Il2CppSystem.Type enumType)
        {
            //IL2CPPcache.SetValue(enumType, null);
        }

        public static void AddEnumValue(Type enumType, object value, string name)
        {
            if (!enumType.IsEnum) throw new Exception($"{enumType} is not a valid Enum!");

            value = (ulong)Convert.ToInt64(value, CultureInfo.InvariantCulture);
            if (!patches.TryGetValue(enumType, out var patch))
            {
                patch = new EnumPatch();
                patches.Add(enumType, patch);
            }

            ClearEnumCache(enumType);

            patch.AddValue((ulong)value, name);
        }
        public static void AddEnumValueInIL2CPP(Il2CppSystem.Type enumType, Il2CppSystem.Object value, string name)
        {
            if (!enumType.IsEnum) throw new Exception($"{enumType} is not a valid Enum!");
            var @ulong = Il2CppSystem.Convert.ToUInt64(value);
            if (!IL2CPPpatches.TryGetValue(enumType.GetHashCode(), out var patch))
            {
                patch = new EnumPatch();
                IL2CPPpatches.Add(enumType.GetHashCode(), patch);
            }

            ClearEnumCacheInIL2CPP(enumType);

            patch.AddValue(@ulong, name);
        }
        
        public class EnumPatch
        {
            private Dictionary<ulong, string> values = new Dictionary<ulong, string>();

            public void AddValue(ulong enumValue, string name)
            {
                if (values.ContainsKey(enumValue)) return;
                values.Add(enumValue, name);
            }

            public void GetArrays(out string[] names, out ulong[] values)
            {
                names = this.values.Values.ToArray();
                values = this.values.Keys.ToArray();
            }
        }
        
    }
}
    