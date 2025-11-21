using System;
using System.Collections;
using System.Reflection;

namespace Models
{
    public class DTOPartyMaster
    {
        // Flags & Keys
        public int? flag { get; set; }
        public long CODE { get; set; }
        public long brnc_code { get; set; }
        public long? Uniq_ID { get; set; }

        // Basic
        public string? AcType { get; set; }              // nvarchar(1) in proc
        public string? nmprefix { get; set; }
        public string? name { get; set; }

        // PAN, Aadhaar, GST
        public string? pan_no { get; set; }
        public string? GSTNo { get; set; }
        public string? AdharNo { get; set; }

        // Permanent Address
        public string? ADDR1 { get; set; }
        public string? ADDR2 { get; set; }
        public string? ADDR3 { get; set; }
        public long? PIN { get; set; }                    // numeric(10,0)

        public long? NationalityCode { get; set; }
        public string? NATIONALITY { get; set; }

        public long? Statecode { get; set; }
        public string? State { get; set; }

        public long? DistCode { get; set; }
        public string? District { get; set; }

        public long? Talukacode { get; set; }
        public string? Taluka { get; set; }

        public long? Citycode { get; set; }
        public string? City { get; set; }

        public long? Area_code { get; set; }
        public string? Area { get; set; }

        public long? chkSameadd { get; set; }

        // Correspondence Address
        public string? CorADDR1 { get; set; }
        public string? CorADDR2 { get; set; }
        public string? CorADDR3 { get; set; }
        public long? CorPIN { get; set; }

        public long? Cor_NationalityCode { get; set; }
        public string? Cor_NATIONALITY { get; set; }

        public long? Cor_Statecode { get; set; }
        public string? Cor_State { get; set; }

        public long? Cor_DistCode { get; set; }
        public string? Cor_District { get; set; }

        public long? Cor_Talukacode { get; set; }
        public string? Cor_Taluka { get; set; }

        public long? Cor_Citycode { get; set; }
        public string? Cor_City { get; set; }

        public long? Cor_Area_code { get; set; }
        public string? Cor_Area { get; set; }

        // Contact
        public string? PHONE { get; set; }
        public string? PHONE1 { get; set; }
        public string? Mobile { get; set; }

        public long? zonecode { get; set; }
        public bool? Send_sms { get; set; }

        // Personal
        public long? AGE { get; set; }
        public DateTime? birthdate { get; set; }
        public string? SEX { get; set; }
        public long? OCCU { get; set; }
        public long? Family_code { get; set; }

        public string? MEMBER_NR { get; set; }
        public int? MEMBER_NO { get; set; }

        public string? ST_DIR { get; set; }
        public long? Ref_STDIR { get; set; }

        // ID/Passport/Voter
        public string? Adhar { get; set; }
        public string? voteridno { get; set; }
        public string? passportno { get; set; }
        public DateTime? passexpdate { get; set; }
        public string? passauth { get; set; }
        public string? otherid { get; set; }

        public string? Driving_License { get; set; }
        public DateTime? Driving_License_ExpDate { get; set; }
        public string? rationno { get; set; }

        // Other Details
        public string? FATHERNAME { get; set; }
        public string? officename { get; set; }

        public string? OFFICEADDR1 { get; set; }
        public string? OFFICEADDR2 { get; set; }
        public string? OFFICEADDR3 { get; set; }

        public long? OFFICEPIN { get; set; }
        public string? OFFICEPHONE { get; set; }
        public string? OFFICEPHONE1 { get; set; }
        public string? EMAIL_ID { get; set; }

        // Beneficiary Bank Details
        public string? Name_Bneficiary { get; set; }
        public string? AccountNo_Bneficiary { get; set; }
        public string? IFSCODE_Bneficiary { get; set; }
        public string? BankName_Bneficiary { get; set; }
        public string? BrName_Bneficiary { get; set; }

        // Company/Firm Details
        public string? COMPREGNO { get; set; }
        public string? COMPREGDT { get; set; }
        public string? COMPBRANCH { get; set; }
        public string? COMPNATURE { get; set; }
        public string? COMPPAIDCAPT { get; set; }
        public decimal? COMPTURNOVER { get; set; }
        public decimal? COMPNETWORTH { get; set; }
        public string? Propritor1 { get; set; }
        public string? Propritor2 { get; set; }

        // Religion / Caste
        public long? Cast { get; set; }
        public long? Religon { get; set; }    // spelled as in proc

        // KYC
        public bool? KycAddrProof { get; set; }
        public long? KycAddrProof_Code { get; set; }
        public bool? KycIdProof { get; set; }
        public long? KycIdProof_Code { get; set; }
        public string? KycIdProof_DocNo { get; set; }
        public string? KycAddrProof_DocNo { get; set; }

        // SYSTEM & META
        public string? opn_by { get; set; }
        public string? IP { get; set; }
        public string? ST_DIR_In { get; set; }

        // Additional system fields
        public string? msg { get; set; }      // output

        // Extra / compatibility
        public string? ledg_no { get; set; }
        public string? Autho_By { get; set; }
        public DateTime? Autho_Date { get; set; }

        public DateTime? opndate { get; set; }
        public DateTime? ac_opn_time { get; set; }
        public string? ac_opn_IP { get; set; }

        public string? authorise { get; set; }
        public string? authorise_by { get; set; }
        public DateTime? Authorise_Date { get; set; }
        public string? Authorise_IP { get; set; }
        public DateTime? Authorise_Time { get; set; }

        public string? For_Correction { get; set; }
        public DateTime? Entry_Date { get; set; }
        public long? Zone { get; set; }

        public DateTime? UID_Date { get; set; }
        public string? UID_By { get; set; }
        public DateTime? UID_Time { get; set; }

        public string? OldID { get; set; }
        public string? BrCustNo { get; set; }
        public List<DTOUploadPhotoSign> Pictures { get; set; } = new List<DTOUploadPhotoSign>();




    }
}