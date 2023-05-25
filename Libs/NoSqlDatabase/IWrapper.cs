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

        public Task<IReturnCode<Tinterface>> GetItem<Tinterface>(string pDatebaseId, string pContainerId, string pItemId);
        public Task<IReturnCode<Tinterface>> GetItem<Tinterface, Tmodel>(string pDatebaseId, string pContainerId, string pItemId) where Tmodel : class, Tinterface;

        public Task<IReturnCode<IList<Tinterface>>> GetItems<Tinterface>(string pDatabaseId, string pContainerId, string pQuery);
        public Task<IReturnCode<IList<Tinterface>>> GetItems<Tinterface, Tmodel>(string pDatabaseId, string pContainerId, string pQuery) where Tmodel : Tinterface;

        public Task<IReturnCode<IList<Tinterface>>> GetItems<Tinterface>(string pDatabaseId, string pContainerId);
        public Task<IReturnCode<IList<Tinterface>>> GetItems<Tinterface, Tmodel>(string pDatabaseId, string pContainerId) where Tmodel : Tinterface;
        
        public Task<IReturnCode<T>> AddItem<T>(string pDatabaseId, string pContainerId, T pItem);
        public Task<IReturnCode<Tinterface>> AddItem<Tinterface, Tmodel>(string pDatabaseId, string pContainerId, Tinterface pItem) where Tmodel : class, Tinterface;
        public Task<IReturnCode<T>> UpdateItem<T>(string pDatabaseId, string pContainerId, T pItem, string pItemId, string pPartionKeyValue);
        public Task<IReturnCode<Tinterface>> UpdateItem<Tinterface, Tmodel>(string pDatabaseId, string pContainerId, Tinterface pItem, string pItemId, string pPartionKeyValue) where Tmodel : class, Tinterface;
        public Task<IReturnCode<bool>> DeleteItem<T>(string pDatabaseId, string pContainerId, string pItemId, string pPartitionKeyValue);     
    }
}
