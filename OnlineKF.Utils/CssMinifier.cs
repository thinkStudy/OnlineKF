//Software License Agreement (BSD License)
//Copyright (c) 2009, Yahoo! Inc.
//All rights reserved.

//Redistribution and use of this software in source and binary forms, with or without modification, are 
//permitted provided that the following conditions are met:

//    * Redistributions of source code must retain the above copyright notice, this list of conditions 
//      and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions 
//      and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of Yahoo! Inc. nor the names of its contributors may be used to endorse or promote 
//      products derived from this software without specific prior written permission of Yahoo! Inc.

//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
//WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
//PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY 
//DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
//PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
//HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
//NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
//POSSIBILITY OF SUCH DAMAGE.

// The following code was adapted by Daniel Crenna from Isaac Schlueter's original code. No modifications
// (other than the attaching the license and adding this notice) have been made.

// http://dimebrain.com/2008/03/a-better-css-mi.html

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace OnlineKF.Utils
{
    public static class CssMinifier
    {
        #region Static Helper Methods

        public static string ToHexString(int value)
        {
            StringBuilder sb = new StringBuilder();
            string input = value.ToString();

            foreach (char digit in input)
            {
                sb.Append(String.Format("{0:x2}", Convert.ToUInt32(digit)));
            }
            return sb.ToString();
        }

        public static string RegexReplace(string input, string pattern, string replacement)
        {
            return Regex.Replace(input, pattern, replacement);
        }

        public static string RegexReplace(string input, string pattern, string replacement, RegexOptions options)
        {
            return Regex.Replace(input, pattern, replacement, options);
        }

        public static int AppendReplacement(Match match, StringBuilder sb, string input, string replacement, int index)
        {
            string preceding = input.Substring(index, match.Index - index);

            sb.Append(preceding);
            sb.Append(replacement);

            return match.Index + match.Length;
        }

        public static void AppendTail(Match match, StringBuilder sb, string input, int index)
        {
            sb.Append(input.Substring(index));
        }

        public static bool EqualsIgnoreCase(string left, string right)
        {
            return String.Compare(left, right, true) == 0;
        }

        public static string RemoveRange(string input, int startIndex, int endIndex)
        {
            return input.Remove(startIndex, endIndex - startIndex);
        }

        #endregion

        #region YUI Compressor's CssMin originally written by Isaac Schlueter

        /// <summary>
        /// Minifies CSS.
        /// </summary>
        /// <param name="css">The CSS content to minify.</param>
        /// <returns>Minified CSS content.</returns>
        public static string CssMinify(string css)
        {
            return CssMinify(css, 0);
        }

        /// <summary>
        /// Minifies CSS with a column width maximum.
        /// </summary>
        /// <param name="css">The CSS content to minify.</param>
        /// <param name="columnWidth">The maximum column width.</param>
        /// <returns>Minified CSS content.</returns>
        public static string CssMinify(string css, int columnWidth)
        {
            css = CssMinifier.RemoveCommentBlocks(css);
            css = CssMinifier.RegexReplace(css, "\\s+", " ");
            css = CssMinifier.RegexReplace(css, "\"\\\\\"}\\\\\"\"", "___PSEUDOCLASSBMH___");
            css = CssMinifier.RemovePrecedingSpaces(css);
            css = CssMinifier.RegexReplace(css, "([!{}:;>+\\(\\[,])\\s+", "$1");
            css = CssMinifier.RegexReplace(css, "([^;\\}])}", "$1;}");
            css = CssMinifier.RegexReplace(css, "([\\s:])(0)(px|em|%|in|cm|mm|pc|pt|ex)", "$1$2");
            css = CssMinifier.RegexReplace(css, ":0 0 0 0;", ":0;");
            css = CssMinifier.RegexReplace(css, ":0 0 0;", ":0;");
            css = CssMinifier.RegexReplace(css, ":0 0;", ":0;");
            css = CssMinifier.RegexReplace(css, "background-position:0;", "background-position:0 0;");
            css = CssMinifier.RegexReplace(css, "(:|\\s)0+\\.(\\d+)", "$1.$2");
            css = CssMinifier.ShortenRgbColors(css);
            css = CssMinifier.ShortenHexColors(css);
            css = CssMinifier.RegexReplace(css, "[^\\}]+\\{;\\}", "");

            if (columnWidth > 0)
            {
                css = CssMinifier.BreakLines(css, columnWidth);
            }

            css = CssMinifier.RegexReplace(css, "___PSEUDOCLASSBMH___", "\"\\\\\"}\\\\\"\"");
            css = css.Trim();

            return css;
        }

        private static string RemoveCommentBlocks(string input)
        {
            int startIndex = 0;
            int endIndex = 0;
            bool iemac = false;

            startIndex = input.IndexOf(@"/*", startIndex);
            while (startIndex >= 0)
            {
                endIndex = input.IndexOf(@"*/", startIndex + 2);
                if (endIndex >= startIndex + 2)
                {
                    if (input[endIndex - 1] == '\\')
                    {
                        startIndex = endIndex + 2;
                        iemac = true;
                    }
                    else if (iemac)
                    {
                        startIndex = endIndex + 2;
                        iemac = false;
                    }
                    else
                    {
                        input = CssMinifier.RemoveRange(input, startIndex, endIndex + 2);
                    }
                }
                startIndex = input.IndexOf(@"/*", startIndex);
            }

            return input;
        }

        private static string ShortenRgbColors(string css)
        {
            StringBuilder sb = new StringBuilder();
            Regex p = new Regex("rgb\\s*\\(\\s*([0-9,\\s]+)\\s*\\)");
            Match m = p.Match(css);

            int index = 0;
            while (m.Success)
            {
                string[] colors = m.Groups[1].Value.Split(',');
                StringBuilder hexcolor = new StringBuilder("#");

                foreach (string color in colors)
                {
                    int val = Int32.Parse(color);
                    if (val < 16)
                    {
                        hexcolor.Append("0");
                    }
                    hexcolor.Append(CssMinifier.ToHexString(val));
                }

                index = CssMinifier.AppendReplacement(m, sb, css, hexcolor.ToString(), index);
                m = m.NextMatch();
            }

            CssMinifier.AppendTail(m, sb, css, index);
            return sb.ToString();
        }

        private static string ShortenHexColors(string css)
        {
            StringBuilder sb = new StringBuilder();
            Regex p = new Regex("([^\"'=\\s])(\\s*)#([0-9a-fA-F])([0-9a-fA-F])([0-9a-fA-F])([0-9a-fA-F])([0-9a-fA-F])([0-9a-fA-F])");
            Match m = p.Match(css);

            int index = 0;
            while (m.Success)
            {
                if (CssMinifier.EqualsIgnoreCase(m.Groups[3].Value, m.Groups[4].Value) &&
                    CssMinifier.EqualsIgnoreCase(m.Groups[5].Value, m.Groups[6].Value) &&
                    CssMinifier.EqualsIgnoreCase(m.Groups[7].Value, m.Groups[8].Value))
                {
                    string replacement = String.Concat(m.Groups[1].Value, m.Groups[2].Value, "#", m.Groups[3].Value, m.Groups[5].Value, m.Groups[7].Value);
                    index = CssMinifier.AppendReplacement(m, sb, css, replacement, index);
                }
                else
                {
                    index = CssMinifier.AppendReplacement(m, sb, css, m.Value, index);
                }

                m = m.NextMatch();
            }

            CssMinifier.AppendTail(m, sb, css, index);
            return sb.ToString();
        }

        private static string RemovePrecedingSpaces(string css)
        {
            StringBuilder sb = new StringBuilder();
            Regex p = new Regex("(^|\\})(([^\\{:])+:)+([^\\{]*\\{)");
            Match m = p.Match(css);

            int index = 0;
            while (m.Success)
            {
                string s = m.Value;
                s = CssMinifier.RegexReplace(css, ":", "___PSEUDOCLASSCOLON___");

                index = CssMinifier.AppendReplacement(m, sb, css, s, index);
                m = m.NextMatch();
            }
            CssMinifier.AppendTail(m, sb, css, index);

            string result = sb.ToString();
            result = CssMinifier.RegexReplace(css, "\\s+([!{};:>+\\(\\)\\],])", "$1");
            result = CssMinifier.RegexReplace(css, "___PSEUDOCLASSCOLON___", ":");

            return result;
        }

        private static string BreakLines(string css, int columnWidth)
        {
            int i = 0;
            int start = 0;

            StringBuilder sb = new StringBuilder(css);
            while (i < sb.Length)
            {
                char c = sb[i++];
                if (c == '}' && i - start > columnWidth)
                {
                    sb.Insert(i, '\n');
                    start = i;
                }
            }
            return sb.ToString();
        }
		
        #endregion
    }
}
