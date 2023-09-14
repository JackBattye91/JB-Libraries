using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search {
    public interface IWrapper<T> {
        JB.Common.IReturnCode LoadData(IList<T> pData);
        JB.Common.IReturnCode<IList<T>> Search(T pSearchItem);
    }
}
