using System;

namespace KeepyMax.Model
{
    public class MeterMBParConfig
    {
        public MeterMBParConfig()
        {
            this.Models = new Models();
            this.MeasurRange = new MeasurRange();
            this.MeasurMeterList = new MeasurMeterList();
        }

        public Models Models { get; set; }
        public MeasurRange MeasurRange { get; set; }
        public MeasurMeterList MeasurMeterList { get; set; }
    }
}
