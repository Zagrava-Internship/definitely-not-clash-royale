using Targeting;

namespace Combat.Interfaces
{
    public interface IAttacker:ITargetable
    {
        int AttackerDamage { get; }
        float AttackerRange { get; }
        float AttackerDelay { get; }
        ITargetable AttackerCurrentTarget { get; }
        void OnTargetAcquired(ITargetable target);
        void OnTargetLost(ITargetable target);
    }
}