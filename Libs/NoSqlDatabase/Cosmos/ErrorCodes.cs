using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;

namespace JB.NoSqlDatabase.Cosmos {
    internal class ErrorCodes {
        private const int SCOPE = 2;

        public const int GET_CONTAINER_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 1;
        public const int CREATE_DATABASE_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 2;
        public const int CREATE_CONTAINER_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 3;
        public const int CREATE_ITEM_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 4;
        public const int GET_ITEMS_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 5;
        public const int UPDATE_ITEM_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 6;
        public const int DELETE_ITEM_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 7;
        public const int GET_DATABASE_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 8;
        public const int NO_CONTAINER_RETURNED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 9;
        public const int BAD_STATUS_CODE_FROM_CREATE_DATABASE = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 10;
        public const int BAD_STATUS_CODE_FROM_CREATE_CONTAINER = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 11;
        public const int ADD_ITEM_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 12;
        public const int BAD_STATUS_CODE_FROM_ADD_ITEM = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 13;
        public const int GET_ITEM_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 14;
        public const int BAD_STATUS_CODE_FROM_UPDATE_ITEM = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 15;
        public const int BAD_STATUS_CODE_FROM_DELETE_ITEM = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 16;
        public const int GET_COSMOS_DATABASE_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 17;
        public const int BAD_STATUS_CODE_FROM_GET_COSMOS_DATABASE = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 18;
        public const int GET_COSMOS_CONTAINER_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 19;
    }
}
