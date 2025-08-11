using UnityEngine;

namespace FurkanKambay.Deckbuilding
{
    [CreateAssetMenu(menuName = "Card")]
    public class CardSO : ScriptableObject
    {
        [SerializeField] private int    id;
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;
        [SerializeField] private int    cost;

        public int    Id          => id;
        public string DisplayName => displayName;
        public Sprite Icon        => icon;
        public string Description => description;
        public int    Cost        => cost;

        public static Card CreateInstance()
        {
            return new Card();
        }
    }
}
