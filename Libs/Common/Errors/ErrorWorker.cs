using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JB.Common {
    public class ErrorWorker {
        public static void AddError(IReturnCode rc, long pErrorCode) {
            AddError(rc, pErrorCode);
        }
        public static void AddError(IReturnCode rc, int pCode, string? pMessage) {
            AddError(rc, pCode, new Exception(pMessage));
        }
        public static void AddError(IReturnCode rc, int pCode, Exception? ex) {
            rc.Errors.Add(new Error(pCode, ex));
        }

        public static void CopyErrors(IReturnCode? pSource, IReturnCode? pDestination) {
            if (pSource != null && pDestination != null) {
                pDestination.ErrorCode = pSource.ErrorCode;
                foreach (IError srcCodes in pSource.Errors) {
                    pDestination?.Errors.Add(srcCodes);
                }
            }
        }

        public static HttpStatusCode GetStatusCode(IReturnCode rc) {
            if (rc.Success) {
                return HttpStatusCode.OK;
            }
            else {
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

                foreach(IError error in rc.Errors) {
                    if (error is INetworkError) {
                        statusCode = (error as INetworkError)!.StatusCode;
                        break;
                    }
                }

                return statusCode;
            }
        }

        public static void LogErrors(ILogger pLog, IReturnCode rc) {
            foreach(IError error in rc.Errors) {
                pLog.LogError($"{error.ErrorCode} - {error.TimeStamp} - {error.Exception?.Message ?? string.Empty}");
            }
        }
    }
}
