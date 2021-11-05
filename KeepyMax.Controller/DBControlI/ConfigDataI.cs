using System;
using System.Collections.Generic;
using KeepyMax.Model;

namespace KeepyMax.Controller.DBControlI
{
    public interface ConfigDataI
    {
        //List<GeneralConfig> GetGeneralConfig(int ComTypeId);
        //SerialCom GetSerialCom();
        //List<MBRegisters> GetParametersMetersList(int MeterId, MBRange range);
        bool SaveMeterListening(MeterDataReaded mdr);
        bool SaveListeningValues(List<MBDataReadedValues> mbdrv);
    }
}
