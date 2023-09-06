﻿using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.Net;

namespace JB.NoSqlDatabase.Cosmos {
    internal class Wrapper : JB.NoSqlDatabase.IWrapper {
        protected CosmosClient? cosmosClient;

        public Wrapper() {
            cosmosClient = null;
        }

        public async Task<IReturnCode<bool>> CreateDatabase(string pDatabaseId) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            
            try {
                string? connectionString = Environment.GetEnvironmentVariable("cosmos-connection-string");
                cosmosClient = new CosmosClient(connectionString);
                DatabaseResponse response = await cosmosClient.CreateDatabaseIfNotExistsAsync(pDatabaseId);
                
                if (System.Net.HttpStatusCode.OK != response.StatusCode) {
                    rc.ErrorCode = ErrorCodes.BAD_STATUS_CODE_FROM_CREATE_DATABASE;
                    rc.Errors.Add(new NetworkError(rc.ErrorCode, response.StatusCode));
                }
            }
            catch (Exception ex) {
                rc.Errors.Add(new NetworkError(ErrorCodes.CREATE_DATABASE_FAILED, HttpStatusCode.InternalServerError, ex));
            }

            return rc;
        }
        public async Task<IReturnCode<Interfaces.IContainer>> GetContainer(string pDatabaseId, string pContainerId) {
            IReturnCode<Interfaces.IContainer> rc = new ReturnCode<Interfaces.IContainer>();
            Container? cosmosContainer = null;
            Interfaces.IContainer? container = null;

            try {
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
            }
            catch(Exception ex) {
                rc.ErrorCode = ErrorCodes.GET_CONTAINER_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = container;
            }

            return rc;
        }
        public async Task<IReturnCode<Interfaces.IContainer>> CreateContainer(string pDataBaseId, string pContainerName, string pPartitionKey) {
            IReturnCode<Interfaces.IContainer> rc = new ReturnCode<Interfaces.IContainer>();
            Database? database = null;
            Interfaces.IContainer container = new Models.Container();
            Container? cosmosContainer = null;

            try {
                if (!pPartitionKey.StartsWith("/")) {
                    pPartitionKey = '/' + pPartitionKey;
                }

                if (rc.Success) {
                    IReturnCode<Database> getDatabaseRc = await GetCosmosDatabase(pDataBaseId);

                    if (getDatabaseRc.Success) {
                        database = getDatabaseRc?.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(getDatabaseRc, rc);
                    }
                }

                if (rc.Success) {
                    if (database != null) {
                        ContainerResponse response = await database.CreateContainerIfNotExistsAsync(pContainerName, pPartitionKey);

                        if (System.Net.HttpStatusCode.Created == response?.StatusCode || System.Net.HttpStatusCode.OK == response?.StatusCode) {
                            cosmosContainer = response.Container;
                        }
                        else {
                            rc.ErrorCode = ErrorCodes.BAD_STATUS_CODE_FROM_CREATE_CONTAINER;
                            rc.Errors.Add(new NetworkError(rc.ErrorCode, response?.StatusCode ?? HttpStatusCode.InternalServerError));
                        }
                    }
                }

                if (rc.Success) {
                    container.ContainerId = cosmosContainer?.Id ?? string.Empty;
                    container.ContainerName = pContainerName;
                    container.PartitionKey = pPartitionKey;
                }
            }
            catch(Exception ex) {
                rc.ErrorCode = ErrorCodes.CREATE_CONTAINER_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = container;
            }

            return rc;
        }

