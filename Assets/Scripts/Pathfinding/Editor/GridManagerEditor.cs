using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Pathfinding.Editor {
    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GridManager grid = (GridManager)target;

            if(GUILayout.Button("Generate Tiles")) {
                grid.BuildTilesFromGeometry();
            }
        }
    }
}
