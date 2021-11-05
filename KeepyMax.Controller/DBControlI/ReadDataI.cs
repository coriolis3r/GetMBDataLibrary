using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeepyMax.Model;

namespace KeepyMax.Controller.DBControlI
{
    public interface ReadDataI
    {
        List<MeterDataReaded> GetListeningMeters(int TimeStart);
        List<MBDataReadedValues> GetListeningValues(string ListeningGuid);
        bool GetPowerDemand(int Timestamp);
    }
}
