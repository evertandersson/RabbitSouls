using Actors.Controllers;
using Debug;
using Gamemode;
using System.Linq;
using UnityEngine;

namespace Actors
{
    public class EnemyManager : MonoBehaviour {
        private static EnemyController[] enemies;
        private static int runningPatrols;

        public static void EndPatrol(EnemyController enemy) {
            if(!enemies.Contains(enemy)) {
                SafeLogger.LogError("Enemy that wasn't registered ended its patrol", enemy);
                return;
            }

            runningPatrols--;

            SafeLogger.Log($"{enemy} ended its patrol. ({runningPatrols} patrols still running)");

            if(runningPatrols == 0) {
                TurnManager.EndTurn(TurnManager.EnemyTag);
            }
        }

        private void Awake() {
            enemies = FindObjectsByType<EnemyController>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);

            TurnManager.OnTurnStarted += OnTurnStarted;
        }

        private void OnDestroy() {
            TurnManager.OnTurnStarted -= OnTurnStarted;
            runningPatrols = 0;
            enemies = null;
        }

        private void OnTurnStarted(string actorTag) {
            if(!actorTag.Equals(TurnManager.EnemyTag)) {
                return;
            }

            runningPatrols = enemies.Length;

            for(int i = 0; i < enemies.Length; i++) {
                enemies[i].Patrol();
            }
        }
    }
}
