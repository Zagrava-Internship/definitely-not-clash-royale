using System;
using System.Collections.Generic;

namespace DefinitelyNotClashRoyale.Domain.Entities
{
    public class GameState
    {
        public List<Unit> Units { get; } = new();
        public event Action<Unit>? OnUnitSpawned;
        public void SpawnUnit(Unit unit)
        {
            Units.Add(unit);
            OnUnitSpawned?.Invoke(unit);
        }
    }
}
