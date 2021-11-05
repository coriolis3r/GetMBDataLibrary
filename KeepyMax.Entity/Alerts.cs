using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepyMax.Model
{
    public class Alerts
    {
        public string AlertIdGuid { get; set; }
        public string SerialMeter { get; set; }
        public int ParameterId { get; set; }
        public int AlarmTypeId { get; set; }
        public double MinValueAl { get; set; }
        public double MaxValueAl { get; set; }
        public int Timestamp { get; set; }
        public double ReadValue { get; set; }
    }
}
