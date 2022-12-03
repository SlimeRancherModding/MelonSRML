using System;
using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppSystem.Collections.Generic;
using UnhollowerBaseLib;

namespace MelonSRML.Patches.SaveSystem
{
    [HarmonyPatch(typeof(LandPlotModel), nameof(LandPlotModel.Push))]
    internal static class LandPlotModelPull
    {
        public static void Prefix(LandPlotModel __instance, ref LandPlot.Id typeId, ref double feederNextTime,
            ref int feederPendingCount,
            ref SlimeFeeder.FeedSpeed feederCycleSpeed,
            ref double collectorNextTime,
            ref ResourceGrowerDefinition resourceGrowerDefinition,
            ref List<LandPlot.Upgrade> upgrades,
            ref Dictionary<string, Il2CppReferenceArray<Ammo.Slot>> siloAmmo,
            ref List<int> siloStorageIndices,
            ref float ashUnits,
            ref HashSet<long> trackedActorIds)
        {
            if (Enum.IsDefined(typeof(LandPlot.Id), typeId))
                return;
            feederNextTime = 0;
            feederPendingCount = 0;
            feederCycleSpeed = SlimeFeeder.FeedSpeed.Normal;
            collectorNextTime = 0;
            resourceGrowerDefinition = null;
            upgrades.Clear();
            siloAmmo.Clear();
            siloStorageIndices.Clear();
            ashUnits = 0f;
            trackedActorIds.Clear();
            typeId = LandPlot.Id.EMPTY;
        }
    }
}