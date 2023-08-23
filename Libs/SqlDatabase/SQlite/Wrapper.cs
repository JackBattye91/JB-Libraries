using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Reflection;
using JB.SqlDatabase.Attributes;

namespace JB.SqlDatabase.SQlite {
    internal class Wrapper : IWrapper {
        public Task<IReturnCode<bool>> CreateDatabase(string pDatabaseName) {
            IReturnCode<bool> rc = new ReturnCode<bool>();

            try {
                if (rc.Success) {
                    using (FileStream dbFile = File.Create(pDatabaseName)) {
                        dbFile.Close();
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.CREATE_DATABASE_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return Task.FromResult(rc);
        }
        public async Task<IReturnCode<bool>> CreateTable<T>(string pDatabaseName, string pTableName) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            SqliteConnection connection = CreateConnection(pDatabaseName);
            StringBuilder queryBuilder = new StringBuilder();

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    queryBuilder.Append($"CREATE TABLE IF NOT EXISTS {pTableName} (");
                    var properties = typeof(T).GetProperties();

                    for (int p = 0; p < properties.Length; p++) {
                        PropertyInfo prop = properties[p];

                        CustomAttributeData? primaryAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(PrimaryKeyAttribute)).FirstOrDefault();
                        CustomAttributeData? notNullAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(NotNullAttribute)).FirstOrDefault();
                        CustomAttributeData? autoIncrementAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(AutoIncrementAttribute)).FirstOrDefault();

                        queryBuilder.Append($"{prop.Name} {Worker.ConvertToDatabaseType(prop.PropertyType)}");

                        if (notNullAttribute != null) {
                            queryBuilder.Append(" NOT NULL");
                        }
                        if (primaryAttribute != null) {
                            queryBuilder.Append(" PRIMARY KEY");
                        }
                        if (autoIncrementAttribute != null) {
                            queryBuilder.Append(" AUTOINCREMENT");
                        }
                        if ((prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) == false) {
                            queryBuilder.Append(" NOT NULL");
                        }

                        if (p + 1 < properties.Length) {
                            queryBuilder.Append(", ");
                        }
                    }

                    queryBuilder.Append($");");
                }

