using Gamemode;
using System;

namespace Actors.Animation
{
    public class PlayerActorAnimator : ActorAnimator {
        private PlayerInventory playerInventory;

        private void Awake() {
            playerInventory = transform.root.GetComponent<PlayerInventory>();
            playerInventory.OnKeyPickedUp += TriggerInteract;
        }

        private void OnDestroy() {
            playerInventory.OnKeyPickedUp -= TriggerInteract;
        }

        private void OnInteract() {
            if(!TurnManager.CurrentActorTag.Equals(TurnManager.PlayerTag)) {
                return;
            }

            TriggerInteract();
        }
        public void TriggerInteract() {
            animator?.SetTrigger("interact");
        }
    }
}
