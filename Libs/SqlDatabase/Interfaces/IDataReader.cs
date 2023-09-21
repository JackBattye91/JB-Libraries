using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.Sqlite;

namespace JB.SqlDatabase.Interfaces {
    public interface IDataReader {
        bool HasRows();
        bool NextRow();
        int RowsAffected();
        object Get(string pName);
        object Get(int pOrdinal);
        bool HasValue(string pName);
        int GetOrdinal(string pName);
        
    }
}