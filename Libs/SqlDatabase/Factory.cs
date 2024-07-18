using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.SqlDatabase {
    public sealed class Factory {
        public static IWrapper CreateSqlWrapperInstance() {
            return new SQlite.Wrapper();
        }
    }
}
