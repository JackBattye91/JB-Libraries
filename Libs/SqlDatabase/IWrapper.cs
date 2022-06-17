using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common.Errors;

namespace JB.SqlDatabase {
    public interface IWrapper {
        Task<IReturnCode<bool>> CreateDatabase(string pDatabaseName);
        Task<IReturnCode<bool>> CreateTable(string pDatabaseName, string pTableName);

    }
}
