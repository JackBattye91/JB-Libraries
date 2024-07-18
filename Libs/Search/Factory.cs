using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search {
    public sealed class Factory {
        public static IWrapper CreateSearchInstance(ESearchAlgorithm pAlgorithm) {
            return pAlgorithm switch {
                ESearchAlgorithm.BK_TREE => new BKTree.Wrapper(),
                _ => new Binary.Wrapper(),
            };
        }
    }
}
