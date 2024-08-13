using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Util;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {
    public List<Transform> targets;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform player;

    public Vector3 offset = new Vector3(5, 0, 6);
    private Vector3 boundsDistance = new Vector3(9, 0, 9);

    public float smoothTime = 1f;

    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    private Vector3 velocity;
    private Camera cam;

    private void Start() {
        cam = GetComponent<Camera>();

        InvokeRepeating(nameof(UpdateTargets), 0, 0.25f);
    }

    private void LateUpdate() {
        UpdateTargets();
        Move();
    }

    private void UpdateTargets() {
        Collider[] colliders = Physics.OverlapBox(player.position, Vector3.one * 7f, Quaternion.identity, enemyMask);

        targets.Clear();
        for (int i = 0; i < colliders.Length; i++) {
            targets.Add(colliders[i].transform);
        }
    }

    private void Move() {
        Vector3 centerPoint = new Vector3(GetCenterPoint().x, transform.position.y, GetCenterPoint().z);

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    Vector3 GetCenterPoint() {
        if(targets.Count == 0) return player.position;

        var bounds = new Bounds(player.position, Vector3.zero);

        for(int i = 0; i < targets.Count; i++) {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
    private void OnDrawGizmos() {
        //ColoredGizmos.DrawWireCube(player.position, Vector3.one * 7f, Color.blue);
    }
}
