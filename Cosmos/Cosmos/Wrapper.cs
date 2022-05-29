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

        public IReturnCode<bool> CreateDatabase(string pDatabaseId) {
            IReturnCode<bool> rc = new ReturnCode<bool>(ErrorCodes.SUCCESS);
            string? connectionString = Environment.GetEnvironmentVariable("cosmos-connection-string");

            cosmosClient = new CosmosClient(connectionString);
            cosmosClient.CreateDatabaseIfNotExistsAsync(pDatabaseId).Wait();

            return rc;
        }

        public IReturnCode<Container> GetContainer(string pDatabaseID, string pContainerId) {
            IReturnCode<Container> rc = new ReturnCode<Container>(ErrorCodes.SUCCESS);

            if (ErrorCodes.SUCCESS == rc.ErrorCode) {
                Container? container = cosmosClient?.GetContainer(pDatabaseID, pContainerId);

                if (null != container) {
                    rc.Data = container;
                }
                else {
                    rc.ErrorCode = 1;
                    ErrorWorker.AddError(rc, rc.ErrorCode);
                }
            }

            return rc;
        }
        public IReturnCode<Container> CreateContainer(string pDataBaseId, string pContainerName) {
            IReturnCode<Container> rc = new ReturnCode<Container>(JB.Common.ErrorCodes.SUCCESS);



            return rc;
        }
        public async Task<IReturnCode<T>> AddItem<T>(string pDatabaseId, string pContainerId, T item) {
            IReturnCode<T> rc = new ReturnCode<T>(ErrorCodes.SUCCESS);
            Container? container = null;

            if (ErrorCodes.SUCCESS == rc.ErrorCode) {
                var getContainerRc = GetContainer(pDatabaseId, pContainerId);


                if (ErrorCodes.SUCCESS == getContainerRc?.ErrorCode) {
                    container = getContainerRc.Data;
                }

                if (ErrorCodes.SUCCESS != getContainerRc?.ErrorCode) {
                    if (getContainerRc != null)
                        ErrorWorker.CopyErrorCode(getContainerRc, rc);
                }
            }

            if (ErrorCodes.SUCCESS == rc.ErrorCode) {
                try {
                    if (container != null) {
                        await container.CreateItemAsync(item);
                    } 
                }
                catch (Exception e) {
                    rc.ErrorCode = 2;
                    ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
                }
            }

            return rc;
        }
        public async Task<IReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId) {
            IReturnCode<IList<T>> rc = new ReturnCode<IList<T>>(ErrorCodes.SUCCESS);
            Container? container = null;
            IList<T> itemsList = new List<T>();

            if (ErrorCodes.SUCCESS == rc.ErrorCode) {
                var getContainerRc = GetContainer(pDatabaseId, pContainerId);

                if (ErrorCodes.SUCCESS == getContainerRc.ErrorCode) {
                    container = getContainerRc.Data;
                }

                if (ErrorCodes.SUCCESS != getContainerRc.ErrorCode) {
                    rc.ErrorCode = getContainerRc.ErrorCode;
                    ErrorWorker.AddError(getContainerRc, getContainerRc.ErrorCode);
                }
            }

            if (ErrorCodes.SUCCESS == rc.ErrorCode) {
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

            if (ErrorCodes.SUCCESS == rc.ErrorCode) {
                rc.Data = itemsList;
            }

            return rc;
        }
        public async Task<IReturnCode<T>> GetItem<T>(string pDatabaseId, string pContainerId, string pItemId) {
            IReturnCode<IList<T>> rc = new ReturnCode<IList<T>>(ErrorCodes.SUCCESS);
            Container? container = null;
            IList<T> itemsList = new List<T>();

            if (ErrorCodes.SUCCESS == rc.ErrorCode) {
                var getContainerRc = GetContainer(pDatabaseId, pContainerId);

                if (ErrorCodes.SUCCESS == getContainerRc.ErrorCode) {
                    container = getContainerRc.Data;
                }

                if (ErrorCodes.SUCCESS != getContainerRc.ErrorCode) {
                    rc.ErrorCode = getContainerRc.ErrorCode;
                    ErrorWorker.AddError(getContainerRc, getContainerRc.ErrorCode);
                }
            }

            if (ErrorCodes.SUCCESS == rc.ErrorCode) {
                QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.id='{pItemId}';");
                using (FeedIterator<T>? feedIterator = container?.GetItemQueryIterator<T>(queryDefinition)) {

                    while (feedIterator?.HasMoreResults == true) {
                        var resultSet = await feedIterator.ReadNextAsync();

                        foreach (T? f in resultSet) {
                            itemsList.Add(f);
                        }
                    }
                }
            }

            if (ErrorCodes.SUCCESS == rc.ErrorCode) {
                rc.Data = itemsList;
            }

            return rc;
        }
    }
}
