using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.NoSqlDatabase {
    public class Factory {
        public static IWrapper CreateNoSqlDatabaseWrapper() {
            return new Cosmos.Wrapper();
        }
    }
}
