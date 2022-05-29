using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace JB.NoSqlDatabase {
    public interface IWrapper {
        public JB.Common.IReturnCode<bool> CreateDatabase(string pDatabaseId);
        public JB.Common.IReturnCode<Container> GetContainer(string pDatabaseId, string pContainerName);
        public JB.Common.IReturnCode<Container> CreateContainer(string pDatabaseId, string pContainerName);
        public Task<JB.Common.IReturnCode<T>> GetItem<T>(string pDatebaseId, string pContainerId, string pItemId);
        public Task<JB.Common.IReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pTableName);
        public Task<JB.Common.IReturnCode<T>> AddItem<T>(string pDatabaseId, string pTableName, T pItem);
    }
}
