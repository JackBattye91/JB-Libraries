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

        Task<IReturnCode<bool>> CreateDatabase(string pDatabaseName);
        Task<IReturnCode<bool>> CreateTable(string pDatabaseName, string pTableName);
        Task<IReturnCode<Interfaces.IDataReader>> RunQuery(string pDatabaseName, string pQuery);
        Task<IReturnCode<Interfaces.IDataReader>> RunStoredProcedure(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters);
        Task<IReturnCode<T>> GetData<T>(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters);
    }
}
