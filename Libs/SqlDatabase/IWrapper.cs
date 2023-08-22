using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;

namespace JB.SqlDatabase {
    public interface IWrapper {
        Task<IReturnCode<bool>> CreateDatabase(string pDatabaseName);
        Task<IReturnCode<bool>> CreateTable<T>(string pDatabaseName, string pTableName);
        Task<IReturnCode<bool>> DeleteTable(string pDatabaseName, string pTableName);

        Task<IReturnCode<Interfaces.IDataReader>> RunQuery(string pDatabaseName, string pQuery);
        Task<IReturnCode<Interfaces.IDataReader>> RunStoredProcedure(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object> pParameters);
        
        Task<IReturnCode<IList<T>>> Get<T>(string pDatabaseName, string pTableName, string? pQueryParameters = null);
        Task<IReturnCode<T>> Insert<T>(string pDatabaseName, string pTableName, T pItem);
        Task<IReturnCode<T>> Update<T>(string pDatabaseName, string pTableName, T pItem, string pQueryParameters);
        Task<IReturnCode<bool>> Delete(string pDatabaseName, string pTableName, string pQueryParameters);
    }
}
