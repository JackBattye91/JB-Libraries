﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather {
    public interface IWrapper {
        public Task<JB.Common.IReturnCode<Interfaces.IForcast>> GetTodaysForcast(string pAreaCode);
        public Task<JB.Common.IReturnCode<IList<Interfaces.IForcast>>> Get3DayForcast(string pAreaCode);
    }
}
