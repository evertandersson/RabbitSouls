using Debug;
using Gamemode;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Actors.Controllers
{
    public class PlayerController : MonoBehaviour {
        [SerializeField] private LayerMask moveTargetMask;

        private HighlightController highlightController;

        private Camera mainCamera;
        private Actor actor;

        private Vector3 moveTarget;
        public static bool InputEnabled { get; private set; }

        private void Awake() {
            // this is bad - but I dont have time to set up any fancier camera stuff right now
            // if it causes bugs, fix it to not use Camera.main.
            mainCamera = Camera.main;

            highlightController = FindObjectOfType<HighlightController>();

            actor = GetComponent<Actor>();
            actor.OnReachedDestination += EndTurn;
            TurnManager.OnTurnStarted += OnTurnStarted;
        }

        private void OnDestroy() {
            actor.OnReachedDestination -= EndTurn;
            TurnManager.OnTurnStarted -= OnTurnStarted;
        }

        private void Start() {
            ResetMoveTarget();
            InputEnabled = TurnManager.CurrentActorTag.Equals(TurnManager.PlayerTag);
        }
        public void ResetMoveTarget() {
            moveTarget = transform.position;
        }

        private void OnTurnStarted(string tag) {
            if(!tag.Equals(TurnManager.PlayerTag)) {
                return;
            }

            InputEnabled = true;
        }

        // input method
        private void OnSelectMove() {
            if(!InputEnabled)
                return;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            UnityEngine.Debug.DrawRay(ray.origin, ray.direction, Color.green, 5f);

            bool validHit = Physics.Raycast(ray, out RaycastHit hit, 100f, moveTargetMask);
            if(!validHit) {
                return;
            }

            Vector3Int moveWorldTarget = Vector3Int.RoundToInt(hit.point);

            if(!actor.TryGeneratePath(moveWorldTarget)) {
                return;
            }

            if(actor.NodeDistanceToTarget(moveWorldTarget) > actor.NodesPerMove)
                return;

            moveTarget = moveWorldTarget;
            SafeLogger.Log($"Successfully picked new move target: {moveTarget}");
        }

        public void CommitMove() {
            if (highlightController.selected == false)
                highlightController.SelectTile();
            else
                highlightController.OnSelectMove();

            bool playerNotOnMoveTarget = Vector3.Distance(transform.position, moveTarget) > float.Epsilon;

            if(playerNotOnMoveTarget) {
                if(actor.TryGeneratePath(moveTarget, true)) {
                    actor.Move();
                }
                else {
                    SafeLogger.LogError($"Player couldnt move to {moveTarget}");
                }
            }
            else {
                EndTurn();
            }

            InputEnabled = false;
            EventSystem.current.SetSelectedGameObject(null);
        }

        private void EndTurn() {
            SafeLogger.Log("Ended player turn");
            TurnManager.EndTurn(TurnManager.PlayerTag);
            FindObjectOfType<MoveIndicator>().HideIndicators();
            highlightController.selected = false;

        }
    }
}
