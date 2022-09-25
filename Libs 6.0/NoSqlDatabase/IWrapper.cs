using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace JB.NoSqlDatabase {
    public interface IWrapper {
        public Task<JB.Common.ReturnCode<bool>> CreateDatabase(string pDatabaseId);
       
        public Task<JB.Common.ReturnCode<Interfaces.IContainer>> GetContainer(string pDatabaseId, string pContainerName);
        public Task<JB.Common.ReturnCode<Interfaces.IContainer>> CreateContainer(string pDatabaseId, string pContainerName, string pPartitionKey);
        
        public Task<JB.Common.ReturnCode<T>> GetItem<T>(string pDatebaseId, string pContainerId, string pItemId);
        public Task<JB.Common.ReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId);
        public Task<JB.Common.ReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId, string pQuery);
        public Task<JB.Common.ReturnCode<T>> AddItem<T>(string pDatabaseId, string pContainerId, T pItem);
        public Task<JB.Common.ReturnCode<T>> UpdateItem<T>(string pDatabaseId, string pContainerId, T pItem, string pItemId, string pPartionKeyValue);
        public Task<JB.Common.ReturnCode<T>> DeleteItem<T>(string pDatabaseId, string pContainerId, string pItemId, string pPartitionKeyValue);     
    }
}
