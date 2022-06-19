using JB.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.Sqlite;

namespace JB.SqlDatabase.SQlite {
    internal class DataReader : Interfaces.IDataReader {
        public bool HasRows { get => return sqlDataReader.HasRows; }
        protected SqlDataReader sqlDataReader { get; set; }

        public DataReader(SqlDataReader pReader) {
            sqlDataReader = pReader;
        }

        public void NextRow() {
            sqlDataReader.Read();
        }
        public object Get(string pName) {
            int ordinal = sqlDataReader.GetOrdinal(pName);
            return sqlDataReader.GetValue(ordinal);
        }

        public bool HasValue(string pName) {
            DataTable dataTabe = sqlDataReader.GetSchemaTable();
            throw new NotImplementedException();
        }
    }
}