using System.Collections.Generic;
using P72CommonLib.Entities;
using P72OilTest.Configuration;

namespace P72OilTest.Domain.Entities
{
    public interface IWellManager
    {
        void AddWells(int wellCount);
        int ReportTotalWellProduction();
        IEnumerable<Well> CurrentWells { get; }
    }

    public class WellManager : IWellManager
    {
        private readonly IDayCounter _dayCounter;
        private readonly IOilServiceConfig _configManager;
        private object wellLock = new object();

        private List<Well> currentWells;
        public IEnumerable<Well> CurrentWells
        {
            get { return currentWells; }
        }
         
        public WellManager(IDayCounter dayCounter, IOilServiceConfig configManager)
        {
            _dayCounter = dayCounter;
            _configManager = configManager;
            currentWells = new List<Well>();
        }

        public void AddWells(int amountOfWells)
        {
            for (int i = 0; i < amountOfWells; i++)
            {
                currentWells.Add(new Well(_configManager.MaxOutput, _configManager.DecayRate, _dayCounter));
            }
        }

        public int ReportTotalWellProduction()
        {
            var totalWellOutput = 0;
            //could parallelize here for better performance, but would need to lock the entire list 
            //when removing. 
            for (int i = currentWells.Count - 1; i >= 0; i--)
            {
                var currentWellAmount = currentWells[i].CurrentAmount();
                totalWellOutput += currentWellAmount;

                if(currentWellAmount == 0)
                    currentWells.RemoveAt(i);
            }

            return totalWellOutput;
        }
    }
}
