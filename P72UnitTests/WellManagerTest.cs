using System;
using System.Linq;
using NUnit;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using P72OilTest.Domain.Entities;
using P72CommonLib;
using P72CommonLib.Entities;
using P72OilTest.Configuration;

namespace P72UnitTests
{
    [TestFixture()]
    public class WellManagerTest
    {
        private Mock<IDayCounter> _dayCounter;
        private Mock<IOilServiceConfig> _oilConfigMock;

        [SetUp]
        public void Init()
        {
            _dayCounter = new Mock<IDayCounter>();
            _oilConfigMock = new Mock<IOilServiceConfig>();
        }

        [Test]
        public void WellManager_AddsWells_Correctly()
        {
            var welltarget = Target();
            welltarget.AddWells(5);
            Assert.AreEqual(welltarget.CurrentWells.Count(), 5);
        }

        [Test]
        public void WellManager_GetTotalOnDayZero_ReturnsFullAmount()
        {

            var welltarget = Target();
            welltarget.AddWells(4);
            var immediateProduction = welltarget.ReportTotalWellProduction();

            Assert.AreEqual(40, immediateProduction);
        }

        [Test]
        public void WellManager_GetTotalOnDayOne_ReturnsPartionAmount()
        {
            var welltarget = Target();
            welltarget.AddWells(4);
            _dayCounter.Setup(c => c.CurrentDay).Returns(1);
            var immediateProduction = welltarget.ReportTotalWellProduction();

            Assert.AreEqual(32, immediateProduction);
        }

        [Test]
        public void WellManager_GetTotalOnDayWellsDryup_ReturnsZero()
        {
            var welltarget = Target();
            welltarget.AddWells(4);
            _dayCounter.Setup(c => c.CurrentDay).Returns(5);
            var immediateProduction = welltarget.ReportTotalWellProduction();

            Assert.AreEqual(0, immediateProduction);
            Assert.IsFalse(welltarget.CurrentWells.Any(c => c.IsActive));
        }

        [Test]
        public void WellManager_GetTotalOnDayaFTERWellsDryup_ReturnsZero()
        {
            var welltarget = Target();
            welltarget.AddWells(4);
            _dayCounter.Setup(c => c.CurrentDay).Returns(6);
            var immediateProduction = welltarget.ReportTotalWellProduction();

            Assert.AreEqual(0, immediateProduction);
           
            Assert.IsFalse(welltarget.CurrentWells.Any(c => c.IsActive));

            var secondRequest = welltarget.ReportTotalWellProduction();
            Assert.IsTrue(!welltarget.CurrentWells.Any());
        }

        private IWellManager Target()
        {
            _dayCounter.Setup(c => c.CurrentDay).Returns(0);
            _oilConfigMock.Setup(c => c.DecayRate).Returns(2);
            _oilConfigMock.Setup(c => c.DrillPeriod).Returns(7);
            _oilConfigMock.Setup(c => c.MaxOutput).Returns(10);
            _oilConfigMock.Setup(c => c.Drills).Returns(4);

            return new WellManager(_dayCounter.Object, _oilConfigMock.Object);
        }
    }
}
