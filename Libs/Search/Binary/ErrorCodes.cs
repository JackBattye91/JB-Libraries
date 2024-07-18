using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search.Binary {
    internal class ErrorCodes {
        private const int SCOPE = 5;

        public const int LOAD_DATA_FAILED = (SCOPE * JB.Common.ErrorCodes.SCOPE_OFFSET) + 1;
    }
}
