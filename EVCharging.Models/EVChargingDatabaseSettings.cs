using System;
using System.Collections.Generic;
using System.Text;

namespace EVCharging.Models
{
    public class EVChargingDatabaseSettings : IEVChargingDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IEVChargingDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
