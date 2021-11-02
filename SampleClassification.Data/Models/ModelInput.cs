using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleClassification.Data.Models
{
    public class ModelInput
    {
        [ColumnName("col0"), LoadColumn(0)]
        public string Book { get; set; }


        [ColumnName("col1"), LoadColumn(1)]
        public string BookTranslation { get; set; }


    }
}
