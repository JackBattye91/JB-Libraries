using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace JB.NoSqlDatabase {
    public interface IWrapper {
        public Task<JB.Common.IReturnCode<bool>> CreateDatabase(string pDatabaseId);
       
        public Task<JB.Common.IReturnCode<Container>> GetContainer(string pDatabaseId, string pContainerName);
        public Task<JB.Common.IReturnCode<Container>> CreateContainer(string pDatabaseId, string pContainerName, string pPartitionKey);
        
        public Task<JB.Common.IReturnCode<T>> GetItem<T>(string pDatebaseId, string pContainerId, string pItemId);
        public Task<JB.Common.IReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId);
        public Task<JB.Common.IReturnCode<T>> AddItem<T>(string pDatabaseId, string pContainerId, T pItem);
        public Task<JB.Common.IReturnCode<T>> UpdateItem<T>(string pDatabaseId, string pContainerId, T pItem, string pItemId);
        public Task<JB.Common.IReturnCode<T>> DeleteItem<T>(string pDatabaseId, string pContainerId, string pItemId, string pPartitionKey);     
    }
}
