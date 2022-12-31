using System;
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
            foreach (var pediaEntry in PediaRegistry.moddedPediaEntries)
            {
                var tryCast = pediaEntry.TryCast<IdentifiablePediaEntry>();
                if (tryCast)
                    __instance.identDict.Add(tryCast.identifiableType, tryCast);
                if (pediaEntry.IsUnlockedInitially)
                {
                    __instance.initUnlocked = __instance.initUnlocked.AddItem(pediaEntry).ToArray();
                }
            }
        }
    }
}