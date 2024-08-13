using TMPro;
using UnityEngine;

namespace Debug
{
    public class ModifyTextOnStart : MonoBehaviour {

        [SerializeField] private string onStartText = "Modified on Start";

        private TextMeshProUGUI _textmesh;

        private void Awake() => _textmesh = GetComponent<TextMeshProUGUI>();

        void Start() => _textmesh.text = onStartText;
    }
}