        public async Task<IReturnCode<T>> AddItem<T>(string pDatabaseId, string pContainerId, T item) {
            IReturnCode<T> rc = new ReturnCode<T>();
            Container? container = null;

            try {
                if (rc.Success) {
                    IReturnCode<Container> getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                    if (getContainerRc.Success) {
                        container = getContainerRc.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(getContainerRc, rc);
                    }
                }

                if (rc.Success) {
                    if (container != null) {
                        ItemResponse<T>? response = await container.CreateItemAsync(item);

                        if (response.StatusCode == HttpStatusCode.Created) {
                            item = response.Resource;
                        }

                        if (HttpStatusCode.Created != response?.StatusCode && HttpStatusCode.OK != response?.StatusCode) {
                            rc.ErrorCode = ErrorCodes.BAD_STATUS_CODE_FROM_ADD_ITEM;
                            rc.Errors.Add(new NetworkError(rc.ErrorCode, response?.StatusCode ?? HttpStatusCode.InternalServerError));
                        }
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.ADD_ITEM_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = item;
            }

            return rc;
        }
        public async Task<IReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId) {
            IReturnCode<IList<T>> rc = new ReturnCode<IList<T>>();
            Container? container = null;
            IList<T> itemsList = new List<T>();

            try {
                if (rc.Success) {
                    IReturnCode<Container> getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                    if (getContainerRc.Success) {
                        container = getContainerRc.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(getContainerRc, rc);
                    }
                }

                if (rc.Success) {
                    QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c");
                    using (FeedIterator<T>? feedIterator = container?.GetItemQueryIterator<T>(queryDefinition)) {

                        while (feedIterator?.HasMoreResults == true) {
                            FeedResponse<T> resultSet = await feedIterator.ReadNextAsync();

                            foreach (T? f in resultSet) {
                                itemsList.Add(f);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.GET_ITEMS_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = itemsList;
            }

            return rc;
        }
        public async Task<IReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId, string pQuery) {
            IReturnCode<IList<T>> rc = new ReturnCode<IList<T>>();
            Container? container = null;
            IList<T> itemsList = new List<T>();

            try {
                if (rc.Success) {
                    IReturnCode<Container> getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                    if (getContainerRc.Success) {
                        container = getContainerRc.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(getContainerRc, rc);
                    }
                }

                if (rc.Success) {
                    QueryDefinition queryDefinition = new QueryDefinition(pQuery);
                    using (FeedIterator<T>? feedIterator = container?.GetItemQueryIterator<T>(queryDefinition)) {
                        while (feedIterator?.HasMoreResults == true) {
                            FeedResponse<T> resultSet = await feedIterator.ReadNextAsync();

                            foreach (T? f in resultSet) {
                                itemsList.Add(f);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.GET_ITEMS_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = itemsList;
            }

            return rc;
        }
        public async Task<IReturnCode<T>> GetItem<T>(string pDatabaseId, string pContainerId, string pItemId) {
            IReturnCode<T> rc = new ReturnCode<T>();
            Container? container = null;
            IList<T> itemsList = new List<T>();

            try {
                if (rc.Success) {
                    IReturnCode<Container> getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                    if (getContainerRc.Success) {
                        container = getContainerRc.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(getContainerRc, rc);
                    }
                }

                if (rc.Success) {
                    QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.id='{pItemId}'");
                    using (FeedIterator<T>? feedIterator = container?.GetItemQueryIterator<T>(queryDefinition)) {

                        if (feedIterator?.HasMoreResults == true) {
                            FeedResponse<T> resultSet = await feedIterator.ReadNextAsync();
                            itemsList.Add(resultSet.First());
                        }
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.GET_ITEM_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = itemsList.Count > 0 ? itemsList[0] : default;
            }

            return rc;
        }
        public async Task<IReturnCode<T>> UpdateItem<T>(string pDatabaseId, string pContainerId, T pItem, string pItemId, string pPartionKeyValue) {
            IReturnCode<T> rc = new ReturnCode<T>();
            Container? container = null;
            T? item = default;

            try {
                if (rc.Success) {
                    IReturnCode<Container> getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                    if (getContainerRc.Success) {
                        container = getContainerRc.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(getContainerRc, rc);
                    }
                }

                if (rc.Success) {
                    if (null != container) {
                        ItemResponse<T> response = await container.ReplaceItemAsync(pItem, pItemId, new PartitionKey(pPartionKeyValue));

                        if (HttpStatusCode.OK == response.StatusCode) {
                            item = response.Resource;
                        }
                        else { 
                            rc.Errors.Add(new NetworkError(ErrorCodes.BAD_STATUS_CODE_FROM_UPDATE_ITEM, response.StatusCode));
                        }
                        
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.UPDATE_ITEM_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = item;
            }

            return rc;
        }
        public async Task<IReturnCode<bool>> DeleteItem<T>(string pDatabaseId, string pContainerId, string pItemId, string pPartitionKeyValue) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            Container? container = null;

            try {
                if (rc.Success) {
                    IReturnCode<Container> getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                    if (getContainerRc.Success) {
                        container = getContainerRc.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(getContainerRc, rc);
                    }
                }

                if (rc.Success) {
                    if (container != null) {
                        ItemResponse<T> resposne = await container.DeleteItemAsync<T>(pItemId, new PartitionKey(pPartitionKeyValue));

                        if (System.Net.HttpStatusCode.OK != resposne.StatusCode && System.Net.HttpStatusCode.NoContent != resposne.StatusCode) {
                            rc.Errors.Add(new NetworkError(ErrorCodes.BAD_STATUS_CODE_FROM_DELETE_ITEM, HttpStatusCode.InternalServerError));
                        }
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.DELETE_ITEM_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError, ex));
            }

            return rc;
        }

        protected async Task<IReturnCode<Database>> GetCosmosDatabase(string pDatabaseId) {
            IReturnCode<Database> rc = new ReturnCode<Database>();
            string? connectionString = Environment.GetEnvironmentVariable("cosmos-connection-string");
            Database? database = null;

            try {
                cosmosClient = new CosmosClient(connectionString);
                DatabaseResponse response = await cosmosClient.CreateDatabaseIfNotExistsAsync(pDatabaseId);
                
                if (System.Net.HttpStatusCode.OK == response.StatusCode) {
                    database = response.Database;
                }
                else if (System.Net.HttpStatusCode.OK != response.StatusCode) {
                    rc.Errors.Add(new NetworkError(ErrorCodes.BAD_STATUS_CODE_FROM_GET_COSMOS_DATABASE, response.StatusCode));
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.GET_COSMOS_DATABASE_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = database;
            }

            return rc;
        }
        protected async Task<IReturnCode<Container>> GetCosmosContainer(string pDatabaseId, string pContainerId) {
            IReturnCode<Container> rc = new ReturnCode<Container>();
            Database? database = null;
            Container? container = null;

            try {
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
                    container = database?.GetContainer(pContainerId);

                    if (null == container) {
                        rc.ErrorCode = ErrorCodes.NO_CONTAINER_RETURNED;
                        rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError));
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.GET_COSMOS_CONTAINER_FAILED;
                rc.Errors.Add(new NetworkError(rc.ErrorCode, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = container;
            }

            return rc;
        }
    }
}
