using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;

namespace JB.Weather {
    public interface IWrapper {
        public Task<IReturnCode<Interfaces.IForcast>> GetTodaysForcast(string pAreaCode);
        public Task<IReturnCode<IList<Interfaces.IForcast>>> Get3DayForcast(string pAreaCode);
    }
}
