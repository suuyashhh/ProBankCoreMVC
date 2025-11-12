using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DTOCityMaster
    {
        public int COUNTRY_CODE { get; set; }
        public int ID { get; set; }
        public int STATE_CODE { get; set; }
        public int DIST_CODE { get; set; }
        public int CITY_CODE { get; set; }
        public int TALUKA_CODE { get; set; }
        public string CITY_NAME { get; set; }
        public string COUNTRY_NAME { get; set; }
        public string Name { get; set; }
        public string STATE_NAME { get; set; }
        public string DIST_NAME { get; set; }
        public string TALUKA_NAME { get; set; }
        public string pin { get; set; }
        public DateTime Entry_Date { get; set; }
    }
}
