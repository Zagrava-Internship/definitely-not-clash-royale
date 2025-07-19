using Targeting;

namespace Combat
{
    public interface IWeapon
    {
        public int Damage { get; set; }
        public float AttackRange{ get; set; }
        public float AttackSpeed{ get; set; }// Animation speed multiplier
        public float AttackDelay{ get; set; } // Delay before the next attack can be initiated
    }
}