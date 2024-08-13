using Debug;
using System.Linq;
using UnityEngine;
using Util;

namespace Actors.Controllers {
    public class EnemyController : MonoBehaviour {
        [SerializeField] private bool loop = false;
        [SerializeField] private Vector2Int[] moveTargets;
        private Actor actor;
        private int currentMoveTargetIndex;
        private int listDirection = 1;

        private void Awake() {
            actor = GetComponent<Actor>();
            actor.OnReachedDestination += EndPatrol;
        }

        private void Start() {
            moveTargets = moveTargets.Prepend(transform.position.XZToVector2Int()).ToArray();
            currentMoveTargetIndex++;
        }

        private void OnDestroy() {
            actor.OnReachedDestination -= EndPatrol;
        }

        public void Patrol() {

            if(currentMoveTargetIndex == moveTargets.Length) {
                if(loop) {
                    currentMoveTargetIndex = 0;
                }
                else {
                    listDirection = -1;
                    currentMoveTargetIndex -= 2;
                }
            }
            else if(currentMoveTargetIndex == -1) {
                listDirection = 1;
                currentMoveTargetIndex += 2;
            }

            Vector2Int moveTarget = moveTargets[currentMoveTargetIndex];
            Vector3 worldMoveTarget = new(moveTarget.x, 0, moveTarget.y);

            bool enemyNotOnMoveTarget = Vector3.Distance(worldMoveTarget, transform.position) > float.Epsilon;

            if(enemyNotOnMoveTarget) {
                if(actor.TryGeneratePath(new(moveTarget.x, 0, moveTarget.y), true)) {
                    actor.Move();
                }
                else {
                    SafeLogger.LogError($"Enemy could not go to moveTarget {moveTarget}");
                }
            }
            else {
                EndPatrol();
            }
        }

        private void EndPatrol() {
            Vector2Int moveTarget = moveTargets[currentMoveTargetIndex];
            Vector3 worldMoveTarget = new(moveTarget.x, 0, moveTarget.y);
            if(Vector3.Distance(worldMoveTarget, transform.position) < float.Epsilon) {
                currentMoveTargetIndex += listDirection;
            }

            EnemyManager.EndPatrol(this);
        }

        private void OnDrawGizmos() {
            for (int i = 0; i < moveTargets.Length; i++) {
                Vector2Int moveTarget = moveTargets[i];

                ColoredGizmos.DrawSphere(new(moveTarget.x, 0, moveTarget.y), 0.5f, Color.red);
            }
        }
    }
}
