using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather {
    public class Factory {
        public static IWrapper CreateWeatherWrapper() {
            return new BBC_Weather.Wrapper();
        }
    }
}
