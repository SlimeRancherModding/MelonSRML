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
            var landPlotTypeId = __instance.landPlot.TypeId;
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
                             model._description = landPlotShopEntry.DescKey;
                             model._title =  landPlotShopEntry.NameKey;
                             model._icon = landPlotShopEntry.icon;
                             var purchaseCost = PurchaseCost.CreateEmpty();
                             purchaseCost.newbuckCost = landPlotShopEntry.cost;
                             model._purchaseCost = purchaseCost;
                             model.IsAvailable = landPlotShopEntry.isUnlocked;
                             model.IsHidden = landPlotShopEntry.isHidden;
                             model._pediaEntry = landPlotShopEntry.pediaEntry;
                             model._actionButtonLabel = fromIndexInList.ActionButtonLabel;
                             model._infoButtonLabel = fromIndexInList.InfoButtonLabel;
                             model._promptMessage = fromIndexInList.PromptMessage;
                             model._plotDefinition = ScriptableObjectUtils.CreateScriptable<PlotDefinition>(definition =>definition.name = landPlotShopEntry.plot.ToString().FirstCharToUpper());
                             model._plotPrefab = SRSingleton<GameContext>.Instance.LookupDirector.GetPlotPrefab(landPlotShopEntry.plot);
                        
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
                            model._upgrade = landPlotShopEntry.upgrade;
                            model._description = landPlotShopEntry.DescKey; 
                            model._title = landPlotShopEntry.NameKey;
                            model._icon = landPlotShopEntry.icon;
                            model._pediaEntry = landPlotShopEntry.pediaEntry;
                            model._promptMessage = plotPurchaseItemModel.PromptMessage;
                            var purchaseCost = PurchaseCost.CreateEmpty();
                            purchaseCost.newbuckCost = landPlotShopEntry.cost;
                            model._purchaseCost = purchaseCost;
                            model._actionButtonLabel = plotPurchaseItemModel.ActionButtonLabel;
                            model._infoButtonLabel = plotPurchaseItemModel.InfoButtonLabel;
                            model.IsAvailable = landPlotShopEntry.isAvailable;
                            model.IsHidden = landPlotShopEntry.isHidden;
                        }));

                }
              
            }
        }
    }
}

