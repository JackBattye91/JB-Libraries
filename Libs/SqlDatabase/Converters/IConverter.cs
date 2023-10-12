using JB.SqlDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.SqlDatabase.Converters {
    public interface IConverter {
        bool CanConvert(Type pType);
        object? Read(IDataReader pReader);
        void Write(IDataWriter pWriter, object? pObject);
    }
}
