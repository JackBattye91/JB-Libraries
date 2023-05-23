using System.Security.Cryptography;
using System.Text;
using JB.Common.Networking.JWT;
using System.IO;
using System.Net;

namespace JB {

    class testClass {
        [JB.SqlDatabase.PrimaryKey]
        public string Name { get; set; }

        public testClass() {
            Name = "John";
        }
    }

    class Program {
        static void Main(string[] args) {
            JB.SqlDatabase.IWrapper sqlWrapper = JB.SqlDatabase.Factory.CreateSqlWrapperInstance();

            sqlWrapper.CreateTable<string>("TestDB", "Table1");
        }
    }
}
