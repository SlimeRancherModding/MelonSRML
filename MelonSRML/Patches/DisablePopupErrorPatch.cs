using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.ErrorHandling;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(PopupErrorHandler), "HandleErrorWithUserResolution")]
    internal static class DisablePopupErrorPatch
    {
        // If the game ever encounters certain errors (e.g., missing translations), it immediately quits a save and/or restarts the game.
        // This is a stupid behavior that will cause issues, so we're disabling that.
        public static bool Prefix() => false;
    }
}
