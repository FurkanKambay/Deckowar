using System;
using UnityEngine;

namespace Deckowar.Core.Commands
{
    [Serializable]
    public class UnitMoveCommand : ICommand
    {
        private Transform target;
        private Heading heading;
        private float steps;

        public UnitMoveCommand(Transform target, Heading heading, int steps)
        {
            this.target = target;
            this.heading = heading;
            this.steps = steps;
        }

        public void Execute()
        {
            target.Translate(steps * heading.GetVector());
        }
    }
}
