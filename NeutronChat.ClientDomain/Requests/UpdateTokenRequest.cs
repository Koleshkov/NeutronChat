using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeutronChat.ClientDomain.Requests
{
    public class UpdateTokenRequest
    {
        public string RefreshToken { get; set; } = "";
    }
}
