using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace JB.NoSqlDatabase.Cosmos {
    internal class Wrapper : JB.NoSqlDatabase.IWrapper {
        CosmosClient? cosmosClient;

        public Wrapper() {
            cosmosClient = null;
        }

        public async Task<IReturnCode<bool>> CreateDatabase(string pDatabaseId) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            string? connectionString = Environment.GetEnvironmentVariable("cosmos-connection-string");

            try {
                cosmosClient = new CosmosClient(connectionString);
                DatabaseResponse response = await cosmosClient.CreateDatabaseIfNotExistsAsync(pDatabaseId);
                
                if (System.Net.HttpStatusCode.OK != response.StatusCode) {
                    rc.ErrorCode = JB.Common.ErrorCodes.BAD_HTTP_STATUS_CODE;
                    ErrorWorker.AddError(rc, rc.ErrorCode);
                }
            }
            catch (Exception e) {
                rc.ErrorCode = ErrorCodes.UNABLE_TO_CREATE_DATABASE;
                ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
            }
            

            return rc;
        }
        public async Task<IReturnCode<Container>> GetContainer(string pDatabaseId, string pContainerId) {
            return await GetCosmosContainer(pDatabaseId, pContainerId);
        }
        public async Task<IReturnCode<Container>> CreateContainer(string pDataBaseId, string pContainerName, string pPartitionKey) {
            IReturnCode<Container> rc = new ReturnCode<Container>(JB.Common.ErrorCodes.SUCCESS);
            Database? database = null;

            if (!pPartitionKey.StartsWith('/')) {
                pPartitionKey = '/' + pPartitionKey;
            }

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                var getDatabaseRc = await GetCosmosDatabase(pDataBaseId);

                if (JB.Common.ErrorCodes.SUCCESS == getDatabaseRc.ErrorCode) {
                    database = getDatabaseRc.Data;
                }
                if (JB.Common.ErrorCodes.SUCCESS != getDatabaseRc.ErrorCode) {
                    ErrorWorker.CopyErrorCode(getDatabaseRc, rc);
                }
            }


            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                try {
                    if (database != null) {
                        ContainerResponse? response = await database.CreateContainerIfNotExistsAsync(pContainerName, pPartitionKey);

                        if (System.Net.HttpStatusCode.Created != response?.StatusCode &&
                            System.Net.HttpStatusCode.OK != response?.StatusCode) {
                            rc.ErrorCode = JB.Common.ErrorCodes.BAD_HTTP_STATUS_CODE;
                            ErrorWorker.AddError(rc, rc.ErrorCode);
                        }
                    }
                }
                catch (Exception e) {
                    rc.ErrorCode = ErrorCodes.UNABLE_TO_CREATE_CONTAINER;
                    ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
                }
            }

            return rc;
        }

        public async Task<IReturnCode<T>> AddItem<T>(string pDatabaseId, string pContainerId, T item) {
            IReturnCode<T> rc = new ReturnCode<T>();
            Container? container = null;

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                var getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                if (JB.Common.ErrorCodes.SUCCESS == getContainerRc?.ErrorCode) {
                    container = getContainerRc.Data;
                }

                if (JB.Common.ErrorCodes.SUCCESS != getContainerRc?.ErrorCode) {
                    if (getContainerRc != null)
                        ErrorWorker.CopyErrorCode(getContainerRc, rc);
                }
            }

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                try {
                    if (container != null) {
                        var response = await container.CreateItemAsync(item);

                        if (System.Net.HttpStatusCode.Created != response?.StatusCode &&
                            System.Net.HttpStatusCode.OK != response?.StatusCode) {
                            rc.ErrorCode = JB.Common.ErrorCodes.BAD_HTTP_STATUS_CODE;
                            ErrorWorker.AddError(rc, rc.ErrorCode);
                        }
                    } 
                }
                catch (Exception e) {
                    rc.ErrorCode = ErrorCodes.UNABLE_TO_CREATE_ITEM;
                    ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
                }
            }

            return rc;
        }
        public async Task<IReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId) {
            IReturnCode<IList<T>> rc = new ReturnCode<IList<T>>();
            Container? container = null;
            IList<T> itemsList = new List<T>();

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                var getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                if (JB.Common.ErrorCodes.SUCCESS == getContainerRc.ErrorCode) {
                    container = getContainerRc.Data;
                }

                if (JB.Common.ErrorCodes.SUCCESS != getContainerRc.ErrorCode) {
                    rc.ErrorCode = getContainerRc.ErrorCode;
                    ErrorWorker.AddError(getContainerRc, getContainerRc.ErrorCode);
                }
            }

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {

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
                catch (Exception e) {
                    rc.ErrorCode = ErrorCodes.UNABLE_TO_GET_ITEMS;
                    ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
                }
                
            }

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                rc.Data = itemsList;
            }

            return rc;
        }
        public async Task<IReturnCode<T>> GetItem<T>(string pDatabaseId, string pContainerId, string pItemId) {
            IReturnCode<T> rc = new ReturnCode<T>();
            Container? container = null;
            IList<T> itemsList = new List<T>();

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                var getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                if (JB.Common.ErrorCodes.SUCCESS == getContainerRc.ErrorCode) {
                    container = getContainerRc.Data;
                }

                if (JB.Common.ErrorCodes.SUCCESS != getContainerRc.ErrorCode) {
                    rc.ErrorCode = getContainerRc.ErrorCode;
                    ErrorWorker.AddError(getContainerRc, getContainerRc.ErrorCode);
                }
            }

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                try {
                    QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.id='{pItemId}'");
                    using (FeedIterator<T>? feedIterator = container?.GetItemQueryIterator<T>(queryDefinition)) {

                        if (feedIterator?.HasMoreResults == true) {
                            var resultSet = await feedIterator.ReadNextAsync();
                            itemsList.Add(resultSet.First());
                        }
                    }
                }
                catch (Exception e) {
                    rc.ErrorCode = ErrorCodes.UNABLE_TO_GET_ITEMS;
                    ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
                }
                
            }

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                rc.Data = itemsList.Count > 0 ? itemsList[0] : default;
            }

            return rc;
        }
        public async Task<IReturnCode<T>> UpdateItem<T>(string pDatabaseId, string pContainerId, T pItem, string pItemId, string pPartionKeyValue) {
            IReturnCode<T> rc = new ReturnCode<T>();
            Container? container = null;

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                var getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                if (JB.Common.ErrorCodes.SUCCESS == getContainerRc?.ErrorCode) {
                    container = getContainerRc.Data;
                }

                if (JB.Common.ErrorCodes.SUCCESS != getContainerRc?.ErrorCode) {
                    if (getContainerRc != null)
                        ErrorWorker.CopyErrorCode(getContainerRc, rc);
                }
            }

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                try {
                    if (null != container) {
                        var response = await container.ReplaceItemAsync(pItem, pItemId, new PartitionKey(pPartionKeyValue));

                        if (System.Net.HttpStatusCode.OK != response.StatusCode) {
                            rc.ErrorCode = JB.Common.ErrorCodes.BAD_HTTP_STATUS_CODE;
                            ErrorWorker.AddError(rc, rc.ErrorCode);
                        }
                    }
                }
                catch (Exception e) {
                    rc.ErrorCode = ErrorCodes.UNABLE_TO_UPDATE_ITEM;
                    ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
                }

            }
            return rc;
        }
        public async Task<JB.Common.IReturnCode<T>> DeleteItem<T>(string pDatabaseId, string pContainerId, string pItemId, string pPartitionKeyValue) {
            IReturnCode<T> rc = new ReturnCode<T>();
            Container? container = null;
            IList<T> itemsList = new List<T>();

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                var getContainerRc = await GetCosmosContainer(pDatabaseId, pContainerId);

                if (JB.Common.ErrorCodes.SUCCESS == getContainerRc.ErrorCode) {
                    container = getContainerRc.Data;
                }

                if (JB.Common.ErrorCodes.SUCCESS != getContainerRc.ErrorCode) {
                    rc.ErrorCode = getContainerRc.ErrorCode;
                    ErrorWorker.AddError(getContainerRc, getContainerRc.ErrorCode);
                }
            }

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                try {
                    if (container != null) {
                        var resposne = await container.DeleteItemAsync<T>(pItemId, new PartitionKey(pPartitionKeyValue));

                        if (System.Net.HttpStatusCode.OK != resposne.StatusCode && System.Net.HttpStatusCode.NoContent != resposne.StatusCode) {
                            rc.ErrorCode = JB.Common.ErrorCodes.BAD_HTTP_STATUS_CODE;
                            ErrorWorker.AddError(rc, rc.ErrorCode);
                        }
                    }
                }
                catch (Exception e) {
                    rc.ErrorCode = ErrorCodes.UNABLE_TO_DELETE_ITEM;
                    ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
                }

            }

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                rc.Data = itemsList.Count > 0 ? itemsList[0] : default;
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
                    rc.ErrorCode = 5;
                    ErrorWorker.AddError(rc, rc.ErrorCode);
                }
            }
            catch (Exception e) {
                rc.ErrorCode = ErrorCodes.UNABLE_TO_GET_DATABASE;
                ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
            }


            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                rc.Data = database;
            }

            return rc;
        }
        protected async Task<IReturnCode<Container>> GetCosmosContainer(string pDatabaseId, string pContainerId) {
            IReturnCode<Container> rc = new ReturnCode<Container>();
            Database? database = null;

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                var databaseRc = await GetCosmosDatabase(pDatabaseId);

                if (JB.Common.ErrorCodes.SUCCESS == databaseRc.ErrorCode) {
                    database = databaseRc.Data;
                }

                if (JB.Common.ErrorCodes.SUCCESS != databaseRc.ErrorCode) {
                    ErrorWorker.CopyErrorCode(databaseRc, rc);
                }
            }

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                try {
                    Container? container = database?.GetContainer(pContainerId);

                    if (null != container) {
                        rc.Data = container;
                    }
                    else {
                        rc.ErrorCode = ErrorCodes.NO_CONTAINER_RETURNED;
                        ErrorWorker.AddError(rc, rc.ErrorCode);
                    }
                }
                catch (Exception e) {
                    rc.ErrorCode = ErrorCodes.UNABLE_TO_GET_CONTAINER;
                    ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
                }
            }

            return rc;
        }
    }
}
