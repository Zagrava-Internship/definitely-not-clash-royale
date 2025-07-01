using System;
using DefinitelyNotClashRoyale.Domain.Behaviors;

namespace DefinitelyNotClashRoyale.Domain.Entities
{
    public class Unit
    {
        public Guid  Id { get; }
        public int Health { get; private set; }
        public bool IsAlive => Health > 0;
        public UnitType Type { get; }
        public Unit(Guid id, int health, UnitType type)
        {
            Id = id;
            Health = health;
            Type = type;
        }

        public void TakeDamage(int amount)
        {
            Health = Math.Max(0, Health - amount);
        }
    }
}
