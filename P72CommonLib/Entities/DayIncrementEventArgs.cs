using System;

namespace P72CommonLib.Entities
{
    public class DayIncrementEventArgs : EventArgs
    {
        public int DaysIncremented { get; }

        public DayIncrementEventArgs(int daysIncremented)
        {
            DaysIncremented = daysIncremented;
        }
    }
}
