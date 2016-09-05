using LanguageFormatRequirements;
using System;
using System.IO;
using System.Linq;

namespace CSharp
{
    public class Format : LanguageFormat
    {
        // Initialize arrays containing significant strings and characters in the language.
        public static string[] Keywords { private set; get; } = new string[] {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch",
            "char", "checked", "class", "const", "continue", "decimal", "default", "delegate",
            "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally",
            "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in",
            "int", "interface", "internal", "lock", "long", "namespace", "new", "null",
            "object", "operator", "out", "override", "params", "private", "protected", "public",
            "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc",
            "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof",
            "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void",
            "volatile", "while", "add", "alias", "ascending", "async", "await", "descending",
            "dynamic", "from", "get", "global", "group", "into", "join", "let",
            "orderby", "partial", "remove", "select", "set", "value", "var", "where", "yield"
        };
        private static readonly char[] _syntaxSugar = "()[]{}<>;:?!*+-/&|%=,. \r\n\t".ToCharArray();

        public Format() : base("CSharp") {
            // Call the base constructor to initialize the language.
        }

        public override void FormatFiles() {
            // Loop through every .cs file in our source folder.
            foreach (string file in Directory.GetFiles(this.SourcePath, "*.cs")) {

                // Get the output filename path, and format the code in the .cs file.
                var fi = new FileInfo(file);
                string fileOutput = this.OutputPath + fi.Name.Remove(fi.Name.IndexOf('.')) + ".html";
                string formattedCode = "<pre class=\"csharp\"><p class=\"code\">" + FormatCode(File.ReadAllText(file)) + "</p></pre>";

                // Write the formatted code to the file.
                File.WriteAllText(fileOutput, formattedCode);
            }
        }

        public static string FormatCode(string code) {
            // The indexing variable declaring what character is currently being processed.
            int ptr = 0;

            // Boolean variables to help process long structures.
            bool inString = false;
            bool inChar = false;
            bool inSingleComment = false;
            bool inMultiComment = false;

            // Variable used to store full words in the code to be examined for keywords.
            string lastWord = string.Empty;

            // Anonymous method used to examine the 'lastWord' variable for keywords.
            var keywordMarker = new Action(() => {
                // Make sure the lastWord variable is not empty.
                if (lastWord != string.Empty) {

                    // Remove the code we currently have in there.
                    ptr -= lastWord.Length;
                    code = code.Remove(ptr, lastWord.Length);

                    // Loop through all the keywords that apply to our current situation.
                    foreach (string keyword in Keywords.Where((key) => lastWord.Contains(key))) {
                        // Get the indexes of the character just before, and after the word.
                        int index = lastWord.IndexOf(keyword);
                        int lowIndex = index - 1;
                        int highIndex = index + keyword.Length;

                        // Make sure that the character there either doesn't exist, or isn't a part of the word.
                        if (lowIndex < 0 || _syntaxSugar.Contains(lastWord[lowIndex])) {
                            if (highIndex >= lastWord.Length || _syntaxSugar.Contains(lastWord[highIndex])) {
                                // We know that this match is a proper keyword. Replace it with the formatting, and break out of the loop.
                                lastWord = lastWord.Remove(index, keyword.Length);
                                lastWord = lastWord.Insert(index, string.Format("<span class=\"keyword\">{0}</span>", keyword));
                                break;
                            }
                        }
                    }


                    // Add the replaced words, and clear the lastWord variable.
                    code = code.Insert(ptr, lastWord);
                    ptr += lastWord.Length;
                    lastWord = string.Empty;
                }
            });



            // Replace all ampersands in the code with &amp;.
            code = code.Replace("&", "&amp;");

            // Replace all > and < characters with &lt; and &gt;
            code = code.Replace(">", "&gt;");
            code = code.Replace("<", "&lt;");

            // Loop through the code until the end is reached.
            do {
                // Get the character at the current index.
                char character = code[ptr];

                // Have we found the start of a comment?
                if (character == '/' && !inString && !inSingleComment && !inMultiComment && !inChar) {
                    // Get the sequence.
                    string sequence = code.SafeSubstring(ptr, 2);

                    // Have we found a single-line comment?
                    if (sequence == "//") {
                        inSingleComment = true;
                        code = code.Insert(ptr, "<span class=\"comment\">");
                        ptr += 22;
                        continue;
                    }

                    // Have we found a multi-line comment?
                    if (sequence == "/*") {
                        inMultiComment = true;
                        code = code.Insert(ptr, "<span class=\"comment\">");
                        ptr += 22;
                        continue;
                    }
                }

                // Have we found the end of a multi-line comment?
                if (character == '*' && !inString && !inSingleComment && inMultiComment && !inChar) {
                    string sequence = code.SafeSubstring(ptr, 2);

                    if (sequence == "*/") {
                        inMultiComment = false;
                        code = code.Insert(ptr + 2, "</span>");
                        ptr += 7;
                        continue;
                    }
                }

                // Figure out if we have a new-line sequence.
                if (character == '\r') {
                    string sequence = code.SafeSubstring(ptr, 2);

                    if (sequence == "\r\n") {
                        if (inSingleComment) {
                            inSingleComment = false;
                            code = code.Insert(ptr, "</span>");
                            ptr += 9;
                            continue;
                        } else {
                            keywordMarker.Invoke();
                        }
                    }
                }

                // If we come across an escaped character, skip the character after it.
                if (character == '\\') {
                    ptr++;
                    continue;
                }

                // Is the character a double-quotation?
                if ((character == '\"' || (character == '@' && code.SafeSubstring(ptr, 2) == "@\"")) && !inSingleComment && !inMultiComment && !inChar) {
                    // Is it the start of a string?
                    if (!inString) {
                        inString = true;
                        code = code.Insert(ptr, "<span class=\"string\">");
                        ptr += 21;
                    } else {
                        inString = false;
                        code = code.Insert(ptr + 1, "</span>");
                        ptr += 7;
                    }
                    if (character == '@') {
                        ptr++;
                    }
                    continue;
                }

                // Is the character a single-quotation?
                if (character == '\'' && !inString && !inSingleComment && !inMultiComment) {
                    // Is it the start of a character?
                    if (!inChar) {
                        inChar = true;
                        code = code.Insert(ptr, "<span class=\"string\">");
                        ptr += 21;
                    } else {
                        inChar = false;
                        code = code.Insert(ptr + 1, "</span>");
                        ptr += 7;
                    }
                    continue;
                }

                // Did we finish a word?
                if (!inString && !inChar && !inSingleComment && !inMultiComment) {
                    if (_syntaxSugar.Contains(character)) {
                        // We did. Mark any keywords.
                        keywordMarker.Invoke();
                        continue;
                    } else {
                        lastWord += character;
                    }
                }
            } while (++ptr < code.Length);

            // Mark any remaining keywords in the last word of the code, and return the formatted code.
            keywordMarker.Invoke();
            return code;
        }
    }
}
