using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public sealed class ErrorLog {
        public static IList<IError> Errors { get; private set; } = new List<IError>();
        public static void Log(IError error) { Errors.Add(error); }

        public static void Save() {
            if (Errors.Count != 0) {
                using (StreamWriter writer = File.CreateText("ErrorLog.txt")) {
                    foreach (Error error in Errors) {
                        writer.WriteLine($"{error.TimeStamp} - {error.ErrorCode} - {error.Exception?.Message} - {error.Exception?.StackTrace}");
                    }
                }
            }
        }
    }
}
