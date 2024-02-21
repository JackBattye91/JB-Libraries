using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JB.Common;
using JB.SqlDatabase.Attributes;
using JB.SqlDatabase.Interfaces;
using JB.SqlDatabase.SQlite.Interfaces;

namespace JB.SqlDatabase.SQlite {
    internal class Worker {
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

        public static IReturnCode<IList<IObjectProperty>> GetObjectProperties(object? pObj) {
            IReturnCode<IList<IObjectProperty>> rc = new ReturnCode<IList<IObjectProperty>>();
            IList<IObjectProperty> propertiesList = new List<IObjectProperty>();

            try {
                if (rc.Success) {
                    PropertyInfo[] properties = pObj?.GetType().GetProperties() ?? Array.Empty<PropertyInfo>();

                    foreach (var prop in properties) {
                        propertiesList.Add(new Models.ObjectProperty() {
                            Name = prop.Name,
                            Value = prop.GetValue(pObj),
                            Attributes = prop.CustomAttributes.ToList()
                        });
                    }
                }
            }
            catch (Exception ex) {
                rc.AddError(new Error(ErrorCodes.GET_OBJECT_VALUES_FAILED, ex));
            }

            if (rc.Success) {
                rc.Data = propertiesList;
            }

            return rc;
        }
    }
}
