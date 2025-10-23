using System;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Deckowar.Common
{
    [PublicAPI]
    public static class EditorDebug
    {
        // ReSharper disable Unity.PerformanceAnalysis
        [HideInCallstack, Conditional("UNITY_EDITOR")]
        public static void Log(string message) => Debug.Log(message);

        // ReSharper disable Unity.PerformanceAnalysis
        [HideInCallstack, Conditional("UNITY_EDITOR")]
        public static void Log(string message, Object context) => Debug.Log(message, context);

        // ReSharper disable Unity.PerformanceAnalysis
        [HideInCallstack, Conditional("UNITY_EDITOR")]
        public static void LogWarning(string message) => Debug.LogWarning(message);

        // ReSharper disable Unity.PerformanceAnalysis
        [HideInCallstack, Conditional("UNITY_EDITOR")]
        public static void LogWarning(string message, Object context) => Debug.LogWarning(message, context);

        // ReSharper disable Unity.PerformanceAnalysis
        [HideInCallstack, Conditional("UNITY_EDITOR")]
        public static void LogError(string message) => Debug.LogError(message);

        // ReSharper disable Unity.PerformanceAnalysis
        [HideInCallstack, Conditional("UNITY_EDITOR")]
        public static void LogError(string message, Object c) => Debug.LogError(message, c);

        // ReSharper disable Unity.PerformanceAnalysis
        [HideInCallstack, Conditional("UNITY_EDITOR")]
        public static void LogException(Exception exception) => Debug.LogException(exception);

        // ReSharper disable Unity.PerformanceAnalysis
        [HideInCallstack, Conditional("UNITY_EDITOR")]
        public static void LogException(Exception exception, Object c) => Debug.LogException(exception, c);
    }
}
