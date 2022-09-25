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
        bool HasRows { get; }
        void NextRow();
        object Get(string pName);
    }
}