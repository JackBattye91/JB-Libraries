using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.SqlDatabase.Attributes {
    [AttributeUsage(AttributeTargets.Property)]
    public class TableAttribute : Attribute {
        public string TableName { get; set; } = string.Empty;
        public string ColumnName { get; set; } = string.Empty;
    }
}
