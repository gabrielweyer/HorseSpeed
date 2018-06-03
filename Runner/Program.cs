using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using HorseSpeed.Runner.Logic;
using HorseSpeed.Runner.Models;

namespace HorseSpeed.Runner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var watch = Stopwatch.StartNew();

                var fileWriter = new FileWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "out"));

                var options = new Options(args);

                var fromClauses = GetFromClauses(options.FromClause);

                foreach (var fromClause in fromClauses)
                {
                    Console.WriteLine($"Processing {fromClause}");
                    Console.WriteLine();

                    var timesTaken = LogQuerier.QueryLogs(fromClause, options.WhereClause);

                    var metrics = Statistics.ComputeMetrics(timesTaken, options.Percentiles);
                    DisplayMetrics(metrics);

                    Console.WriteLine();

                    if (ShouldWriteTimingFile())
                    {
                        var outFilePath = fileWriter.GenerateOutFilePath(fromClause);
                        fileWriter.WriteTimingFile(timesTaken, outFilePath);
                        Console.WriteLine($"Wrote '{fromClause}' to '{outFilePath}'.");
                    }

                    Console.WriteLine($"Processed {timesTaken.Count} records.");
                    Console.WriteLine();
                }

                Console.WriteLine($"Processing time: {Math.Round(watch.Elapsed.TotalMilliseconds)} ms");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                DisplayException(ex);
                Console.ResetColor();
            }
            finally
            {
                if (Debugger.IsAttached)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        private static void DisplayException(Exception ex)
        {
            while (true)
            {
                if (ex == null) return;

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.GetType());
                Console.WriteLine(ex.StackTrace);

                ex = ex.InnerException;
            }
        }

        public static bool ShouldWriteTimingFile()
        {
            var createTimingFileConfigurationValue = ConfigurationManager.AppSettings["WriteTimingFile"];
            var createTimingFile = createTimingFileConfigurationValue != null && createTimingFileConfigurationValue == "True";
            return createTimingFile;
        }

        private static void DisplayMetrics(Metrics metrics)
        {
            Console.WriteLine($"Mean: {Math.Round(metrics.Mean)} ms");
            Console.WriteLine($"Median: {metrics.Median} ms");
            Console.WriteLine($"Population standard deviation: {Math.Round(metrics.PopulationStandardDeviation)} ms");
            Console.WriteLine();

            foreach (var percentiles in metrics.Percentiles)
            {
                Console.WriteLine($"{percentiles.Key}th percentile: {percentiles.Milliseconds} ms");
            }
        }

        private static IEnumerable<string> GetFromClauses(string fromClause)
        {
            var fromClauses = new List<string>();

            if (File.Exists(fromClause) || fromClause.IndexOf("*", StringComparison.Ordinal) != -1)
            {
                fromClauses.Add(fromClause);
            }
            else if (Directory.Exists(fromClause))
            {
                fromClauses.AddRange(Directory.GetFiles(fromClause, "*.log", SearchOption.TopDirectoryOnly));
            }

            return fromClauses;
        }
    }
}
