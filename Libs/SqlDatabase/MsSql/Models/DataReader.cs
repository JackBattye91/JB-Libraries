using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace JB.SqlDatabase.MsSql.Models {

    internal class DataReader : SqlDatabase.Interfaces.IDataReader {
        SqlDataReader SqlDataReader;

        public DataReader(SqlDataReader pSqlDataReader) {
            SqlDataReader = pSqlDataReader;
        }

        public object Get(string pName) {
            int ordinal = SqlDataReader.GetOrdinal(pName);
            return SqlDataReader.GetValue(ordinal);
        }

        public object Get(int pOrdinal) {
            return SqlDataReader.GetValue(pOrdinal);
        }

        public int GetOrdinal(string pName) {
            return SqlDataReader.GetOrdinal(pName);
        }

        public bool HasRows() {
            return SqlDataReader.HasRows;
        }

        public bool HasValue(string pName) {
            return Get(pName) != null;
        }

        public bool NextRow() {
            return SqlDataReader.NextResult();
        }

        public int RowsAffected() {
            return SqlDataReader.RecordsAffected;
        }
    }
}
