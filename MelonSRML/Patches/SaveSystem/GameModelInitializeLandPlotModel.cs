using System;
using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.DataModel;

namespace MelonSRML.Patches.SaveSystem
{
    [HarmonyPatch(typeof(GameModel), nameof(GameModel.InitializeLandPlotModel))]
    public static class GameModelInitializeLandPlotModel
    {
        public static void Postfix(GameModel __instance, string plotId)
        {
            LandPlotModel instanceLandPlot = __instance.landPlots[plotId];

            if (!Enum.IsDefined(typeof(LandPlot.Id), instanceLandPlot.typeId))
            {
                instanceLandPlot.typeId = LandPlot.Id.NONE;
            }
        }
    }
}