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
        public async Task<IReturnCode<bool>> CreateDatabase(string pDatabaseName) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            SqlConnection? connection = null;

            try {
                if (rc.Success) {
                    IReturnCode<SqlConnection> connectRc = CreateConnection();

                    if (connectRc.Success) {
                        connection = connectRc.Data;
                    }

                    if (connectRc.Failed) {
                        ErrorWorker.CopyErrors(connectRc, rc);
                    }
                }

                if (rc.Success) {
                    SqlCommand command = connection!.CreateCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = $"CREATE DATABASE {pDatabaseName};";

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.CREATE_DATABASE_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return rc;
        }

        public async Task<IReturnCode<bool>> CreateTable<T>(string pDatabaseName, string pTableName) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            SqlConnection? connection = null;

            try {
                if (rc.Success) {
                    IReturnCode<SqlConnection> connectRc = CreateConnection();

                    if (connectRc.Success) {
                        connection = connectRc.Data;
                    }

                    if (connectRc.Failed) {
                        ErrorWorker.CopyErrors(connectRc, rc);
                    }
                }

                if (rc.Success) {
                    if (connection?.Database.Equals(pDatabaseName) != true) {
                        rc.ErrorCode = ErrorCodes.CONNECTED_TO_INCORRECT_DATABASE;
                        rc.Errors.Add(new Error(rc.ErrorCode, new Exception("Connected to incorrect database")));
                    }
                }

                if (rc.Success) {
                    SqlCommand command = connection!.CreateCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = $"CREATE TABLE {pTableName}(params);";

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.CREATE_DATABASE_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return rc;
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

        public Task<IReturnCode<IList<Tinterface>>> Get<Tinterface, Tmodel>(string pDatabaseName, string pTableName, string? pQueryParameters = null) where Tmodel : Tinterface {
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


        protected IReturnCode<SqlConnection> CreateConnection() {
            IReturnCode<SqlConnection> rc = new ReturnCode<SqlConnection>();
            string? connectionString = null;
            SqlConnection? sqlConnection = null;

            try {
                if (rc.Success) {
                    connectionString = Environment.GetEnvironmentVariable(Consts.EnvironmentVariables.ConnectionString);

                    if (string.IsNullOrEmpty(connectionString)) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_GET_CONNECTION_STRING;
                        rc.Errors.Add(new Error(rc.ErrorCode, new Exception("Unable to find connection string")));
                    }
                }

                if (rc.Success) {
                    sqlConnection = new SqlConnection(connectionString!);
                    sqlConnection.Open();
                }

                if (rc.Success) {
                    if (sqlConnection?.State != System.Data.ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_CONNECTION_TO_SERVER;
                        rc.Errors.Add(new Error(rc.ErrorCode, new Exception("Connection is not open")));
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.CREATE_CONNECTION_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = sqlConnection;
            }

            return rc;
        }
    }
}
