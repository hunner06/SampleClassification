using System.IO;
using System.Linq;

namespace SampleClassification.ConsoleApp
{
    public static class DataManagement
    {
        //private config
       
        public static void MergeCSVs(string filePath)
        {
            var allCsv = Directory.EnumerateFiles(filePath, "*.csv", SearchOption.TopDirectoryOnly);
            string[] header =
            {
                File.ReadLines(allCsv.First()).First(l => !string.IsNullOrWhiteSpace(l))
            };

            // Get CSV Data
            var mergedData = allCsv.SelectMany(csv => File.ReadLines(csv).SkipWhile(l => string.IsNullOrWhiteSpace(l)).Skip(1));

            var combinedCSV_FilePath = Path.Combine(filePath, "Combined.csv");

            // skip header of each file
            File.WriteAllLines(combinedCSV_FilePath, header.Concat(mergedData));
        }

        public static void PrepareCSV(string filePath)
        {
            var combinedCSV_FilePath = Path.Combine(filePath, "Combined.csv");

            if (!File.Exists(combinedCSV_FilePath))
            {
                MergeCSVs(filePath);
            }

        }
    }
}