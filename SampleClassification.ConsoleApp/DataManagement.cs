using SampleClassification.Data;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using SampleClassification.Data.Models;

namespace SampleClassification.ConsoleApp
{
    public static class DataManagement
    {
        //private config
       
        //public static void MergeCSVs(string filePath)
        //{
        //    var allCsv = Directory.EnumerateFiles(filePath, "*.csv", SearchOption.TopDirectoryOnly);
        //    string[] header =
        //    {
        //        File.ReadLines(allCsv.First()).First(l => !string.IsNullOrWhiteSpace(l))
        //    };

        //    // Get CSV Data
        //    var mergedData = allCsv.SelectMany(csv => File.ReadLines(csv).SkipWhile(l => string.IsNullOrWhiteSpace(l)).Skip(1));

        //    var combinedCSV_FilePath = Path.Combine(filePath, "Combined.csv");

        //    // skip header of each file
        //    File.WriteAllLines(combinedCSV_FilePath, header.Concat(mergedData));
        //}

        //public static void PrepareCSV(string filePath)
        //{
        //    var combinedCSV_FilePath = Path.Combine(filePath, "Combined.csv");

        //    if (!File.Exists(combinedCSV_FilePath))
        //    {
        //        MergeCSVs(filePath);
        //    }

        //}
        public static void LoadSqlFromCSV(string filePath, ClassificationDataContext db)
        {
            var files = Directory.EnumerateFiles(filePath, "*.csv");
            foreach (string file in files)
            {
                //using FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                // Use the file stream to read data.

                using TextFieldParser csvParser = new TextFieldParser(file);

                csvParser.SetDelimiters(new string[] { "," });
                csvParser.TrimWhiteSpace = true;
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    var fields = csvParser.ReadFields();
                   
                    db.Add(new ModelInput
                    {
                        Book = fields[0],
                        BookTranslation = fields[1]
                    });
                }
                db.SaveChanges();

            }

        }
    }
}