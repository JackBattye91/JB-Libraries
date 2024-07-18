using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search.BKTree.Interfaces
{
    public interface INode {
        char Value { get; set; }
        IList<INode> Children { get; }
    }
}
