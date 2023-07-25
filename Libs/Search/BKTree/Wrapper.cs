using JB.Search.Interfaces;
using JB.Search.BKTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Search.BKTree.Interfaces;

namespace JB.Search.BKTree {
    internal class Wrapper : IWrapper {
        public ISearchResults<string> Search(string pPartialString, int pPage = 0, int pPageSize = 25) {
            ISearchResults<string> searchResultsList = new StringSearchResults();
            INode node = new Node();

            try {

            }
            catch { 

            }

            return searchResultsList;
        }
    }
}
