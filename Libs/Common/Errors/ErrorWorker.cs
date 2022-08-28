using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public class ErrorWorker {
        public static void AddError<T>(ReturnCode<T> rc, long pErrorCode) {
            AddError(rc, pErrorCode);
        }
        public static void AddError<T>(ReturnCode<T> rc, long pErrorCode, string? pMessage) {
            AddError(rc, pErrorCode, new Exception(pMessage));
        }
        public static void AddError<T>(ReturnCode<T> rc, long pErrorCode, Exception ex) {
            rc.Errors.Add(new Error(pErrorCode, ex));
        }

        public static void CopyErrors<T, U>(ReturnCode<T>? pSource, ReturnCode<U>? pDestination) {
            if (pSource != null && pDestination != null) {
                pSource.Success = pDestination.Success;

                foreach (Error srcCodes in pSource.Errors) {
                    pDestination?.Errors.Add(srcCodes);
                }
            }
        }
    }
}
