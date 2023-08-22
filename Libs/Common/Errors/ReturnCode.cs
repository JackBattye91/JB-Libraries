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
    }
    public interface INetworkError : IError {
        HttpStatusCode StatusCode { get; set; }
    }
    public class Error : IError {
        public int ErrorCode { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }

        public Error() {
            ErrorCode = 0;
            Exception = null;
            TimeStamp = DateTime.UtcNow;
        }

        public Error(int code, Exception? exception = null) {
            ErrorCode = code;
            Exception = exception;
            TimeStamp = DateTime.Now;
        }
    }
    public class NetworkError : INetworkError {
        public int ErrorCode { get; set; }
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }
        public HttpStatusCode StatusCode { get; set; }

        public NetworkError() {
            ErrorCode = 0;
            Exception = null;
            TimeStamp = DateTime.UtcNow;
            StatusCode = HttpStatusCode.OK;
        }

        public NetworkError(int pErrorCode, HttpStatusCode pStatusCode, Exception? pException = null) {
            ErrorCode = pErrorCode;
            Exception = pException;
            TimeStamp = DateTime.Now;
            StatusCode = pStatusCode;
        }
    }

    public interface IReturnCode<T> {
        bool Success { get; }
        bool Failed { get; }
        T? Data { get; set; }
        int ErrorCode { get; set; }
        IList<IError> Errors { get; set; }
    }
    public class ReturnCode<T> : IReturnCode<T> {
        public T? Data { get; set; }
        public bool Success { get { return (ErrorCode == 0); } }
        public bool Failed { get { return !Success; } }
        public int ErrorCode { get; set; }
        public IList<IError> Errors { get; set; }

        public ReturnCode() {
            Data = default;
            ErrorCode = 0;
            Errors = new List<IError>();
        }

        public ReturnCode(int pErrorCode) {
            Data = default;
            ErrorCode = pErrorCode;
            Errors = new List<IError>();
        }
    }
}
