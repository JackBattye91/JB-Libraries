using JB.Common.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public interface IError {
        Exception? Exception { get; set; }
        DateTime TimeStamp { get; }
        ErrorType ErrorType { get; }
        string? FilePath { get; }
        int? LineNumber { get; }
    }
    public interface INetworkError : IError {
        HttpStatusCode StatusCode { get; set; }
    }
    public class Error : IError {
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }
        public ErrorType ErrorType { get; protected set; }
        public string? FilePath { get; protected set; }
        public int? LineNumber { get; protected set; }

        public Error(Exception? exception = null, ErrorType pErrorType = ErrorType.ERROR, [CallerLineNumber] int? pLineNumber = null, [CallerFilePath] string? pFilePath = null) {
            Exception = exception;
            TimeStamp = DateTime.Now;
            ErrorType = pErrorType;
            FilePath = pFilePath;
            LineNumber = pLineNumber;
        }

        public Error(string pMessage, ErrorType pErrorType = ErrorType.ERROR, [CallerLineNumber] int? pLineNumber = null, [CallerFilePath] string? pFilePath = null)
        {
            Exception = new Exception(pMessage);
            TimeStamp = DateTime.Now;
            ErrorType = pErrorType;
            FilePath = pFilePath;
            LineNumber = pLineNumber;
        }
    }
    public class NetworkError : INetworkError {
        public Exception? Exception { get; set; }
        public DateTime TimeStamp { get; protected set; }
        public HttpStatusCode StatusCode { get; set; }
        public ErrorType ErrorType { get; protected set; }
        public string? FilePath { get; protected set; }
        public int? LineNumber { get; protected set; }

        public NetworkError(HttpStatusCode pStatusCode, Exception? pException = null, ErrorType pErrorType = ErrorType.ERROR, [CallerLineNumber] int? pLineNumber = null, [CallerFilePath] string? pFilePath = null) {
            Exception = pException;
            TimeStamp = DateTime.Now;
            StatusCode = pStatusCode;
            ErrorType = pErrorType;
            FilePath = pFilePath;
            LineNumber = pLineNumber;
        }
    }

    public interface IReturnCode {
        bool Success { get; }
        bool Failed { get; }
        IReadOnlyList<IError> Errors { get; }
        void AddError(IError pError);
    }
    public interface IReturnCode<T> : IReturnCode {
        T? Data { get; set; }
    }
    public class ReturnCode : IReturnCode {
        protected IList<IError> errors = new List<IError>();
        public bool Success { get { return errors.Any(x => x.ErrorType == ErrorType.ERROR); } }
        public bool Failed { get { return !Success; } }


        public IReadOnlyList<IError> Errors { get { return errors.ToArray(); } }
        public void AddError(IError pError) { 
            errors.Add(pError);
        }
    }
    public class ReturnCode<T> : IReturnCode<T> {
        protected IList<IError> errors = new List<IError>();
        public T? Data { get; set; }
        public bool Success { get { return errors.Any(x => x.ErrorType == ErrorType.ERROR); } }
        public bool Failed { get { return !Success; } }
        public IReadOnlyList<IError> Errors { get { return errors.ToArray(); } }
        public void AddError(IError pError) {
            errors.Add(pError);
        }
    }
}
