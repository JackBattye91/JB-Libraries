using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search {
    public sealed class Factory {
        public static IWrapper<T> CreateSearchInstance<T>(ESearchAlgorithm pAlgorithm) {
            return pAlgorithm switch {
                ESearchAlgorithm.BK_TREE => new BKTree.Wrapper<T>(),
                _ => new Binary.Wrapper<T>(),
            };
        }
    }
}
