using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DTOTalukaMaster
    {
        public int COUNTRY_CODE { get; set; }
        public int STATE_CODE { get; set; }
        public int DIST_CODE { get; set; }
        public int TALUKA_CODE { get; set; }
        public string TALUKA_NAME { get; set; }
    }
}
