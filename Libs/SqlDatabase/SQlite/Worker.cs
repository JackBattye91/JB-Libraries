using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;

namespace JB.SqlDatabase.SQlite {
    internal class Worker {
        public static IReturnCode<T> ParseData<T>(Interfaces.IDataReader? dataReader) where T : struct {
            IReturnCode<T> rc = new ReturnCode<T>();
            T obj = default;

            try {
                Type objType = typeof(T);
                var properties = objType.GetProperties();

                while (dataReader?.NextRow() ?? false) {
                    foreach(var prop in properties) {
                        object value = dataReader.HasValue(prop.Name);
                        objType.GetProperty(prop.Name)?.SetValue(obj, value);
                    }

                    dataReader.NextRow();
                }
            }
            catch (Exception ex) {
                rc.Errors.Add(new SqlDatabaseError(ErrorCodes.PARSE_DATA_FAILED, ex));
            }

            return rc;
        }

        public static string ConvertToDatabaseType(Type type) {
            if (type == typeof(byte)) {
                return "SMALLINT";
            }

            return string.Empty;
        }
    }
}
