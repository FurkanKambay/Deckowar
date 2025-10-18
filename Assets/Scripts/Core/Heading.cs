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
    }
}
