using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.Interfaces
{
    public interface IEmailBody
    {
        string Content { get; set; }
        bool IsHtml { get; set; }
    }
}
