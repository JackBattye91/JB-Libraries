using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace JB.NoSqlDatabase {
    internal class Wrapper : IWrapper {
        CosmosClient? cosmosClient;

        public Wrapper() {
            cosmosClient = null;
        }

        public IReturnCode<bool> CreateDatabase(string pDatabaseId) {
            JB.Common.IReturnCode<bool> rc = new JB.Common.ReturnCode<bool>(JB.Common.ErrorCodes.SUCCESS);
            string? connectionString = Environment.GetEnvironmentVariable("cosmos-connection-string");

            cosmosClient = new CosmosClient(connectionString);
            cosmosClient.CreateDatabaseIfNotExistsAsync(pDatabaseId).Wait();

            return rc;
        }

        public IReturnCode<Container> GetContainer(string pDatabaseID, string pContainerId) {
            JB.Common.IReturnCode<Container> rc = new JB.Common.ReturnCode<Container>(JB.Common.ErrorCodes.SUCCESS);

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
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

        public async Task<JB.Common.IReturnCode<T>> AddItem<T>(string pDatabaseId, string pContainerId, T item) {
            IReturnCode<T> rc = new ReturnCode<T>(ErrorCodes.SUCCESS);
            Container? container = null;

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                var getContainerRc = GetContainer(pDatabaseId, pContainerId);


                if (JB.Common.ErrorCodes.SUCCESS == getContainerRc?.ErrorCode) {
                    container = getContainerRc.Data;
                }

                if (JB.Common.ErrorCodes.SUCCESS != getContainerRc.ErrorCode) {
                    ErrorWorker.CopyErrorCode(getContainerRc, rc);
                }
            }


            try {
                ItemResponse<T> response = 
            }
            catch (Exception e) {
                
            }


            return rc;
        }

        public async Task<JB.Common.IReturnCode<IList<T>>> GetItems<T>(string pDatabaseId, string pContainerId, string pItemId) where T : class {
            IReturnCode<IList<T>> rc = new ReturnCode<IList<T>>(JB.Common.ErrorCodes.SUCCESS);
            Container? container = null;
            IList<T> itemsList = new List<T>();

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                var getContainerRc = GetContainer(pDatabaseId, pContainerId);

                if (JB.Common.ErrorCodes.SUCCESS == getContainerRc.ErrorCode) {
                    container = getContainerRc.Data;
                }

                if (JB.Common.ErrorCodes.SUCCESS != getContainerRc.ErrorCode) {
                    rc.ErrorCode = getContainerRc.ErrorCode;
                    ErrorWorker.AddError(getContainerRc, getContainerRc.ErrorCode);
                }
            }

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.id='{ pItemId }';");
                using (FeedIterator<T>? feedIterator = container?.GetItemQueryIterator<T>(queryDefinition)) {

                    while(feedIterator?.HasMoreResults == true) {
                        var resultSet = await feedIterator.ReadNextAsync();

                        foreach(T? f in resultSet) {
                            itemsList.Add(f);
                        }
                    }
                }
            }


            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                rc.Data = itemsList;
            }

            return rc;
        }
    }
}
