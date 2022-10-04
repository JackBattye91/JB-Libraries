using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.NoSqlDatabase.Cosmos.Models {
    internal class Container : Interfaces.IContainer {
        public string? ContainerId { get; set; }
        public string? ContainerName { get; set; }
        public string? PartitionKey { get; set; }
    }
}
