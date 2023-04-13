using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MelonSRML.SR2;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(PediaDirector), nameof(PediaDirector.Awake))]
    internal static class PediaDirectorAwakePatch
    {
        public static void Prefix(PediaDirector __instance)
        {
            foreach (KeyValuePair<IdentifiableType, IdentifiablePediaEntry> keyValuePair in PediaRegistry.addedPedias)
            {
                if (__instance.identDict.ContainsKey(keyValuePair.Key))
                    __instance.identDict.Add(keyValuePair.Key, keyValuePair.Value);

                if (keyValuePair.Value.IsUnlockedInitially)
                {
                    __instance.initUnlocked = __instance.initUnlocked.AddItem(keyValuePair.Value).ToArray();
                }
            }
        }
    }
}