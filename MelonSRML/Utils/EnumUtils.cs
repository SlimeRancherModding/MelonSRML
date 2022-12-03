using System;
using System.Collections.Generic;
using System.Linq;

namespace MelonSRML.Utils
{

    public static class EnumUtils
    {

        public static object Parse(Type enumType, string value)
        {
            if (!enumType.IsEnum)
                throw new Exception($"The given type isn't an enum ({enumType.FullName} isn't an Enum)");

            try
            {
                return System.Enum.Parse(enumType, value);
            }
            catch
            {
                return null;
            }
        }

     
        public static object Parse(Type enumType, string value, bool ignoreCase)
        {
            if (!enumType.IsEnum)
                throw new Exception($"The given type isn't an enum ({enumType.FullName} isn't an Enum)");

            try
            {
                return System.Enum.Parse(enumType, value, ignoreCase);
            }
            catch
            {
                return null;
            }
        }

      
        public static object FromInt(Type enumType, int value)
        {
            if (!enumType.IsEnum)
                throw new Exception($"The given type isn't an enum ({enumType.FullName} isn't an Enum)");

            return System.Enum.ToObject(enumType, value);
        }

       
        public static string[] GetAllNames(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new Exception($"The given type isn't an enum ({enumType.FullName} isn't an Enum)");

            return System.Enum.GetNames(enumType);
        }

      
        public static object[] GetAll(Type enumType)
        {
            List<object> enums = new List<object>();

            foreach (string name in GetAllNames(enumType))
            {
                object value = Parse(enumType, name);
                if (value != null)
                    enums.Add(value);
            }

            return enums.ToArray();
        }

     
        public static bool IsDefined(Type enumType, string value)
        {
            try
            {
                return System.Enum.IsDefined(enumType, value);
            }
            catch
            {
                return false;
            }
        }

     
        public static bool HasEnumValue(Type enumType, string value)
        {
            foreach (string name in GetAllNames(enumType))
            {
                if (name.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        public static object GetMinValue(Type enumType)
        {
            return GetAll(enumType).Cast<int>().Min();
        }

        public static object GetMaxValue(Type enumType)
        {
            return GetAll(enumType).Cast<int>().Max();
        }

      
        public static T Parse<T>(string value, T errorReturn = default) where T : System.Enum
        {
            try
            {
                return (T)System.Enum.Parse(typeof(T), value);
            }
            catch
            {
                return errorReturn;
            }
        }

        public static T Parse<T>(string value, bool ignoreCase, T errorReturn = default) where T : System.Enum
        {
            try
            {
                return (T)System.Enum.Parse(typeof(T), value, ignoreCase);
            }
            catch
            {
                return errorReturn;
            }
        }

      
        public static T FromInt<T>(int value) where T : System.Enum
        {
            return (T)System.Enum.ToObject(typeof(T), value);
        }

        public static string[] GetAllNames<T>() where T : System.Enum
        {
            return System.Enum.GetNames(typeof(T));
        }
        
        public static T[] GetAll<T>(T errorReturn = default) where T : System.Enum
        {
            List<T> enums = new List<T>();

            foreach (string name in GetAllNames<T>())
                enums.Add(Parse<T>(name, errorReturn));

            return enums.ToArray();
        }

        public static bool IsDefined<T>(string value) where T : System.Enum
        {
            try
            {
                return System.Enum.IsDefined(typeof(T), value);
            }
            catch
            {
                return false;
            }
        }

    
        public static bool HasEnumValue<T>(string value) where T : System.Enum
        {
            foreach (string name in GetAllNames<T>())
            {
                if (name.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        public static T GetMinValue<T>() where T : System.Enum
        {
            return GetAll<T>().Min();
        }

        public static T GetMaxValue<T>() where T : System.Enum
        {
            return GetAll<T>().Max();
        }
    }
}