using HarmonyLib;

namespace MelonSRML.Patches;

// Token: 0x02000003 RID: 3
[HarmonyPatch(typeof(VacuumItem), nameof(VacuumItem.Expel), typeof(GameObject), typeof(bool), typeof(float), typeof(SlimeAppearance.AppearanceSaveSet))]
internal class WeaponVacuumExpelPatch
{
    // Token: 0x06000003 RID: 3 RVA: 0x000020E0 File Offset: 0x000002E0
    public static void Prefix(VacuumItem __instance, ref bool ignoreEmotions)
    {
        if (__instance._player.Ammo.GetSelectedEmotions() == null)
        {
            ignoreEmotions = true;
        }
    }
}