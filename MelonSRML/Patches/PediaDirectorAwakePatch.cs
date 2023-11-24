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
            foreach (var pediaEntry in PediaRegistry.moddedPediaEntries)
            {
                var tryCast = pediaEntry.TryCast<IdentifiablePediaEntry>();
                if (tryCast)
                    __instance._identDict.Add(tryCast._identifiableType, tryCast);
                if (pediaEntry._isUnlockedInitially)
                {
                    __instance._initUnlocked = __instance._initUnlocked.AddItem(pediaEntry).ToArray();
                }
            }
        }
    }
}