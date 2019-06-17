using System;
using P72CommonLib.Entities;

namespace P72OilTest.Domain.Entities
{
    public class Well
    {
        private readonly int _intialOutput;
        private readonly IDayCounter _counter;
        private readonly int _rateOfDecrease;
        private int startDay;
        public bool IsActive { get; protected set; }


        public Well(int intialOutput, int rateOfDecrease, IDayCounter counter)
        {
            _intialOutput = intialOutput;
            _counter = counter;
            _rateOfDecrease = rateOfDecrease;
            startDay = _counter.CurrentDay;
            IsActive = true;
        }

        public int CurrentAmount()
        {
           var currentAmount = Math.Max(_intialOutput - (_counter.CurrentDay - startDay) * _rateOfDecrease, 0);

           if (currentAmount == 0)
               IsActive = false;

           return currentAmount;
        }
    }
}
