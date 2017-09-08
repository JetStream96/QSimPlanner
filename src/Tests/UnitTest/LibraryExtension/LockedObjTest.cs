using NUnit.Framework;
using QSP.LibraryExtension;
using System.Collections.Generic;
using System.Threading;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class LockedObjTest
    {
        [Test]
        public static void ObjTest()
        {
            var list = new List<int>() { 0 };
            var obj = new LockedObj<List<int>>(list);
            int? value = null;

            obj.Execute(x =>
            {
                new Thread(() =>
                {
                    x[0] = 0;
                    Thread.Sleep(1000);
                    value = x[0];                    
                }).Start();
            });

            obj.Execute(x =>
            {
                x[0] = 10;
            });

            while (value == null) Thread.Sleep(100);
            Assert.AreEqual(0, value);
        }
    }
}
