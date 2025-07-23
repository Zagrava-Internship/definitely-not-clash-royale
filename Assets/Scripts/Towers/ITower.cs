using Targeting;
using Units;

namespace Towers
{
    public interface ITower
    {
        void InitializeTower(UnitConfig towerConfig, Team team);
    }
}