using HarmonyLib;
using System.Linq;
using System.Reflection;

namespace MelonSRML.Patches
{
    [HarmonyPatch]
    public static class Il2cppDetourMethodPatcherReportExceptionPatch
    {
        public static MethodInfo TargetMethod()
        {
            Assembly assembly = AccessTools.AllAssemblies().FirstOrDefault(x => x.GetName().Name.Equals("Il2CppInterop.HarmonySupport"));
            var Il2CppDetourMethodPatcher = assembly.GetTypes().FirstOrDefault(x => x.Name == "Il2CppDetourMethodPatcher");
            return AccessTools.Method(Il2CppDetourMethodPatcher, "ReportException");
        }
        public static bool Prefix(System.Exception ex)
        {
            MelonLogger.Error("During invoking native->managed trampoline", ex);
            return false;
        }
    }
}
