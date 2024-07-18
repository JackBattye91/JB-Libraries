using JB.Common;
using JB.NoSqlDatabase.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace JB.NoSqlDatabase
{
    public interface INoSqlDatabaseService
    {
        Task<IReturnCode> CreateDatabase(string pDatabaseId);

        Task<IReturnCode<Interfaces.IContainer>> GetContainer(string pDatabaseId, string pContainerName);
        Task<IReturnCode<Interfaces.IContainer>> CreateContainer(string pDatabaseId, string pContainerName, string pPartitionKey);

        Task<IReturnCode<Tmodel>> GetItem<Tmodel>(string pDatebaseId, string pContainerId, string pItemId);
        Task<IReturnCode<Tinterface>> GetItem<Tinterface, Tmodel>(string pDatebaseId, string pContainerId, string pItemId) where Tmodel : class, Tinterface;

        Task<IReturnCode<IList<Tmodel>>> GetItems<Tmodel>(string pDatabaseId, string pContainerId, string pQuery);
        Task<IReturnCode<IList<Tinterface>>> GetItems<Tinterface, Tmodel>(string pDatabaseId, string pContainerId, string pQuery) where Tmodel : Tinterface;

        Task<IReturnCode<IList<Tmodel>>> GetItems<Tmodel>(string pDatabaseId, string pContainerId);
        Task<IReturnCode<IList<Tinterface>>> GetItems<Tinterface, Tmodel>(string pDatabaseId, string pContainerId) where Tmodel : Tinterface;

        Task<IReturnCode<Tmodel>> AddItem<Tmodel>(string pDatabaseId, string pContainerId, Tmodel pItem);
        Task<IReturnCode<Tinterface>> AddItem<Tinterface, Tmodel>(string pDatabaseId, string pContainerId, Tinterface pItem) where Tmodel : class, Tinterface;
        Task<IReturnCode<Tmodel>> UpdateItem<Tmodel>(string pDatabaseId, string pContainerId, Tmodel pItem, string pItemId, string pPartionKeyValue);
        Task<IReturnCode<Tinterface>> UpdateItem<Tinterface, Tmodel>(string pDatabaseId, string pContainerId, Tinterface pItem, string pItemId, string pPartionKeyValue) where Tmodel : class, Tinterface;
        Task<IReturnCode> DeleteItem<Tmodel>(string pDatabaseId, string pContainerId, string pItemId, string pPartitionKeyValue);
    }

    public class NoSqlDatabaseService : INoSqlDatabaseService
    {
        private readonly IWrapper _wrapper;
        public NoSqlDatabaseService(string? connectionString = null)
        {
            _wrapper = Factory.CreateNoSqlDatabaseWrapper(connectionString);
        }

        public async Task<IReturnCode<Tmodel>> AddItem<Tmodel>(string pDatabaseId, string pContainerId, Tmodel pItem) =>
            await _wrapper.AddItem(pDatabaseId, pContainerId, pItem);

        public async Task<IReturnCode<Tinterface>> AddItem<Tinterface, Tmodel>(string pDatabaseId, string pContainerId, Tinterface pItem) where Tmodel : class, Tinterface =>
            await _wrapper.AddItem<Tinterface, Tmodel>(pDatabaseId, pContainerId, pItem);

        public async Task<IReturnCode<IContainer>> CreateContainer(string pDatabaseId, string pContainerName, string pPartitionKey) =>
            await _wrapper.CreateContainer(pDatabaseId, pContainerName, pPartitionKey);

        public async Task<IReturnCode> CreateDatabase(string pDatabaseId) =>
            await _wrapper.CreateDatabase(pDatabaseId);

        public async Task<IReturnCode> DeleteItem<Tmodel>(string pDatabaseId, string pContainerId, string pItemId, string pPartitionKeyValue) =>
            await _wrapper.DeleteItem<Tmodel>(pDatabaseId, pContainerId, pItemId, pPartitionKeyValue);

        public async Task<IReturnCode<IContainer>> GetContainer(string pDatabaseId, string pContainerName) =>
            await _wrapper.GetContainer(pDatabaseId, pContainerName);

        public async Task<IReturnCode<Tmodel>> GetItem<Tmodel>(string pDatebaseId, string pContainerId, string pItemId) =>
            await _wrapper.GetItem<Tmodel>(pDatebaseId, pContainerId, pItemId);

        public async Task<IReturnCode<Tinterface>> GetItem<Tinterface, Tmodel>(string pDatebaseId, string pContainerId, string pItemId) where Tmodel : class, Tinterface =>
            await _wrapper.GetItem<Tinterface, Tmodel>(pDatebaseId, pContainerId, pItemId);

        public async Task<IReturnCode<IList<Tmodel>>> GetItems<Tmodel>(string pDatabaseId, string pContainerId, string pQuery) =>
            await _wrapper.GetItems<Tmodel>(pDatabaseId, pContainerId, pQuery);
        public async Task<IReturnCode<IList<Tinterface>>> GetItems<Tinterface, Tmodel>(string pDatabaseId, string pContainerId, string pQuery) where Tmodel : Tinterface =>
            await _wrapper.GetItems<Tinterface, Tmodel>(pDatabaseId, pContainerId, pQuery);

        public async Task<IReturnCode<IList<Tmodel>>> GetItems<Tmodel>(string pDatabaseId, string pContainerId) =>
            await _wrapper.GetItems<Tmodel>(pDatabaseId, pContainerId);

        public async Task<IReturnCode<IList<Tinterface>>> GetItems<Tinterface, Tmodel>(string pDatabaseId, string pContainerId) where Tmodel : Tinterface =>
            await _wrapper.GetItems<Tinterface, Tmodel>(pDatabaseId, pContainerId);

        public async Task<IReturnCode<Tmodel>> UpdateItem<Tmodel>(string pDatabaseId, string pContainerId, Tmodel pItem, string pItemId, string pPartionKeyValue) =>
            await _wrapper.UpdateItem<Tmodel>(pDatabaseId, pContainerId, pItem, pItemId, pPartionKeyValue);

        public async Task<IReturnCode<Tinterface>> UpdateItem<Tinterface, Tmodel>(string pDatabaseId, string pContainerId, Tinterface pItem, string pItemId, string pPartionKeyValue) where Tmodel : class, Tinterface =>
            await _wrapper.UpdateItem<Tinterface, Tmodel>(pDatabaseId, pContainerId, pItem, pItemId, pPartionKeyValue);
    }

    public static class NoSqlDatabaseServiceExtension
    {
        public static IServiceCollection AddNoSqlDatabaseService(this IServiceCollection services, string? connectionString = null)
        {
            services.AddTransient<INoSqlDatabaseService, NoSqlDatabaseService>((inter) => { return new NoSqlDatabaseService(connectionString); });
            return services;
        }
    }
}
