using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CSharpTesting
{
    [TestClass]
    public class Keywords
    {
        [TestMethod]
        public void SingleKeywords() {
            foreach (string keyword in CSharp.Format.Keywords) {
                string code = keyword;
                string expected = string.Format("<span class=\"keyword\">{0}</span>", keyword);
                string result = CSharp.Format.FormatCode(keyword);
                Assert.AreEqual(expected, result);
            }
        }

        [TestMethod]
        public void ComplexKeywords() {
            string code = "int x = new int[]";
            string expected = "<span class=\"keyword\">int</span> x = <span class=\"keyword\">new</span> <span class=\"keyword\">int</span>[]";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TypeInference() {
            string code = "var manager = new TestManager();";
            string expected = "<span class=\"keyword\">var</span> manager = <span class=\"keyword\">new</span> TestManager();";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ForEachStatement() {
            string code = "int total = 0;\r\nforeach (int number in numberCollection) {\r\n\ttotal += number;\r\n}";
            string expected = "<span class=\"keyword\">int</span> total = 0;\r\n<span class=\"keyword\">foreach</span> (<span class=\"keyword\">int</span> number <span class=\"keyword\">in</span> numberCollection) {\r\n\t" +
                "total += number;\r\n}";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Accessors() {
            string code = "public readonly int val = 0;";
            string expected = "<span class=\"keyword\">public</span> <span class=\"keyword\">readonly</span> <span class=\"keyword\">int</span> val = 0;";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);

            code += "\r\n" + code;
            expected += "\r\n" + expected;
            result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void KeywordsInVariableNames() {
            string code = "var inty = 0;";
            string expected = "<span class=\"keyword\">var</span> inty = 0;";
            string result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);

            code = "var xintx = 0;";
            expected = "<span class=\"keyword\">var</span> xintx = 0;";
            result = CSharp.Format.FormatCode(code);
            Assert.AreEqual(expected, result);
        }
    }
}
