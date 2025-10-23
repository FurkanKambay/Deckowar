using System;
using Deckowar.Core;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Deckowar
{
    public class TurnTimeManager : MonoBehaviour
    {
        public event Action<TurnTimeManager> OnTurnChanged;

        [Header("Input")]
        [SerializeField] private InputActionReference readyInput;

        public int CurrentTurn { get; private set; }
        public Faction CurrentFaction { get; private set; }

        private void Awake()
        {
            Assert.IsNotNull(readyInput);
            readyInput.asset.Enable();

            CurrentTurn = -1;
            CurrentFaction = Faction.None;
        }

        private void Start()
        {
            ProceedToNextTurn();
        }

        private void Update()
        {
            if (readyInput.action.triggered)
                ProceedToNextTurn();
        }

        public void ProceedToNextTurn()
        {
            CurrentTurn++;
            CurrentFaction = CurrentTurn % 2 == 0 ? Faction.Player : Faction.Enemy;

            OnTurnChanged?.Invoke(this);
        }
    }
}
