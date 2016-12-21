using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QSP.LibraryExtension.Tasks;

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

                Func<Task> task0 = () => Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(200);
                    results.Add(0);
                });

                Func<Task> task1 = () => Task.Factory.StartNew(() =>
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

        [Test]
        public void CancelCurrentTaskCleanupShouldBeDone()
        {
            var results = new List<int>();

            var test = new Thread(() =>
            {
                var q = new TaskQueue();

                var ts = new CancellationTokenSource();
                Action cleanup = () => results.Add(42);

                Func<Task> task = async () =>
                {
                    await Task.Factory.StartNew(() => Thread.Sleep(200));

                    if (ts.IsCancellationRequested)
                    {
                        ts.Token.ThrowIfCancellationRequested();
                    }

                    results.Add(0);
                };

                q.Add(task, ts, cleanup);

                // Cancel the task.
                q.CancelCurrentTask();
            });

            test.Start();

            // Let tasks finish.
            Thread.Sleep(400);

            Assert.IsTrue(results.SequenceEqual(new[] { 42 }));
        }

        [Test]
        public void CancelAllTasksCleanupAndClearEntireQueue()
        {
            var results = new List<int>();

            var test = new Thread(() =>
            {
                var q = new TaskQueue();

                var ts = new CancellationTokenSource();
                Action cleanup = () => results.Add(42);

                Func<Task> task0 = async () =>
                {
                    await Task.Factory.StartNew(() => Thread.Sleep(200));

                    if (ts.IsCancellationRequested)
                    {
                        ts.Token.ThrowIfCancellationRequested();
                    }

                    results.Add(0);
                };

                Func<Task> task1 = () => Task.Factory.StartNew(() =>
                {
                    results.Add(1);
                });

                q.Add(task0, ts, cleanup);
                q.Add(task1, new CancellationTokenSource(), () => { });

                // Cancel all tasks.
                q.CancelAllTasks();
            });

            test.Start();

            // Let tasks finish.
            Thread.Sleep(400);

            Assert.IsTrue(results.SequenceEqual(new[] { 42 }));
        }
    }
}