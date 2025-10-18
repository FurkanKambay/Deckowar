namespace Deckowar.Core
{
    public enum Faction
    {
        None,
        Player,
        Enemy
    }

    public static class FactionExtensions
    {
        public static Heading GetHeading(this Faction faction) => faction switch
        {
            Faction.Player => Heading.East,
            Faction.Enemy => Heading.West,
            _ => Heading.None
        };
    }
}
