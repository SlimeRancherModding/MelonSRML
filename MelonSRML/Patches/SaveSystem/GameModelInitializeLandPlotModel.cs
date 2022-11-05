using System;
using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.UI.ButtonBehavior;
using UnhollowerBaseLib.Runtime;
using UnhollowerRuntimeLib;
using Object = Il2CppSystem.Object;

namespace MelonSRML.Patches.SaveSystem;

[HarmonyPatch(typeof(GameModel), nameof(GameModel.InitializeLandPlotModel))]
public static class GameModelInitializeLandPlotModel
{
    public static void Postfix(GameModel __instance, string plotId)
    {
        var instanceLandPlot = __instance.landPlots[plotId];

        if (!Enum.IsDefined(typeof(LandPlot.Id), instanceLandPlot.typeId))
        {
            instanceLandPlot.typeId = LandPlot.Id.NONE;
        }
        //__instance.PromptForUpgrade(new PlotUpgradePurchaseItemModel().);
    }
}