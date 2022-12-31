using System;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using MelonSRML.SR2.Ranch;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(LandPlot), nameof(LandPlot.ApplyUpgrades))]
    internal static class ApplyUpgradesPatch
    {
        public static void Prefix(LandPlot __instance, IEnumerable<LandPlot.Upgrade> upgrades)
        {
            List<LandPlot.Upgrade> landPlots = new List<LandPlot.Upgrade>(upgrades);
            foreach (ModdedPlotUpgrader plotUpgrader in __instance.GetComponents<ModdedPlotUpgrader>())
            {
                var typeByName = AccessTools.TypeByName(plotUpgrader.GetIl2CppType().Name);
                var invoke = EntryPoint.TryCast.MakeGenericMethod(typeByName).Invoke(plotUpgrader, Array.Empty<object>());
                var methodInfo = invoke.GetType().GetMethod("Apply", AccessTools.all);
                foreach (var upgrade in landPlots)
                {
                    methodInfo?.Invoke(invoke, new object[]
                    {
                        upgrade
                    });
                }
            }
                
        }
    }
}
