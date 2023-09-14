using System.Security.Cryptography;
using System.Text;
using JB.Common.Networking.JWT;
using System.IO;
using System.Net;

namespace JB {
    class Program {
        static void Main(string[] args) {
            JB.Search.IWrapper searchWrapper = JB.Search.Factory.CreateSearchInstance(Search.ESearchAlgorithm.BINARY);
            searchWrapper.Search(new List<string>(), (x) => { return x.Length > 0; });
        }
    }
}
