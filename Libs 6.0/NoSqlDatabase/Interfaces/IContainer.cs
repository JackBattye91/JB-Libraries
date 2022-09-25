using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.NoSqlDatabase.Interfaces {
    public interface IContainer {
        string? ContainerId { get; set; }
        string? ContainerName { get; set; }
        string? PartitionKey { get; set; }
    }
}
