using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MelonSRML.Utils
{
    public static class PrefabUtility
    {
        public static Transform DisabledParent;
        static PrefabUtility()
        {
            DisabledParent = new GameObject("DeactivedObject").transform;
            DisabledParent.gameObject.SetActive(false);
            Object.DontDestroyOnLoad(DisabledParent.gameObject);
            DisabledParent.gameObject.hideFlags |= HideFlags.HideAndDontSave;
        }

        /// <summary>
        /// Copys A Prefab
        /// </summary>
        public static GameObject CopyPrefab(GameObject prefab)
        {
            var newG = Object.Instantiate(prefab, DisabledParent);
            return newG;
        }
    }
}
