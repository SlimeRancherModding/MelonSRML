using Il2CppSystem;
using MelonLoader;
using System.Collections.Generic;
using System.Linq;
using UnhollowerRuntimeLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MelonSRML.SR2
{
    public static class SRLookup
    {
        private static readonly Dictionary<Type, Object[]> cache = new Dictionary<Type, Object[]>();

        public static T Get<T>(string name) where T : Object
        {
            Type selected = Il2CppType.From(typeof(T));
            if (!cache.ContainsKey(selected))
                cache.Add(selected, Resources.FindObjectsOfTypeAll<T>());

            T found = cache[selected].FirstOrDefault(x => x.name == name)?.Cast<T>();
            if (found == null)
            {
                cache[selected] = Resources.FindObjectsOfTypeAll<T>();
                found = cache[selected].FirstOrDefault(x => x.name == name)?.Cast<T>();
            }

            return found;
        }
        public static T GetCopy<T>(string name) where T : Object => 
            Object.Instantiate(Get<T>(name));

        public static GameObject CopyPrefab(GameObject g) => Object.Instantiate(g, EntryPoint.prefabParent);

        public static GameObject GetPrefabCopy(string name) => CopyPrefab(Get<GameObject>(name));

        public static GameObject InstantiateInactive(GameObject g) => InstantiateInactive(g, false);

        public static GameObject InstantiateInactive(GameObject g, bool keepOriginalName)
        {
            GameObject obj = Object.Instantiate(g, EntryPoint.prefabParent);
            obj.SetActive(false);
            obj.transform.SetParent(null);
            obj.hideFlags = g.hideFlags;
            
            if (keepOriginalName)
                obj.name = g.name;
            
            return obj;
        }

        static SRLookup()
        {
            EntryPoint.prefabParent = new GameObject("RuntimePrefabs").transform;
            EntryPoint.prefabParent.gameObject.SetActive(false);
            Object.DontDestroyOnLoad(EntryPoint.prefabParent.gameObject);
            EntryPoint.prefabParent.hideFlags = HideFlags.HideAndDontSave;
        }
    }
}
