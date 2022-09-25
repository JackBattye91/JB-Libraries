using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;

namespace JB.NoSqlDatabase.Cosmos {
    internal class ErrorCodes {
        private const int SCOPE = 2;

        public static Error UNABLE_TO_GET_CONTAINER =      new(SCOPE, 1);
        public static Error UNABLE_TO_CREATE_DATABASE =    new(SCOPE, 2);
        public static Error UNABLE_TO_CREATE_CONTAINER =   new(SCOPE, 3);
        public static Error UNABLE_TO_CREATE_ITEM  =       new(SCOPE, 4);
        public static Error UNABLE_TO_GET_ITEMS  =         new(SCOPE, 5);
        public static Error UNABLE_TO_UPDATE_ITEM  =       new(SCOPE, 6);
        public static Error UNABLE_TO_DELETE_ITEM  =       new(SCOPE, 7);
        public static Error UNABLE_TO_GET_DATABASE =       new(SCOPE, 8);
        public static Error NO_CONTAINER_RETURNED =        new(SCOPE, 9);
    }
}
