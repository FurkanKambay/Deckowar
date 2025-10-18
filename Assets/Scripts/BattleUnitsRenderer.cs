using UnityEngine;

namespace Deckowar
{
    public sealed class BattleUnitsRenderer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BattleManager battleManager;

        private void OnEnable()
        {
            // var spawnedUnit = Instantiate(unitPrefab, spawnPoint, Quaternion.identity, transform);
        }
    }
}
