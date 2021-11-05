using System;
using System.Collections.Generic;

namespace KeepyMax.Model
{
    public class MBDataReadedValues
    {
        public MBDataReadedValues()
        {
            this.MeasurType = new MeasurType();
            this.MeterParameters = new MeterParameters();
        }
        public string ListeningGuid { get; set; }
        public MeasurType MeasurType { get; set; }
        public MeterParameters MeterParameters { get; set; }
        public float ListeningValue { get; set; }
    }

    public class Reading
    {
        public Guid DeviceID { get; set; }

        public List<MBDataReadedValues> ListeningValues { get; set; }
    }
}
