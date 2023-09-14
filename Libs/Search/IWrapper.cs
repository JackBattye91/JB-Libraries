using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search {
    public interface IWrapper {
        JB.Common.IReturnCode<IList<T>> Search<T>(IList<T> pDataSet, Func searchCondition);
    }
}
