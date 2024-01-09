using JB.Common.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public interface IError {
        int ErrorCode { get; set; }
        Exception? Exception { get; set; }
        DateTime TimeStamp { get; }
        ErrorType ErrorType { get; }
    }
    public interface INetworkError : IError {
        HttpStatusCode StatusCode { get; set; }
    }
    public class Error : IError {
        public int ErrorCode { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }
        public ErrorType ErrorType { get; protected set; }

        public Error() {
            ErrorCode = 0;
            Exception = null;
            TimeStamp = DateTime.UtcNow;
            ErrorType = Consts.ErrorType.ERROR;
        }

        public Error(int code, Exception? exception = null, ErrorType pErrorType = ErrorType.ERROR) {
            ErrorCode = code;
            Exception = exception;
            TimeStamp = DateTime.Now;
            ErrorType = pErrorType;
        }
    }
    public class NetworkError : INetworkError {
        public int ErrorCode { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }
        public HttpStatusCode StatusCode { get; set; }
        public ErrorType ErrorType { get; protected set; }

        public NetworkError() {
            ErrorCode = 0;
            Exception = null;
            TimeStamp = DateTime.UtcNow;
            StatusCode = HttpStatusCode.OK;
            ErrorType = Consts.ErrorType.ERROR;
        }

        public NetworkError(int pErrorCode, HttpStatusCode pStatusCode, Exception? pException = null, ErrorType pErrorType = ErrorType.ERROR) {
            ErrorCode = pErrorCode;
            Exception = pException;
            TimeStamp = DateTime.Now;
            StatusCode = pStatusCode;
            ErrorType = pErrorType;
        }
    }

    public interface IReturnCode {
        bool Success { get; }
        bool Failed { get; }
        int ErrorCode { get; protected set; }
        IReadOnlyList<IError> Errors { get; }
        void AddError(IError pError);
    }
    public interface IReturnCode<T> : IReturnCode {
        T? Data { get; set; }
    }
    public class ReturnCode : IReturnCode {
        protected IList<IError> errors = new List<IError>();
        public bool Success { get { return (ErrorCode == 0); } }
        public bool Failed { get { return !Success; } }
        public int ErrorCode { get; set; }
        public IReadOnlyList<IError> Errors { get { return errors.ToArray(); } }
        public void AddError(IError pError) { 
            if (pError.ErrorType == ErrorType.ERROR) {
                ErrorCode = pError.ErrorCode;
            }
            errors.Add(pError);
        }
    }
    public class ReturnCode<T> : IReturnCode<T> {
        protected IList<IError> errors = new List<IError>();
        public T? Data { get; set; }
        public bool Success { get { return (ErrorCode == 0); } }
        public bool Failed { get { return !Success; } }
        public int ErrorCode { get; set; } = 0;
        public IReadOnlyList<IError> Errors { get { return errors.ToArray(); } }
        public void AddError(IError pError) {
            if (pError.ErrorType == ErrorType.ERROR) {
                ErrorCode = pError.ErrorCode;
            }
            errors.Add(pError);
        }
    }
}
