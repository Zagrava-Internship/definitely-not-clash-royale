using System;
using System.Collections.Generic;
using Units.Enums;
using Units.Strategies.Movement;
using UnityEngine;

namespace Units.Factories
{
    /// <summary>
    /// Factory for attaching a movement strategy to a GameObject based on MovementType.
    /// </summary>
    public static  class MovementFactory
    {
        private static readonly Dictionary<MovementType, Func<GameObject, IMovementStrategy>> Map =
            new()
            {
                { MovementType.Ground,  go => go.AddComponent<GroundMovement>()  }, 
                { MovementType.Flying,  go => go.AddComponent<FlyingMovement>()  },
                { MovementType.Static,  go => go.AddComponent<StaticMovement>()  }
            };
        
        public static void AddMovement(GameObject go, MovementType type)
        {
            if (!Map.TryGetValue(type, out var creator))
                throw new ArgumentException($"MovementFactory: No strategy registered for {type}"); 
            creator(go);
        }
    }
}