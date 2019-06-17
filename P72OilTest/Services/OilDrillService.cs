using System;
using System.Collections.Generic;
using System.Linq;
using P72CommonLib.Entities;
using P72OilTest.Configuration;
using P72OilTest.Domain.Entities;

namespace P72OilTest.Services
{
    public interface IP72Service
    {
        void Run();
    }

    public class OilDrillService : IP72Service
    {
        private readonly IOilServiceConfig _oilServiceConfig;
        private readonly IDayCounter _dayCounter;
        private readonly IWellManager _wellManager;
        private bool _steadyState;
        private Drill[] _drills;
        private Dictionary<int, int> _previousPeriodOutput;
        private Dictionary<int, int> _currentPeriodOutput; 


        public OilDrillService(IOilServiceConfig oilServiceConfig, IDayCounter dayCounter,  IWellManager wellManager, IDrillManager drillManager)
        {
            _oilServiceConfig = oilServiceConfig;
            _dayCounter = dayCounter;
            _wellManager = wellManager;
            _drills = drillManager.Drills;
            _previousPeriodOutput = new Dictionary<int, int>(_oilServiceConfig.DrillPeriod);
            _currentPeriodOutput = new Dictionary<int, int>(_oilServiceConfig.DrillPeriod);
        }

        public void Run()
        {
            DisplaySettings();
            LoopThroughDays();
        }

        private void LoopThroughDays()
        {
            int wellProduction = 0;
            int activeWells = 0;
            _currentPeriodOutput[0] = 0;

            while (!_steadyState)
            {
                Console.WriteLine($"Day: {_dayCounter.CurrentDay}");
                Console.WriteLine($"    Well Production: {wellProduction}");
                Console.WriteLine($"    Wells Active: {activeWells}");

                _dayCounter.Increment();
                wellProduction = _wellManager.ReportTotalWellProduction();
                activeWells = _wellManager.CurrentWells.Count();
                _currentPeriodOutput[_dayCounter.CurrentDay] = wellProduction;

                if ((_dayCounter.CurrentDay + 1) % _oilServiceConfig.DrillPeriod == 0)
                {
                    if (!(_steadyState = ComparePreviousRuns(_currentPeriodOutput, _previousPeriodOutput)))
                    {
                        _previousPeriodOutput = _currentPeriodOutput;
                        _currentPeriodOutput = new Dictionary<int, int>(_oilServiceConfig.DrillPeriod);
                    }
                }
            }

            var maxOutput = _previousPeriodOutput.Values.Max();
            var dayOfMaxOutput = _previousPeriodOutput.Keys.First(c => _previousPeriodOutput[c] == maxOutput);

            Console.WriteLine($"A steady state was reached on day {dayOfMaxOutput}");
            Console.WriteLine($"The max output was {maxOutput}");
        }

        private bool ComparePreviousRuns(Dictionary<int, int> currentPeriod, Dictionary<int, int> previousPeriod)
        {
            if (currentPeriod.Count == 0 || previousPeriod.Count == 0)
                return false;

            foreach (var key in currentPeriod.Keys)
            {
                if (currentPeriod[key] != previousPeriod[key - _oilServiceConfig.DrillPeriod])
                    return false;
            }

            return true;
        }

        private void DisplaySettings()
        {
            Console.WriteLine($"Drills: {_oilServiceConfig.Drills}");
            Console.WriteLine($"DrillPeriod: {_oilServiceConfig.DrillPeriod}");
            Console.WriteLine($"MaxOutput: {_oilServiceConfig.MaxOutput}");
            Console.WriteLine($"DecayRate: {_oilServiceConfig.DecayRate}");
            Console.WriteLine($"Day: {_dayCounter.CurrentDay}");
        }
    }
}
