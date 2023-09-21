using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace JB.SqlDatabase.SQlite.Models
{
    internal class DataReader : SqlDatabase.Interfaces.IDataReader {
        protected SqliteDataReader SqlDataReader { get; set; }

        public DataReader(SqliteDataReader pReader)
        {
            SqlDataReader = pReader;
        }

        public bool HasRows() {
            return SqlDataReader.HasRows;
        }

        public bool NextRow()
        {
            return SqlDataReader.Read();
        }

        public int RowsAffected() {
            return SqlDataReader.RecordsAffected;
        }

        public object Get(string pName)
        {
            int ordinal = SqlDataReader.GetOrdinal(pName);
            return SqlDataReader.GetValue(ordinal);
        }

        public object Get(int pOrdinal) {
            return SqlDataReader.GetValue(pOrdinal);
        }

        public bool HasValue(string pName)
        {
            return Get(pName) != null;
        }
        public int GetOrdinal(string pName) {
            return SqlDataReader.GetOrdinal(pName);
        }
    }
}