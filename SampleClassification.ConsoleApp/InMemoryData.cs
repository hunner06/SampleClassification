using SampleClassification.Data.Models;
using SampleClassification.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SampleClassification.ConsoleApp
{
    public class InMemoryData
    {
        private string _inputDataLocation;

        public ModelInput[] _parishBookData = new ModelInput[] { };
        public InMemoryData(string inputDataLocation)
        {
            _inputDataLocation = inputDataLocation;
        }

        public void InitializeData()
        {
            var allCsv = Directory.EnumerateFiles(_inputDataLocation, "*.csv", SearchOption.TopDirectoryOnly);
            foreach(var file in allCsv)
            {
                //var csv = 
            }
        }


    }
}
