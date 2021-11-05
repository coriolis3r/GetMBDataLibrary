using System;
using System.Collections.Generic;

namespace KeepyMax.Model
{
    public class Models
    {
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int MeterTypeId { get; set; }
        public int MBFunctionId { get; set; }
    }

    public class MBModelsL
    {
        //public int ModelId { get; set; }
        public string model { get; set; }
        //public List<MBModelL> MBModel { get; set; }
        public List<MBModelL> mbList { get; set; }
    }

    public class MBModelL
    {
        public int MeasureTypeId { get; set; }
        public List<MBRegisters> mbregisters { get; set; }

    }
}
