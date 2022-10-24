using Il2CppSystem;
using MelonLoader;
using System.Collections.Generic;
using System.Linq;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace MelonSRML.SR2
{
    public static class SRLookup
    {
        private static Dictionary<Type, List<UnityEngine.Object>> collected = new Dictionary<Type, List<UnityEngine.Object>>();

        public static T Get<T>(string name) where T : UnityEngine.Object
        {
            Type selected = Il2CppType.From(typeof(T));
            if (!collected.ContainsKey(selected))
                collected.Add(selected, Resources.FindObjectsOfTypeAll(selected).ToList());

            return collected[selected].Find(x => x.name == name).Cast<T>();
        }
        public static T GetCopy<T>(string name) where T : UnityEngine.Object => 
            UnityEngine.Object.Instantiate(Get<T>(name));

        public static GameObject CopyPrefab(GameObject g) => GameObject.Instantiate(g, EntryPoint.prefabParent);

        public static GameObject GetPrefabCopy(string name) => CopyPrefab(Get<GameObject>(name));

        static SRLookup()
        {
            EntryPoint.prefabParent = new GameObject("RuntimePrefabs").transform;
            EntryPoint.prefabParent.gameObject.SetActive(false);
            GameObject.DontDestroyOnLoad(EntryPoint.prefabParent.gameObject);
            EntryPoint.prefabParent.hideFlags = HideFlags.HideAndDontSave;
        }
    }
}
