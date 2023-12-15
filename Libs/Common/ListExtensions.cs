using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common
{
    public static class ListExtensions
    {
        public static IList<Tinterface> ToInterfaceList<Tinterface, Tmodel>(this IList<Tmodel> pObjectList) where Tmodel : Tinterface
        {
            IList<Tinterface> interfaceList = new List<Tinterface>();

            foreach (Tmodel obj in pObjectList)
            {
                interfaceList.Add(obj);
            }

            return interfaceList;
        }
    }
}
