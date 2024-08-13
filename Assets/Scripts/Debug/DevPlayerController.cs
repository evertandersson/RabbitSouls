using Menu;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Debug
{
    public class DevPlayerController : MonoBehaviour {
        [SerializeField] private float speed = 3.0f;

        private CharacterController _controller;
        private Vector3 _move;

        // input method
        private void OnMove(InputValue inputValue) {
            Vector2 input = inputValue.Get<Vector2>().normalized;
            _move = new Vector3(input.x, 0, input.y) * speed;
        }

        private void Awake() {
            _controller = GetComponent<CharacterController>();
        }

        private void Update() {
            if(PauseMenu.gameIsPaused)
                return;

            // look
            if (_move.sqrMagnitude > 0)
                transform.forward = _move.normalized;

            _controller.SimpleMove(_move);
        }
    }
}
