using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Reflection;

namespace JB.SqlDatabase.SQlite {
    internal class Wrapper : IWrapper {
        public async Task<ReturnCode<bool>> CreateDatabase(string pDatabaseName) {
            SqliteConnection connection = new SqliteConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "";

            SqliteDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync()) {
                string id = reader["id"] as string ?? string.Empty;
            }

            throw new NotImplementedException();
        }

        public Task<ReturnCode<bool>> CreateTable(string pDatabaseName, string pTableName) {
            throw new NotImplementedException();
        }

        public async Task<ReturnCode<Interfaces.IDataReader>> RunQuery(string pDatabaseName, string pQuery) {
            ReturnCode<Interfaces.IDataReader> rc = new ReturnCode<Interfaces.IDataReader>();
            Interfaces.IDataReader? dataReader = null;

            try {
                SqliteConnection connection = new SqliteConnection();
                await connection.OpenAsync();

                SqliteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = pQuery;

                dataReader = new DataReader(await command.ExecuteReaderAsync());
            }
            catch(Exception ex) {
                rc = new(3, ex);
            }

            if (rc.Success) {
                rc.Data = dataReader;
            }
            
            return rc;
        }
        
        public async Task<ReturnCode<Interfaces.IDataReader>> RunStoredProcedure(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters) {
            ReturnCode<Interfaces.IDataReader> rc = new();
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

                    dataReader = new DataReader(await command.ExecuteReaderAsync());
                }
            }
            catch(Exception ex) {
                rc = new(6, ex);
            }

            if (rc.Success) {
                rc.Data = dataReader;
            }
            
            return rc;
        }

        public async Task<ReturnCode<T>> GetData<T>(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters) {
            ReturnCode<T> rc = new ReturnCode<T>();
            Interfaces.IDataReader? dataReader = null;
            T? item = default;

            if (rc.Success) {
                ReturnCode<Interfaces.IDataReader> dataReaderRc = await RunStoredProcedure(pDatabaseName, pStoreProcedureName, pParameters);

                if (dataReaderRc.Success) {
                    dataReader = dataReaderRc.Data;
                }
                else {
                    ErrorWorker.CopyErrors(dataReaderRc, rc);
                }
            }
            
            if (rc.Success) {
                PropertyInfo[] propInfo = typeof(T).GetProperties();
                foreach(PropertyInfo prop in propInfo) {
                    prop.SetValue(item, 0);
                }
            }

            return rc;
        }
    }
}
