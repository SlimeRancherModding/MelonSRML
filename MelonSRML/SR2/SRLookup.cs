using Il2CppSystem;
using System.Collections.Generic;
using System.Linq;
using Il2CppInterop.Runtime;
using Object = UnityEngine.Object;

namespace MelonSRML.SR2
{
    public static class SRLookup
    {
        private static Dictionary<Type, List<Object>> collected = new Dictionary<Type, List<Object>>();

        public static T Get<T>(string name) where T : Object
        {
            Type selected = Il2CppType.From(typeof(T));
            if (!collected.ContainsKey(selected))
                collected.Add(selected, Resources.FindObjectsOfTypeAll(selected).ToList());

            return collected[selected].Find(x => x.name == name)?.Cast<T>();
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
