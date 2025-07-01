using DefinitelyNotClashRoyale.Domain.Entities;
using DefinitelyNotClashRoyale.Domain.Enums;

namespace DefinitelyNotClashRoyale.Domain.Behaviors
{
    public class MeleeAttackBehavior: IAttackBehavior
    {
        private readonly int _damage;

        public MeleeAttackBehavior(int damage)
        {
            _damage = damage;
        }

        public bool CanAttack(Unit self, Unit target)
        {
            if (self == null || target == null) return false;

            if (self.Status is UnitStatus.Attacking or UnitStatus.Stunned or UnitStatus.Dead)
                return false;

            if (target.Status == UnitStatus.Dead)
                return false;

            return target.Type == UnitType.Ground;
        }

        public int GetDamage(Unit self, Unit target)
        {
            return _damage;
        }
    }
}