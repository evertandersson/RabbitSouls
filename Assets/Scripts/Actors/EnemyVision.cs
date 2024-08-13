using Actors.Controllers;
using Menus;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Actors
{
    public class EnemyVision : MonoBehaviour
    {
        [SerializeField] private GameObject indicatorPrefab;
        [SerializeField] private LayerMask geometryMask;
        [SerializeField] private Material playerDetectedMaterial;
        private GameObject player;

        private Actor actor;
        private Transform indicatorParent;
        private List<GameObject> indicators;

        private void Awake() {
            player = FindObjectOfType<PlayerController>().gameObject;
            actor = transform.root.GetComponent<Actor>();

            SpawnIndicators();
        }

        private void SpawnIndicators() {
            int range = 3;

            if(indicatorParent == null) {
                indicatorParent = new GameObject("Indicator Parent").transform;
                indicatorParent.SetParent(transform);
            }

            indicators = new List<GameObject>();

            for(int y = 2; y <= range; y++) {
                for (int x = -1; x <= 1; x += 2) {
                    AddIndicator(new(x, 0.01f, y));
                }
            }
            for(int y = 0; y <= range; y++) {
                AddIndicator(new(0, 0.01f, y));
            }
        }

        private void AddIndicator(Vector3 pos) {
            GameObject indicator = Instantiate(indicatorPrefab, transform.position + pos, indicatorPrefab.transform.rotation, indicatorParent);
            indicator.name = $"Indicator {pos.XZToVector2Int()}";
            indicators.Add(indicator);
        }

        private void Update() {
            Vector2Int playerIntPos = player.transform.position.XZToVector2Int();

            for (int i = 0; i < indicators.Count; i++) {
                GameObject indicator = indicators[i];
                Vector3 indicatorPos = indicator.transform.position;

                bool blockedByWalls = IsBlockedByGeometry(indicatorPos);
                bool canPathFind = actor.TryGeneratePath(indicatorPos);
                if(!canPathFind || blockedByWalls) {
                    indicator.gameObject.SetActive(false);
                    continue;
                }

                if(!indicator.gameObject.activeSelf) {
                    indicator.gameObject.SetActive(true);
                }

                Vector2Int indicatorIntPos = indicator.transform.position.XZToVector2Int();

                if(Vector2Int.Distance(indicatorIntPos, playerIntPos) > float.Epsilon) {
                    continue;
                }

                Time.timeScale = 0;
                indicators[i].GetComponent<Renderer>().material = playerDetectedMaterial;
                //ugly fix but should work
                player.GetComponentInChildren<PlayerDetected>().ShowUI();
                enabled = false;
                return;
            }
        }

        private bool IsBlockedByGeometry(Vector3 targetPos) {
            Vector3 start = transform.position + Vector3.up * 0.5f;
            Vector3 end = targetPos + Vector3.up * 0.5f;
            UnityEngine.Debug.DrawLine(start, end, Color.red);
            return Physics.Linecast(start, end, geometryMask);
        }
    }
}
