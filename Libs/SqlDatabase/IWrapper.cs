using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;

namespace JB.SqlDatabase {
    public interface IWrapper {
        Task<ReturnCode<bool>> CreateDatabase(string pDatabaseName);
        Task<ReturnCode<bool>> CreateTable(string pDatabaseName, string pTableName);
        Task<ReturnCode<Interfaces.IDataReader>> RunQuery(string pDatabaseName, string pQuery);
        Task<ReturnCode<Interfaces.IDataReader>> RunStoredProcedure(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters);
        Task<ReturnCode<T>> GetData<T>(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters);
    }
}
