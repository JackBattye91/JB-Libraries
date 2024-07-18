using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JB.SqlDatabase.Interfaces;

namespace JB.SqlDatabase.SQlite.Models
{
    internal class ObjectProperty : IObjectProperty {
        public string Name { get; set; } = string.Empty;
        public object? Value { get; set; } = null;
        public IList<CustomAttributeData> Attributes { get; set; } = new List<CustomAttributeData>();
    }
}
