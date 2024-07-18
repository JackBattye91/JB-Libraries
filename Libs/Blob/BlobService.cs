using JB.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JB.Blob
{
    public interface IBlobService
    {
        Task<IReturnCode<string>> UploadBlobAsync(byte[] pData, string pContainerName, string pFileName);
        Task<IReturnCode<string>> UploadBlobAsync(Stream pStream, string pContainerName, string pFileName);
        Task<IReturnCode<Stream>> GetBlobAsStreamAsync(string pContainerName, string pFileName);
        Task<IReturnCode<byte[]>> GetBlobAsArrayAsync(string pContainerName, string pFileName);
    }

    public class BlobService : IBlobService
    {
        private readonly IWrapper _wrapper;
        public BlobService(string? pConnectString)
        {
            _wrapper = Factory.CreateBlobWrapper(pConnectString);
        }

        public async Task<IReturnCode<string>> UploadBlobAsync(byte[] pData, string pContainerName, string pFileName) => 
            await _wrapper.UploadBlobAsync(pData, pContainerName, pFileName);
        public async Task<IReturnCode<string>> UploadBlobAsync(Stream pStream, string pContainerName, string pFileName) =>
            await _wrapper.UploadBlobAsync(pStream, pContainerName, pFileName);
        public async Task<IReturnCode<Stream>> GetBlobAsStreamAsync(string pContainerName, string pFileName) =>
            await _wrapper.GetBlobAsStreamAsync(pContainerName, pFileName);
        public async Task<IReturnCode<byte[]>> GetBlobAsArrayAsync(string pContainerName, string pFileName) =>
            await _wrapper.GetBlobAsArrayAsync(pContainerName, pFileName);
    }

    public static class BlobServiceExtension
    {
        public static IServiceCollection AddBlobService(this IServiceCollection services, string? connectionString = null)
        {
            services.AddTransient<IBlobService, BlobService>((inter) => { return new BlobService(connectionString); });
            return services;
        }
    }
}
