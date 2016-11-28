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

            Assert.IsTrue("0123456789".ReplaceAny(new char[] { '0', '1', '9' }, "ab")
                == "abab2345678ab");

            Assert.IsTrue("0123456789".ReplaceAny(new char[] { '9', '1', '9', '0' }, "ab")
                == "abab2345678ab");
        }

        [Test]
        public void RemoveHtmlTagsTest()
        {
            Assert.IsTrue("<shouldRemoveThis>".RemoveHtmlTags() == "");
            Assert.IsTrue("123<456>789".RemoveHtmlTags() == "123789");
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

        [Test]
        public void TrimEmptyLinesTest()
        {
            var s1 = "123";
            Assert.IsTrue(s1 == s1.TrimEmptyLines());

            var s2 = "  \n\t\n  \n456\n \n789\n\t";
            Assert.IsTrue("456\n \n789" == s2.TrimEmptyLines());
        }
    }
}
