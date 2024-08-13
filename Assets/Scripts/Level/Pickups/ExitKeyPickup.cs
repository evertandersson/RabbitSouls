using System;
using UnityEngine;

namespace Level.Pickups
{
    public class ExitKeyPickup : MonoBehaviour {
        public event Action OnPickedUp;

        public void PickUp() {
            OnPickedUp?.Invoke();
            OnPickedUp = null;
            Destroy(gameObject);
        }
    }
}
