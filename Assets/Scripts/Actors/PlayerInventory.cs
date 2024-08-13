using Level.Pickups;
using System;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    public event Action OnKeyPickedUp;

    [SerializeField, Range(0.01f, 1f)] private float checkRate = 0.25f;
    [SerializeField] private LayerMask pickupMask;

    private void Start() {
        InvokeRepeating(nameof(CheckForPickups), 0 , checkRate);
    }

    private void CheckForPickups() {
        Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.up, Vector3.one * 0.5f, Quaternion.identity, pickupMask);
        Collider pickupCollider = colliders.FirstOrDefault();

        if(pickupCollider == null)
            return;

        if(!pickupCollider.TryGetComponent(out ExitKeyPickup pickup)) {
            return;
        }

        pickup.PickUp();
        OnKeyPickedUp?.Invoke();
    }
}
