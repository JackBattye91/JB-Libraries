﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JB.Common {
    public class ErrorWorker {
        public static void CopyErrors(IReturnCode? pSource, IReturnCode? pDestination) {
            if (pSource != null && pDestination != null) {
                foreach (IError srcCodes in pSource.Errors) {
                    pDestination?.AddError(srcCodes);
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
                pLog.LogError($"{error.TimeStamp} - {error.Exception?.Message ?? string.Empty}");
            }
        }
    }
}
