using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather.Interfaces {
    public interface IForcast {
        DateTime Date { get; set; }
        string Description { get; set; }

    }
}
