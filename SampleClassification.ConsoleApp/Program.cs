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
using SampleClassification.Data;

namespace SampleClassification.ConsoleApp
{
    class Program
    {
        public static string _TRAIN_DATA_FILEPATH = @"C:\ML.NET_Project\ParishBooks\TrainModel\";
        public static string _PredictCSV_Location = @"C:\ML.NET_Project\ParishBooks\PredictFiles";
        static void Main(string[] args)
        {

            using var db = new ClassificationDataContext();



            

            //Load SQL
            //DataManagement.LoadSqlFromCSV(_TRAIN_DATA_FILEPATH, db);

            //Run to retrain 
           //ModelBuilder.TrainModel(_TRAIN_DATA_FILEPATH);

            //Run to predict

            
            CsvInput.PredictFromCSV(_PredictCSV_Location, true, db);


        }


    }
}
