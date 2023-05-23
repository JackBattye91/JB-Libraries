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
        public async Task<IReturnCode<bool>> CreateDatabase(string pDatabaseName) {
            SqliteConnection connection = new SqliteConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = $"CREATE DATABASE {pDatabaseName};";

            SqliteDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync()) {
                string id = reader["id"] as string ?? string.Empty;
            }

            throw new NotImplementedException();
        }

        public async Task<IReturnCode<bool>> CreateTable<T>(string pDatabaseName, string pTableName) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            SqliteConnection connection = new SqliteConnection();
            StringBuilder queryBuilder = new StringBuilder();

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.Errors.Add(new SqlDatabaseError(ErrorCodes.UNABLE_TO_OPEN_DATA_BASE));
                    }
                }

                if (rc.Success) {
                    queryBuilder.Append($"CREATE TABLE [IF NOT EXISTS] {pDatabaseName} (");
                    var properties = typeof(T).GetProperties();

                    foreach (var prop in properties) {
                        CustomAttributeData? primaryAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(PrimaryKeyAttribute)).FirstOrDefault();
                        CustomAttributeData? notNullAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(NotNullAttribute)).FirstOrDefault();

                        queryBuilder.Append($"{prop.Name} {Worker.ConvertToDatabaseType(prop.PropertyType)}");

                        if (notNullAttribute != null) {
                            queryBuilder.Append(" NOT NULL");
                        }
                        if (primaryAttribute != null) {
                            queryBuilder.Append(" PRIMARY KEY");
                        }
                    }

                    queryBuilder.Append($");");
                }

                if (rc.Success) { 
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = queryBuilder.ToString();

                    SqliteDataReader reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync()) {
                        string id = reader["id"] as string ?? string.Empty;
                    }
                }
            }
            catch (Exception ex) {
                rc.Errors.Add(new SqlDatabaseError(ErrorCodes.CREATE_TABLE_FAILED, ex));
            }

            return rc;
        }

        public async Task<IReturnCode<Interfaces.IDataReader>> RunQuery(string pDatabaseName, string pQuery) {
            IReturnCode<Interfaces.IDataReader> rc = new ReturnCode<Interfaces.IDataReader>();
            Interfaces.IDataReader? dataReader = null;

            try {
                SqliteConnection connection = new SqliteConnection();
                await connection.OpenAsync();

                SqliteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = pQuery;

                dataReader = new Models.DataReader(await command.ExecuteReaderAsync());
            }
            catch(Exception ex) {
                rc.Errors.Add(new SqlDatabaseError(ErrorCodes.RUN_QUERY_FAILED, ex));
            }

            if (rc.Success) {
                rc.Data = dataReader;
            }
            
            return rc;
        }
        
        public async Task<IReturnCode<Interfaces.IDataReader>> RunStoredProcedure(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters) {
            IReturnCode<Interfaces.IDataReader> rc = new ReturnCode<Interfaces.IDataReader>();
            Interfaces.IDataReader? dataReader = null;

            try {
                if (rc.Success) {
                    SqliteConnection connection = new SqliteConnection();
                    await connection.OpenAsync();

                    SqliteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, object> pair in pParameters) {
                        command.Parameters.AddWithValue(pair.Key, pair.Value);
                    }

                    dataReader = new Models.DataReader(await command.ExecuteReaderAsync());
                }
            }
            catch(Exception ex) {
                rc.Errors.Add(new SqlDatabaseError(ErrorCodes.RUN_STORE_PROCEDURE_FAILED, ex));
            }

            if (rc.Success) {
                rc.Data = dataReader;
            }
            
            return rc;
        }

        public async Task<IReturnCode<T>> GetData<T>(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters) where T : struct {
            IReturnCode<T> rc = new ReturnCode<T>();
            Interfaces.IDataReader? dataReader = null;
            T item = default;

            try {
                if (rc.Success) {
                    IReturnCode<Interfaces.IDataReader> dataReaderRc = await RunStoredProcedure(pDatabaseName, pStoreProcedureName, pParameters);

                    if (dataReaderRc.Success) {
                        dataReader = dataReaderRc.Data;

                        while (dataReader?.NextRow() ?? false) {
                            IReturnCode<T> parseDataRc = Worker.ParseData<T>(dataReader);

                            if (parseDataRc.Success) {
                                item = parseDataRc.Data;
                            }
                            if (parseDataRc.Failed) {
                                ErrorWorker.CopyErrors(parseDataRc, rc);
                            }
                        }
                    }
                    else {
                        ErrorWorker.CopyErrors(dataReaderRc, rc);
                    }
                }

                if (rc.Success) {
                    PropertyInfo[] propInfo = typeof(T).GetProperties();
                    foreach (PropertyInfo prop in propInfo) {
                        prop.SetValue(item, 0);
                    }
                }
            }
            catch (Exception ex) {
                rc.Errors.Add(new SqlDatabaseError(ErrorCodes.GET_DATA_FAILED, ex));
            }

            return rc;
        }
    }
}
