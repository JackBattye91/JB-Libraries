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
                    rc.Errors.Add(new Error() { ErrorCode = rc.ErrorCode, StackTrace = Environment.StackTrace });
                }
            }

            return rc;
        }
    }
}
