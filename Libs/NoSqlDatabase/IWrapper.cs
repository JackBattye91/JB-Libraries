using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using JB.Common;

namespace JB.NoSqlDatabase {
    public interface IWrapper {
        public Task<IReturnCode> CreateDatabase(string pDatabaseId);
       
        public Task<IReturnCode<Interfaces.IContainer>> GetContainer(string pDatabaseId, string pContainerName);
        public Task<IReturnCode<Interfaces.IContainer>> CreateContainer(string pDatabaseId, string pContainerName, string pPartitionKey);

        public Task<IReturnCode<Tmodel>> GetItem<Tmodel>(string pDatebaseId, string pContainerId, string pItemId);
        public Task<IReturnCode<Tinterface>> GetItem<Tinterface, Tmodel>(string pDatebaseId, string pContainerId, string pItemId) where Tmodel : class, Tinterface;

        public Task<IReturnCode<IList<Tmodel>>> GetItems<Tmodel>(string pDatabaseId, string pContainerId, string pQuery);
        public Task<IReturnCode<IList<Tinterface>>> GetItems<Tinterface, Tmodel>(string pDatabaseId, string pContainerId, string pQuery) where Tmodel : Tinterface;

        public Task<IReturnCode<IList<Tmodel>>> GetItems<Tmodel>(string pDatabaseId, string pContainerId);
        public Task<IReturnCode<IList<Tinterface>>> GetItems<Tinterface, Tmodel>(string pDatabaseId, string pContainerId) where Tmodel : Tinterface;
        
        public Task<IReturnCode<Tmodel>> AddItem<Tmodel>(string pDatabaseId, string pContainerId, Tmodel pItem);
        public Task<IReturnCode<Tinterface>> AddItem<Tinterface, Tmodel>(string pDatabaseId, string pContainerId, Tinterface pItem) where Tmodel : class, Tinterface;
        public Task<IReturnCode<Tmodel>> UpdateItem<Tmodel>(string pDatabaseId, string pContainerId, Tmodel pItem, string pItemId, string pPartionKeyValue);
        public Task<IReturnCode<Tinterface>> UpdateItem<Tinterface, Tmodel>(string pDatabaseId, string pContainerId, Tinterface pItem, string pItemId, string pPartionKeyValue) where Tmodel : class, Tinterface;
        public Task<IReturnCode> DeleteItem<Tmodel>(string pDatabaseId, string pContainerId, string pItemId, string pPartitionKeyValue);
    }
}
