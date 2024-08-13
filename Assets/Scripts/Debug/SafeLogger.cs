namespace Debug {
    public static class SafeLogger {
        public static void Log(object message) {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(message);
#endif
        }
        public static void Log(object message, UnityEngine.Object context) {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(message, context);
#endif
        }

        public static void LogWarning(object message) {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(message);
#endif
        }
        public static void LogWarning(object message, UnityEngine.Object context) {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(message, context);
#endif
        }

        public static void LogError(object message) {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(message);
#endif
        }
        public static void LogError(object message, UnityEngine.Object context) {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(message, context);
#endif
        }
    }
}
