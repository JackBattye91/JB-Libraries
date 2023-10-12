using JB.SqlDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.SqlDatabase.Converters {
    public class IntConverter : IConverter {
        public bool CanConvert(Type pType) {
            return pType == typeof(int);
        }

        public object? Read(IDataReader pReader) {

        }

        public void Write(IDataWriter pWriter, object? pObject) {
            throw new NotImplementedException();
        }
    }
}
