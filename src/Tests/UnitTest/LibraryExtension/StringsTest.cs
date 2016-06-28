using NUnit.Framework;
using QSP.LibraryExtension;
using System;
using static QSP.LibraryExtension.Strings;

namespace UnitTest.LibraryExtensionTest
{
    [TestFixture]
    public class StringsTest
    {
        [Test]
        public void ReplaceAnyTest()
        {
            Assert.IsTrue("".ReplaceAny(new char[] { '0', '1' }, "abc") == "");

            Assert.IsTrue(
                "0123456789".ReplaceAny(new char[] { '0', '1', '9' }, "ab")
                == "abab2345678ab");
            Assert.IsTrue(
                "0123456789".ReplaceAny(new char[] { '9', '1', '9', '0' }, "ab")
                == "abab2345678ab");
        }

        [Test]
        public void RemoveHtmlTagsTest()
        {
            Assert.IsTrue("<shouldRemoveThis>".RemoveHtmlTags() == "");
            Assert.IsTrue("123<456>789".RemoveHtmlTags() == "123789");
        }

        [Test]
        public void NthOccurenceCorrectnessTest()
        {
            string str = "1235444abc4565656abc456566abc5651561abc15";
            string target = "abc";

            Assert.AreEqual(7, str.NthOccurence(target, 1));
            Assert.AreEqual(17, str.NthOccurence(target, 2));

            str = "1234a1234aa1234bb1234";
            target = "1234";

            Assert.AreEqual(0, str.NthOccurence(target, 1));
            Assert.AreEqual(5, str.NthOccurence(target, 2));
            Assert.AreEqual(11, str.NthOccurence(target, 3));
            Assert.AreEqual(17, str.NthOccurence(target, 4));

        }

        [Test]
        public void NthOccurenceCannotFoundTest()
        {
            string str = "1235444abc4565656abc456566abc5651561abc15";
            string target = "123456";

            Assert.AreEqual(-1, str.NthOccurence(target, 0));
            Assert.AreEqual(-1, str.NthOccurence(target, 1));
            Assert.AreEqual(-1, str.NthOccurence(target, 2));
        }

        [Test]
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

        [Test]
        public void IndicesOfTest_NotFound()
        {
            string str = "12312312312312";
            var result = str.IndicesOf("abc", 0, str.Length - 1);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void IndicesOfTest_TestIndex()
        {
            string str = "010101010";
            var result = str.IndicesOf("01", 1, str.Length - 1);

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(2, result[0]);
            Assert.AreEqual(4, result[1]);
            Assert.AreEqual(6, result[2]);

        }

        [Test]
        public void IndicesOfTest_TestCount()
        {
            string str = "010101010";
            var result = str.IndicesOf("10", 0, 3);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0]);
        }

        [Test]
        public void ShiftToRightValidCount()
        {
            string s = @"123
456
789";

            string expected = @"   123
   456
   789";

            Assert.IsTrue(s.ShiftToRight(3) == expected);
        }

        [Test]
        public void ShiftToRightInvalidCount()
        {
            string s = @"123";

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            s.ShiftToRight(-5));
        }
    }
}
