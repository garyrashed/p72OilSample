using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P72OilTest.Configuration;
using P72OilTest.Services;

namespace P72OilTest.Domain.Entities
{
    public interface IDrillManager
    {
        Drill[] Drills { get; }
    }

    public class DrillManager : IDrillManager
    {
        private readonly IOilServiceConfig _oilServiceConfig;
        private readonly IDrillFactory _drillFactory;
        public Drill[] Drills { get; }

        public DrillManager(IOilServiceConfig oilServiceConfig, IDrillFactory  drillFactory)
        {
            _oilServiceConfig = oilServiceConfig;
            _drillFactory = drillFactory;
            Drills = _drillFactory.MakeDrills(_oilServiceConfig.Drills, _oilServiceConfig.DrillPeriod);
        }


    }
}
