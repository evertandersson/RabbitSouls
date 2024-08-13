using UnityEngine;

namespace Util
{
    public class EventSystemSingleton : MonoBehaviour {
        private static EventSystemSingleton instance;

        private void Awake() {
            if(!instance) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if(instance != this) {
                Destroy(gameObject);
                return;
            }
        }
    }
}
