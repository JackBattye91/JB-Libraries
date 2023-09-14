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
    internal class Wrapper<T> : IWrapper<T> {
        public IReturnCode LoadData(IList<T> pData) {
            throw new NotImplementedException();
        }

        public IReturnCode<IList<T>> Search(T pSearchItem) {
            throw new NotImplementedException();
        }
    }
}
