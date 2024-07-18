using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search.Interfaces {
    public interface ISearchResults : IPagination {
        IList<string> Suggestions { get; set; }
        IList<char> NextCharacters { get; set; }
    }
}
