using JB.Common;
using JB.SqlDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;

namespace JB.SqlDatabase.MsSql {
    internal class Wrapper : SqlDatabase.IWrapper {
        public Task<IReturnCode<bool>> CreateDatabase(string pDatabaseName) {
            throw new NotImplementedException();
        }

        public Task<IReturnCode<bool>> CreateTable<T>(string pDatabaseName, string pTableName) {
            throw new NotImplementedException();
        }

        public Task<IReturnCode<bool>> Delete(string pDatabaseName, string pTableName, string pQueryParameters) {
            throw new NotImplementedException();
        }

        public Task<IReturnCode<bool>> DeleteTable(string pDatabaseName, string pTableName) {
            throw new NotImplementedException();
        }

        public Task<IReturnCode<IList<T>>> Get<T>(string pDatabaseName, string pTableName, string? pQueryParameters = null) {
            throw new NotImplementedException();
        }

        public Task<IReturnCode<T>> Insert<T>(string pDatabaseName, string pTableName, T pItem) {
            throw new NotImplementedException();
        }

        public Task<IReturnCode<IDataReader>> RunQuery(string pDatabaseName, string pQuery) {
            throw new NotImplementedException();
        }

        public Task<IReturnCode<IDataReader>> RunStoredProcedure(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters) {
            throw new NotImplementedException();
        }

        public Task<IReturnCode<T>> Update<T>(string pDatabaseName, string pTableName, T pItem, string pQueryParameters) {
            throw new NotImplementedException();
        }


        protected SqlConnection? CreateConnection() {
            try {
                string connectionString = Environment.GetEnvironmentVariable(Consts.EnvironmentVariables.ConnectionString) ?? "";
                return new SqlConnection(connectionString);
            }
            catch { return null; }
        }
    }
}
