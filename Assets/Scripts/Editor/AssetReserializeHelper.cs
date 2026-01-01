#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Deckowar.Editor
{
    public sealed class AssetReserializeHelper : MonoBehaviour
    {
        [MenuItem("Tools/Force Reserialize Assets")]
        private static void ForceReserializeAssets() =>
            AssetDatabase.ForceReserializeAssets();
    }
}
#endif
