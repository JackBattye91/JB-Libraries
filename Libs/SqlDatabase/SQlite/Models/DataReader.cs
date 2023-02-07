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
    internal class DataReader : Interfaces.IDataReader
    {
        protected SqliteDataReader sqlDataReader { get; set; }

        public DataReader(SqliteDataReader pReader)
        {
            sqlDataReader = pReader;
        }

        public bool HasRows() {
            return sqlDataReader.HasRows;
        }

        public bool NextRow()
        {
            return sqlDataReader.Read();
        }
        public object Get(string pName)
        {
            int ordinal = sqlDataReader.GetOrdinal(pName);
            return sqlDataReader.GetValue(ordinal);
        }
        public object Get(int pOrdinal) {
            return sqlDataReader.GetValue(pOrdinal);
        }

        public bool HasValue(string pName)
        {
            DataTable dataTabe = sqlDataReader.GetSchemaTable();

            foreach(DataColumn column in dataTabe.Columns) {
                if (column.ColumnName.ToLower().Equals(pName.ToLower())) {
                    return true;
                }
            }

            return false;
        }
        public int GetOrdinal(string pName) {
            return sqlDataReader.GetOrdinal(pName);
        }
    }
}