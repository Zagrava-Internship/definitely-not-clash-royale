using Targeting;

namespace Combat.Interfaces
{
    public interface IAttacker:ITargetable
    {
        int Damage { get; }
        float AttackRange { get; }
        float AttackDelay { get; }
        void OnTargetAcquired(ITargetable target);
        void OnTargetLost(ITargetable target);
    }
}