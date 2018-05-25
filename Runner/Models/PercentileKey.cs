using System;
using System.Globalization;

namespace HorseSpeed.Runner.Models
{
    internal class PercentileKey
    {
        private readonly decimal _key;

        public PercentileKey(decimal percentileOn100Scale)
        {
            if (percentileOn100Scale < 0 || percentileOn100Scale > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(percentileOn100Scale), percentileOn100Scale, "Percentile should be between 0 and a 100.");
            }

            _key = percentileOn100Scale;
        }

        public static implicit operator PercentileKey(decimal percentileOn100Scale)
        {
            return new PercentileKey(percentileOn100Scale);
        }

        public static implicit operator decimal(PercentileKey value)
        {
            return value._key;
        }

        public override string ToString()
        {
            return _key.ToString(CultureInfo.InvariantCulture);
        }
    }
}