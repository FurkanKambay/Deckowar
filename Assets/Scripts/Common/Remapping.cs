using JetBrains.Annotations;
using UnityEngine;

namespace FurkanKambay.Common
{
    [PublicAPI]
    public readonly struct Remapping
    {
        private readonly float value;
        private readonly float inputA;
        private readonly float inputB;

        internal Remapping(float value, float inputA, float inputB)
        {
            this.value = value;
            this.inputA = inputA;
            this.inputB = inputB;
        }

        public float To(float a, float b) =>
            a + ((b - a) * (value - inputA) / (inputB - inputA));

        public float ToClamped(float min, float max) =>
            Mathf.Clamp(To(min, max), Mathf.Min(min, max), Mathf.Max(min, max));

        public float To01() => To(0, 1);
        public float ToClamped01() => ToClamped(0, 1);

        public float ToPercent() => To(0, 100);
        public float ToClampedPercent() => ToClamped(0, 100);
    }
}
