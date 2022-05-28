using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace JB.NoSqlDatabase {
    public interface IWrapper {
        public JB.Common.IReturnCode<bool> CreateDatabase(string pDatabaseId);
        public JB.Common.IReturnCode<Container> GetContainer(string pDatabaseId, string pContainerID);
    }
}
