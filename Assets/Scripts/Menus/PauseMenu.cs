using Gamemode;
using Menu;
using Menus;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool gameIsPaused = false;

        public GameObject pauseMenuUI;
        public GameObject pauseButtonUI;
        public GameObject optionsMenuUI;
        public GameObject restartMenuUI;

        private MainMenu mainMenu;
        private DisplayRound displayRound;

        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }

        void Start() {
            displayRound = FindObjectOfType<DisplayRound>();
            SceneManager.sceneLoaded += SetDisplayRound;
        }

        void SetDisplayRound(Scene scene, LoadSceneMode mode) {
            displayRound = FindObjectOfType<DisplayRound>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (gameIsPaused) ResumeGame();
                else PauseGame();
            }
        }

        public void PauseGame()
        {
            pauseMenuUI.SetActive(true);
            displayRound.gameObject.SetActive(false);
            pauseButtonUI.SetActive(false);
            Time.timeScale = 0;
            gameIsPaused = true;
            EventSystem.current.SetSelectedGameObject(null);
        }
        public void ResumeGame()
        {
            pauseMenuUI.SetActive(false);
            displayRound.gameObject.SetActive(true);
            pauseButtonUI.SetActive(true);
            optionsMenuUI.SetActive(false);
            restartMenuUI.SetActive(false);
            Time.timeScale = 1;
            gameIsPaused = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
        public void RestartGame()
        {
            pauseMenuUI.SetActive(false);
            restartMenuUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
        }
        public void RestartConfirmed()
        {
            print("Restart Level");
            restartMenuUI.SetActive(false);
            pauseButtonUI.SetActive(true);
            displayRound.gameObject.SetActive(true);
            Time.timeScale = 1;
            gameIsPaused = false;
            EventSystem.current.SetSelectedGameObject(null);
            SceneController.LoadCurrentLevel();
        }
        public void MainMenu()
        {
            Time.timeScale = 1;
            gameIsPaused = false;
            SceneController.LoadMainMenu();
        }
        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
