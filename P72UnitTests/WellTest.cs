using System;
using System.Net.NetworkInformation;
using NUnit;
using NUnit.Framework;
using Moq;
using NUnit.Framework.Internal;
using P72CommonLib.Entities;
using P72OilTest.Domain.Entities;


namespace P72UnitTests
{
    [TestFixture]
    public class WellTest
    {
        private Mock<IDayCounter> _dayCounter;


        [SetUp]
        public void Init()
        {
            _dayCounter = new Mock<IDayCounter>();
        }

        [Test]
        public void WellCreated_DayZero_ReturnsFullAmount()
        {
            var testOutput = 10;
            var outPut = WellDays(testOutput, 10, 5, 0, _dayCounter);
            Assert.AreEqual(testOutput, outPut.CurrentAmount());
        }

        [Test]
        public void WellCreated_Day3_ReturnsPartialAmount()
        {
            var testOutput = 10;
            var expectedOutput = 2;
            var outPut = WellDays(testOutput, 2, 5, 4, _dayCounter);
            Assert.AreEqual(expectedOutput, outPut.CurrentAmount());
        }

        [Test]
        public void WellCreated_Day5_ReturnsZeroAndMarkedInActive()
        {
            var testOutput = 10;
            var expectedOutput = 0;
            var outPut = WellDays(testOutput, 2, 5, 5, _dayCounter);
            Assert.AreEqual(expectedOutput, outPut.CurrentAmount());
            Assert.IsTrue(outPut.IsActive == false);
        }

        [Test]
        public void WellCreated_Day6_ReturnsZeroAndMarkedInActive()
        {
            var testOutput = 10;
            var expectedOutput = 0;
            var outPut = WellDays(testOutput, 2, 5, 6, _dayCounter);
            Assert.AreEqual(expectedOutput, outPut.CurrentAmount());
            Assert.IsTrue(outPut.IsActive == false);
        }

        private Well WellDays(int initialOutput, int rateOfDecrease,int initialDay,  int daysForward,  Mock<IDayCounter> _dayCounter)
        {
            _dayCounter.Setup(c => c.CurrentDay).Returns(initialDay);
            var well = new Well(initialOutput, rateOfDecrease, _dayCounter.Object);

            if (daysForward > 0)
                _dayCounter.Setup(c => c.CurrentDay).Returns(initialDay + daysForward);

  
            return well;
        }


    }
}
