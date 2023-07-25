using JB.Search.BKTree.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search.BKTree.Models {
    internal class Node : Interfaces.INode {
        public char Value { get; set; }
        public IList<INode> Children { get; set; }

        public Node() {
            Children = new List<INode>();
        }
    }
}
