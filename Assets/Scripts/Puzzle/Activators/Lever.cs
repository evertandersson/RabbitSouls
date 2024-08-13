using Actors.Animation;
using Gamemode;
using Puzzle.Mechanisms;
using System;
using System.Linq;
using UnityEngine;

namespace Puzzle.Activators
{
    public class Lever : Activator {
        [SerializeField] private Mechanism[] mechanisms;

        private Animator animator;
        private bool interactable;
        private bool isActivated;

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        protected override void Interact(GameObject player) {
            Activate();

            player.GetComponentInChildren<PlayerActorAnimator>().TriggerInteract();
        }
        public void Activate() {
            isActivated = !isActivated;

            animator.Play(isActivated ? "toggle_on" : "toggle_off", 0);

            ToggleMechanisms();
        }

        void ToggleMechanisms() {
            for(int i = 0; i < mechanisms.Length; i++) {
                mechanisms[i].Activate();
            }
        }
    }
}
