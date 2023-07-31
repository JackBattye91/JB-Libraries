using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common {
    public sealed class Worker {
        public static IList<Tinterface> ConvertToInterfaceList<Tinterface, Tobject>(IList<Tobject> pObjectList) where Tobject : Tinterface {
            IList<Tinterface> interfaceList = new List<Tinterface>();

            foreach(Tobject obj in pObjectList) {
                interfaceList.Add(obj);
            }

            return interfaceList;
        }
    }
}
