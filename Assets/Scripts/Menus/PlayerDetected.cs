using Actors;
using Gamemode;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menus
{
    public class PlayerDetected : MonoBehaviour {
        [SerializeField] private GameObject UI;
        [SerializeField] private TextMeshProUGUI turnLabel;

        private PauseMenu pauseMenu;
        private DisplayRound displayRound;
        private EnemyVision enemyVision;

        private void Awake() {
            pauseMenu = FindObjectOfType<PauseMenu>();
            displayRound = FindObjectOfType<DisplayRound>();
        }

        public void ShowUI() {
            turnLabel.text = TurnManager.TurnsTakenThisLevel.ToString();
            displayRound.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(false);
            UI.SetActive(true);
        }

        public void MainMenu() {
            SceneController.LoadMainMenu();
        }

        public void RestartGame() {
            print("Restart Level");
            displayRound.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
            UI.SetActive(false);
            Time.timeScale = 1;
            EventSystem.current.SetSelectedGameObject(null);
            SceneController.LoadCurrentLevel();
        }
    }
}
