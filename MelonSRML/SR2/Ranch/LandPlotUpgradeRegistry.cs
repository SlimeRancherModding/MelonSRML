using System;
using System.Collections.Generic;
using UnityEngine.Localization;

namespace MelonSRML.SR2.Ranch
{
    public static class LandPlotUpgradeRegistry
    {
        public struct UpgradeShopEntry
        {
            public LandPlot.Upgrade upgrade;
            public Sprite icon;
            public int cost;
            public PediaEntry pediaEntry;
            public Func<bool> isAvailable;
            public Func<bool> isHidden;


            private string landplotName;

            public string LandPlotName
            {
                get => landplotName ?? upgrade.ToString().ToLower();
                set => landplotName = value.ToLower();
            }

            public LocalizedString DescKey;
            public LocalizedString NameKey;
        }

        internal static Dictionary<LandPlot.Id, List<UpgradeShopEntry>> moddedUpgradeShopEntry = new Dictionary<LandPlot.Id, List<UpgradeShopEntry>>();
        public static void RegisterPurchasableUpgrade(UpgradeShopEntry entry, LandPlot.Id landPlotId)
        {
            if (moddedUpgradeShopEntry.TryGetValue(landPlotId, out var list))
            {
                list.Add(entry);
            }
            else
            {
                moddedUpgradeShopEntry.Add(landPlotId, new List<UpgradeShopEntry>{entry});
            }
        }
        public static void RegisterPlotUpgrader<T>(LandPlot.Id plot) where T :  ModdedPlotUpgrader
        {
            GameContext.Instance.LookupDirector.GetPlotPrefab(plot).AddComponent<T>();
        }
    }
    
}

