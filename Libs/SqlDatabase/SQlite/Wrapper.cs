using JB.Common.Errors;
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
        public async Task<IReturnCode<bool>> CreateDatabase(string pDatabaseName) {
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

        public Task<IReturnCode<bool>> CreateTable(string pDatabaseName, string pTableName) {
            throw new NotImplementedException();
        }

        public async Task<IReturnCode<Interfaces.IDataReader>> RunQuery(string pDatabaseName, string pQuery) {
            IReturnCode<Interfaces.IDataReader> rc = new ReturnCode<Interfaces.IDataReader>();
            Interfaces.IDataReader? dataReader = null;

            try {
                SqliteConnection connection = new SqliteConnection();
                await connection.OpenAsync();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = pQuery;

                dataReader = new DataReader(await command.ExecuteReaderAsync());
            }
            catch(Exception e) {
                rc.ErrorCode = 9;
                ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
            }

            if (rc.ErrorCode == ErrorCodes.SUCCESS) {
                rc.Data = dataReader;
            }
            
            return rc;
        }
        
        public async Task<IReturnCode<Interfaces.IDataReader>> RunStoredProcedure(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters) {
            IReturnCode<Interfaces.IDataReader> rc = new ReturnCode<Interfaces.IDataReader>();
            Interfaces.IDataReader? dataReader = null;

            try {
                SqliteConnection connection = new SqliteConnection();
                await connection.OpenAsync();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                foreach(KeyValuePair<string, object> pair in pParameters) {
                    sqlCommand.Parameters.AddWithValue(pair.Key, pair.Value);
                }

                dataReader = new DataReader(await command.ExecuteReaderAsync());
            }
            catch(Exception e) {
                rc.ErrorCode = 9;
                ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
            }

            if (rc.ErrorCode == ErrorCodes.SUCCESS) {
                rc.Data = dataReader;
            }
            
            return rc;
        }

        public async Task<IReturnCode<T>> GetData<T>(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters) {
            IReturnCode<T> rc = new ReturnCode<T>();
            Interfaces.IDataReader? dataReader = null;
            T? item = default;

            if (rc.ErrorCode == ErrorCodes.SUCCESS) {
                IReturnCode<Interfaces.IDataReader> dataReaderRc = await RunStoredProcedure(pDatabaseName, pStoreProcedureName, pParameters);

                if (dataReaderRc.ErrorCode == ErrorCodes.SUCCESS) {
                    dataReader = dataReaderRc.Data;
                }
                if (dataReaderRc.ErrorCode != ErrorCodes.SUCCESS) {
                    ErrorWorker.AddError(rc, rc.ErrorCode);
                }
            }
            
            if (rc.ErrorCode == ErrorCodes.SUCCESS) {
                PropertyInfo[] propInfo = typeof(T).GetProperties();
                foreach(PropertyInfo prop in propInfo) {
                    prop.SetValue(item, 0);
                }
            }

            return rc;
        }
    }
}
