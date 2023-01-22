using HarmonyLib;
using PlayerStatsSystem;
using PluginAPI.Core;

namespace CustomSpawn.Patches
{
    [HarmonyPatch(typeof(HealthStat))]
    public static class HealthStatPatch
    {
        [HarmonyPatch(nameof(HealthStat.MaxValue), MethodType.Getter)] // thanks GBN#1862 for your help !
        public static bool Prefix(ref float __result, HealthStat __instance)
        {
            if (__instance?.Hub == null || Player.Get(__instance.Hub) == null || !PluginClass.Singleton.config.SpawnConfigs.TryGetValue(Player.Get(__instance.Hub).Role, out Config.SpawnConfig spawnConfig) || spawnConfig.MaxHealth == default)
                return true;

            __result = spawnConfig.MaxHealth;
            return false;
        }
    }
}