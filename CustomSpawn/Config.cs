using PlayerRoles;
using System.Collections.Generic;
using System.ComponentModel;

namespace CustomSpawn
{
    public class Config
    {
        [Description("Enabled ? :")]
        public bool Enabled { get; set; } = true;

        [Description("List of spawn configurations :")]
        public Dictionary<RoleTypeId, SpawnConfig> SpawnConfigs { get; set; } = new Dictionary<RoleTypeId, SpawnConfig>()
        {
            { RoleTypeId.ClassD, new SpawnConfig { Health = 80, MaxHealth = 160, AhpProcesses = new List<AhpConfig>() { new AhpConfig { CurrentAmount = 20, DecayRate = 0, Efficacy = 1, Limit = 100 } } } },
            { RoleTypeId.Scientist, new SpawnConfig { Health = 120, MaxHealth = 130, MaxHumeShield = 10, AhpProcesses = new List<AhpConfig>() { new AhpConfig { CurrentAmount = 50, DecayRate = -10, Efficacy = 1, Limit = 100, SustainTime = 10 } }, Effects = new List<EffectConfig>(){ new EffectConfig(){ EffectName = "scp207", Add = false, Duration = 10, Intensity = 10 } } } },
            { RoleTypeId.Scp173, new SpawnConfig { MaxHumeShield = 500, HumeShieldRegen = 1, HumeShieldRegenCooldown = 5, AhpProcesses = new List<AhpConfig>() { new AhpConfig { CurrentAmount = 300, DecayRate = -10, Efficacy = 1, Limit = 1000 } } } }
        };

        public struct SpawnConfig
        {
            public float Health { get; set; }

            public float MaxHealth { get; set; }

            public List<AhpConfig> AhpProcesses { get; set; }

            public float MaxHumeShield { get; set; }

            public float HumeShield { get; set; }

            public float HumeShieldRegen { get; set; }

            public float HumeShieldRegenCooldown { get; set; }

            public List<EffectConfig> Effects { get; set; }
        }

        public struct AhpConfig
        {
            public float CurrentAmount { get; set; }
            public float Limit { get; set; }
            public float DecayRate { get; set; }
            public float Efficacy { get; set; }
            public float SustainTime { get; set; }
            public bool Persistant { get; set; }
        }

        public struct EffectConfig
        {
            public string EffectName { get; set; }
            public float Duration { get; set; }
            public byte Intensity { get; set; }
            public bool Add { get; set; }
        }
    }
}