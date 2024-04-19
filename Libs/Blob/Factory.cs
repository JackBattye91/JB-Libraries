using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Blob
{
    public class Factory
    {
        public static IWrapper CreateBlobWrapper(string? pConnectionString = null)
        {
            return new AzureStorageAccount.Wrapper(pConnectionString);
        }
    }
}
