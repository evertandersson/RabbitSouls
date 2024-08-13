using Actors.Controllers;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Puzzle.Activators {

    /*
    The player actor should really be responsible for this.
    Overlapbox was a bodge to avoid triggers (no rigidbodies in the game)
    so if we wanted to do this correctly we would do it differently
    -kim
    */

    [RequireComponent(typeof(PlayerInput))]
    public abstract class Activator : MonoBehaviour {
        [SerializeField] private LayerMask playerMask;

        // input method
        private void OnInteract() {
            if(!PlayerController.InputEnabled) {
                return;
            }

            Collider[] colliders = new Collider[1];

            int size = Physics.OverlapBoxNonAlloc(transform.position, Vector3.one * 0.5f, colliders, Quaternion.identity, playerMask);
            if(size < 1) {
                return;
            }

            Collider playerCollider = colliders.FirstOrDefault();

            // null ref is in fact NOT possible.
            Interact(playerCollider.gameObject);
        }

        protected abstract void Interact(GameObject player);

        private void OnDrawGizmos() {
            ColoredGizmos.DrawWireCube(transform.position, Vector3.one, Color.green);
        }
    }
}
