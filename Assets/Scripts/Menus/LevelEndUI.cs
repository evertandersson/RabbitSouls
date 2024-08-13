using Gamemode;
using Level;
using Menu;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class LevelEndUI : MonoBehaviour
    {
        [SerializeField] private GameObject UI;
        [SerializeField] private TextMeshProUGUI turnLabel;

        private DisplayRound displayRound;
        private PauseMenu pauseMenu;
        private ExitDoor exitDoor;

        private void Awake() {
            pauseMenu = FindObjectOfType<PauseMenu>();
            displayRound = FindObjectOfType<DisplayRound>();

            exitDoor = transform.root.GetComponent<ExitDoor>();
            exitDoor.OnLevelEnd += ShowUI;
        }

        private void OnDestroy() {
            exitDoor.OnLevelEnd -= ShowUI;
        }

        private void ShowUI() {
            turnLabel.text = TurnManager.TurnsTakenThisLevel.ToString();
            pauseMenu.gameObject.SetActive(false);
            displayRound.gameObject.SetActive(false);
            UI.SetActive(true);
        }

        public void MainMenu() {
            SceneController.LoadMainMenu();
        }

        public void NextLevel() {
            pauseMenu.gameObject.SetActive(true);
            SceneController.LoadNextLevel();
        }

        public void RestartGame() {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
