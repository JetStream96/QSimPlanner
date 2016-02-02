﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Tracks.Common.TDM.Parser;
using System.Linq;

namespace Tests.RouteFinding.Tracks.Common.TDM.Parser
{
    [TestClass]
    public class TdmParserTest
    {
        [TestMethod]
        public void HasBothRtsAndRmk()
        {
            string msg =
@"TDM TRK [Ident]
[TimeStart] [TimeEnd]
[Main Route]
[Main Route continued]
RTS/[RouteFrom/To 1]
[RouteFrom/To 2]
[RouteFrom/To 3]
RMK/[Remarks]
[Remarks continued]";

            var parser = new TdmParser(msg);

            var result = parser.Parse();

            Assert.IsTrue(result.Ident == "[Ident]");
            Assert.IsTrue(result.TimeStart == "[TimeStart]");
            Assert.IsTrue(result.TimeEnd == "[TimeEnd]");
            Assert.IsTrue(result.MainRoute == @"[Main Route]
[Main Route continued]
");
            Assert.IsTrue(Enumerable.SequenceEqual(result.ConnectionRoutes,
                            new string[] {"[RouteFrom/To 1]",
                                          "[RouteFrom/To 2]",
                                          "[RouteFrom/To 3]" }));
            Assert.IsTrue(result.Remarks == @"[Remarks]
[Remarks continued]");
        }

        [TestMethod]
        public void HasOnlyRts()
        {
            string msg =
@"TDM TRK [Ident]
[TimeStart] [TimeEnd]
[Main Route]
[Main Route continued]
RTS/[RouteFrom/To 1]
[RouteFrom/To 2]
[RouteFrom/To 3]";

            var parser = new TdmParser(msg);

            var result = parser.Parse();

            Assert.IsTrue(result.Ident == "[Ident]");
            Assert.IsTrue(result.TimeStart == "[TimeStart]");
            Assert.IsTrue(result.TimeEnd == "[TimeEnd]");
            Assert.IsTrue(result.MainRoute == @"[Main Route]
[Main Route continued]
");
            Assert.IsTrue(Enumerable.SequenceEqual(result.ConnectionRoutes,
                            new string[] {"[RouteFrom/To 1]",
                                          "[RouteFrom/To 2]",
                                          "[RouteFrom/To 3]" }));
            Assert.IsTrue(result.Remarks == "");
        }

        [TestMethod]
        public void HasOnlyRmk()
        {
            string msg =
@"TDM TRK [Ident]
[TimeStart] [TimeEnd]
[Main Route]
[Main Route continued]
RMK/[Remarks]
[Remarks continued]";

            var parser = new TdmParser(msg);

            var result = parser.Parse();

            Assert.IsTrue(result.Ident == "[Ident]");
            Assert.IsTrue(result.TimeStart == "[TimeStart]");
            Assert.IsTrue(result.TimeEnd == "[TimeEnd]");
            Assert.IsTrue(result.MainRoute == @"[Main Route]
[Main Route continued]
");
            Assert.AreEqual(0, result.ConnectionRoutes.Count);
            Assert.IsTrue(result.Remarks == @"[Remarks]
[Remarks continued]");
        }

        [TestMethod]
        public void HasNeitherRtsNorRmk()
        {
            string msg =
@"TDM TRK [Ident]
[TimeStart] [TimeEnd]
[Main Route]
[Main Route continued]
";

            var parser = new TdmParser(msg);

            var result = parser.Parse();

            Assert.IsTrue(result.Ident == "[Ident]");
            Assert.IsTrue(result.TimeStart == "[TimeStart]");
            Assert.IsTrue(result.TimeEnd == "[TimeEnd]");
            Assert.IsTrue(result.MainRoute == @"[Main Route]
[Main Route continued]
");
            Assert.AreEqual(0, result.ConnectionRoutes.Count);
            Assert.IsTrue(result.Remarks == "");
        }
    }
}
