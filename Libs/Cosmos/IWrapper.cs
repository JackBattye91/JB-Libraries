using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace JB.NoSqlDatabase {
    public interface IWrapper {
        public Task<JB.Common.Errors.IReturnCode<bool>> CreateDatabase(string pDatabaseId);
       
        public Task<JB.Common.Errors.IReturnCode<Interfaces.IContainer>> GetContainer(string pDatabaseId, string pContainerName);
        public Task<JB.Common.Errors.IReturnCode<Interfaces.IContainer>> CreateContainer(string pDatabaseId, string pContainerName, string pPartitionKey);
        
        public Task<JB.Common.Errors.IReturnCode<T>> GetItem<T>(string pDatebaseId, string pContainerId, string pItemId);
        public Task<JB.Common.Errors.IReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId);
        public Task<JB.Common.Errors.IReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId, string pQuery);
        public Task<JB.Common.Errors.IReturnCode<T>> AddItem<T>(string pDatabaseId, string pContainerId, T pItem);
        public Task<JB.Common.Errors.IReturnCode<T>> UpdateItem<T>(string pDatabaseId, string pContainerId, T pItem, string pItemId, string pPartionKeyValue);
        public Task<JB.Common.Errors.IReturnCode<T>> DeleteItem<T>(string pDatabaseId, string pContainerId, string pItemId, string pPartitionKeyValue);     
    }
}
