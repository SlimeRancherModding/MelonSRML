using HarmonyLib;
using Il2CppSystem;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(ScriptableObject), nameof(ScriptableObject.CreateInstance), typeof(Type))]
    internal static class ScriptableObjectCreateInstancePatch
    {
        public static void Postfix(ScriptableObject __result)
        {
            if (__result != null)
                __result.hideFlags |= HideFlags.HideAndDontSave;
        }
    }
}

