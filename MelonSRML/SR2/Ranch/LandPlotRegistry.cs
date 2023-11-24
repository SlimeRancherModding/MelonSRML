using System;
using System.Collections.Generic;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using MelonSRML.Utils.Extensions;
using UnityEngine.Localization;

namespace MelonSRML.SR2.Ranch
{
    public static class LandPlotRegistry
    {
        internal static readonly List<LandPlotShopEntry> moddedLandPlotShopEntries = new List<LandPlotShopEntry>();
        public struct LandPlotShopEntry
        {
            public LandPlot.Id plot;
            public Sprite icon;
            public int cost;
            public PediaEntry pediaEntry;
            public Func<bool> isUnlocked;
            public Func<bool> isHidden;


            public LocalizedString NameKey;
            public LocalizedString DescKey;
        }
        
       

        public static void RegisterPurchasableLandPlot(LandPlotShopEntry entry, GameObject prefab)
        {
            SRSingleton<GameContext>.Instance.LookupDirector.plotPrefabs.AddAndRemoveRangeWhere(new[] { prefab }, (o, gameObject) => o.GetComponentInChildren<LandPlot>().TypeId == gameObject.GetComponentInChildren<LandPlot>().TypeId);
            var typeId = prefab.GetComponentInChildren<LandPlot>().TypeId;
            if (SRSingleton<GameContext>.Instance.LookupDirector.plotPrefabDict.ContainsKey(typeId))
            {
                SRSingleton<GameContext>.Instance.LookupDirector.plotPrefabDict.Remove(typeId);
            }

            SRSingleton<GameContext>.Instance.LookupDirector.plotPrefabDict.Add(typeId, prefab);
            moddedLandPlotShopEntries.Add(entry);
        }
        
    }
}
