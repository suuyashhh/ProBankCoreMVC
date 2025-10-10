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
        public String? LOGIN_IP { get; set; }
        public string? NAME { get; set; }
        public string? AUTHORITY { get; set; }
        public string? ACTIVATE { get; set; }
    }
}

