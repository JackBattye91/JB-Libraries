using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common.Errors {
    public sealed class Error {
        public sealed class Code {
            public int Scope { get; set; }
            public int ErrorCode { get; set; }

            public Code() {
                Scope = ErrorCode = 0;
            }
            public Code(int scope, int errorCode) {
                Scope = scope;
                ErrorCode = errorCode;
            }

            public override string ToString() {
                return $"{Scope}-{ErrorCode}";
            }
            public override bool Equals(object? obj) {
                return base.Equals(obj);
            }
            public override int GetHashCode() {
                return base.GetHashCode();
            }

            public static bool operator==(Code left, int right) {
                return (left.ErrorCode == right);
            }
            public static bool operator !=(Code left, int right) {
                return (left.ErrorCode != right);
            }
        }

        public Code ErrorCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public Error() {
            ErrorCode = new Code();
            Message = string.Empty;
            StackTrace = string.Empty;
        }
        public Error(int scope, int code) {
            ErrorCode = new Code(scope, code);
            Message = string.Empty;
            StackTrace = string.Empty;
        }
        public Error(Code errorCode, string message, string stackTrace) {
            ErrorCode = errorCode;
            Message = message;
            StackTrace = stackTrace;
        }
    }
}
