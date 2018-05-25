namespace HorseSpeed.Runner.Models
{
    internal class Percentile
    {
        public readonly PercentileKey Key;
        public readonly int Milliseconds;

        public Percentile(PercentileKey key, int milliseconds)
        {
            Key = key;
            Milliseconds = milliseconds;
        }
    }
}