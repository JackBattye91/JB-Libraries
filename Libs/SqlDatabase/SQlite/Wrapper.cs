using JB.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.Sqlite;

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
    }
}
