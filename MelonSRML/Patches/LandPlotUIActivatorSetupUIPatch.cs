using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.Ranch;
using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppMonomiPark.SlimeRancher.UI.Plot;
using MelonSRML.SR2.Ranch;
using MelonSRML.Utils;
using MelonSRML.Utils.Extensions;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(LandPlotUIActivator), nameof(LandPlotUIActivator.SetupUI))]

    internal static class LandPlotUIActivatorSetupUIPatch
    {
        public static List<LandPlot.Id> LandPlotsOnce = new List<LandPlot.Id>();
        public static void Prefix(LandPlotUIActivator __instance, GameObject ui)
        {
            var landPlotTypeId = __instance.landPlot.typeId;
            if (LandPlotsOnce.FirstOrDefault(x => x ==landPlotTypeId) != LandPlot.Id.NONE)
                return;
            
            LandPlotsOnce.Add(landPlotTypeId);
            LandPlotUIRoot landPlotUIRoot = ui.GetComponent<LandPlotUIRoot>();
          
            if (landPlotTypeId == LandPlot.Id.EMPTY)
            {
                var objectFromIndexInList = landPlotUIRoot.menuConfig.categories.GetObjectFromIndexInList(0);
                foreach (PlotPatchPurchaseItemModel plotPurchaseItemModel in from landPlotShopEntry in LandPlotRegistry.moddedLandPlotShopEntries let fromIndexInList = objectFromIndexInList.ItemsIncludingHidden.GetObjectFromIndexInList(0) select ScriptableObjectUtils.CreateScriptable<PlotPatchPurchaseItemModel>(model =>
                         {
                             var tocapitalize = landPlotShopEntry.plot.ToString().FirstCharToUpper();
                             model.name = tocapitalize + " Patch";
                             model.description = landPlotShopEntry.DescKey;
                             model.title =  landPlotShopEntry.NameKey;
                             model.icon = landPlotShopEntry.icon;
                             var purchaseCost = PurchaseCost.CreateEmpty();
                             purchaseCost.newbuckCost = landPlotShopEntry.cost;
                             model.purchaseCost = purchaseCost;
                             model.IsAvailable = landPlotShopEntry.isUnlocked;
                             model.IsHidden = landPlotShopEntry.isHidden;
                             model.pediaEntry = landPlotShopEntry.pediaEntry;
                             model.actionButtonLabel = fromIndexInList.actionButtonLabel;
                             model.infoButtonLabel = fromIndexInList.infoButtonLabel;
                             model.promptMessage = fromIndexInList.promptMessage;
                             model.plotDefinition = ScriptableObjectUtils.CreateScriptable<PlotDefinition>(definition =>definition.name = landPlotShopEntry.plot.ToString().FirstCharToUpper());
                             model.plotPrefab = SRSingleton<GameContext>.Instance.LookupDirector.GetPlotPrefab(landPlotShopEntry.plot);
                        
                         }))
                    objectFromIndexInList.items.Add(plotPurchaseItemModel);
            }
           
            if (LandPlotUpgradeRegistry.moddedUpgradeShopEntry.TryGetValue(landPlotTypeId, out var list))
            {
                
                var objectFromIndexInList = landPlotUIRoot.menuConfig.categories.GetObjectFromIndexInList(0);
                var plotPurchaseItemModel = objectFromIndexInList.ItemsIncludingHidden.GetObjectFromIndexInList(1);
                foreach (var landPlotShopEntry in list)
                {
                   objectFromIndexInList.ItemsIncludingHidden.Add(
                        ScriptableObjectUtils.CreateScriptable<PlotUpgradePurchaseItemModel>(model =>
                        {
                            var tocapitalize = landPlotShopEntry.upgrade.ToString().FirstCharToUpper();
                            model.name = tocapitalize + " Upgrade";
                            model.upgrade = landPlotShopEntry.upgrade;
                            model.description = landPlotShopEntry.DescKey; 
                            model.title = landPlotShopEntry.NameKey;
                            model.icon = landPlotShopEntry.icon;
                            model.pediaEntry = landPlotShopEntry.pediaEntry;
                            model.promptMessage = plotPurchaseItemModel.promptMessage;
                            var purchaseCost = PurchaseCost.CreateEmpty();
                            purchaseCost.newbuckCost = landPlotShopEntry.cost;
                            model.purchaseCost = purchaseCost;
                            model.actionButtonLabel = plotPurchaseItemModel.actionButtonLabel;
                            model.infoButtonLabel = plotPurchaseItemModel.infoButtonLabel;
                            model.IsAvailable = landPlotShopEntry.isAvailable;
                            model.IsHidden = landPlotShopEntry.isHidden;
                        }));

                }
              
            }
        }
    }
}

