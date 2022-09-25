using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace JB.NoSqlDatabase.Cosmos {
    internal class Wrapper : JB.NoSqlDatabase.IWrapper {
        protected CosmosClient? cosmosClient;

        public Wrapper() {
            cosmosClient = null;
        }

        public async Task<ReturnCode<bool>> CreateDatabase(string pDatabaseId) {
            ReturnCode<bool> rc = new ReturnCode<bool>();
            string? connectionString = Environment.GetEnvironmentVariable("cosmos-connection-string");

            try {
                cosmosClient = new CosmosClient(connectionString);
                DatabaseResponse response = await cosmosClient.CreateDatabaseIfNotExistsAsync(pDatabaseId);
                
                if (System.Net.HttpStatusCode.OK != response.StatusCode) {
                    rc = new(JB.Common.ErrorCodes.BAD_HTTP_STATUS_CODE);
                }
            }
            catch (Exception ex) {
                rc = new(ErrorCodes.UNABLE_TO_CREATE_DATABASE, ex);
            }
            

            return rc;
        }
        public async Task<ReturnCode<Interfaces.IContainer>> GetContainer(string pDatabaseId, string pContainerId) {
            ReturnCode<Interfaces.IContainer> rc = new ReturnCode<Interfaces.IContainer>();
            Container? cosmosContainer = null;
            Interfaces.IContainer? container = null;

            if (rc.Success) {
                var getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                if (getContainerRc.Success) {
                    cosmosContainer = getContainerRc.Data;
                }
                if (getContainerRc.Success == false) {
                    ErrorWorker.CopyErrors(getContainerRc, rc);
                }
            }

            if (rc.Success) {
                container = new Models.Container() {
                    ContainerName = pContainerId,
                    PartitionKey = string.Empty,
                    ContainerId = cosmosContainer?.Id ?? string.Empty
                };
            }

            return rc;
        }
        public async Task<ReturnCode<Interfaces.IContainer>> CreateContainer(string pDataBaseId, string pContainerName, string pPartitionKey) {
            ReturnCode<Interfaces.IContainer> rc = new ReturnCode<Interfaces.IContainer>();
            Database? database = null;
            Interfaces.IContainer container = new Models.Container();
            Container? cosmosContainer = null;

            if (!pPartitionKey.StartsWith("/")) {
                pPartitionKey = '/' + pPartitionKey;
            }

            if (rc.Success) {
                ReturnCode<Database> getDatabaseRc = await GetCosmosDatabase(pDataBaseId);

                if (getDatabaseRc.Success) {
                    database = getDatabaseRc?.Data;
                }
                else {
                    ErrorWorker.CopyErrors(getDatabaseRc, rc);
                }
            }

            if (rc.Success) {
                try {
                    if (database != null) {
                        ContainerResponse response = await database.CreateContainerIfNotExistsAsync(pContainerName, pPartitionKey);

                        if (System.Net.HttpStatusCode.Created != response?.StatusCode || System.Net.HttpStatusCode.OK != response?.StatusCode) {
                            rc = new(JB.Common.ErrorCodes.BAD_HTTP_STATUS_CODE);
                        }
                        else if (System.Net.HttpStatusCode.Created == response?.StatusCode || System.Net.HttpStatusCode.OK == response?.StatusCode) {
                            cosmosContainer = response.Container;
                        }

                    }
                }
                catch (Exception ex) {
                    rc = new(ErrorCodes.UNABLE_TO_CREATE_CONTAINER, ex);
                }
            }

            if (rc.Success) {
                container.ContainerId = cosmosContainer?.Id ?? string.Empty;
                container.ContainerName = pContainerName;
                container.PartitionKey = pPartitionKey;
            }

            return rc;
        }

        public async Task<ReturnCode<T>> AddItem<T>(string pDatabaseId, string pContainerId, T item) {
            ReturnCode<T> rc = new ReturnCode<T>();
            Container? container = null;

            if (rc.Success) {
                ReturnCode<Container> getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                if (getContainerRc.Success) {
                    container = getContainerRc.Data;
                }
                else {
                    ErrorWorker.CopyErrors(getContainerRc, rc);
                }
            }

            if (rc.Success) {
                try {
                    if (container != null) {
                        var response = await container.CreateItemAsync(item);

                        if (System.Net.HttpStatusCode.Created != response?.StatusCode &&
                            System.Net.HttpStatusCode.OK != response?.StatusCode) {
                            rc = new(JB.Common.ErrorCodes.BAD_HTTP_STATUS_CODE);
                        }
                    } 
                }
                catch (Exception ex) {
                    rc = new(ErrorCodes.UNABLE_TO_CREATE_ITEM, ex);
                }
            }

            return rc;
        }
        public async Task<ReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId) {
            ReturnCode<IList<T>> rc = new ReturnCode<IList<T>>();
            Container? container = null;
            IList<T> itemsList = new List<T>();

            if (rc.Success) {
                var getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                if (getContainerRc.Success) {
                    container = getContainerRc.Data;
                }
                else {
                    ErrorWorker.CopyErrors(getContainerRc, rc);
                }
            }

            if (rc.Success) {
                try {
                    QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c");
                    using (FeedIterator<T>? feedIterator = container?.GetItemQueryIterator<T>(queryDefinition)) {

                        while (feedIterator?.HasMoreResults == true) {
                            var resultSet = await feedIterator.ReadNextAsync();

                            foreach (T? f in resultSet) {
                                itemsList.Add(f);
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    rc = new(ErrorCodes.UNABLE_TO_GET_ITEMS, ex);
                }
            }

            if (rc.Success) {
                rc.Data = itemsList;
            }

            return rc;
        }
        public async Task<ReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId, string pQuery) {
            ReturnCode<IList<T>> rc = new ReturnCode<IList<T>>();
            Container? container = null;
            IList<T> itemsList = new List<T>();

            if (rc.Success) {
                ReturnCode<Container> getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                if (getContainerRc.Success) {
                    container = getContainerRc.Data;
                }
                else {
                    ErrorWorker.CopyErrors(getContainerRc, rc);
                }
            }

            if (rc.Success) {

                try {
                    QueryDefinition queryDefinition = new QueryDefinition(pQuery);
                    using (FeedIterator<T>? feedIterator = container?.GetItemQueryIterator<T>(queryDefinition)) {

                        while (feedIterator?.HasMoreResults == true) {
                            var resultSet = await feedIterator.ReadNextAsync();

                            foreach (T? f in resultSet) {
                                itemsList.Add(f);
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    rc = new(ErrorCodes.UNABLE_TO_GET_ITEMS, ex);
                }

            }

            if (rc.Success) {
                rc.Data = itemsList;
            }

            return rc;
        }
        public async Task<ReturnCode<T>> GetItem<T>(string pDatabaseId, string pContainerId, string pItemId) {
            ReturnCode<T> rc = new ReturnCode<T>();
            Container? container = null;
            IList<T> itemsList = new List<T>();

            if (rc.Success) {
                var getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                if (getContainerRc.Success) {
                    container = getContainerRc.Data;
                }
                else {
                    ErrorWorker.CopyErrors(getContainerRc, rc);
                }
            }

            if (rc.Success) {
                try {
                    QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.id='{pItemId}'");
                    using (FeedIterator<T>? feedIterator = container?.GetItemQueryIterator<T>(queryDefinition)) {

                        if (feedIterator?.HasMoreResults == true) {
                            var resultSet = await feedIterator.ReadNextAsync();
                            itemsList.Add(resultSet.First());
                        }
                    }
                }
                catch (Exception ex) {
                    rc = new(ErrorCodes.UNABLE_TO_GET_ITEMS, ex);
                }
                
            }

            if (rc.Success) {
                rc.Data = itemsList.Count > 0 ? itemsList[0] : default;
            }

            return rc;
        }
        public async Task<ReturnCode<T>> UpdateItem<T>(string pDatabaseId, string pContainerId, T pItem, string pItemId, string pPartionKeyValue) {
            ReturnCode<T> rc = new ReturnCode<T>();
            Container? container = null;

            if (rc.Success) {
                ReturnCode<Container> getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                if (getContainerRc.Success) {
                    container = getContainerRc.Data;
                }
                else {
                    ErrorWorker.CopyErrors(getContainerRc, rc);
                }
            }

            if (rc.Success) {
                try {
                    if (null != container) {
                        var response = await container.ReplaceItemAsync(pItem, pItemId, new PartitionKey(pPartionKeyValue));

                        if (System.Net.HttpStatusCode.OK != response.StatusCode) {
                            rc = new(JB.Common.ErrorCodes.BAD_HTTP_STATUS_CODE);
                        }
                    }
                }
                catch (Exception ex) {
                    rc = new(ErrorCodes.UNABLE_TO_UPDATE_ITEM, ex);
                }

            }
            return rc;
        }
        public async Task<ReturnCode<T>> DeleteItem<T>(string pDatabaseId, string pContainerId, string pItemId, string pPartitionKeyValue) {
            ReturnCode<T> rc = new ReturnCode<T>();
            Container? container = null;
            IList<T> itemsList = new List<T>();

            if (rc.Success) {
                var getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                if (getContainerRc.Success) {
                    container = getContainerRc.Data;
                }
                else {
                    ErrorWorker.CopyErrors(getContainerRc, rc);
                }
            }

            if (rc.Success) {
                try {
                    if (container != null) {
                        ItemResponse<T> resposne = await container.DeleteItemAsync<T>(pItemId, new PartitionKey(pPartitionKeyValue));

                        if (System.Net.HttpStatusCode.OK != resposne.StatusCode && System.Net.HttpStatusCode.NoContent != resposne.StatusCode) {
                            rc = new(JB.Common.ErrorCodes.BAD_HTTP_STATUS_CODE);
                        }
                    }
                }
                catch (Exception ex) {
                    rc = new(ErrorCodes.UNABLE_TO_DELETE_ITEM, ex);
                }

            }

            if (rc.Success) {
                rc.Data = itemsList.Count > 0 ? itemsList[0] : default;
            }

            return rc;
        }

        protected async Task<ReturnCode<Database>> GetCosmosDatabase(string pDatabaseId) {
            ReturnCode<Database> rc = new ReturnCode<Database>();
            string? connectionString = Environment.GetEnvironmentVariable("cosmos-connection-string");
            Database? database = null;

            try {
                cosmosClient = new CosmosClient(connectionString);
                DatabaseResponse response = await cosmosClient.CreateDatabaseIfNotExistsAsync(pDatabaseId);
                
                if (System.Net.HttpStatusCode.OK == response.StatusCode) {
                    database = response.Database;
                }
                else if (System.Net.HttpStatusCode.OK != response.StatusCode) {
                    rc = new(5);
                }
            }
            catch (Exception ex) {
                rc = new(ErrorCodes.UNABLE_TO_GET_DATABASE, ex);
            }


            if (rc.Success) {
                rc.Data = database;
            }

            return rc;
        }
        protected async Task<ReturnCode<Container>> GetCosmosContainer(string pDatabaseId, string pContainerId) {
            ReturnCode<Container> rc = new ReturnCode<Container>();
            Database? database = null;

            if (rc.Success) {
                var databaseRc = await GetCosmosDatabase(pDatabaseId);

                if (databaseRc.Success) {
                    database = databaseRc.Data;
                }
                else {
                    ErrorWorker.CopyErrors(databaseRc, rc);
                }
            }

            if (rc.Success) {
                try {
                    Container? container = database?.GetContainer(pContainerId);

                    if (null != container) {
                        rc.Data = container;
                    }
                    else {
                        rc = new(ErrorCodes.NO_CONTAINER_RETURNED);
                    }
                }
                catch (Exception ex) {
                    rc = new(ErrorCodes.UNABLE_TO_GET_CONTAINER, ex);
                }
            }

            return rc;
        }
    }
}
