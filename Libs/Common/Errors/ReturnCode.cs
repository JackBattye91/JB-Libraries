using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
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
