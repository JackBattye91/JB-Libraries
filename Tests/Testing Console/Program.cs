using System.Security.Cryptography;
using System.Text;
using JB.Common.Networking.JWT;
using System.IO;
using System.Net;
using JB.Common;
using Newtonsoft.Json;

namespace JB {

    enum State {
        Unknown = 0, 
        Success = 1,
    }

    class subClass {
        [JB.SqlDatabase.Attributes.PrimaryKey]
        public string Name { get; set; } = "SubClass";
    }

    class testClass {
        [JB.SqlDatabase.Attributes.PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Who";
        public string Description { get; set; } = "desc";
        public State State { get; set; } = State.Success;
        public bool Active { get; set; } = true;
        public float Percentage { get; set; } = 4.32f;
        public double Precision { get; set; } = 9.876654;
        public byte Small { get; set; } = 4;
        [JB.SqlDatabase.Attributes.Table(TableName ="testSub", ColumnName ="Name")]
        public subClass Sub { get; set; } = new subClass();

        public override string ToString() {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    class Program {
        static async Task Main(string[] args) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            string dbName = "DatabaseName.db";
            JB.SqlDatabase.IWrapper sqlWrapper = JB.SqlDatabase.Factory.CreateSqlWrapperInstance();
            await sqlWrapper.CreateDatabase(dbName);
            await sqlWrapper.CreateTable<testClass>(dbName, "testClass");
            testClass cls = new testClass() { Name = "John1", Description = "Blond" };

            /*
            if (rc.Success) {
                IReturnCode<bool> deleteTableRc = await sqlWrapper.DeleteTable(dbName, "testClass");

                if (deleteTableRc.Failed) {
                    ErrorWorker.CopyErrors(deleteTableRc, rc);
                }
            }
            if (rc.Success) {
                IReturnCode<bool> createTableRc = await sqlWrapper.CreateTable<testClass>(dbName, "testClass");

                if (createTableRc.Failed) {
                    ErrorWorker.CopyErrors(createTableRc, rc);
                }
            }

            
            
            if (rc.Success) {
                IReturnCode<IList<testClass>> getDataRc = await sqlWrapper.Get<testClass>(dbName, "testClass");

                if (getDataRc.Failed) {
                    ErrorWorker.CopyErrors(getDataRc, rc);
                }
            }
            */

            /*
            if (rc.Success) {
                IReturnCode<testClass> getDataRc = await sqlWrapper.Insert<testClass>(dbName, "testClass", cls);

                if (getDataRc.Failed) {
                    ErrorWorker.CopyErrors(getDataRc, rc);
                }
            }

            if (rc.Success) {
                IReturnCode<IList<testClass>> getDataRc = await sqlWrapper.Get<testClass>(dbName, "testClass");

                if (getDataRc.Success) {
                    foreach(var data in getDataRc.Data!) {
                        Console.WriteLine(data);
                    }
                }
                if (getDataRc.Failed) {
                    ErrorWorker.CopyErrors(getDataRc, rc);
                }
            }
            */
            
            if (rc.Success) {
                IReturnCode<testClass> getDataRc = await sqlWrapper.Update(dbName, "testClass", cls, "Name = 'John'");

                if (getDataRc.Failed) {
                    ErrorWorker.CopyErrors(getDataRc, rc);
                }
            }
            
            if (rc.Failed) {
                foreach(var error in rc.Errors) {
                    Console.WriteLine($"{error.ErrorCode} - {error.Exception?.Message}");
                }
            }

            Console.ReadLine();
        }
    }
}
