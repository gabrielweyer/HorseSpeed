using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HorseSpeed.Runner.Models
{
    internal class Options
    {
        public readonly string FromClause;
        public readonly string WhereClause;
        public readonly IReadOnlyList<PercentileKey> Percentiles;
        private readonly IReadOnlyList<PercentileKey> _defaultPercentiles = new List<PercentileKey> { 50, 70, 95 };

        public Options(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(args), args, "Incorrect number of arguments, read the Readme.md file.");
            }

            FromClause = args[0];
            WhereClause = args[1];

            Percentiles = args.Length > 2 ? ParsePercentilesArgument(args[2]) : _defaultPercentiles;
        }

        private static List<PercentileKey> ParsePercentilesArgument(string percentilesArgument)
        {
            return percentilesArgument
                .Split(',')
                .Select(q => new PercentileKey(Convert.ToDecimal((string) q, CultureInfo.InvariantCulture)))
                .ToList();
        }
    }
}