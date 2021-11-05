using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeepyMax.Model;

namespace KeepyMax.Controller.DBControlI
{
    public interface AlertsControlI
    {
        bool SaveConnectionAlertList(List<ConnectionAlertList> calL);
        List<Alerts> ReadRangeAlerts();
        bool DeleteAlertsRange();
        bool GetRangeAlerts(int Timestamp);
        List<string> ReadConnectionAlerts(int Timestamp);
        bool InsertAlertsConfig(List<Alerts> ALertsL);
        bool DeleteAlertsConfig();
        bool DeleteConnectionAlert(int ts);
    }
}
