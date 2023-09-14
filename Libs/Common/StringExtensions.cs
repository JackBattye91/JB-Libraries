using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public static class StringExtensions {
        public static string PascalCase(this string pStr) {
            StringBuilder builder = new StringBuilder();
            char lastChar = '\0';

            for(int c = 0; c < pStr.Length; c++) {
                char currChar = pStr[c];

                if (c == 0) {
                    builder.Append(char.ToUpper(currChar));
                }
                else if (lastChar == '_') {
                    builder.Append(char.ToUpper(currChar));
                }
                else if (currChar != '_') {
                    builder.Append(currChar);
                }

                lastChar = currChar;
            }

            return builder.ToString();
        }

        public static string CamelCase(this string pStr) {
            StringBuilder builder = new StringBuilder();
            char lastChar = '\0';

            for (int c = 0; c < pStr.Length; c++) {
                char currChar = pStr[c];

                if (c == 0) {
                    builder.Append(char.ToLower(currChar));
                }
                else if (lastChar == '_') {
                    builder.Append(char.ToUpper(currChar));
                }
                else if (currChar != '_') {
                    builder.Append(currChar);
                }

                lastChar = currChar;
            }

            return builder.ToString();
        }
    }
}
