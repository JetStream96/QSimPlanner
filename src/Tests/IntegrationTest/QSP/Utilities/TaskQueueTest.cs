using NUnit.Framework;
using QSP.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IntegrationTest.QSP.Utilities
{
    [TestFixture]
    public class TaskQueueTest
    {
        [Test]
        public void TasksShouldRunInInsertionOrder()
        {
            var results = new List<int>();

            var test = new Thread(() =>
            {
                var q = new TaskQueue();

                var task0 = new Task(() =>
                {
                    Thread.Sleep(200);
                    results.Add(0);
                });

                var task1 = new Task(() =>
                {
                    results.Add(1);
                });

                q.Add(task0, new CancellationTokenSource(), () => { });
                q.Add(task1, new CancellationTokenSource(), () => { });
            });

            test.Start();

            // Let tasks finish.
            Thread.Sleep(400);

            Assert.IsTrue(results.SequenceEqual(new[] { 0, 1 }));
        }
    }
}