                if (rc.Success) { 
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = queryBuilder.ToString();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.CREATE_TABLE_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return rc;
        }
        public async Task<IReturnCode<bool>> DeleteTable(string pDatabaseName, string pTableName) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            SqliteConnection connection = CreateConnection(pDatabaseName);

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"DROP TABLE {pTableName};";
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.DELETE_TABLE_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return rc;
        }


        public async Task<IReturnCode<Interfaces.IDataReader>> RunQuery(string pDatabaseName, string pQuery) {
            IReturnCode<Interfaces.IDataReader> rc = new ReturnCode<Interfaces.IDataReader>();
            Interfaces.IDataReader? dataReader = null;
            SqliteConnection connection = CreateConnection(pDatabaseName); ;

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = pQuery;

                    dataReader = new Models.DataReader(await command.ExecuteReaderAsync());
                }
            }
            catch(Exception ex) {
                rc.ErrorCode = ErrorCodes.RUN_QUERY_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = dataReader;
            }
            
            return rc;
        }
        public Task<IReturnCode<Interfaces.IDataReader>> RunStoredProcedure(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters) {
            throw new NotImplementedException();
        }


        public async Task<IReturnCode<IList<T>>> Get<T>(string pDatabaseName, string pTableName, string? pQueryParameters = null) {
            IReturnCode<IList<T>> rc = new ReturnCode<IList<T>>();
            Interfaces.IDataReader? dataReader = null;
            SqliteConnection connection = CreateConnection(pDatabaseName);
            StringBuilder queryBuilder = new StringBuilder();
            IList<T> itemsList = new List<T>();

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    queryBuilder.Append($"SELECT * FROM {pTableName}");

                    if (pQueryParameters?.Length > 0) {
                        queryBuilder.Append($" WHERE {pQueryParameters}");
                    }

                    queryBuilder.Append(";");
                }

                if (rc.Success) {
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = queryBuilder.ToString();

                    dataReader = new Models.DataReader(await command.ExecuteReaderAsync());
                }

                if (rc.Success) {
                    IReturnCode<IList<T?>> populateObjRc = Worker.PopulateObjects<T>(dataReader!);

                    if (populateObjRc.Success) {
                        itemsList = populateObjRc.Data!;
                    }

                    if (populateObjRc.Failed) {
                        ErrorWorker.CopyErrors(populateObjRc, rc);
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.GET_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = itemsList;
            }

            return rc;
        }
        public async Task<IReturnCode<T>> Insert<T>(string pDatabaseName, string pTableName, T pItem) {
            IReturnCode<T> rc = new ReturnCode<T>();
            Interfaces.IDataReader? dataReader = null;
            SqliteConnection connection = CreateConnection(pDatabaseName);
            StringBuilder queryBuilder = new StringBuilder();
            IDictionary<string, object?> dataMap = new Dictionary<string, object?>();

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    IReturnCode<IDictionary<string, object?>> getValuesRc = Worker.GetObjectValues(pItem);

                    if (getValuesRc.Success) {
                        dataMap = getValuesRc.Data!;
                    }

                    if (getValuesRc.Failed) {
                        ErrorWorker.CopyErrors(getValuesRc, rc);
                    }
                }

                if (rc.Success) {
                    queryBuilder.Append($"INSERT INTO {pTableName}(");

                    IList<string> keyList = dataMap.Keys.ToList();
                    for (int k = 0; k < keyList.Count; k++) {
                        string key = keyList[k];
                        queryBuilder.Append($"{key}");

                        if (k + 1 < keyList.Count) {
                            queryBuilder.Append(',');
                        }
                    }
                    queryBuilder.Append(") VALUES (");

                    IList<object?> valueList = dataMap.Values.ToList();
                    for (int v = 0; v < valueList.Count; v++) {
                        object? value = valueList[v];

                        if (value?.GetType() ==  typeof(string)) {
                            queryBuilder.Append($"'{value}'");
                        }
                        else {
                            queryBuilder.Append($"{value}");
                        }
                        
                        if (v + 1 < valueList.Count) {
                            queryBuilder.Append(',');
                        }
                    }

                    queryBuilder.Append(");");
                }

                if (rc.Success) {
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = queryBuilder.ToString();

                    dataReader = new Models.DataReader(await command.ExecuteReaderAsync());
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.INSERT_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = pItem;
            }

            return rc;
        }
        public async Task<IReturnCode<T>> Update<T>(string pDatabaseName, string pTableName, T pItem, string pQueryParameters) {
            IReturnCode<T> rc = new ReturnCode<T>();
            Interfaces.IDataReader? dataReader = null;
            SqliteConnection connection = CreateConnection(pDatabaseName);
            StringBuilder queryBuilder = new StringBuilder();
            IDictionary<string, object?> dataMap = new Dictionary<string, object?>();

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    IReturnCode<IDictionary<string, object?>> getValuesRc = Worker.GetObjectValues(pItem);

                    if (getValuesRc.Success) {
                        dataMap = getValuesRc.Data!;
                    }

                    if (getValuesRc.Failed) {
                        ErrorWorker.CopyErrors(getValuesRc, rc);
                    }
                }

                if (rc.Success) {
                    queryBuilder.Append($"UPDATE {pTableName} SET");

                    for(int p = 0; p < dataMap.Count; p++) {
                        KeyValuePair<string, object?> prop = dataMap.ElementAt(p);

                        if (prop.Value?.GetType() == typeof(string)) {
                            queryBuilder.Append($" {prop.Key} = '{prop.Value}'");
                        }
                        else {
                            queryBuilder.Append($" {prop.Key} = {prop.Value}");
                        }
                        

                        if (p + 1 < dataMap.Count) {
                            queryBuilder.Append(',');
                        }
                    }

                    queryBuilder.Append($" WHERE {pQueryParameters};");
                }

                if (rc.Success) {
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = queryBuilder.ToString();

                    dataReader = new Models.DataReader(await command.ExecuteReaderAsync());
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.INSERT_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = pItem;
            }

            return rc;
        }
        public async Task<IReturnCode<bool>> Delete(string pDatabaseName, string pTableName, string pQueryParameters) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            SqliteConnection connection = CreateConnection(pDatabaseName);

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"DELETE FROM {pTableName} WHERE {pQueryParameters}";

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.INSERT_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return rc;
        }


        protected static SqliteConnection CreateConnection(string pDatabaseName) {
            return new SqliteConnection($"Data Source={pDatabaseName}");
        }
    }
}
