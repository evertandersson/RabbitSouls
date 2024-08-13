using UnityEngine;

namespace Util {
    public static class Extensions {
        public static Vector2Int XZToVector2Int(this Vector3 v) {
            return new( Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.z));
        }
    }
}
