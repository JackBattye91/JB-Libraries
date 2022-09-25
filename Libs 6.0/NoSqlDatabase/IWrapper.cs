using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using JB.Common;

namespace JB.NoSqlDatabase {
    public interface IWrapper {
        public Task<IReturnCode<bool>> CreateDatabase(string pDatabaseId);
       
        public Task<IReturnCode<Interfaces.IContainer>> GetContainer(string pDatabaseId, string pContainerName);
        public Task<IReturnCode<Interfaces.IContainer>> CreateContainer(string pDatabaseId, string pContainerName, string pPartitionKey);
        
        public Task<IReturnCode<T>> GetItem<T>(string pDatebaseId, string pContainerId, string pItemId);
        public Task<IReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId);
        public Task<IReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId, string pQuery);
        public Task<IReturnCode<T>> AddItem<T>(string pDatabaseId, string pContainerId, T pItem);
        public Task<IReturnCode<T>> UpdateItem<T>(string pDatabaseId, string pContainerId, T pItem, string pItemId, string pPartionKeyValue);
        public Task<IReturnCode<bool>> DeleteItem<T>(string pDatabaseId, string pContainerId, string pItemId, string pPartitionKeyValue);     
    }
}
