using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Events;
using PlayerRoles;
using PlayerStatsSystem;
using System.Linq;
using MEC;
using HarmonyLib;
using PlayerRoles.PlayableScps.HumeShield;

namespace CustomSpawn
{
    public class PluginClass
    {
        private Harmony harmony;

        public static PluginClass Singleton { get; private set; }

        [PluginPriority(LoadPriority.Low)]
        [PluginEntryPoint("Custom Spawn", "1.0.0", "Change the spawn setting of classes.", "Bonjemus")]
        void LoadPlugin()
        {
            Singleton = this;

            harmony = new Harmony("Patches");
            harmony.PatchAll();

            EventManager.RegisterEvents(this);
            PluginHandler eventHandler = PluginHandler.Get(this);
            eventHandler.SaveConfig(this, nameof(config));
        }

        [PluginConfig("configs/CustomSpawn.yml")]
        public Config config;

        [PluginEvent(ServerEventType.PlayerSpawn)]
        void OnSpawn(Player player, RoleTypeId role)
        {
            try
            {
                Timing.CallDelayed(0.5f, () =>
                {
                    if (player == null || !config.SpawnConfigs.TryGetValue(player.Role, out Config.SpawnConfig spawnConfig))
                        return;

                    if (spawnConfig.Health != -1)
                        player.Health = spawnConfig.Health;

                    if (((HumeShieldStat)player.ReferenceHub.playerStats.StatModules[4]).TryGetHsModule(out HumeShieldModuleBase controller) && controller is DynamicHumeShieldController DHSC)
                    {
                        if (spawnConfig.HumeShield != -1)
                            DHSC.HsCurrent = spawnConfig.HumeShield;

                        if (spawnConfig.HumeShieldRegen != -1)
                            DHSC.RegenerationRate = spawnConfig.HumeShieldRegen;

                        if (spawnConfig.HumeShieldRegenCooldown != -1)
                            DHSC.RegenerationCooldown = spawnConfig.HumeShieldRegenCooldown;
                    }

                    if (spawnConfig.Effects != null && spawnConfig.Effects.Count > 0)
                        foreach (Config.EffectConfig effectConfig in spawnConfig.Effects)
                            player.EffectsManager.ChangeState(effectConfig.EffectName, effectConfig.Intensity, effectConfig.Duration, effectConfig.Add);


                    if (spawnConfig.AhpProcesses == null || spawnConfig.AhpProcesses.Count == 0)
                        return;

                    if (player.ReferenceHub.playerStats.StatModules[1] is AhpStat ahpStat)
                    {
                        for (int i = 0; i < spawnConfig.AhpProcesses.Count; i++)
                        {
                            Config.AhpConfig ahpConfig = spawnConfig.AhpProcesses[i];
                            AhpStat.AhpProcess ahpProcess = ahpStat._activeProcesses.ElementAtOrDefault(i);

                            if (ahpProcess != null)
                                ahpProcess = new AhpStat.AhpProcess(ahpConfig.CurrentAmount, ahpConfig.Limit, ahpConfig.DecayRate, ahpConfig.Efficacy, ahpConfig.SustainTime, ahpConfig.Persistant);
                            else
                                ahpStat.ServerAddProcess(ahpConfig.CurrentAmount, ahpConfig.Limit, ahpConfig.DecayRate, ahpConfig.Efficacy, ahpConfig.SustainTime, ahpConfig.Persistant);
                        }
                    }
                });
            }
            catch (System.Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }

    /*public class CustomHealthStat : HealthStat
    {
        public override float MaxValue => CustomMaxValue == default ? base.MaxValue : CustomMaxValue; // Thanks ced777ric for the idea !
        public float CustomMaxValue { get; set; }

        //public override float MinValue => default ? base.MinValue : CustomMinValue; // curious to know what will happen
        //public float CustomMinValue { get; set; }
    }*/

    /*public class CustomHumeShieldStat : HumeShieldStat
    {
        public override float MaxValue => CustomMaxValue == default ? base.MaxValue : CustomMaxValue;
        public float CustomMaxValue { get; set; }

        public float CustomHsRegneration { get; set; }

        public override void Update()
        {
            base.Update();
            Log.Debug("updated base method");
            if (!NetworkServer.active || CustomHsRegneration == 0f)
                return;

            float num = CustomHsRegneration * Time.deltaTime;
            if (num > 0f)
            {
                if (CurValue < MaxValue)
                    CurValue = Mathf.MoveTowards(CurValue, MaxValue, num);
            }

            else if (CurValue > 0f)
                CurValue += num;
        }
    }*/

    /*public class CustomStaminaStat : StaminaStat
    {
        public override float MaxValue => CustomMaxValue == default ? base.MaxValue : CustomMaxValue;
        public float CustomMaxValue { get; set; }

        //public override float MinValue => default ? base.MinValue : CustomMinValue; // curious to know what will happen
        //public float CustomMinValue { get; set; }
    }*/
}