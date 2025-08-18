using UnityEngine;

namespace FurkanKambay.Data
{
    [CreateAssetMenu]
    public class UnitSO : ScriptableObject
    {
        [SerializeField] private float moveSpeed;

        public float MoveSpeed => moveSpeed;
    }
}
