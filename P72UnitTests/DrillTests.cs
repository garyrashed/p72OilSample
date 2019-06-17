using System.Linq;
using Moq;
using NUnit.Framework;
using P72CommonLib.Entities;
using P72OilTest.Domain.Entities;

namespace P72UnitTests
{
    [TestFixture()]
    public class DrillTests
    {
        private Mock<IDayCounter> dayCounterMock;
        private Mock<IWellManager> wellManagerMock;
        private Drill[] drills;
        private int day;
        private int wellcount;

        [SetUp]
        public void Setup()
        {
            day = 0;
            wellcount = 0;
            dayCounterMock = new Mock<IDayCounter>();
            wellManagerMock = new Mock<IWellManager>();

            dayCounterMock.Setup(c => c.CurrentDay).Returns(day);
            dayCounterMock.Setup(c => c.Increment())
                .Callback(() => dayCounterMock.Setup(c => c.CurrentDay).Returns(++day));

            wellManagerMock.Setup(c => c.AddWells(It.IsAny<int>())).Callback<int>(c => wellcount += c);
        }

        [Test]
        public void MakeDrills_throughFactory_IsSuccessfull()
        {
            var drills =  target(4, 7);

            Assert.AreEqual(4, drills.Length);
            foreach (var drill in drills)
            {
                Assert.IsTrue(drill.DrillPeriod == 7);
            }
        }

        [Test]
        public void MakeDrills_UpdateOneDay_NoWellsmade()
        {
            int calls = 0;
            wellManagerMock.Setup(c => c.AddWells(It.IsAny<int>())).Callback(() => calls++);
            var drills = target(4, 7);

            dayCounterMock.Object.Increment();
            dayCounterMock.Raise(m=> m.DayChange += null, this, new DayIncrementEventArgs(1));

            Assert.AreEqual(0, calls);
        }

        [Test]
        public void MakeDrills_UpdateOneDay_Wellsmade()
        {
            int calls = 0;
            wellManagerMock.Setup(c => c.AddWells(It.IsAny<int>())).Callback(() => calls++);
            var drills = target(4, 7);

            dayCounterMock.Object.Increment();
            dayCounterMock.Raise(m => m.DayChange += null, this, new DayIncrementEventArgs(7));

            Assert.AreEqual(4, calls);
        }

        [Test]
        public void MakeDrills_UpdateThreeMultiples_Wellsmade()
        {
            int calls = 0;
            wellManagerMock.Setup(c => c.AddWells(It.IsAny<int>())).Callback(() => calls++);
            var drills = target(4, 7);

            dayCounterMock.Object.Increment();
            dayCounterMock.Raise(m => m.DayChange += null, this, new DayIncrementEventArgs(22));

            Assert.AreEqual(12, calls);
            Assert.IsFalse(drills.Any(c => c.CurrentPeriod != 1));
        }

        private Drill[] target(int drillCount, int drillPeriod)
        {
            DrillFactory f = new DrillFactory(dayCounterMock.Object, wellManagerMock.Object);
            return f.MakeDrills(drillCount, drillPeriod);
        }
    }
}
