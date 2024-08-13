using Gamemode;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public class MoveIndicator : MonoBehaviour {
        [SerializeField] private GameObject indicatorPrefab;

        private Actor actor;
        private Transform indicatorParent;
        private List<GameObject> indicators;

        private void Awake() {
            actor = GetComponent<Actor>();
            actor.OnStartedMove += OnActorStartedMove;
            TurnManager.OnTurnStarted += OnTurnStarted;

            SpawnIndicators();
        }

        private void OnDestroy() {
            actor.OnStartedMove -= OnActorStartedMove;
            TurnManager.OnTurnStarted -= OnTurnStarted;
        }
        private void OnActorStartedMove() {
            HideIndicators();
        }
        private void OnTurnStarted(string actorTag) {
            if(!actorTag.Equals(TurnManager.PlayerTag)) {
                return;
            }

            DisplayValidIndicators();
        }

        private void Start() {
            DisplayValidIndicators();
        }

        public void HideIndicators() {
            for(int i = 0; i < indicators.Count; i++) {
                indicators[i].SetActive(false);
            }
        }

        private void SpawnIndicators() {
            int range = actor.NodesPerMove;

            if(indicatorParent == null) {
                indicatorParent = new GameObject("Indicator Parent").transform;
                indicatorParent.SetParent(transform);
                indicatorParent.localPosition = Vector3.zero;
            }

            indicators = new List<GameObject>();

            for(int y = -range; y <= range; y++) {
                for(int x = -range; x <= range; x++) {
                    GameObject indicator = Instantiate(indicatorPrefab, transform.position + new Vector3(x, 0.01f, y), indicatorPrefab.transform.rotation, indicatorParent);
                    indicators.Add(indicator);
                }
            }
        }
        public void DisplayValidIndicators() {
            for(int i = 0; i < indicators.Count; i++) {
                GameObject indicator = indicators[i];
                Vector3Int indicatorIntPos = Vector3Int.RoundToInt(indicator.transform.position);

                bool pathBlocked = !actor.TryGeneratePath(indicatorIntPos);
                bool indicatorOutsideRange = actor.NodeDistanceToTarget(indicatorIntPos) > actor.NodesPerMove;
                bool positionIsBlocked = actor.CheckPositionIsBlocked(indicatorIntPos);

                bool active = !(pathBlocked || indicatorOutsideRange || positionIsBlocked);

                indicator.SetActive(active);
            }
        }
    }
}
