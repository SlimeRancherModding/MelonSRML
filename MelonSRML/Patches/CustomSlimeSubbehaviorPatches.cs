using HarmonyLib;
using MelonSRML.SR2.Slime;

namespace MelonSRML.Patches
{
    public static class CustomSlimeSubbehaviorPatches
    {
        internal static Il2CppSystem.Type moddedType;

        [HarmonyPatch(typeof(RockSlimeRoll), "Action")]
        public static class ActionPatch
        {
            public static bool Prefix(RockSlimeRoll __instance)
            {
                if (__instance.GetIl2CppType().BaseType == moddedType)
                {
                    __instance.TryCast<ModdedSlimeSubbehavior>().ModdedAction();
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(RockSlimeRoll), "Relevancy")]
        public static class RelevancyPatch
        {
            public static bool Prefix(RockSlimeRoll __instance, bool isGrounded, ref float __result)
            {
                if (__instance.GetIl2CppType().BaseType == moddedType)
                {
                    __result = __instance.TryCast<ModdedSlimeSubbehavior>().ModdedRelevancy(isGrounded);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(RockSlimeRoll), "Selected")]
        public static class SelectedPatch
        {
            public static bool Prefix(RockSlimeRoll __instance)
            {
                if (__instance.GetIl2CppType().BaseType == moddedType)
                {
                    __instance.TryCast<ModdedSlimeSubbehavior>().ModdedSelected();
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(RockSlimeRoll), "Deselected")]
        public static class DeselectedPatch
        {
            public static bool Prefix(RockSlimeRoll __instance)
            {
                if (__instance.GetIl2CppType().BaseType == moddedType)
                {
                    __instance.TryCast<ModdedSlimeSubbehavior>().ModdedDeselected();
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(RockSlimeRoll), "CanRethink")]
        public static class CanRethinkPatch
        {
            public static bool Prefix(RockSlimeRoll __instance, ref bool __result)
            {
                if (__instance.GetIl2CppType().BaseType == moddedType)
                {
                    __result = __instance.TryCast<ModdedSlimeSubbehavior>().ModdedCanRethink();
                    return false;
                }
                return true;
            }
        }

        /*[HarmonyPatch(typeof(RockSlimeRoll), "Forbids")]
        public static class ForbidsPatch
        {
            public static bool Prefix(RockSlimeRoll __instance, ModdedSubbehaviour toMaybeForbid, ref bool __result)
            {
                if (__instance.GetIl2CppType() == UnhollowerRuntimeLib.Il2CppType.Of<ModdedSlimeSubbehavior>())
                {
                    __result = __instance.TryCast<ModdedSlimeSubbehavior>().ModdedForbids(toMaybeForbid);
                    return false;
                }
                return true;
            }
        }*/

        [HarmonyPatch(typeof(RockSlimeRoll), "Awake")]
        public static class AwakePatch
        {
            public static void Prefix(RockSlimeRoll __instance)
            {
                if (__instance.GetIl2CppType().BaseType == moddedType)
                    __instance.TryCast<ModdedSlimeSubbehavior>().ModdedAwake();
            }
        }
    }
}
