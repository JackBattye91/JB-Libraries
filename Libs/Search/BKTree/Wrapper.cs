using JB.Search.Interfaces;
using JB.Search.BKTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Search.BKTree.Interfaces;
using JB.Common;

namespace JB.Search.BKTree {
    internal class Wrapper : IWrapper {
        public JB.Common.IReturnCode<IList<T>> Search<T>(IList<T> pDataSet, Func<T> searchCondition) {
            throw new NotImplementedException();
        }
    }
}
