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
                if (!pediaEntry)
                    continue;
                pediaEntry._unlockInfoProvider = __instance.Cast<IUnlockInfoProvider>();
            }
        }
    }
}