using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search {
    public interface IWrapper {
        Interfaces.ISearchResults<string> Search(string pPartialString, int pPage = 0, int pPageSize = 25);
    }
}
