using System;
using UnityEngine;

namespace Actors.Animation
{
    [RequireComponent(typeof(Animator))]
    public class ActorAnimator : MonoBehaviour {
        [SerializeField, Min(1.0f)] private float rotationSpeed = 10f;
        [SerializeField, Min(float.Epsilon)] private float minimumMoveStep = 0.01f;

        protected Animator animator;
        private Transform rootTransform;
        private Vector3 previousPos;
        private Vector3 moveDelta;

        public Vector3 TargetLookDirection { get; private set; }

        private void Start() {
            animator = GetComponent<Animator>();
            rootTransform = transform.root;
            TargetLookDirection = rootTransform.forward;
            previousPos = rootTransform.position;
        }

        private void FixedUpdate() {
            Movement();
            SetTargetLookDirection();
            AddFixedUpdateAnimation();
        }

        protected virtual void AddFixedUpdateAnimation() {}

        private void Movement() {
            Vector3 currentPos = rootTransform.position;
            moveDelta = currentPos - previousPos;

            animator?.SetBool("moving", moveDelta.magnitude > minimumMoveStep);

            previousPos = currentPos;
        }

        private void SetTargetLookDirection() {
            Vector3 moveDir = moveDelta.normalized;
            bool lookingTowardMoveDir = 1 - Vector3.Dot(moveDir, TargetLookDirection) < float.Epsilon;

            if(moveDir.magnitude > float.Epsilon && !lookingTowardMoveDir) {
                TargetLookDirection = moveDir;
            }
        }

        private void Update() {
            Rotation();
            AddUpdateAnimation();
        }
        protected virtual void AddUpdateAnimation() {}
        private void Rotation() {
            float lookDot = Vector3.Dot(transform.forward, TargetLookDirection);
            bool lookingTowardTargetDir = 1 - lookDot < 0.001f;
            if(lookingTowardTargetDir) {
                transform.forward = TargetLookDirection;
                return;
            }

            float rotationDelta = Time.deltaTime * rotationSpeed;
            transform.forward = Vector3.RotateTowards(transform.forward, TargetLookDirection, rotationDelta, 0);
        }
    }
}
