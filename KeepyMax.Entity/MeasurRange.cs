using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepyMax.Model
{
    public class MeasurRange
    {
        public MeasurRange()
        {
            this.MeasurType = new MeasurType();
            //this.Models = new Models();
        }

        //public Models Models { get; set; }
        public MeasurType MeasurType { get; set; }
        public string Range { get; set; }

    }
}
