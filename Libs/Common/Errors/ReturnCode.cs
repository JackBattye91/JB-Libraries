using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public interface  IError {
        int Scope { get; }
        int Code { get; set; }
        Exception? Exception { get; set; }
        DateTime TimeStamp { get; }
        HttpStatusCode StateCode { get; set; }
    }
    internal class Error : IError {
        public int Scope { get; protected set; }
        public int Code { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }
        public HttpStatusCode StateCode { get; set; }

        public Error() {
            Scope = 0;
            Code = 0;
            Exception = null;
            TimeStamp = DateTime.UtcNow;
            StateCode = HttpStatusCode.OK;
        }

        public Error(int scope, int code, Exception? exception = null) {
            Scope = scope;
            Code = code;
            Exception = exception;
            TimeStamp = DateTime.Now;
            StateCode = HttpStatusCode.OK;
        }
    }
    internal class CommonError : IError {
        public int Scope { get; protected set; }
        public int Code { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }
        public HttpStatusCode StateCode { get; set; }

        public CommonError() {
            Scope = 0;
            Code = 0;
            Exception = null;
            TimeStamp = DateTime.Now;
            StateCode = HttpStatusCode.OK;
        }
        public CommonError(int code, Exception? exception = null) {
            Scope = 0;
            Code = code;
            Exception = exception;
            TimeStamp = DateTime.Now;
            StateCode = HttpStatusCode.OK;
        }
        public CommonError(int code, HttpStatusCode statusCode, Exception? exception = null) {
            Scope = 0;
            Code = code;
            Exception = exception;
            TimeStamp = DateTime.Now;
            StateCode = statusCode;
        }
    }

    public interface IReturnCode<T> {
        bool Success { get; }
        bool Failed { get; }
        T? Data { get; set; }
        IList<IError> Errors { get; set; }
    }
    public class ReturnCode<T> : IReturnCode<T> {
        public T? Data { get; set; }
        public bool Success { get { return (Errors.Count == 0); } }
        public bool Failed { get { return !Success; } }
        public IList<IError> Errors { get; set; }

        public ReturnCode() {
            Data = default;
            Errors = new List<IError>();
        }

        public ReturnCode(long code) {
            Data = default;
            Errors = new List<IError>();
        }
        public ReturnCode(IError error) {
            Data = default;
            Errors = new List<IError>(new[] { error });
        }

        public ReturnCode(int scope, int code, Exception ex) {
            Data = default;
            Errors = new List<IError>(new[] { new Error(scope, code, ex) });
        }

        public ReturnCode(IError error, Exception ex) {
            Data = default;
            error.Exception = ex;
            Errors = new List<IError>(new[] { error });
        }
    }
}
