using UnityEngine;

namespace Util {
    public static class ColoredGizmos {

        private static Color previousColor = Gizmos.color;

        private static void SetColor(Color color) {
            previousColor = Gizmos.color;
            Gizmos.color = color;
        }

        private static void ResetColor() => SetColor(previousColor);

        public static void DrawSphere(Vector3 center, float radius, Color color) {
            SetColor(color);
            Gizmos.DrawSphere(center, radius);
            ResetColor();
        }
        public static void DrawCube(Vector3 center, Vector3 size, Color color) {
            SetColor(color);
            Gizmos.DrawCube(center, size);
            ResetColor();
        }

        public static void DrawWireCube(Vector3 center, Vector3 size, Color color) {
            SetColor(color);
            Gizmos.DrawWireCube(center, size);
            ResetColor();
        }
    }
}
