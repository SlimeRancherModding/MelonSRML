using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using MelonSRML.SR2;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(PediaDirector), nameof(PediaDirector.Awake))]
    internal static class PediaDirectorAwakePatch
    {
        public static void Prefix(PediaDirector __instance)
        {
            foreach (var pediaEntry in PediaRegistry.pediasToPatch)
            {
                var identPediaEntry = pediaEntry.TryCast<IdentifiablePediaEntry>();
                if (identPediaEntry)
                    __instance._identDict.TryAdd(identPediaEntry.IdentifiableType, pediaEntry);

                if (pediaEntry.IsUnlockedInitially)
                    __instance._initUnlocked = __instance._initUnlocked.AddItem(pediaEntry).ToArray();
            }
        }
    }
}