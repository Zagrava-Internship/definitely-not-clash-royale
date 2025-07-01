using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public UnitSpawner unitSpawner;
        public UnitData knightData;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                unitSpawner.Spawn(knightData);
            }
        }
    }
}