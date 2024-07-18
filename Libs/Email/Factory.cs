using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JB.Email
{
    public class Factory
    {
        public static IWrapper CreateEmailWrapper(string? pApiKey = null)
        {
            return new SendGrid.Wrapper(pApiKey);
        }
    }
}
