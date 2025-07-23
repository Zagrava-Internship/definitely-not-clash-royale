using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Deck;
using Mana;
using Maps.MapManagement.Grid;
using Spawners;
using Targeting;
using UnityEngine;

namespace AI
{
    public class BotPlayerController : MonoBehaviour
    {
        [Header("AI Settings")] [SerializeField]
        private float decisionInterval = 1f;

        [SerializeField] private Team myTeam = Team.Team2;

        [Header("References")] [SerializeField]
        private DeckController deckController;

        [SerializeField] private MonoBehaviour manaProvider;
        [SerializeField] private MonoBehaviour manaSpenderProvider;

        private IManaReadOnly _mana;
        private IManaSpender _manaSpender;
        private Coroutine _aiLoop;

        private void Awake()
        {
            if (deckController == null)
                throw new System.InvalidOperationException("DeckController is not assigned");

            _mana = manaProvider as IManaReadOnly
                    ?? throw new System.InvalidOperationException("manaProvider does not implement IManaReadOnly");

            _manaSpender = manaSpenderProvider as IManaSpender
                           ?? throw new System.InvalidOperationException(
                               "manaSpenderProvider does not implement IManaSpender");

        }

        private void OnEnable()
        {
            _aiLoop = StartCoroutine(AILoop());
        }

        private void OnDisable()
        {
            if (_aiLoop != null) StopCoroutine(_aiLoop);
        }

        private IEnumerator AILoop()
        {
            var wait = new WaitForSeconds(decisionInterval);
            while (true)
            {
                TryPlayCard();
                yield return wait;
            }
        }

        private void TryPlayCard()
        {
            var hand = deckController.CurrentHand;

            var playable = hand.Where(c => _mana.CanSpend(c.Cost)).ToList();

            var bestCard = playable
                .OrderByDescending(c => c.Cost)
                .FirstOrDefault();

            if (bestCard == null)
            {
                return;
            }


            var spawnPos = GetRandomSpawnPosition();
            if (!spawnPos.HasValue)
            {
                return;
            }


            UnitSpawner.Spawn(bestCard.UnitToSpawn, spawnPos.Value, myTeam);
            _manaSpender.Spend(bestCard.Cost);

            int index = hand.IndexOf(bestCard);
            if (index >= 0)
            {
                deckController.PlayCard(index);
            }
        }

        private Vector3? GetRandomSpawnPosition()
        {
            var gm = GridManager.Instance;
            if (gm == null)
            {
                return null;
            }

            var settings = gm.gridSettingsData;

            var candidates = new List<GridNode>();
            var halfY = (int)settings.height / 2;

            for (var x = 0; x < settings.width; ++x)
            {
                for (var y = 0; y < settings.height; ++y)
                {
                    var isOnMySide = myTeam == Team.Team1
                        ? y < halfY
                        : y >= halfY;
                    if (!isOnMySide)
                        continue;

                    var worldPos = settings.originPosition
                                   + new Vector3(x * settings.cellWidth, y * settings.cellHeight, 0f);
                    var node = gm.GetNodeFromWorldPoint(worldPos);
                    if (node == null)
                    {
                        continue;
                    }

                    candidates.Add(node);
                }
            }
            
            if (candidates.Count == 0)
            {
                return null;
            }

            var chosen = candidates[Random.Range(0, candidates.Count)];
            return chosen.WorldPosition;
        }
    }
}