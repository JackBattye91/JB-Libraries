﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Weather.Interfaces {
    public interface IGpsCoordinate {
        float Latitude { get; set; }
        float Longitude { get; set; }
    }
}
