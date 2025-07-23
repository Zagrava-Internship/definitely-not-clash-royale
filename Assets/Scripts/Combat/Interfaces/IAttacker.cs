using Targeting;

namespace Combat.Interfaces
{
    public interface IAttacker:ITargetable
    {
        int AttackerDamage { get; }
        float AttackerRange { get; }
        float AttackerDelay { get; }

    }
}