using System;


namespace P72CommonLib.Entities
{
    public interface IDayCounter
    {
        void Increment();

        int CurrentDay { get; }

        event EventHandler<DayIncrementEventArgs> DayChange;
    }

    public class DayCounter : IDayCounter
    {
        private int _currentDay;

        public DayCounter()
        {
            _currentDay = 0;
            DayChange += (sender, args) => { };
        }

        public int CurrentDay => _currentDay;

        public event EventHandler<DayIncrementEventArgs> DayChange;

        public void Increment()
        {
            _currentDay++;
            OnIncrement(new DayIncrementEventArgs(1));
        }
        
        protected void OnIncrement(DayIncrementEventArgs days)
        {
            DayChange.Invoke(this, days);
        }
    }
}