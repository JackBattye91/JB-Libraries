using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JB.Common;
using JB.SqlDatabase.Attributes;
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

        public static string? ConvertToDatabaseType(Type type) {
            if (type == typeof(string)) {
                return "TEXT";
            }
            else if (type == typeof(int) || type == typeof(long) || type == typeof(byte) || type == typeof(bool) || type.IsEnum) {
                return "INTEGER";
            }
            else if (type == typeof(float) || type == typeof(double)) {
                return "REAL";
            }

            return null;
        }

        public static IReturnCode<IList<T?>> PopulateObjects<T>(IDataReader pDataReader) {
            IReturnCode<IList<T?>> rc = new ReturnCode<IList<T?>>();
            IList<T?> objList = new List<T?>();

            try {
                if (rc.Success) {
                    IReturnCode<IList<object?>> popObject = PopulateObjects(pDataReader, typeof(T));

                    if (popObject.Success) {
                        foreach(var obj in popObject.Data!) {
                            if (obj is T) {
                                objList.Add((T?)obj);
                            }
                        }
                    }
                    if (popObject.Failed) {
                        ErrorWorker.CopyErrors(popObject, rc);
                    }
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
        public static IReturnCode<IList<object?>> PopulateObjects(IDataReader pDataReader, Type pObjectType) {
            IReturnCode<IList<object?>> rc = new ReturnCode<IList<object?>>();
            IList<object?> objList = new List<object?>();

            try {
                PropertyInfo[] properties = pObjectType.GetProperties();

                while (pDataReader.NextRow()) {
                    object? obj = default;
                    obj = Activator.CreateInstance(pObjectType);

                    foreach (var prop in properties) {
                        if (pDataReader.HasValue(prop.Name)) {
                            object value = pDataReader.Get(prop.Name);

                            if (prop.PropertyType == typeof(int)) {
                                prop.SetValue(obj, Convert.ToInt32(value));
                            }
                            else if (prop.PropertyType.IsEnum) {
                                int intValue = Convert.ToInt32(value);
                                prop.SetValue(obj, prop.PropertyType.GetEnumValues().GetValue(intValue));
                            }
                            else if (prop.PropertyType == typeof(string)) {
                                prop.SetValue(obj, (string)value);
                            }
                            else if (prop.PropertyType == typeof(bool)) {
                                prop.SetValue(obj, (long)value != 0);
                            }
                            else if (prop.PropertyType.IsClass) {
                                string itemId = (string)value;
                                CustomAttributeData? tableAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(TableAttribute)).FirstOrDefault();

                                if (tableAttribute != null) {
                                     
                                }
                            }
                        }
                    }

                    objList.Add(obj);
                }
            }
            catch (Exception ex) {
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
