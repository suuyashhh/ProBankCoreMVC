using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DTOPartyMaster
    {
        public int CODE { get; set; }
        public int BRNC_CODE { get; set; }
        public string UNIQ_ID { get; set; }
        public string ACTYPE { get; set; }
        public string NAME_PREFIX { get; set; }
        public string NAME { get; set; }
        public string ADDR1 { get; set; }
        public string ADDR2 { get; set; }
        public string ADDR3 { get; set; }
        public string PIN { get; set; }
        public string PHONE1 { get; set; }
        public string MOBILE { get; set; }
        public string OCCU { get; set; }
        public string PAN_NO { get; set; }
        public string ADHARNO { get; set; }
        public string EMAIL_ID { get; set; }
        public string NATIONALITY { get; set; }
        public string FATHERNAME { get; set; }
        public string OFFICENAME { get; set; }
        public string OFFICEADDR1 { get; set; }
        public string OFFICEADDR2 { get; set; }
        public string OFFICEADDR3 { get; set; }
        public string OFFICEPIN { get; set; }
        public DateTime? ENTRY_DATE { get; set; }
        public string AREA_CODE { get; set; }
        public string CITYCODE { get; set; }
        public string CITY { get; set; }
        public string TALUKACODE { get; set; }
        public string TALUKA { get; set; }
        public string DISTRICTCODE { get; set; }
        public string DISTRICT { get; set; }
        public string STATECODE { get; set; }
        public string STATE { get; set; }
        public string NATIONALITYCODE { get; set; }
        public string RELIGION { get; set; }
        public string CAST { get; set; }
    }
}
