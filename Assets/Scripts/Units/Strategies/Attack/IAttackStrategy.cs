using Combat.Interfaces;
using Targeting;

namespace Units.Strategies.Attack
{
    public interface IAttackStrategy
    {
        float Range { get; }
        float AttackDelay { get; }
        void Attack(IAttacker attacker, ITargetable target);
    }
}