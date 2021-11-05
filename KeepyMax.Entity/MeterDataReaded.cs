using System.Collections.Generic;

namespace KeepyMax.Model
{
    public class MeterDataReaded
    {
        public MeterDataReaded()
        {
            this.GeneralConfig = new GeneralConfig();
            this.MBDataReadedValuesL = new List<MBDataReadedValues>();
        }
        public System.Guid ListeningGuid { get; set; }
        public GeneralConfig GeneralConfig { get; set; }
        public int Timestamp { get; set; }
        public ushort DataSendedStatus { get; set; }
        public List<MBDataReadedValues> MBDataReadedValuesL { get; set; }
    }
}
