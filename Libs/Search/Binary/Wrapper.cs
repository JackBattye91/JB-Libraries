using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search.Binary {
    internal class Wrapper<T> : JB.Search.IWrapper<T> {
        IList<T>? dataList = null;
        public IReturnCode LoadData(IList<T> pData) {
            IReturnCode rc = new ReturnCode();

            try {
                if (rc.Success) {
                    dataList = pData;
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.LOAD_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return rc;
        }

        public IReturnCode<IList<T>> Search(T pSearchItem) {

        }
    }
}
