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
        public int brnc_Code { get; set; }
        public int Uniq_ID { get; set; }
        public int AcType { get; set; }
        public string name_Prefix { get; set; }
        public string name { get; set; }
        public string ename { get; set; }
        public string ADDR1 { get; set; }
        public string ADDR2 { get; set; }
        public string ADDR3 { get; set; }
        public string CorAddr1 { get; set; }
        public string CorAddr2 { get; set; }
        public string CorAddr3 { get; set; }
        public string PIN { get; set; }
        public string CorPincCode { get; set; }
        public int AGE { get; set; }
        public string SEX { get; set; }
        public DateTime birthdate { get; set; }
        public string PHONE { get; set; }
        public string PHONE1 { get; set; }
        public string Mobile { get; set; }
        public int OCCU { get; set; }
        public int Family_code { get; set; }
        public string pan_no { get; set; }
        public string AdharNo { get; set; }
        public Int64 income { get; set; }
        public string EMAIL_ID { get; set; }
        public string NATIONALITY { get; set; }
        public string FATHERNAME { get; set; }
        public string officename { get; set; }
        public string OFFICEADDR1 { get; set; }
        public string OFFICEADDR2 { get; set; }
        public string OFFICEADDR3 { get; set; }
        public string OFFICEPIN { get; set; }
        public string OFFICEPHONE { get; set; }
        public string OFFICEPHONE1 { get; set; }
        public int KycIdProof { get; set; }
        public int KycIdProof_Code { get; set; }
        public int KycAddrProof { get; set; }
        public int KycAddrProof_Code { get; set; }
        public string GST_No { get; set; }
        public DateTime? ENTRY_DATE { get; set; }
        public int Area_code { get; set; }
        public string Area { get; set; }
        public int CityCode { get; set; }
        public string City { get; set; }
        public int TalukaCode { get; set; }
        public string Taluka { get; set; }
        public int DistrictCode { get; set; }
        public string District { get; set; }
        public int StateCode { get; set; }
        public string State { get; set; }
        public int NationalityCode { get; set; }
        public int Religion { get; set; }
        public int Cast { get; set; }
        public int Cor_NationalityCode { get; set; }
        public string Cor_NATIONALITY { get; set; }
        public int Cor_StateCode { get; set; }
        public string Cor_State { get; set; }
        public int Cor_DistrictCode { get; set; }
        public string Cor_District { get; set; }
        public int Cor_TalukaCode { get; set; }
        public string Cor_Taluka { get; set; }
        public int Cor_CityCode { get; set; }
        public string Cor_City { get; set; }
        public int Cor_Area_code { get; set; }
        public string Cor_Area { get; set; }
        public int Chk_SameAddress { get; set; }
    }
}
