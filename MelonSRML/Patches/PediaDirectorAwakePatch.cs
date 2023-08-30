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
            foreach (var pediaEntry in PediaRegistry.addedPedias)
            {
                var identPediaEntry = pediaEntry.TryCast<IdentifiablePediaEntry>();
                if (identPediaEntry && !__instance.identDict.ContainsKey(identPediaEntry.identifiableType))
                    __instance.identDict.Add(identPediaEntry.identifiableType, pediaEntry);

                if (pediaEntry.IsUnlockedInitially)
                    __instance.initUnlocked = __instance.initUnlocked.AddItem(pediaEntry).ToArray();
            }
        }
    }
}