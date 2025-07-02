using Cards;
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
        
        public void OnCardPlayed(CardData card)
        {
            if (card != null && card.unitToSpawn != null)
                unitSpawner.Spawn(card.unitToSpawn);
            else
                Debug.LogWarning("CardData или UnitData не назначены!");
        }

    }
}