using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JB.SqlDatabase.SQlite.Interfaces {
    internal interface IObjectProperty {
        public string Name { get; set; }
        public object? Value { get; set; }
        public IList<CustomAttributeData> Attributes { get; set; }
    }
}
