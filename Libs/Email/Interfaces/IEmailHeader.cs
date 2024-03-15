using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Email.Interfaces
{
    public interface IEmailHeader
    {
        string Sender { get; set; }
        IEnumerable<string> To { get; set; }
        IEnumerable<string> Cc { get; set; }
        IEnumerable<string> Bcc { get; set; }
        string Subject { get; set; }
    }
}
