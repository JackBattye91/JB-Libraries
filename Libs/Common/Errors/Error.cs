using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common.Errors {
    public interface IError {
        public long ErrorCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public IList<string> ErrorMessageData { get; set; }
    }

    public class Error : IError {
        public long ErrorCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public IList<string> ErrorMessageData { get; set; }

        public Error() {
            ErrorCode = 0;
            Message = string.Empty;
            StackTrace = string.Empty;
            ErrorMessageData = new List<string>();
        }
    }
}
