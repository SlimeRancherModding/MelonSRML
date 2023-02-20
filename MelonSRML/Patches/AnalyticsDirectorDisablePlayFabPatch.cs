using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.Analytics;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(AnalyticsDirector), "Init")]
    internal static class AnalyticsDirectorDisablePlayFabPatch
    {
        public static bool Prefix() => false;
    }

    [HarmonyPatch(typeof(AnalyticsDirector), "SendPlayerEvent")]
    internal static class AnalyticsDirectorDisablePlayFabWarningsPatch
    {
        public static bool Prefix() => false;
    }
}
