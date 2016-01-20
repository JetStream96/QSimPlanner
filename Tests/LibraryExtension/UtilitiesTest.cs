using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.LibraryExtension;

namespace Tests
{

    [TestClass()]
    public class LibraryExtensionTesting
    {

        [TestMethod()]
        public void NthOccurenceCorrectnessTest()
        {
            string str = "1235444abc4565656abc456566abc5651561abc15";
            string target = "abc";

            Assert.AreEqual(7, Strings.NthOccurence(str, target, 1));
            Assert.AreEqual(17, Strings.NthOccurence(str, target, 2));

            str = "1234a1234aa1234bb1234";
            target = "1234";

            Assert.AreEqual(0, Strings.NthOccurence(str, target, 1));
            Assert.AreEqual(5, Strings.NthOccurence(str, target, 2));
            Assert.AreEqual(11, Strings.NthOccurence(str, target, 3));
            Assert.AreEqual(17, Strings.NthOccurence(str, target, 4));

        }

        [TestMethod()]
        public void NthOccurenceCannotFoundTest()
        {
            string str = "1235444abc4565656abc456566abc5651561abc15";
            string target = "123456";

            Assert.AreEqual(-1, Strings.NthOccurence(str, target, 0));
            Assert.AreEqual(-1, Strings.NthOccurence(str, target, 1));
            Assert.AreEqual(-1, Strings.NthOccurence(str, target, 2));
        }

        [TestMethod()]
        public void SubStringTest()
        {
            var str = "blablabla  123 4568 88 !! #$4832";

            Assert.AreEqual("blablabla123456888!!#$4832", str.Substring(0, str.Length, new char[] { ' ' }));
            Assert.AreEqual("blblbl  ", str.Substring(0, 11, new char[] { 'a' }));
            Assert.AreEqual("blablabla123456888#$4832", str.Substring(0, str.Length, new char[] { '!', ' ' }));
        }

        [TestMethod()]
        public void IndicesOfTest_Found()
        {
            string str = "12312312312312";
            var result = str.IndicesOf("12", 0, str.Length - 1);

            Assert.AreEqual(5, result.Count);

            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(3, result[1]);
            Assert.AreEqual(6, result[2]);
            Assert.AreEqual(9, result[3]);
            Assert.AreEqual(12, result[4]);

        }

        [TestMethod()]
        public void IndicesOfTest_NotFound()
        {
            string str = "12312312312312";
            var result = str.IndicesOf("abc", 0, str.Length - 1);

            Assert.AreEqual(0, result.Count);

        }

        [TestMethod()]
        public void IndicesOfTest_TestIndex()
        {
            string str = "010101010";
            var result = str.IndicesOf("01", 1, str.Length - 1);

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(2, result[0]);
            Assert.AreEqual(4, result[1]);
            Assert.AreEqual(6, result[2]);

        }

        [TestMethod()]
        public void IndicesOfTest_TestCount()
        {
            string str = "010101010";
            var result = str.IndicesOf("10", 0, 3);

            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(1, result[0]);

        }

    }
}
