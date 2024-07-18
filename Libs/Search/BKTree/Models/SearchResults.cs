using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search.BKTree.Models {
    internal class StringSearchResults : JB.Search.Interfaces.ISearchResults {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public IList<string> Suggestions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IList<char> NextCharacters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public StringSearchResults()
        {
        }
    }
}
