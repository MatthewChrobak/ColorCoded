using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CSharpTesting
{
    [TestClass]
    public class EscapeChars
    {
        [TestMethod]
        public void SimpleAmpersand() {
            string code = "&";
            string expected = "&amp;";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DoubleAmpersand() {
            string code = "x && y";
            string expected = "x &amp;&amp; y";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GreaterThan() {
            string code = ">";
            string expected = "&gt;";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void LessThan() {
            string code = "<";
            string expected = "&lt;";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void NotEqual() {
            string code = "<>";
            string expected = "&lt;&gt;";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ReverseNotEqual() {
            string code = "><";
            string expected = "&gt;&lt;";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ComplexComparison() {
            string code = ">><<<><>";
            string expected = "&gt;&gt;&lt;&lt;&lt;&gt;&lt;&gt;";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }
    }
}
