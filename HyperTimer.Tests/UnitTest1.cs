using System;
using System.Timers.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Timers;
using System.Timers.Providers;


namespace HyperTimerTest
{
    [TestClass]
    public class UnitTest1
    {
        private void Configure()
        {
            HyperTimer.Configure()
                .UseTimerServicesProvider(new DefaultTimerServicesProvider()); 
        }

        [TestMethod]
        public void Test_Repeat_Just()
        {
            int targetCyclesCompleted = 5;
            IHyperTimer timer = null;

            timer = HyperTimer.StartNew(500, null, (o, e) =>
            {
                Assert.AreEqual(timer.CyclesCompleted, targetCyclesCompleted);
                timer.Dispose();
            })
                .RepeatJust(targetCyclesCompleted);
        }

        [TestMethod]
        public void Test_Repeat_More()
        {
            int targetCyclesCompleted = 10;
            int incrementedCycles = 5;

            IHyperTimer timer = null;
            timer = HyperTimer.StartNew(500, null, (o, e) =>
            {
                Assert.AreEqual(timer.CyclesCompleted, targetCyclesCompleted + incrementedCycles);
                timer.Dispose();
            }).RepeatJust(targetCyclesCompleted)
                .Wait(1000)
                .RepeatMore(incrementedCycles);
        }

        [TestMethod]
        public void Test_Stop_In()
        {
            int targetCyclesCompleted = 10;
            int stopInCycles = 5;
            IHyperTimer timer = null;
            timer = HyperTimer.StartNew(1000, null, (o, e) =>
            {
                Assert.AreEqual(timer.CyclesCompleted, targetCyclesCompleted - stopInCycles);
                timer.Dispose();
            }).RepeatForever()
                .RepeatJust(targetCyclesCompleted)
                .Wait(500)
                .StopIn(stopInCycles);
        }

        [TestMethod]
        public void Test_Throttle()
        {
            int targetCyclesCompleted = 10;
            TimeSpan throttleTime = TimeSpan.FromSeconds(5);
            int intervalInMilliseconds = 1000;

            IHyperTimer timer = null;

            timer = HyperTimer.StartNew(intervalInMilliseconds, null, (o, e) =>
            {
                Assert.IsTrue(TimeSpan.FromMilliseconds(intervalInMilliseconds * targetCyclesCompleted) - throttleTime - timer.TotalElapsed < TimeSpan.FromSeconds(1));
                timer.Dispose();
            }).RepeatForever()
                .RepeatJust(targetCyclesCompleted)
                .Wait(500)
                .Throttle(TimeSpan.FromSeconds(5));
        }


        [TestMethod]
        public void Test_Delay()
        {
            int targetCyclesCompleted = 10;
            TimeSpan delayTime = TimeSpan.FromSeconds(5);
            int intervalInMilliseconds = 1000;

            IHyperTimer timer = null;

            timer = HyperTimer.StartNew(intervalInMilliseconds, null, (o, e) =>
            {
                Assert.IsTrue(TimeSpan.FromMilliseconds(intervalInMilliseconds * targetCyclesCompleted) + delayTime - timer.TotalElapsed < TimeSpan.FromSeconds(1));
                timer.Dispose();
            }).RepeatForever()
                .RepeatJust(targetCyclesCompleted)
                .Wait(500)
                .Delay(TimeSpan.FromSeconds(5));
        }
    }
}
