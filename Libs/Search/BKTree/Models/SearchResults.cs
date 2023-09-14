using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search.BKTree.Models {
    internal class StringSearchResults : JB.Search.Interfaces.ISearchResults<T> {
        public IList<string> Data { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }

        public StringSearchResults()
        {
            Data = new List<string>();
        }
    }
}
