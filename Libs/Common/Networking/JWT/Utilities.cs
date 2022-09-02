﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JB.Common.Networking.JWT {
    public sealed class Utilities {
        public static string? Sign(IWebToken token, byte[] key) {
            string retValue;

            try {
                // setup header
                token.Header.Add("alg", "HS256");
                token.Header.Add("typ", "JWT");
                string? headerData = ConvertToBase64(token.Header);
                if (string.IsNullOrEmpty(headerData)) {
                    return null;
                }

                // setup payload
                string? payloadData = ConvertToBase64(token.Payload);
                if (string.IsNullOrEmpty(payloadData)) {
                    return null;
                }

                retValue = $"{headerData}.{payloadData}";

                using (HMACSHA256 sha = new HMACSHA256(key)) {
                    byte[] signatureData = sha.ComputeHash(Encoding.UTF8.GetBytes(retValue));
                    retValue += "." + Convert.ToBase64String(signatureData);
                }
            }
            catch (Exception ex) {
                return null;
            }

            return retValue;
        }
        public static bool Validate(IWebToken token, byte[] key) {
            try {
                string tokenData = string.Empty;
                string signature = string.Empty;

                string? headerData = ConvertToBase64(token.Header);
                if (string.IsNullOrEmpty(headerData)) {
                    return false;
                }

                // setup payload
                string? payloadData = ConvertToBase64(token.Payload);
                if (string.IsNullOrEmpty(payloadData)) {
                    return false;
                }

                tokenData = $"{headerData}.{payloadData}";

                using (HMACSHA256 sha = new HMACSHA256(key)) {
                    byte[] signatureData = sha.ComputeHash(Encoding.UTF8.GetBytes(tokenData));
                    signature = Convert.ToBase64String(signatureData);
                }

                if (signature.Equals(token.Signature)) {
                    return true;
                }
            }
            catch (Exception ex) {

            }

            return false;
        }

        public static string? ConvertToBase64(IDictionary<string, string> data) {
            try {
                string jsonData = JsonConvert.SerializeObject(data, Formatting.None);
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonData));
            }
            catch (Exception ex) {
                return null;
            }
        }
        public static IDictionary<string, string>? ConvertFromBase64(string base64String) {
            IDictionary<string, string>? dic = new Dictionary<string, string>();

            try {
                byte[] data = Convert.FromBase64String(base64String);
                string plainText = Encoding.UTF8.GetString(data);

                dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(plainText);
            }
            catch (Exception e) {
                return null;
            }

            return dic;
        }
    }
}