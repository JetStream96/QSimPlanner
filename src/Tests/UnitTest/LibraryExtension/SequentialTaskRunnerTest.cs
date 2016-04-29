using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.LibraryExtension;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTest.LibraryExtension
{
    [TestClass]
    public class SequentialTaskRunnerTest
    {
        [TestMethod]
        public void RunTaskSequenceCorrectness()
        {
            var runner = new SequentialTaskRunner();

            var list = new List<int>();

            var task1 = new Task(() =>
            {
                Thread.Sleep(100);
                list.Add(1);
            });

            var task2 = new Task(() => list.Add(2));

            runner.AddTask(task1);
            runner.AddTask(task2);

            task2.Wait();

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
        }
    }
}
