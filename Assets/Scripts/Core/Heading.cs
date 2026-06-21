using UnityEngine;

namespace Deckowar.Core
{
    public enum Heading
    {
        None,
        West,
        East
    }

    public static class HeadingExtensions
    {
        public static Heading GetOpposite(this Heading heading) => heading switch
        {
            Heading.West => Heading.East,
            Heading.East => Heading.West,
            _ => Heading.None
        };

        public static Faction GetFaction(this Heading heading) => heading switch
        {
            Heading.West => Faction.Enemy,
            Heading.East => Faction.Player,
            _ => Faction.None
        };

        public static Vector3 GetVector(this Heading heading)
        {
            return heading switch
            {
                Heading.West => Vector3.left,
                Heading.East => Vector3.right,
                _ => Vector3.zero
            };
        }
    }
}
