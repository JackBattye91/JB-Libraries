using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public class ErrorWorker {
        public static void AddError<T>(IReturnCode<T> rc, long pErrorCode) {
            AddError(rc, pErrorCode, null, null);
        }
        public static void AddError<T>(IReturnCode<T> rc, long pErrorCode, string? pMessage) {
            AddError(rc, pErrorCode, pMessage, null);
        }
        public static void AddError<T>(IReturnCode<T> rc, long pErrorCode, string? pMessage, string? pStackTrace) {
            rc.Errors.Add(new Error() { 
                    ErrorCode = pErrorCode, 
                    Message = pMessage ?? String.Empty,
                    StackTrace = pStackTrace ?? Environment.StackTrace
                });
        }

        public static void CopyErrorCode<T, U>(IReturnCode<T> pSource, IReturnCode<U> pDestination) {
            pDestination.ErrorCode = pSource.ErrorCode;

            foreach (var srcCodes in pSource.Errors) {
                pDestination.Errors.Add(srcCodes);
            }
        }
    }
}
