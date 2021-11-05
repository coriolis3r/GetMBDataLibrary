using System;
using System.Collections.Generic;

namespace KeepyMax.Model
{
    public class MeterListening
    {
        public MeterListening()
        {
            this.ListeningValue = new List<ListeningValue>();
        }
        public Guid ListeningID { get; set; }
        public string MeterSerial { get; set; }
        public List<ListeningValue> ListeningValue { get; set; }
        public DateTime ListeningDate { get; set; }
    }
}
