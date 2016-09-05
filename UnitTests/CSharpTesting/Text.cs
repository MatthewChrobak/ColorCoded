using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CSharpTesting
{
    [TestClass]
    public class Text
    {
        [TestMethod]
        public void SingleComment() {
            string code = "//this is a comment\r\n";
            string expected = "<span class=\"comment\">//this is a comment</span>\r\n";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MultiLineComment() {
            string code = "/*this is a comment\r\n*\r\n*\r\n*/";
            string expected = "<span class=\"comment\">/*this is a comment\r\n*\r\n*\r\n*/</span>";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void InlineComment() {
            string code = "var x = \"100\" + // test\r\n\t\"200\";";
            string expected = "<span class=\"keyword\">var</span> x = <span class=\"string\">\"100\"</span> + <span class=\"comment\">// test</span>\r\n\t<span class=\"string\">\"200\"</span>;";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }
    }
}
