using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public  class DTOAgentMaster
    {
        public int ID { get; set; }
        public int brnc_code { get; set; }
        public int code { get; set; }
        public int code1 { get; set; }
        public int drcode1 { get; set; }
        public int PrintYn { get; set; }
        public int AuthHour { get; set; }
        public int NextAcc { get; set; }
        public int NextRect { get; set; }
        public int SbGlc { get; set; }
        public int SbSlc { get; set; }
        public int MaxAMount { get; set; }
        public int TRF_CODE1 { get; set; }
        public int TRF_CODE2 { get; set; }
        public int Machine_type { get; set; }
        public int RINT_RATE { get; set; }
        public int LINT_RATE { get; set; }
        public int TDSglc { get; set; }
        public int TDSslc { get; set; }
        public int Pyg_Amt_Digit { get; set; }
        public int NoOfHolidays { get; set; }
        public int Party_Code { get; set; }
        public int OTP { get; set; }
        public String NAME { get; set; }
        public String? MNAME { get; set; }
        public String? PASSWORD { get; set; }
        public String MobileNo { get; set; }
        public String? RadyToCash { get; set; }
        public String? lockflag { get; set; }
        public String? OLDCode { get; set; }
        public String Active_YN { get; set; }
        public String? mob_no { get; set; }
        public String? auth { get; set; }
        public DateTime? Entry_Date { get; set; }
        public DateTime? Join_date { get; set; }
        public DateTime? Resign_Date { get; set; }
        public DateTime? OTP_ValidTime { get; set; }
        public DateTime? Coll_Start_Date { get; set; }
        public DateTime? Coll_End_Date { get; set; }
    }
}
