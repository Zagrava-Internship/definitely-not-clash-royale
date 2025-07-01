using System;
using DefinitelyNotClashRoyale.Domain.Enums;

namespace DefinitelyNotClashRoyale.Domain.Entities
{
    public class Unit
    {
        public int Id { get; }
        public int Health { get; private set; }
        public bool IsAlive => Health > 0 && Status != UnitStatus.Dead;
        public UnitType Type { get; }
        public UnitStatus Status { get; private set; }
        public Unit(int id, int health, UnitType type)
        {
            Id = id;
            Health = health;
            Type = type;
            Status = UnitStatus.Idle;
        }

        public void TakeDamage(int amount)
        {
            if (Status == UnitStatus.Dead) return;

            Health = Math.Max(0, Health - amount);
            if (Health == 0)
                Status = UnitStatus.Dead;
        }
        
        public void SetStatus(UnitStatus status)
        {
            if (Status == UnitStatus.Dead) return;
            Status = status;
            if (Health == 0)
                Status = UnitStatus.Dead;

        }
    }
}
