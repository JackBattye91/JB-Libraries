using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
                foreach (IError srcCodes in pSource.Errors) {
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

        public static void LogErrors<T>(ILogger pLog, IReturnCode<T> rc) {
            foreach(IError error in rc.Errors) {
                pLog.LogError($"{error.Scope} - {error.Code} - {error.TimeStamp} - {error.Exception?.Message ?? string.Empty}");
            }
        }
    }
}
