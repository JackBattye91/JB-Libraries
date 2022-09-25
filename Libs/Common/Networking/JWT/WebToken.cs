﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Buffers.Text;
using System.Net;

namespace JB.Common.Networking.JWT {
    public interface IWebToken {
        IDictionary<string, string> Header { get; }
        IDictionary<string, string> Payload { get; }
        string Signature { get; }
    }

    public class WebToken : IWebToken {
        public IDictionary<string, string> Header { get; protected set; }

        public IDictionary<string, string> Payload { get; set; }

        public string Signature { get; protected set; }

        public WebToken() {
            Header = new Dictionary<string, string>();
            Payload = new Dictionary<string, string>();
            Signature = string.Empty;
        }
        public WebToken(IDictionary<string, string> payload) {
            Header = new Dictionary<string, string>();
            Payload = payload;
            Signature = string.Empty;
        }
    }
}
