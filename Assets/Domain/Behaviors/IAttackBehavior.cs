using DefinitelyNotClashRoyale.Domain.Entities;

namespace DefinitelyNotClashRoyale.Domain.Behaviors
{
    public interface IAttackBehavior
    {
        bool CanAttack(Unit self, Unit target);
        int GetDamage(Unit self, Unit target);

        void Attack(Unit self, Unit target)
        {
            if (!CanAttack(self, target)) return;
            target.TakeDamage(GetDamage(self, target));
        }
    }
}