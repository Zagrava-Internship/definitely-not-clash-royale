using Targeting;

namespace Combat.Interfaces
{
    public interface IAttacker:ITargetable
    {
        int Damage { get; }
    }
}