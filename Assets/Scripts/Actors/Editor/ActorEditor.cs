using UnityEditor;
using UnityEngine;

namespace Actors.Editor
{
    [CustomEditor(typeof(Actor))]
    public class ActorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            Actor actor = (Actor)target;

            if(EditorApplication.isPlaying) {
                if(GUILayout.Button("Move")) {
                    actor.Move();
                }
            }
        }
    }
}
