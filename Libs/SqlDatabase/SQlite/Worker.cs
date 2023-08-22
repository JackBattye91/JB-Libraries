using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JB.Common;
using JB.SqlDatabase.Interfaces;

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
                rc.ErrorCode = ErrorCodes.PARSE_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return rc;
        }

        public static string ConvertToDatabaseType(Type type) {
            if (type == typeof(byte)) {
                return "SMALLINT";
            }
            else if (type == typeof(string)) {
                return "TEXT";
            }

            return string.Empty;
        }


        public static IReturnCode<IList<T?>> PopulateObjects<T>(IDataReader pDataReader) {
            IReturnCode<IList<T?>> rc = new ReturnCode<IList<T?>>();
            IList<T?> objList = new List<T?>();

            try {
                var properties = typeof(T).GetProperties();
                
                while(pDataReader.NextRow()) {
                    T? obj = default;
                    obj = (T?)Activator.CreateInstance(typeof(T));

                    foreach (var prop in properties) {
                        if (pDataReader.HasValue(prop.Name)) {
                            object value = pDataReader.Get(prop.Name);
                            prop.SetValue(obj, value);
                        }
                    }

                    objList.Add(obj);
                }
            }
            catch(Exception ex) {
                rc.ErrorCode = ErrorCodes.POPULATE_OBJECT_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = objList;
            }

            return rc;
        }

        public static IReturnCode<IDictionary<string, object?>> GetObjectValues<T>(T pObj) {
            IReturnCode<IDictionary<string, object?>> rc = new ReturnCode<IDictionary<string, object?>>();
            IDictionary<string, object?> valuesMap = new Dictionary<string, object?>();

            try {
                if (rc.Success) {
                    PropertyInfo[] properties = typeof(T).GetProperties();

                    foreach (var prop in properties) {
                        valuesMap.Add(prop.Name, prop.GetValue(pObj));
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.GET_OBJECT_VALUES_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = valuesMap;
            }

            return rc;
        }
    }
}
