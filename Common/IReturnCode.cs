using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public interface IReturnCode<T> {
        public T? Data { get; set; }
        public long ErrorCode { get; set; }
        public IList<JB.Common.Error> Errors { get; set; }
    }

    public class ReturnCode<T> : IReturnCode<T> {
        public T? Data { get; set; }
        public long ErrorCode { get; set; }
        public IList<JB.Common.Error> Errors { get; set; }

        public ReturnCode(long pErrorCode) {
            Data = default(T);
            ErrorCode = pErrorCode;
            Errors = new List<JB.Common.Error>();
        }
    }
}
