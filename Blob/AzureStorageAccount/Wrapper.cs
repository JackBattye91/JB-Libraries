using JB.Common;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;


namespace JB.Blob.AzureStorageAccount
{
    internal class Wrapper : IWrapper
    {
        private readonly BlobServiceClient? ServiceClient;
        public Wrapper(string? pConnectionString)
        {
            if (pConnectionString != null) {
                ServiceClient = new BlobServiceClient(pConnectionString);
            }
        }

        public async Task<IReturnCode<string>> UploadBlobAsync(byte[] pData, string pContainerName, string pFileName)
        {
            IReturnCode<string> rc = new ReturnCode<string>();
            BlobContainerClient? containerClient = null;
            string? blobUri = null;

            try
            {
                if (rc.Success)
                {
                    containerClient = GetContainer(pContainerName);

                    if (containerClient == null)
                    {
                        rc.AddError(new Error("Unable to connect to container"));
                    }
                }

                if (rc.Success)
                {
                    BinaryData binaryData = new BinaryData(pData);
                    BlobClient client = containerClient!.GetBlobClient(pFileName);
                    if (client.Exists())
                    {
                        rc.AddError(new Error($"Blob {pFileName} already exists"));
                    }
                    else
                    {
                        Response<BlobContentInfo> response = await client.UploadAsync(binaryData);
                        blobUri = client.Uri.AbsoluteUri;
                    }
                }

                if (rc.Success)
                {
                    rc.Data = blobUri;
                }
            }
            catch (Exception ex)
            {
                rc.AddError(new Error(ex));
            }

            return rc;
        }

        public async Task<IReturnCode<string>> UploadBlobAsync(Stream pStream, string pContainerName, string pFileName)
        {
            IReturnCode<string> rc = new ReturnCode<string>();
            BlobContainerClient? containerClient = null;
            string? blobUri = null;
            try
            {
                if (rc.Success)
                {
                    containerClient = GetContainer(pContainerName);

                    if (containerClient == null)
                    {
                        rc.AddError(new Error("Unable to connect to container"));
                    }
                }

                if (rc.Success)
                {
                    BlobClient client = containerClient!.GetBlobClient(pFileName);
                    if (client.Exists())
                    {
                        rc.AddError(new Error($"Blob {pFileName} already exists"));
                    }
                    else
                    {
                        Response<BlobContentInfo> response = await client.UploadAsync(pStream);
                        blobUri = client.Uri.AbsolutePath;
                    }
                }

                if (rc.Success)
                {
                    rc.Data = blobUri;
                }
            }
            catch (Exception ex)
            {
                rc.AddError(new Error(ex));
            }

            return rc;
        }

        public async Task<IReturnCode<Stream>> GetBlobAsStreamAsync(string pContainerName, string pFileName)
        {
            IReturnCode<Stream> rc = new ReturnCode<Stream>();
            BlobContainerClient? containerClient = null;
            Stream? stream = null;

            try
            {
                if (rc.Success)
                {
                    containerClient = GetContainer(pContainerName);

                    if (containerClient == null)
                    {
                        rc.AddError(new Error("Unable to connect to container"));
                    }
                }

                if (rc.Success)
                {
                    BlobClient client = containerClient!.GetBlobClient(pFileName);
                    if (client.Exists())
                    {
                        Response<BlobDownloadStreamingResult> response = await client.DownloadStreamingAsync();
                        stream = response.Value.Content;
                    }
                    else
                    {
                        rc.AddError(new Error($"Blob {pFileName} does not exists"));
                    }
                }

                if (rc.Success)
                {
                    rc.Data = stream;
                }
            }
            catch (Exception ex)
            {
                rc.AddError(new Error(ex));
            }

            return rc;
        }
        public async Task<IReturnCode<byte[]>> GetBlobAsArrayAsync(string pContainerName, string pFileName)
        {
            IReturnCode<byte[]> rc = new ReturnCode<byte[]>();
            BlobContainerClient? containerClient = null;
            byte[]? data = null;

            try
            {
                if (rc.Success)
                {
                    containerClient = GetContainer(pContainerName);

                    if (containerClient == null)
                    {
                        rc.AddError(new Error("Unable to connect to container"));
                    }
                }

                if (rc.Success)
                {
                    BlobClient client = containerClient!.GetBlobClient(pFileName);
                    if (client.Exists())
                    {
                        Response<BlobDownloadResult> response = await client.DownloadContentAsync();
                        data = response.Value.Content.ToArray();
                    }
                    else
                    {
                        rc.AddError(new Error($"Blob {pFileName} does not exists"));
                    }
                }

                if (rc.Success)
                {
                    rc.Data = data;
                }
            }
            catch (Exception ex)
            {
                rc.AddError(new Error(ex));
            }

            return rc;
        }

        private BlobContainerClient? GetContainer(string pContainerName)
        {
            return ServiceClient?.GetBlobContainerClient(pContainerName);
        }
    }
}
