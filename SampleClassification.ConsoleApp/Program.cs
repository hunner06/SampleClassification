//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleClassification.Model;

namespace SampleClassification.ConsoleApp
{
    class Program
    {
        public static string _TRAIN_DATA_FILEPATH = @"C:\ML.NET_Project\ParishBooks\TrainModel\";
        public static string _PredictCSV_Location = @"C:\ML.NET_Project\ParishBooks\PredictFiles";
        static void Main(string[] args)
        {

            //Run to predict
            //CsvInput.PredictFromCSV(_PredictCSV_Location, true);

            //Run to retrain 
            ModelBuilder.TrainModel(_TRAIN_DATA_FILEPATH);


            //// Create single instance of sample data from first line of dataset for model input
            //ModelInput sampleData = new ModelInput()
            //{
            //    Col0 = @"CANC",
            //};

            //// Make a single prediction on the sample data and print results
            //var predictionResult = ConsumeModel.Predict(sampleData);

            //Console.WriteLine("Using model to make single prediction -- Comparing actual Col1 with predicted Col1 from sample data...\n\n");
            //Console.WriteLine($"Col0: {sampleData.Col0}");
            //Console.WriteLine($"\n\nPredicted Col1 value {predictionResult.Prediction} \nPredicted Col1 scores: [{String.Join(",", predictionResult.Score)}]\n\n");
            //Console.WriteLine("=============== End of process, hit any key to finish ===============");
            //Console.ReadKey();
        }


    }
}
