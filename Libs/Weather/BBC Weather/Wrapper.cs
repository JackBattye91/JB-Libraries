using JB.Weather.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather.BBC_Weather {
    internal class Wrapper : IWrapper {
        public JB.Common.IReturnCode<Interfaces.IForcast> GetTodaysForcast() {
            JB.Common.IReturnCode<Interfaces.IForcast> rc = new JB.Common.ReturnCode<Interfaces.IForcast>();

            

            return rc;
        }
    }
}
