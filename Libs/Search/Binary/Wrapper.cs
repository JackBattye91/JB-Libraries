using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search.Binary {
    internal class Wrapper : JB.Search.IWrapper {
        public JB.Common.IReturnCode<IList<T>> Search<T>(IList<T> pDataSet, Func<T, bool> searchCondition) {
            throw new NotImplementedException();
        }
    }
}
