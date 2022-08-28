using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;


namespace JB.Common {
    public class NetworkHelper {
        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static async Task<ReturnCode<string>> GetStringResponse(string url, HttpMethod? method = null, string? content = null) {
            ReturnCode<string> rc = new();
            string? response = null;
            HttpClient? client = null;
            HttpRequestMessage? requestMessage = null;
            HttpResponseMessage? responseMessage = null;

            if (method == null) {
                method = HttpMethod.Get;
            }

            if (rc.Success) {
                client = new HttpClient();
                requestMessage = new HttpRequestMessage(method, url);

                if (content != null) {
                    requestMessage.Content = new StringContent(content);
                }
                
                responseMessage = await client.SendAsync(requestMessage);

                if (HttpStatusCode.OK != responseMessage.StatusCode) {
                    rc = new(ErrorCodes.BAD_HTTP_STATUS_CODE);
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
        public static async Task<ReturnCode<string>> GetStringResponse(string url, HttpMethod? method = null, IDictionary<string,string>? content = null) {
            ReturnCode<string> rc = new();
            string? response = null;
            HttpClient? client = null;
            HttpRequestMessage? requestMessage = null;
            HttpResponseMessage? responseMessage = null;

            if (method == null) {
                method = HttpMethod.Get;
            }

            if (rc.Success) {
                client = new HttpClient();
                requestMessage = new HttpRequestMessage(method, url);

                if (content != null) {
                    requestMessage.Content = new FormUrlEncodedContent(content);
                }

                responseMessage = await client.SendAsync(requestMessage);

                if (HttpStatusCode.OK != responseMessage.StatusCode) {
                    rc = new(ErrorCodes.BAD_HTTP_STATUS_CODE);
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
