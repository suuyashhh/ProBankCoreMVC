using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DTODistrictMaster
    {
        public int Country_Code { get; set; }
        public int State_Code { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public DateTime Entry_Date { get; set; }
    }
}
