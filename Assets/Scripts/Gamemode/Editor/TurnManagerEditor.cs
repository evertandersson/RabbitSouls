using System;
using UnityEditor;
using UnityEngine;

namespace Gamemode.Editor
{
    [CustomEditor(typeof(TurnManager))]
    public class TurnManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if(!EditorApplication.isPlaying)
                return;

            GUILayout.Label($"Player turns taken: {TurnManager.TurnsTakenThisLevel}");

            if(GUILayout.Button($"End {TurnManager.CurrentActorTag}'s Turn")) {
                TurnManager.EndTurn(TurnManager.CurrentActorTag);
            }
        }
    }
}
