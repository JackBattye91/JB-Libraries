using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public class Error {
        public int Scope { get; set; }
        public int ErrorCode { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }

        public Error() {
            Scope = 0;
            ErrorCode = 0;
            Exception = null;
        }

        public Error(int scope, int error, Exception? exception = null) {
            Scope = scope;
            ErrorCode = error;
            Exception = exception;
            TimeStamp = DateTime.Now;
        }

        public Error(long code, Exception? exception = null) {
            Scope = (int)(code >> 32);
            ErrorCode = (int)(code & 0xFFFFFFFF);
            Exception = exception;
            TimeStamp = DateTime.Now;
        }
    }

    public class ReturnCode<T> {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public List<Error> Errors { get; set; }

        public ReturnCode() {
            Data = default;
            Success = true;
            Errors = new List<Error>();
        }

        public ReturnCode(long code) {
            Data = default;
            Success = code == 0 ? true : false;
            Errors = new List<Error>();
        }
        public ReturnCode(Error error) {
            Data = default;
            Success = error.ErrorCode == 0 ? true : false;
            Errors = new List<Error>(new[] { error });
        }

        public ReturnCode(long code, Exception ex) {
            Data = default;
            Success = code == 0 ? true : false;
            Errors = new List<Error>(new[] { new Error(code, ex) });
        }

        public ReturnCode(Error error, Exception ex) {
            Data = default;
            Success = error.ErrorCode == 0 ? true : false;
            error.Exception = ex;
            Errors = new List<Error>(new[] { error });
        }

        public ReturnCode(int scope, int error, Exception ex) {
            Data = default;
            Success = error == 0 ? true : false;
            Errors = new List<Error>(new[] { new Error(scope, error, ex) });
        }

        public override bool Equals(object? obj) {
            if (obj is bool) {
                return ((obj as bool?) == Success);
            }
            return false;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
