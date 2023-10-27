using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace JB.Common.Networking.JWT {
    public sealed class Utilities {
        public static IReturnCode<string> Sign(IWebToken token, byte[] key) {
            IReturnCode<string> rc = new ReturnCode<string>();
            string? signedToken = null;
            string? headerData = null;
            string? payloadData = null;

            try {
                if (rc.Success) {
                    // setup header
                    token.Header.Add("alg", "HS256");
                    token.Header.Add("typ", "JWT");

                    IReturnCode<string> convertToBase64Rc = ConvertToBase64(token.Header);
                    if (convertToBase64Rc.Success) {
                        headerData = convertToBase64Rc.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(convertToBase64Rc, rc);
                    }
                }

                if (rc.Success) {
                    // setup payload
                    IReturnCode<string> convertToBase64Rc = ConvertToBase64(token.Payload);
                    if (convertToBase64Rc.Success) {
                        payloadData = convertToBase64Rc.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(convertToBase64Rc, rc);
                    }
                }

                if (rc.Success) {
                    signedToken = $"{headerData}.{payloadData}";

                    using (HMACSHA256 sha = new HMACSHA256(key)) {
                        byte[] signatureData = sha.ComputeHash(Encoding.UTF8.GetBytes(signedToken));
                        signedToken += "." + Convert.ToBase64String(signatureData);
                    }
                }
            }
            catch (Exception ex) {
                rc.AddError(new NetworkError(ErrorCodes.SIGNING_TOKEN_FAILED, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = signedToken;
            }

            return rc;
        }
        public static IReturnCode<bool> Validate(IWebToken token, byte[] key) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            string? headerData = null;
            string? payloadData = null;
            string signature = string.Empty;

            try {
                if (rc.Success) {
                    IReturnCode<string> convertHeaderToBase64Rc = ConvertToBase64(token.Header);
                    if (convertHeaderToBase64Rc.Success) {
                        headerData = convertHeaderToBase64Rc.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(convertHeaderToBase64Rc, rc);
                    }
                }

                if (rc.Success) {
                    IReturnCode<string> convertPayloadToBase64Rc = ConvertToBase64(token.Payload);
                    if (convertPayloadToBase64Rc.Success) {
                        payloadData = convertPayloadToBase64Rc.Data;
                    }
                    else {
                        ErrorWorker.CopyErrors(convertPayloadToBase64Rc, rc);
                    }
                }

                if (rc.Success) {
                    string tokenData = $"{headerData}.{payloadData}";

                    using (HMACSHA256 sha = new HMACSHA256(key)) {
                        byte[] signatureData = sha.ComputeHash(Encoding.UTF8.GetBytes(tokenData));
                        signature = Convert.ToBase64String(signatureData);
                    }
                }

                if (rc.Success) {
                    if (signature.Equals(token.Signature) == false) {
                        rc.AddError(new NetworkError(ErrorCodes.TOKEN_SIGNATURE_DO_NOT_MATCH, HttpStatusCode.InternalServerError));
                    }
                }
            }
            catch (Exception ex) {
                rc.AddError(new NetworkError(ErrorCodes.VALIDATE_TOKEN_FAILED, HttpStatusCode.InternalServerError, ex));
            }

            return rc;
        }

        public static IReturnCode<string> ConvertToBase64(IDictionary<string, string> data) {
            IReturnCode<string> rc = new ReturnCode<string>();
            string? base64String = null;

            try {
                if (rc.Success) {
                    string jsonData = JsonConvert.SerializeObject(data, Formatting.None);
                    base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonData));
                }

                if (string.IsNullOrEmpty(base64String)) {
                    rc.AddError(new NetworkError(ErrorCodes.BAD_STATUS_CODE_RETURNED, HttpStatusCode.InternalServerError));
                }
            }
            catch (Exception ex) {
                rc.AddError(new NetworkError(ErrorCodes.CONVERT_TO_BASE_64_FAILED, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = base64String;
            }

            return rc;
        }
        public static IDictionary<string, string>? ConvertFromBase64(string base64String) {
            IReturnCode<IDictionary<string, string>> rc = new ReturnCode<IDictionary<string, string>>();
            IDictionary<string, string>? dic = new Dictionary<string, string>();

            try {
                if (rc.Success) {
                    byte[] data = Convert.FromBase64String(base64String);
                    string plainText = Encoding.UTF8.GetString(data);

                    dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(plainText);
                }
            }
            catch (Exception ex) {
                rc.AddError(new NetworkError(ErrorCodes.CONVERT_FROM_BASE_64_FAILED, HttpStatusCode.InternalServerError, ex));
            }

            if (rc.Success) {
                rc.Data = dic;
            }

            return dic;
        }
    }
}
