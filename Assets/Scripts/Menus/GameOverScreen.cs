using Gamemode;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI turnCounterLabel;

    private void Start() {
        turnCounterLabel.text = TurnManager.TurnsTakenTotal().ToString();
    }
}
