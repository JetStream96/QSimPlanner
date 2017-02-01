using NUnit.Framework;
using QSP.Common;
using System;
using System.IO;
using IntegrationTest.Util;
using static QSP.LibraryExtension.FileNameGenerator;
using static QSP.LibraryExtension.Paths;

namespace IntegrationTest.QSP.LibraryExtension
{
    [TestFixture, SingleThreaded]
    public class FileNameGeneratorTest
    {
        public static string FolderPath()
        {
            var baseDir = CommonUtil.AssemblyDirectory();
            return Path.Combine(baseDir, "QSP/LibraryExtension/FileNameGeneratorTesting");
        }

        [Test]
        public void ContainIllegalCharShouldThrowException()
        {
            PrepareEmptyFolder();
            Assert.Throws<ArgumentException>(() =>
                Generate(FolderPath(), @"?!!//\", ".txt", i => i.ToString()));
        }

        [Test]
        public void NoCollisionShouldReturnOriginalName()
        {
            PrepareEmptyFolder();
            var expected = Path.Combine(FolderPath(), "abc123.txt");
            var result = Generate(FolderPath(), @"abc123", ".txt", i => i.ToString());

            Assert.IsTrue(GetUri(expected).Equals(GetUri(result)));
        }

        [Test]
        public void RenameToAvoidCollisionTest()
        {
            PrepareEmptyFolder();
            File.Create(Path.Combine(FolderPath(), "123.txt")).Close();
            File.Create(Path.Combine(FolderPath(), "123(1).txt")).Close();

            var expected = Path.Combine(FolderPath(), "123(2).txt");
            var result = Generate(FolderPath(), @"123", ".txt", i => $"({i})");

            Assert.IsTrue(GetUri(expected).Equals(GetUri(result)));
        }

        [Test]
        public void BadNumberFormatShouldNotEnterInfiniteLoop()
        {
            PrepareEmptyFolder();
            File.Create(Path.Combine(FolderPath(), "123.txt")).Close();
            Func<int, string> badFormat = i => "";

            Assert.Throws<NoFileNameAvailException>(() =>
                Generate(FolderPath(), @"123", ".txt", badFormat));
        }

        private static void PrepareEmptyFolder()
        {
            if (Directory.Exists(FolderPath()))
            {
                Directory.Delete(FolderPath(), true);
            }

            Directory.CreateDirectory(FolderPath());
        }
    }
}
