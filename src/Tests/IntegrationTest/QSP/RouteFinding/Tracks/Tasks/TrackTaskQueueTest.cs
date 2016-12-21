using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Tasks;

namespace IntegrationTest.QSP.RouteFinding.Tracks.Tasks
{
    [TestFixture]
    public class TrackTaskQueueTest
    {
        [Test]
        public void AddTaskTest()
        {
            var results = new List<string>();

            var test = new Thread(() =>
            {
                var q = new TrackTaskQueue();

                var ts0 = new CancellationTokenSource();

                Func<Task> task0 = async () =>
                {
                    results.Add("Start 0");

                    await Task.Factory.StartNew(() => Thread.Sleep(200));
                    if (ts0.IsCancellationRequested)
                    {
                        ts0.Token.ThrowIfCancellationRequested();
                    }

                    results.Add("Finish 0");
                };

                q.Add(task0, ts0, () => results.Add("Cancel 0"));

                var ts1 = new CancellationTokenSource();

                Func<Task> task1 = async () =>
                {
                    results.Add("Start 1");

                    await Task.Factory.StartNew(() => Thread.Sleep(200));
                    if (ts1.IsCancellationRequested)
                    {
                        ts1.Token.ThrowIfCancellationRequested();
                    }

                    results.Add("Finish 1");
                };

                q.Add(task1, ts1, () => results.Add("Cancel 1"));

                var ts2 = new CancellationTokenSource();

                Func<Task> task2 = () => Task.Factory.StartNew(() =>
                {
                    results.Add("Start 2");

                    if (ts2.IsCancellationRequested)
                    {
                        ts2.Token.ThrowIfCancellationRequested();
                    }

                    results.Add("Finish 2");
                });

                q.Add(task2, ts2, () => results.Add("Cancel 2"));
            });

            test.Start();

            // Let the test finish.
            Thread.Sleep(400);

            // Task0 was started and canceled. Task1 is removed from queue when task2 was added. 
            // So task1 never starts. Task2 runs and finishes. 
            var expected = new[] { "Start 0", "Cancel 0", "Start 2", "Finish 2" };

            Assert.IsTrue(results.SequenceEqual(expected));
        }
    }
}