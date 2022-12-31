using HarmonyLib;
using Il2CppSystem.Collections.Generic;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(AnalyticsUtil), nameof(AnalyticsUtil.ReportPerIdentifiableData))]
    internal static class AnalyticsUtilReportPerIdentifiableDataPatch
    {
        public static bool Prefix(IEnumerable<IdentifiableType> ids) => false;

    }
}

