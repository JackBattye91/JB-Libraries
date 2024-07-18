using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common.Errors {
    public class JBException : Exception {
        public string? SourceFile { get; protected set; } = null;
        public int? LineNumber { get; protected set; } = null;

        public JBException(string? pMessage = null, Exception? pInnerException = null, [CallerLineNumber] int? pLineNumber = null, [CallerFilePath] string? pFilePath = null) : base(pMessage, pInnerException) {
            LineNumber = pLineNumber;
            SourceFile = pFilePath;
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Error Occurred");
            builder.AppendLine($"\tLine: {LineNumber}");
            builder.AppendLine($"\tFile: {SourceFile}");

            if (Message != null) {
                builder.AppendLine($"\tMessage: {Message}");
            }
            else if (!string.IsNullOrEmpty(InnerException?.Message)) {
                builder.AppendLine($"\tMessage: {InnerException.Message}");
            }

            return builder.ToString();
        }
    }
}
