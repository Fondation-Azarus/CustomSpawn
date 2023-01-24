using HarmonyLib;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerRoles.PlayableScps.Scp049.Zombies;
using PluginAPI.Core;


namespace CustomSpawn.Patches
{
    [HarmonyPatch(typeof(DynamicHumeShieldController))]
    public static class DynamicHumeShieldControllerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(DynamicHumeShieldController.HsMax), MethodType.Getter)] // Thanks Axwabo#2534
        public static bool PrefixGetHsMax(ref float __result, DynamicHumeShieldController __instance)
        {
            if (__instance?.Owner == null || Player.Get(__instance.Owner) == null || !PluginClass.Singleton.config.SpawnConfigs.TryGetValue(Player.Get(__instance.Owner).Role, out Config.SpawnConfig spawnConfig) || spawnConfig.MaxHumeShield == default)
                return true;

            __result = spawnConfig.MaxHumeShield;
            return false;
        }
    }

    [HarmonyPatch(typeof(ZombieShieldController))]
    public static class ZombieShieldControllerPatch
    {
        [HarmonyPatch(nameof(ZombieShieldController.HsMax), MethodType.Getter)]
        public static bool Prefix(ref float __result, ZombieShieldController __instance)
        {
            if (__instance?.Owner == null || Player.Get(__instance.Owner) == null || !PluginClass.Singleton.config.SpawnConfigs.TryGetValue(Player.Get(__instance.Owner).Role, out Config.SpawnConfig spawnConfig) || spawnConfig.MaxHumeShield == default)
                return true;

            __result = spawnConfig.MaxHumeShield;
            return false;
        }
    }

    /*private static bool TryGetMaxHS(ReferenceHub hub, out float maxHS)
    {
        maxHS = 0;
        if (hub == null || Player.Get(hub) == null || !PluginClass.Singleton.config.SpawnConfigs.TryGetValue(Player.Get(hub).Role, out Config.SpawnConfig spawnConfig) || spawnConfig.MaxHumeShield == default)
            return false;
        maxHS = spawnConfig.MaxHumeShield;
        return true;
    }*/

}