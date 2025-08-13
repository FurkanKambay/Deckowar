using UnityEngine;

namespace FurkanKambay
{
    internal sealed class Castle : MonoBehaviour
    {
        [Header("State")]
        [SerializeField] private int health;

        public int Health => health;
    }
}
