using Microsoft.ML;
using Microsoft.ML.Trainers;
using Microsoft.VisualBasic.FileIO;
using SampleClassification.Data;
using SampleClassification.Data.Models;
using SampleClassification.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SampleClassification.ConsoleApp
{
    internal class CsvInput
    {
        public static void PredictFromCSV(string filePath, bool hasResults, ClassificationDataContext db)
        {
            // var csvList = new List<ModelInput>();
            var newCsv = new StringBuilder();
            // var filePath = "C:\\Users\\hguest\\Documents\\RESOURCES\\.NET ML\\parishTest\\app\\SampleClassification\\SampleClassification.ConsoleApp\\Data";
            var newCSVfileName = Path.Combine(filePath, "results.csv");
            var inputFile = Path.Combine(filePath, "inputData.csv");
            var sameFindingsCount = 0;
            var count = 0;
            var matchCount = 0;
            var allCount = 0;
            try
            {
                using TextFieldParser csvParser = new(inputFile);
                // parser.TextFieldType = FieldType.Delimited;
                csvParser.HasFieldsEnclosedInQuotes = true;
                csvParser.SetDelimiters(",");
                csvParser.TrimWhiteSpace = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                newCsv.AppendLine("Original, Inital Translation,Prediction Result,Same Finding, score");

                while (!csvParser.EndOfData)
                {
                    //Process row
                    string[] fields = csvParser.ReadFields();

                    //var resultsPrevious = fields.Length == 2;

                    if (fields.Length == 1 || hasResults)
                    {
                        var inputData = fields[0];
                        var originalTranslation = hasResults ? fields[1] : "";
                        var result = "";
                        var score = "";
                        var sameFindings = true;
                        var predictionResult = new ModelOutput();

                        //See if there is a match
                        var cleanInput = CleanInput(inputData).ToUpper().Trim();
                        var match = db.ModelInput.ToList().FirstOrDefault(x => CleanInput(x.Book).ToUpper().Trim() == cleanInput);
                        //FindExactMatch();
                        if (match?.Id > 0)
                        {
                            matchCount++;
                            score = "MATCH";
                            result = match.BookTranslation;
                        }
                        else
                        {
                            ModelInput sampleData = new ModelInput()
                            {
                                Book = inputData,
                            };

                            // Make a single prediction on the sample data and print results
                            predictionResult = ConsumeModel.Predict(sampleData);
                            result = predictionResult.Prediction;
                            score = ConsumeModel.GetTopScore(predictionResult);

                            sameFindings = predictionResult.Prediction.ToUpper() == originalTranslation.ToUpper();
                            count++;
                            if (sameFindings && hasResults)
                            {
                                sameFindingsCount++;
                            }
                            else
                            {
                                // score = ConsumeModel.GetTop3(predictionResult);
                            }
                        }
                        allCount++;
                        var newLine = string.Format("{0},{1},{2},{3},{4}", inputData, originalTranslation, result, sameFindings, score);
                        newCsv.AppendLine(newLine);
                    }
                }
                var calcAccuracy = hasResults && ((sameFindingsCount > 0) && count > 0);
                var accuracy = calcAccuracy ? Math.Floor((sameFindingsCount / count) * 100.0) : 0;
                newCsv.AppendLine($"Processed:,{allCount}");
                newCsv.AppendLine($"Match Count:,{matchCount}");
                newCsv.AppendLine($"Accuracy correct:,{accuracy}");
                newCsv.AppendLine($"Same Findings count: {sameFindingsCount} of {count}");
                File.WriteAllText(newCSVfileName, newCsv.ToString());
                Console.WriteLine($"Same Findings count: {sameFindingsCount} of {count}");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        private static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[^\w\.@-]", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public static void ReTrainFromCSV(string filePath)
        {
            ///Load pre-trained model
            ///
            // Create MLContext
            MLContext mlContext = new MLContext();

            // Define DataViewSchema of data prep pipeline and trained model

            // Load trained model
            ITransformer trainedModel = mlContext.Model.Load(ConsumeModel.MLNetModelPath, out DataViewSchema modelSchema);

            ///Extract pre-trained model parameters
            ///
            // Extract trained model parameters
            LinearRegressionModelParameters originalModelParameters =
                ((ISingleFeaturePredictionTransformer<object>)trainedModel).Model as LinearRegressionModelParameters;

            // Get Data From CSV
            var csvList = new List<ModelInput>();
            //var filePath = "C:\\Users\\hguest\\Documents\\RESOURCES\\.NET ML\\parishTest\\app\\SampleClassification\\SampleClassification.ConsoleApp\\Data";
            var inputFile = Path.Combine(filePath, "correctedInputData.csv");
            try
            {
                using TextFieldParser parser = new TextFieldParser(inputFile);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //Process row
                    string[] fields = parser.ReadFields();
                    var rawInputCol = fields[0];
                    var labelCol = fields[1];
                    //foreach (string field in fields)
                    //{
                    //    //TODO: Process field
                    //}

                    ModelInput inputData = new ModelInput()
                    {
                        Book = rawInputCol,
                        BookTranslation = labelCol
                    };
                    csvList.Add(inputData);
                }

                ///Re-train model
                ///
                //Load New Data
                IDataView newData = mlContext.Data.LoadFromEnumerable(csvList);

                // Preprocess Data
                IDataView transformedNewData = trainedModel.Transform(newData);

                //// Retrain model
                //RegressionPredictionTransformer<OneVersusAllTrainer> retrainedModel =
                //    mlContext.MulticlassClassification.Trainers.OneVersusAll()
                //        .Fit(transformedNewData, originalModelParameters);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }
    }
}