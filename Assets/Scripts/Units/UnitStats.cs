using UnityEngine;

namespace Units
{
    public class UnitStats:MonoBehaviour
    {
        [SerializeField] private UnitConfig _config;
        public UnitConfig Config => _config;
        
        public int MaxHealth => _config.health;
        public float Speed => _config.speed;
        public int Damage => _config.weaponData.Damage;
        public float AttackRange => _config.weaponData.AttackRange;
        public float AttackSpeed => _config.weaponData.AttackSpeed;
        public float AttackDelay => _config.weaponData.AttackDelay;
        public float AggressionRange => _config.aggressionRange;

        public void InitializeStats(UnitConfig unitConfig)
        {
            if (!unitConfig)
            {
                throw new System.ArgumentNullException(nameof(unitConfig), "UnitStats.InitializeStats: UnitConfig is null");
            }
            _config = unitConfig;
        }
    }
}