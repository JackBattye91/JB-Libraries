using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace NoSqlDatabase.Tests.Models {
    internal class TestModel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<string> TestItems { get; set; }

        public TestModel() {
            Name = String.Empty;
            TestItems = new List<string>();
        }
    }
}
