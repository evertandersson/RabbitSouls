using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamemode
{
    // Singleton
    public class SceneController : MonoBehaviour {

        private static SceneController instance;

        [SerializeField] private bool loadMainMenuOnStart = false;
        [SerializeField] private string mainMenuScene = "main_menu";
        [SerializeField] private string gameOverScene = "game_over";
        [SerializeField] private string hudScene = "add_hud";
        [SerializeField] private string[] levelScenes;

        private int currentLevelIndex = -1;

        public static string CurrentLevel => instance.levelScenes[instance.currentLevelIndex];

        private void Awake() {
            if(instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if(instance != this) {
                Destroy(gameObject);
                return;
            }

            // do any initialization here
            if(instance.levelScenes.Length == 0) {
                instance.levelScenes = new[] { SceneManager.GetActiveScene().name };
                currentLevelIndex = 0;
            }
        }

        private void Start() {
            if (loadMainMenuOnStart)
                LoadMainMenu();
        }

        // leave public - reuse for restarting level. //Kim
        public static void LoadCurrentLevel() {
            SceneManager.LoadScene(CurrentLevel);
        }

        public static void LoadFirstLevel() {
            instance.currentLevelIndex = 0;
            SceneManager.LoadScene(instance.hudScene, LoadSceneMode.Additive);
            LoadCurrentLevel();
            TurnManager.Instantiate();
        }

        public static void LoadNextLevel() {

            if(!instance) {
                UnityEngine.Debug.LogError("Scene Controller was asked to load next level without instance!");
            }

            instance.currentLevelIndex++;
            if(instance.currentLevelIndex >= instance.levelScenes.Length) {
                SceneManager.LoadScene(instance.gameOverScene);
                return;
            }

            LoadCurrentLevel();
        }

        public static void LoadMainMenu() {
            SceneManager.LoadScene(instance.mainMenuScene);
        }
    }
}
