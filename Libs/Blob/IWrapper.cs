using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;

namespace JB.Blob
{
    public interface IWrapper
    {
        Task<IReturnCode<string>> UploadBlobAsync(byte[] pData, string pContainerName, string pFileName);
        Task<IReturnCode<string>> UploadBlobAsync(Stream pStream, string pContainerName, string pFileName);
        Task<IReturnCode<Stream>> GetBlobAsStreamAsync(string pContainerName, string pFileName);
        Task<IReturnCode<byte[]>> GetBlobAsArrayAsync(string pContainerName, string pFileName);
    }
}
