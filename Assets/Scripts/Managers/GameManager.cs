using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public UnitSpawner unitSpawner;
        public UnitData knightData;
        public UnitData miniPekkaData;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                unitSpawner.Spawn(miniPekkaData);
            }
        }
    }
}