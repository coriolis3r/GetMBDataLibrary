using System;

namespace KeepyMax.Model
{
    public class ModbusMapValues
    {
        public int ParameterID { get; set; }
        public string Name { get; set; }
        public int MBRegID { get; set; }
        public float Multiplier { get; set; }
        public float Value { get; set; }
        
    }
}
