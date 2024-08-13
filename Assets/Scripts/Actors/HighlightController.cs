using Actors.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour {
    [SerializeField] private LayerMask indicatorMask;
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;

    private Transform selectedTile;
    private Renderer highlightedTile;
    public bool selected = false;

    private void Update() {
        if (Time.timeScale == 1) {
            if(selected == false) HighlightTile();
            else if(highlightedTile != null) SelectTile();
            else selected = false;
        }
    }
    public void OnSelectMove() {
        HighlightTile();
        SelectTile();
    }

    public void HighlightTile() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100f, indicatorMask)) {
            var selection = hit.transform;

            var tile = selection.GetComponent<Renderer>();
            if(tile != null) {
                // return if tile is the selected one

                if(highlightedTile != null) {
                    if(tile != highlightedTile) {
                        highlightedTile.material = defaultMaterial;
                    }
                }

                highlightedTile = tile;
                highlightedTile.material = highlightMaterial;
            }
            return;
        }
        if (highlightedTile != null) {
            highlightedTile.material = defaultMaterial;
            highlightedTile = null;
        }
    }

    public void SelectTile() {
        selected = true;
    }

}
