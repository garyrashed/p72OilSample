
using P72CommonLib.Entities;

namespace P72OilTest.Domain.Entities
{
    public class Drill
    {
        private readonly int _drillPeriod;
        private readonly IDayCounter _counter;
        private readonly IWellManager _wellManager;
        private int _currentPeriod;
        private bool _startDrilling;
        private bool _drillUpdated;
        private int startDate = 0;
        public bool DrillUpdate
        {
            get { return _drillUpdated; }
        }

        public int CurrentPeriod
        {
            get { return _currentPeriod; }
        }

        public Drill(int drillPeriod, IDayCounter counter, IWellManager wellManager)
        {
            _drillPeriod = drillPeriod;
            _counter = counter;
            _wellManager = wellManager;
            _currentPeriod = 0;
            _counter.DayChange += _counter_DayChange;
            startDate = _counter.CurrentDay;
        }

        private void _counter_DayChange(object sender, DayIncrementEventArgs e)
        {
            var daysToIncrement = e.DaysIncremented;
            UpdateDrillState(daysToIncrement);
        }

        public int DrillPeriod
        {
            get => _drillPeriod;
        }

        private void UpdateDrillStateForADay()
        {
            _currentPeriod++;

            if (_currentPeriod % DrillPeriod == 0 && _currentPeriod / DrillPeriod == 1)
            {
                _wellManager.AddWells(1);
                _currentPeriod = 0;
            }
               
        }

        private void UpdateDrillState(int daysToUpdate)
        {
            for (int i = 0; i < daysToUpdate; i++)
            {
                UpdateDrillStateForADay();
            }
        }
    }

    public interface IDrillFactory
    {
        Drill[] MakeDrills(int drillCount, int drillPeriod);
    }
    
    public class DrillFactory : IDrillFactory
    {
        private readonly IDayCounter _counter;
        private readonly IWellManager _wellManager;

        public DrillFactory(IDayCounter counter, IWellManager wellManager)
        {
            _counter = counter;
            _wellManager = wellManager;
        }

        public Drill[] MakeDrills(int drillCount, int drillPeriod)
        {
            var drills =  new Drill[drillCount];
            for (int i = 0; i < drillCount; i++)
            {
                drills[i] = new Drill(drillPeriod, _counter, _wellManager);
            }

            return drills;
        }
    }

}
