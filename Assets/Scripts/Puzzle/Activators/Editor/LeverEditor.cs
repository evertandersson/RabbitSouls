using UnityEditor;
using UnityEngine;

namespace Puzzle.Activators.Editor {
    [CustomEditor(typeof(Lever))]
    public class LeverEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            Lever lever = (Lever)target;

            if(!EditorApplication.isPlaying) {
                return;
            }

            if(GUILayout.Button("Toggle")) {
                lever.Activate();
            }
        }
    }
}
