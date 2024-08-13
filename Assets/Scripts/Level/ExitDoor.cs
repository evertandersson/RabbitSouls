using Debug;
using Level.Pickups;
using System;
using System.Linq;
using UnityEngine;
namespace Level
{
    public class ExitDoor : MonoBehaviour {
        [SerializeField] private ExitKeyPickup exitKey;

        public event Action OnLevelEnd;

        private Animator animator;

        private bool KeyPickedUp => exitKey == null;

        private void Awake() {
            animator = GetComponent<Animator>();
            exitKey.OnPickedUp += OnKeyPickedUp;
        }

        private void OnKeyPickedUp() {
            exitKey.OnPickedUp -= OnKeyPickedUp;
            exitKey = null;

            animator.Play("open", 0);

            InvokeRepeating(nameof(CheckForPlayer), 0f, .25f);
        }

        private void CheckForPlayer() {
            Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.up, Vector3.one * 0.4f);
            Collider playerCollider = colliders.FirstOrDefault(c => c.transform.root.CompareTag("Player"));

            if(playerCollider == null)
                return;

            if(KeyPickedUp) {
                CancelInvoke(nameof(CheckForPlayer));
                SafeLogger.Log("Ended level");
                OnLevelEnd?.Invoke();
            }
        }
    }
}
