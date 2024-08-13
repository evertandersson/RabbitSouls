using Actors;
using Gamemode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class DisplayRound : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI roundsDisplay;
        [SerializeField] private TextMeshProUGUI whosTurn;
        [SerializeField] private Button button;

        private Actor actor;

        private void Awake() {
            actor = transform.root.GetComponent<Actor>();

            actor.OnStartedMove += DisableDuringMove;
            TurnManager.OnTurnStarted += ToggleForTurn;
        }
        private void Start() {
            button.interactable = TurnManager.CurrentActorTag.Equals(TurnManager.PlayerTag);
        }

        private void DisableDuringMove() => button.interactable = false;

        private void ToggleForTurn(string actorTag) {
            button.interactable = actorTag.Equals(TurnManager.PlayerTag);
        }

        private void Update() {
            roundsDisplay.text = "Round: " + TurnManager.TurnsTakenThisLevel;
            if(TurnManager.CurrentActorTag == "Player")
                whosTurn.text = "Players Turn";
            else if(TurnManager.CurrentActorTag == "Enemy")
                whosTurn.text = "Enemys Turn";

        }

        private void OnDestroy() {
            actor.OnStartedMove -= DisableDuringMove;
            TurnManager.OnTurnStarted -= ToggleForTurn;
        }
    }
}
