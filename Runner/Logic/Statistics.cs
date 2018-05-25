using System;
using System.Collections.Generic;
using System.Linq;
using HorseSpeed.Runner.Models;

namespace HorseSpeed.Runner.Logic
{
    internal static class Statistics
    {
        public static Metrics ComputeMetrics(IReadOnlyList<int> orderedList, IReadOnlyList<PercentileKey> percentileKeys)
        {
            return new Metrics
            (
                orderedList.Average(),
                GetMedian(orderedList),
                GetPopulationStandardDeviation(orderedList),
                GetPercentiles(orderedList, percentileKeys)
            );
        }

        private static double GetPopulationStandardDeviation(IReadOnlyList<int> values)
        {
            var avg = values.Average();
            return Math.Sqrt(values.Average(v => (v - avg) * (v - avg)));
        }

        private static List<Percentile> GetPercentiles(IReadOnlyList<int> orderedList, IReadOnlyList<PercentileKey> percentileKeys)
        {
            var percentiles = from percentileKeyToCompute in percentileKeys
                              let percentile = GetPercentile(orderedList, percentileKeyToCompute)
                              select new Percentile(percentileKeyToCompute, percentile);

            return percentiles.ToList();
        }

        private static int GetPercentile(IReadOnlyList<int> orderedList, PercentileKey percentileKey)
        {
            var index = (int)Math.Ceiling(percentileKey / 100 * orderedList.Count);

            if (index > 0)
            {
                index--;
            }

            return orderedList[index];
        }

        private static decimal GetMedian(IReadOnlyList<int> orderedList)
        {
            if (orderedList.Count % 2 != 0) return orderedList[orderedList.Count / 2];

            var lowerMedian = orderedList[orderedList.Count / 2 - 1];
            var upperMedian = orderedList[orderedList.Count / 2];

            return (lowerMedian + upperMedian) / 2m;
        }
    }
}