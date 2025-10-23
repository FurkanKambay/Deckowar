using System;
using Deckowar.Core;
using UnityEngine;

namespace Deckowar
{
    public class TurnTimeManager : MonoBehaviour
    {
        public event Action<TurnTimeManager> OnTurnChanged;

        public int CurrentTurn { get; private set; }
        public Faction CurrentFaction { get; private set; }

        private void Awake()
        {
            CurrentTurn = 0;
            CurrentFaction = Faction.Player;
        }

        public void ProceedToNextTurn()
        {
            CurrentTurn++;
            CurrentFaction = CurrentTurn % 2 == 0 ? Faction.Player : Faction.Enemy;

            OnTurnChanged?.Invoke(this);
        }
    }
}
