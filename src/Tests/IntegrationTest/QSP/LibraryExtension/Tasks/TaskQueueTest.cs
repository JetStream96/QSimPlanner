using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using QSP.LibraryExtension.Tasks;

namespace IntegrationTest.QSP.LibraryExtension.Tasks
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

                Func<Task> task0 = () => Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(200);
                    results.Add(0);
                });

                Func<Task> task1 = () => Task.Factory.StartNew(() =>
                {
                    results.Add(1);
                });

                q.Add(task0);
                q.Add(task1);
            });

            test.Start();

            // Let tasks finish.
            Thread.Sleep(400);

            Assert.IsTrue(results.SequenceEqual(new[] { 0, 1 }));
        }
    }
}