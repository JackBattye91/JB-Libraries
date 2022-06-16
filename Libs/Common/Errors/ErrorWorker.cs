using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common.Errors {
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
                Message = pMessage ?? string.Empty,
                StackTrace = pStackTrace ?? Environment.StackTrace
            });
        }

        public static void CopyErrorCode<T, U>(IReturnCode<T>? pSource, IReturnCode<U>? pDestination) {
            if (pSource != null && pDestination != null) {
                pDestination.ErrorCode = pSource.ErrorCode;

                foreach (Error srcCodes in pSource.Errors) {
                    pDestination?.Errors.Add(srcCodes);
                }
            }
            
        }
    }
}
