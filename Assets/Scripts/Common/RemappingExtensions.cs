using JetBrains.Annotations;

namespace FurkanKambay.Common
{
    [PublicAPI]
    public static class RemappingExtensions
    {
        public static Remapping Remap(this float value, float min, float max) => new(value, min, max);
        public static Remapping Remap(this int value, float min, float max) => new(value, min, max);

        public static Remapping Remap01(this float value) => new(value, 0, 1);
        public static Remapping Remap01(this int value) => new(value, 0, 1);

        public static Remapping RemapPercent(this float value) => new(value, 0, 100);
        public static Remapping RemapPercent(this int value) => new(value, 0, 100);
    }
}
