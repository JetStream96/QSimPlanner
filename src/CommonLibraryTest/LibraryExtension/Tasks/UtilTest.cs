using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static CommonLibrary.LibraryExtension.Tasks.Util;

namespace CommonLibraryTest.LibraryExtension.Tasks
{
    [TestFixture]
    public class UtilTest
    {
        [Test]
        public async Task RunPeriodicIndeedRunsAtInterval()
        {
            var list = new List<int>();

            NoAwait(() => RunPeriodic(() => list.Add(0),
                                      new TimeSpan(0, 0, 0, 0, 50),
                                      new CancellationToken()));

            await Task.Delay(200);

            Assert.True(list.Count > 1);
        }

        [Test]
        public async Task RunPeriodicAsyncIndeedRunsAtInterval()
        {
            var list = new List<int>();

            NoAwait(() => RunPeriodicAsync(() => list.Add(0),
                new TimeSpan(0, 0, 0, 0, 50),
                new CancellationToken()));

            await Task.Delay(200);

            Assert.True(list.Count > 1);
        }

        [Test]
        public async Task RunAsyncWithTimeoutTestFuncFinishes()
        {
            Func<int> wait = () =>
            {
                Thread.Sleep(10);
                return 4;
            };

            var (timedout, res) = await RunAsyncWithTimeout(wait, 200);

            Assert.IsFalse(timedout);
            Assert.AreEqual(4, res);
        }

        [Test]
        public async Task RunAsyncWithTimeoutTestTimeout()
        {
            Func<int> wait = () =>
            {
                Thread.Sleep(200);
                return 4;
            };

            var (timedout, res) = await RunAsyncWithTimeout(wait, 10);

            Assert.IsTrue(timedout);
        }
    }
}
