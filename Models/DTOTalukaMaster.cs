using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DTOTalukaMaster
    {
        public int Code { get; set; }
        public int Dist_code { get; set; }
        public string name { get; set; }
        public int State_Code { get; set; }
        public string Country_Name { get; set; }
        public string State_Name { get; set; }
        public string District_Name { get; set; }
        public string mname { get; set; }
        public int Country_Code { get; set; }
    }
}
