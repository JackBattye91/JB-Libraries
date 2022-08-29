using System.Security.Cryptography;
using System.Text;
using JB.Common.Networking.JWT;
using System.IO;
using System.Net;

namespace JB {
    class Program {
        static void Main(string[] args) {
            IDictionary<string, string> payload = new Dictionary<string, string>();
            payload.Add("sub", "hello world");
            IWebToken token = new WebToken(payload);

            byte[] key = Encoding.UTF8.GetBytes("HelloWorld");
            string? tokenStr = WebToken.Sign(token, key);

            using (FileStream fStream = File.OpenWrite("testData.txt")) {
                using (StreamWriter writer = new StreamWriter(fStream)) {
                    writer.WriteLine("Key: " + Encoding.UTF8.GetString(key));
                    writer.WriteLine("Token: " + tokenStr);
                }
            }

            Console.WriteLine("Key: " + Encoding.UTF8.GetString(key));
            Console.WriteLine("Token: " + tokenStr);
            Console.ReadLine();

            bool validated = false;
            if (tokenStr != null) {
                IWebToken? parsedToken = WebToken.Parse(tokenStr);

                if (parsedToken != null) {
                    validated = WebToken.Validate(parsedToken, key);
                }
            }

            Console.WriteLine($"Validated: {validated}");
            Console.ReadLine();
        }
    }
}
