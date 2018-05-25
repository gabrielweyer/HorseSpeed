using System.Collections.Generic;

namespace HorseSpeed.Runner.Models
{
    internal class Metrics
    {
        public readonly double Mean;
        public readonly decimal Median;
        public readonly double PopulationStandardDeviation;
        public readonly IReadOnlyList<Percentile> Percentiles;

        public Metrics(double mean, decimal median, double populationStandardDeviation, IReadOnlyList<Percentile> percentiles)
        {
            Mean = mean;
            Median = median;
            PopulationStandardDeviation = populationStandardDeviation;
            Percentiles = percentiles;
        }
    }
}