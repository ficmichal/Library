namespace Library.BuildingBlocks.Domain
{
    public class Version
    {
        public int Value { get; }

        public static Version Zero()
        {
            return new(0);
        }

        private Version(int version)
        {
            Value = version;
        }
    }
}
