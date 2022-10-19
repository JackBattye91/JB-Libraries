using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public class ErrorWorker {
        public static void AddError<T>(IReturnCode<T> rc, long pErrorCode) {
            AddError(rc, pErrorCode);
        }
        public static void AddError<T>(IReturnCode<T> rc, int pScope, int pCode, string? pMessage) {
            AddError(rc, pScope, pCode, new Exception(pMessage));
        }
        public static void AddError<T>(IReturnCode<T> rc, int pScope, int pCode, Exception ex) {
            rc.Errors.Add(new Error(pScope, pCode, ex));
        }

        public static void CopyErrors<T, U>(IReturnCode<T>? pSource, IReturnCode<U>? pDestination) {
            if (pSource != null && pDestination != null) {
                foreach (Error srcCodes in pSource.Errors) {
                    pDestination?.Errors.Add(srcCodes);
                }
            }
        }

        public static HttpStatusCode GetStatusCode<T>(IReturnCode<T> rc) {
            if (rc.Success) {
                return HttpStatusCode.OK;
            }
            else {
                IError lastError = rc.Errors.Last();
                return (lastError as INetworkError)?.StatusCode ?? HttpStatusCode.InternalServerError;
            }
        }
    }
}
