using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.VisualBasic.FileIO;
using SampleClassification.Data.Models;
using SampleClassification.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SampleClassification.ConsoleApp
{
    class CsvInput
    {

        public static void PredictFromCSV(string filePath, bool hasResults)
        {
            // var csvList = new List<ModelInput>();
            var newCsv = new StringBuilder();
           // var filePath = "C:\\Users\\hguest\\Documents\\RESOURCES\\.NET ML\\parishTest\\app\\SampleClassification\\SampleClassification.ConsoleApp\\Data";
            var newCSVfileName = Path.Combine(filePath, "results.csv");
            var inputFile = Path.Combine(filePath, "inputData.csv");
            var sameFindingsCount = 0;
            var count = 0;
            try
            {
                using TextFieldParser parser = new TextFieldParser(inputFile);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
              
                while (!parser.EndOfData)
                {
                    //Process row
                    string[] fields = parser.ReadFields();

                    //var resultsPrevious = fields.Length == 2;
                    
                    if (fields.Length == 1 || hasResults)
                    {
                        var inputData = fields[0];
                        var originalTranslation = fields[1];
                        //foreach (string field in fields)
                        //{
                        //    //TODO: Process field
                        //}

                        ModelInput sampleData = new ModelInput()
                        {
                            Book = inputData,
                        };

                        // Make a single prediction on the sample data and print results
                        var predictionResult = ConsumeModel.Predict(sampleData);
                        // var perdictionScore = $"[{ String.Join(",", predictionResult.Score)}]";
                        var sameFindings = predictionResult.Prediction.ToUpper() == originalTranslation.ToUpper();
                        count++;
                        if (sameFindings)
                        {
                            sameFindingsCount++;
                        }

                        //var newLine = $"{inputData},{predictionResult.Prediction},{perdictionScore}";
                        // var newLine = $"{inputData},{predictionResult.Prediction},{originalTranslation},{sameFindings}";
                        var newLine = string.Format("{0},{1},{2},{3}", inputData, predictionResult.Prediction, originalTranslation, sameFindings);
                        newCsv.AppendLine(newLine);
                    }
                    
                }

                File.WriteAllText(newCSVfileName, newCsv.ToString());
                Console.WriteLine($"Same Findings count: {sameFindingsCount} of {count}");
                Console.ReadLine();
            }
            catch(Exception ex) {
                Console.Write(ex);
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
