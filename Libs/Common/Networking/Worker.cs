using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;


namespace JB.Common.Networking
{
    public class Worker
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static async Task<IReturnCode<string>> GetStringResponse(string url, HttpMethod? method, IDictionary<string, string>? headers, string? content, string? mimeType) {
            ReturnCode<string> rc = new();
            string? response = null;
            HttpClient? client = null;
            HttpRequestMessage? requestMessage = null;
            HttpResponseMessage? responseMessage = null;

            if (rc.Success) {
                client = new HttpClient();
                requestMessage = new HttpRequestMessage(method ?? HttpMethod.Get, url);

                if (content != null) {
                    requestMessage.Content = new StringContent(content, Encoding.UTF8, mimeType);
                }
                
                foreach (KeyValuePair<string, string> header in headers ?? new Dictionary<string, string>()) {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }

                responseMessage = await client.SendAsync(requestMessage);

                if (HttpStatusCode.OK != responseMessage.StatusCode) {
                    rc.Errors.Add(new CommonError(ErrorCodes.BAD_STATUS_CODE_RETURNED, responseMessage.StatusCode));
                }
                if (HttpStatusCode.OK == responseMessage.StatusCode) {
                    response = await responseMessage.Content.ReadAsStringAsync();
                }
            }

            if (rc.Success)
            {
                rc.Data = response;
            }

            return rc;
        }
        public static async Task<ReturnCode<string>> GetStringResponse(string url, HttpMethod? method, IDictionary<string, string>? headers, IDictionary<string, string>? content) {
            ReturnCode<string> rc = new ReturnCode<string>();
            string? response = null;
            HttpClient? client = null;
            HttpRequestMessage? requestMessage = null;
            HttpResponseMessage? responseMessage = null;

            if (rc.Success) {
                client = new HttpClient();
                requestMessage = new HttpRequestMessage(method ?? HttpMethod.Get, url);

                if (content != null) {
                    requestMessage.Content = new FormUrlEncodedContent(content);
                }
                
                foreach (KeyValuePair<string, string> header in headers ?? new Dictionary<string, string>()) {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }

                responseMessage = await client.SendAsync(requestMessage);

                if (HttpStatusCode.OK != responseMessage.StatusCode) {
                    rc.Errors.Add(new CommonError(ErrorCodes.BAD_STATUS_CODE_RETURNED, responseMessage.StatusCode));
                }

                if (HttpStatusCode.OK == responseMessage.StatusCode) {
                    response = await responseMessage.Content.ReadAsStringAsync();
                }
            }

            if (rc.Success) {
                rc.Data = response;
            }

            return rc;
        }
    }
}
