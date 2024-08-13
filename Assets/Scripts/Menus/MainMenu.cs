using Gamemode;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour {

        private void Awake() {
            CleanLastGameSession();
        }
        private void CleanLastGameSession() {
            PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
            if (pauseMenu != null)
                Destroy(pauseMenu.transform.root.gameObject);

            TurnManager turnManager = FindObjectOfType<TurnManager>();
            if (turnManager != null)
                Destroy(turnManager.transform.root.gameObject);
        }

        public void PlayGame() {
            SceneController.LoadFirstLevel();
        }

        public void Credits() {
            SceneManager.LoadScene("Credits");
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
