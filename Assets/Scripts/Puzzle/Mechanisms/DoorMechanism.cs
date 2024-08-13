using Pathfinding;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Puzzle.Mechanisms
{
    public class DoorMechanism : Mechanism {
        [FormerlySerializedAs("isOpen")] [SerializeField] private bool isClosed = true;
        [SerializeField] private Collider geometryCollider;

        private Animator animator;
        private GridManager gridManager;

        private void Awake() {
            animator = GetComponent<Animator>();
            gridManager = FindObjectOfType<GridManager>();
            Animate();
        }

        private void Start() {
            ToggleGeometry();
        }

        public override void Activate() {
            isClosed = !isClosed;
            ToggleGeometry();
            Animate();
        }

        private void ToggleGeometry() {
            gridManager.SetTile(transform.position, isClosed);
            geometryCollider.enabled = isClosed;
        }

        private void Animate() {
            animator.Play(isClosed ? "close" : "open", 0);
        }
    }
}
