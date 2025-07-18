using Targeting;

namespace Units.Strategies
{
    public interface IAttackStrategy
    {
        float Range { get; }
        float AttackDelay { get; }
        void Attack(Unit unit, ITargetable target);
    }
}