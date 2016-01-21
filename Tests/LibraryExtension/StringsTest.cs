using Microsoft.VisualStudio.TestTools.UnitTesting;
using static QSP.LibraryExtension.Strings;
namespace Tests.LibraryExtensionTest
{
    [TestClass]
    public class StringsTest
    {
        [TestMethod]
        public void RemoveHtmlTagsTest()
        {
            Assert.IsTrue("<shouldRemoveThis>".RemoveHtmlTags() == "");
            Assert.IsTrue("123<456>789".RemoveHtmlTags() == "123789");
        }
    }
}
