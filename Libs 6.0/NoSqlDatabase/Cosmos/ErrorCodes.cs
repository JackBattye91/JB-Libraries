using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;

namespace JB.NoSqlDatabase.Cosmos {
    internal class ErrorCodes {
        private const int SCOPE = 2;

        public const int GET_CONTAINER_FAILED = 1;
        public const int CREATE_DATABASE_FAILED = 2;
        public const int CREATE_CONTAINER_FAILED = 3;
        public const int CREATE_ITEM_FAILED = 4;
        public const int GET_ITEMS_FAILED = 5;
        public const int UPDATE_ITEM_FAILED = 6;
        public const int DELETE_ITEM_FAILED = 7;
        public const int GET_DATABASE_FAILED = 8;
        public const int NO_CONTAINER_RETURNED = 9;
        public const int BAD_STATUS_CODE_FROM_CREATE_DATABASE = 10;
        public const int BAD_STATUS_CODE_FROM_CREATE_CONTAINER = 11;
        public const int ADD_ITEM_FAILED = 12;
        public const int BAD_STATUS_CODE_FROM_ADD_ITEM = 13;
        public const int GET_ITEM_FAILED = 14;
        public const int BAD_STATUS_CODE_FROM_UPDATE_ITEM = 15;
        public const int BAD_STATUS_CODE_FROM_DELETE_ITEM = 16;
        public const int GET_COSMOS_DATABASE_FAILED = 17;
        public const int BAD_STATUS_CODE_FROM_GET_COSMOS_DATABASE = 18;
        public const int GET_COSMOS_CONTAINER_FAILED = 19;
    }
}
