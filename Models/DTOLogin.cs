using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DTOLogin
    {
        // Input fields (login request)
        public string INI { get; set; }
        public string CODE { get; set; }

        // Returned user info (nullable when used only as login input)
        public string? LOGIN_IP { get; set; }
        public string? NAME { get; set; }
        public int? WORKING_BRANCH { get; set; }
        public string? ALLOW_BR_SELECTION { get; set; }
    }
}

