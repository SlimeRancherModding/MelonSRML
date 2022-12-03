using System;
using UnityEngine.Localization;

namespace MelonSRML.Utils
{
    public static class ScriptableObjectUtils
    {
        public static T CreateScriptable<T>(Action<T> constructor = null) where T : ScriptableObject
        {
            var instance = ScriptableObject.CreateInstance<T>();
            constructor?.Invoke(instance);
            return instance;
        }
    }
}