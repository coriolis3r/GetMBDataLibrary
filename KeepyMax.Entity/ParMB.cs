using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepyMax.Model
{
    public class ParMB
    {
        public ParMB()
        {
            this.mbregisters = new List<MBRegisters>();
        }

        public int MeasureTypeId { get; set; }
        public List<MBRegisters> mbregisters { get; set; }
    }
}
