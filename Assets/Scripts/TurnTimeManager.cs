using System;
using UnityEngine;

namespace Deckowar
{
    public class TurnTimeManager : MonoBehaviour
    {
        public event Action<TurnTimeManager, int> OnTurnChanged;

        public int CurrentTurn { get; private set; }

        public void ProceedToNextTurn()
        {
            CurrentTurn++;
            OnTurnChanged?.Invoke(this, CurrentTurn);
        }
    }
}
