using Gamemode;
using UnityEditor;
using UnityEngine;

namespace Actors.Controllers.Editor {
    [CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : UnityEditor.Editor{
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if(!EditorApplication.isPlaying) {
                return;
            }

            if(!TurnManager.CurrentActorTag.Equals(TurnManager.PlayerTag)) {
                return;
            }

            PlayerController player = (PlayerController)target;

            if(GUILayout.Button("Commit Move")) {
                player.CommitMove();
            }
        }
    }
}
