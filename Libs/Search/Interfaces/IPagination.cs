using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Search.Interfaces {
    public interface IPagination {
        int Page { get; set; }
        int PageSize { get; set; }
        int Total { get; set; }
    }
}
