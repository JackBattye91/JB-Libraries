using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Email.Interfaces;
using JB.Common;

namespace JB.Email
{
    public interface IWrapper
    {
        Task<IReturnCode> Send(IEmailHeader pHeader, IEmailBody pBody);
    }
}
