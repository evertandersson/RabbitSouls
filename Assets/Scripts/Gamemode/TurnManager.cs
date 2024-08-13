using Debug;
using Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamemode
{
    // Singleton
    public class TurnManager : MonoBehaviour {
        private static TurnManager instance;

        // TAGS ARE TEMPORARY. WILL REPLACE WITH LOOKING FOR COMPONENT TYPES (EnemyController/PlayerController) //Kim
        public const string PlayerTag = "Player";
        public const string EnemyTag = "Enemy";

        private Dictionary<string, int> levelTurnDict;
        private string currentActorTag = string.Empty;

        public static event Action<string> OnTurnStarted;

        [SerializeField] private bool playerStarts = true;

        public static string CurrentActorTag => instance.currentActorTag;

        // not turning this into a method is ugly,
        // but it means I dont have to add () to all the calls.
        // there are too many for me to want to.
        public static int TurnsTakenThisLevel {
            get {
                if(instance.levelTurnDict.TryGetValue(SceneController.CurrentLevel, out int value)) {
                    return value;
                }

                instance.levelTurnDict.Add(SceneController.CurrentLevel, 0);

                return 0;
            }
        }

        public static int TurnsTakenTotal() {
            KeyValuePair<string, int>[] turnsPerScenePairs = instance.levelTurnDict.ToArray();
            int totalTurns = 0;
            for (int i = 0; i < turnsPerScenePairs.Length; i++) {
                totalTurns += turnsPerScenePairs[i].Value;
            }

            return totalTurns;
        }

        public static TurnManager Instantiate() {
            return new GameObject("Turn Manager").AddComponent<TurnManager>();
        }

        private void Awake() {
            if(!instance) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if(instance != this) {
                Destroy(gameObject);
                return;
            }

            // do any initialization here
            currentActorTag = StartingActor;
        }
        private string StartingActor => playerStarts ? PlayerTag : EnemyTag;

        private void Start() {
            levelTurnDict = new Dictionary<string, int> { { SceneController.CurrentLevel, 0 } };
            SceneManager.sceneLoaded += OnSceneLoaded;
            StartNextTurn();
        }

        private void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void StartNextTurn() {
            OnTurnStarted?.Invoke(CurrentActorTag);
            SafeLogger.Log($"Started turn for {CurrentActorTag}");
        }

        private bool TryChangeTurn(string actorTag) {
            switch(actorTag) {
                case PlayerTag:
                    IncrementTurns();
                    currentActorTag = EnemyTag;
                    break;
                case EnemyTag:
                    currentActorTag = PlayerTag;
                    break;
                default:
                    SafeLogger.LogError($"Tried to end turn with invalid tag '{actorTag}'");
                    return false;
            }

            return true;
        }
        private void IncrementTurns() {
            int turnsTaken = levelTurnDict[SceneController.CurrentLevel]++;
            SafeLogger.Log($"Turns taken: {levelTurnDict[SceneController.CurrentLevel]}");
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (mode.Equals(LoadSceneMode.Additive))
                return;

            bool sceneAdded = levelTurnDict.TryAdd(scene.name, 0);
            if(!sceneAdded)
                levelTurnDict[scene.name] = 0;

            currentActorTag = StartingActor; 
        }

        public static void EndTurn(string actorTag) {
            if (!instance.TryChangeTurn(actorTag))
                return;

            instance.StartNextTurn();
        }
    }
}
