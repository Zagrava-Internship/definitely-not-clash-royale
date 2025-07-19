using System;
using System.Collections.Generic;
using Combat;
using Units.Enums;
using Units.Strategies.Attack;
using UnityEngine;

namespace Units.Factories
{
    /// <summary>
    /// Factory for attaching an attack strategy to a GameObject based on AttackType.
    /// </summary>
    public static class AttackFactory
    {
        private static readonly Dictionary<AttackType, Func<GameObject, UnitConfig, IAttackStrategy>> Map =
            new()
            {
                { AttackType.Melee, (go, _) => go.AddComponent<MeleeAttackStrategy>() },
                {
                    AttackType.Ranged, (go, cfg) =>
                    {
                        var strat = go.AddComponent<ParticleAttackStrategy>();
                        if (cfg.weaponData is RangedWeaponData rangedWeaponData)
                            strat.SetProjectilePrefab(rangedWeaponData.particlePrefab);
                        return strat;
                    }
                },
            };
        
        public static void AddAttack(GameObject go, AttackType type, UnitConfig cfg)
        {
            if (!Map.TryGetValue(type, out var creator))
                throw new ArgumentException($"AttackFactory: No strategy registered for {type}"); 
            creator(go, cfg);
        }
    }
}