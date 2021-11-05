using System;
using System.Collections.Generic;

namespace KeepyMax.Model
{
    public class GeneralConfig
    {
        public GeneralConfig()
        {
            //this.MeterMBParConfigList = new List<MeterMBParConfig>(); 
        }
        public string DeviceIdGuid { get; set; }
        public int ComTypeId { get; set; }
        //public string Alias { get; set; }
        public int NCPU { get; set; }
        public string IPAddress { get; set; }
        //public bool ConStatus { get; set; }
        public string ModelName { get; set; }

        public List<MeterMBParConfig> MeterMBParConfigList { get; set; }

    }
}