using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using MelonSRML.Console;

namespace MelonSRML.Patches
{
    [HarmonyPatch(typeof(OptionsModel), "Push")]
    internal static class OptionsModelPushPatch
    {
        public static void Prefix() => KeyBindManager.Push();
    }
}