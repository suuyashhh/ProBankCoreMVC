using System.Data;
using System.Globalization;
using Dapper;
using Microsoft.Data.SqlClient;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class PartyMasterRepository : IPartyMaster
    {
        private readonly DapperContext _dapperContext;
        private readonly PhotoDapperContext _photodapperContext;

        public PartyMasterRepository(DapperContext dapperContext,PhotoDapperContext photoDapperContext)
        {
            _dapperContext = dapperContext;
            _photodapperContext = photoDapperContext;
        }

        //public async Task save(DTOPartyMaster partymaster)
        //{
        //    try
        //    {
        //        var query = "sp_Insert_Update_prtymast";
        //        Int64 newId = await GeneratePartyMasterCode();
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@CODE", newId);

        //        using (var connection = _dapperContext.CreateConnection())
        //        {
        //            await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<string> Save(DTOPartyMaster p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));

            try
            {
                var query = "sp_Insert_Update_prtymast";
                var photoquery = "sp_Upload_Photo_and_Sign";

                // Generate new CODE exactly as you did before (used for insert)
                long newCode = await GeneratePartyMasterCode();

                var prm = new DynamicParameters();

                // required flags
                prm.Add("@flag", 1, DbType.Int32);                // 1 = insert
                prm.Add("@CODE", newCode, DbType.Int64);

                // Map EVERY parameter as declared in your SP header.
                // NOTE: parameter names match the ALTER PROCEDURE header you pasted.

                prm.Add("@brnc_code", p.brnc_code, DbType.Decimal); // numeric(18,0)
                prm.Add("@AcType", p.AcType ?? string.Empty, DbType.String);
                prm.Add("@name", p.name ?? string.Empty, DbType.String);
                prm.Add("@nmprefix", p.nmprefix ?? string.Empty, DbType.String);
                prm.Add("@pan_no", p.pan_no ?? string.Empty, DbType.String);
                prm.Add("@GSTNo", p.GSTNo ?? string.Empty, DbType.String);

                prm.Add("@ADDR1", p.ADDR1 ?? string.Empty, DbType.String);
                prm.Add("@ADDR2", p.ADDR2 ?? string.Empty, DbType.String);
                prm.Add("@ADDR3", p.ADDR3 ?? string.Empty, DbType.String);
                prm.Add("@PIN", p.PIN, DbType.Int64);

                prm.Add("@NationalityCode", p.NationalityCode ?? 0, DbType.Decimal);
                prm.Add("@NATIONALITY", p.NATIONALITY ?? string.Empty, DbType.String);

                // use parameter name exactly as SP header: @Statecode etc.
                prm.Add("@Statecode", p.Statecode ?? 0, DbType.Decimal);
                prm.Add("@State", p.State ?? string.Empty, DbType.String);

                prm.Add("@DistCode", p.DistCode ?? 0, DbType.Decimal);
                prm.Add("@District", p.District ?? string.Empty, DbType.String);

                prm.Add("@Talukacode", p.Talukacode ?? 0, DbType.Decimal);
                prm.Add("@Taluka", p.Taluka ?? string.Empty, DbType.String);

                prm.Add("@Citycode", p.Citycode ?? 0, DbType.Decimal);
                prm.Add("@City", p.City ?? string.Empty, DbType.String);

                prm.Add("@Area_code", p.Area_code ?? 0, DbType.Decimal);
                prm.Add("@Area", p.Area ?? string.Empty, DbType.String);
                prm.Add("@chkSameadd", p.chkSameadd ?? 0, DbType.Decimal);

                // Correspondence
                prm.Add("@CorADDR1", p.CorADDR1 ?? string.Empty, DbType.String);
                prm.Add("@CorADDR2", p.CorADDR2 ?? string.Empty, DbType.String);
                prm.Add("@CorADDR3", p.CorADDR3 ?? string.Empty, DbType.String);
                prm.Add("@CorPIN", p.CorPIN, DbType.Int64);

                prm.Add("@Cor_NationalityCode", p.Cor_NationalityCode ?? 0, DbType.Decimal);
                prm.Add("@Cor_NATIONALITY", p.Cor_NATIONALITY ?? string.Empty, DbType.String);
                prm.Add("@Cor_Statecode", p.Cor_Statecode ?? 0, DbType.Decimal);
                prm.Add("@Cor_State", p.Cor_State ?? string.Empty, DbType.String);
                prm.Add("@Cor_DistCode", p.Cor_DistCode ?? 0, DbType.Decimal);
                prm.Add("@Cor_District", p.Cor_District ?? string.Empty, DbType.String);
                prm.Add("@Cor_Talukacode", p.Cor_Talukacode ?? 0, DbType.Decimal);
                prm.Add("@Cor_Taluka", p.Cor_Taluka ?? string.Empty, DbType.String);
                prm.Add("@Cor_Citycode", p.Cor_Citycode ?? 0, DbType.Decimal);
                prm.Add("@Cor_City", p.Cor_City ?? string.Empty, DbType.String);
                prm.Add("@Cor_Area_code", p.Cor_Area_code ?? 0, DbType.Decimal);
                prm.Add("@Cor_Area", p.Cor_Area  ?? string.Empty, DbType.String);

                // Phone / zone / sms
                prm.Add("@PHONE", p.PHONE ?? string.Empty, DbType.String);
                prm.Add("@PHONE1", p.PHONE1 ?? string.Empty, DbType.String);
                prm.Add("@zonecode", p.zonecode ?? 0, DbType.Decimal);
                prm.Add("@Send_sms", p.Send_sms ?? false, DbType.Boolean);

                // Personal numeric fields
                prm.Add("@AGE", p.AGE ?? 0, DbType.Decimal);
                prm.Add("@birthdate", p.birthdate ?? (DateTime?)null, DbType.DateTime);
                prm.Add("@SEX", p.SEX ?? string.Empty, DbType.String);
                prm.Add("@OCCU", p.OCCU ?? 0, DbType.Decimal);
                prm.Add("@Family_code", p.Family_code ?? 0, DbType.Decimal);
                prm.Add("@MEMBER_NR", p.MEMBER_NR ?? string.Empty, DbType.String);
                prm.Add("@MEMBER_NO", p.MEMBER_NO ?? 0, DbType.Decimal);

                // Status fields
                prm.Add("@ST_DIR", p.ST_DIR ?? "O", DbType.String);
                prm.Add("@Ref_STDIR", p.Ref_STDIR ?? 0, DbType.Decimal);

                // IDs & docs
                prm.Add("@AdharNo", p.AdharNo ?? string.Empty, DbType.String);
                prm.Add("@voteridno", p.voteridno ?? string.Empty, DbType.String);
                prm.Add("@passportno", p.passportno ?? string.Empty, DbType.String);
                prm.Add("@passexpdate", p.passexpdate ?? (DateTime?)null, DbType.DateTime);
                prm.Add("@passauth", p.passauth ?? string.Empty, DbType.String);
                prm.Add("@otherid", p.otherid ?? string.Empty, DbType.String);
                prm.Add("@Driving_License", p.Driving_License ?? string.Empty, DbType.String);
                prm.Add("@Driving_License_ExpDate", p.Driving_License_ExpDate ?? (DateTime?)null, DbType.DateTime);
                prm.Add("@rationno", p.rationno ?? string.Empty, DbType.String);

                // Other details
                prm.Add("@FATHERNAME", p.FATHERNAME ?? string.Empty, DbType.String);
                prm.Add("@officename", p.officename ?? string.Empty, DbType.String);
                prm.Add("@OFFICEADDR1", p.OFFICEADDR1 ?? string.Empty, DbType.String);
                prm.Add("@OFFICEADDR2", p.OFFICEADDR2 ?? string.Empty, DbType.String);
                prm.Add("@OFFICEADDR3", p.OFFICEADDR3 ?? string.Empty, DbType.String);
                prm.Add("@OFFICEPIN", p.OFFICEPIN, DbType.Int64);
                prm.Add("@OFFICEPHONE", p.OFFICEPHONE ?? string.Empty, DbType.String);
                prm.Add("@OFFICEPHONE1", p.OFFICEPHONE1 ?? string.Empty, DbType.String);
                prm.Add("@EMAIL_ID", p.EMAIL_ID ?? string.Empty, DbType.String);

                //Benefi/ciary bank info(SP header includes these)
                prm.Add("@Name_Bneficiary", p.Name_Bneficiary ?? string.Empty, DbType.String);
                prm.Add("@AccountNo_Bneficiary", p.AccountNo_Bneficiary ?? string.Empty, DbType.String);
                prm.Add("@IFSCODE_Bneficiary", p.IFSCODE_Bneficiary ?? string.Empty, DbType.String);
                prm.Add("@BankName_Bneficiary", p.BankName_Bneficiary ?? string.Empty, DbType.String);
                prm.Add("@BrName_Bneficiary", p.BrName_Bneficiary ?? string.Empty, DbType.String);
                prm.Add("@BankName_Bneficiary", "");
                prm.Add("@BrName_Bneficiary", "");


                // Company / Firm
                prm.Add("@COMPREGNO", p.COMPREGNO ?? string.Empty, DbType.String);
                prm.Add("@COMPREGDT", p.COMPREGDT ?? string.Empty, DbType.String);
                prm.Add("@COMPBRANCH", p.COMPBRANCH ?? string.Empty, DbType.String);
                prm.Add("@COMPNATURE", p.COMPNATURE ?? string.Empty, DbType.String);
                prm.Add("@COMPPAIDCAPT", p.COMPPAIDCAPT ?? string.Empty, DbType.String);
                prm.Add("@COMPTURNOVER", p.COMPTURNOVER ?? 0m, DbType.Decimal);
                prm.Add("@COMPNETWORTH", p.COMPNETWORTH ?? 0, DbType.Decimal);
                prm.Add("@Propritor1", p.Propritor1 ?? string.Empty, DbType.String);
                prm.Add("@Propritor2", p.Propritor2 ?? string.Empty, DbType.String);

                // Cast / religion / KYC codes
                prm.Add("@Cast", p.Cast ?? 0, DbType.Decimal);
                prm.Add("@Religon", p.Religon ?? 0, DbType.Decimal);

                prm.Add("@KycAddrProof", p.KycAddrProof ?? false, DbType.Boolean);
                prm.Add("@KycAddrProof_Code", p.KycAddrProof_Code ?? 0, DbType.Decimal);
                prm.Add("@KycIdProof", p.KycIdProof ?? false, DbType.Boolean);
                prm.Add("@KycIdProof_Code", p.KycIdProof_Code ?? 0, DbType.Decimal);

                // KYC doc numbers
                //prm.Add("@KycIdProof_DocNo", p.KycIdProof_DocNo ?? string.Empty, DbType.String);
                //prm.Add("@KycAddrProof_DocNo", p.KycAddrProof_DocNo ?? string.Empty, DbType.String);

                // opn and ip
                prm.Add("@opn_by", p.opn_by ?? string.Empty, DbType.String);
                prm.Add("@IP", p.IP ?? string.Empty, DbType.String);

                // default output param
                prm.Add("@msg", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);

                // many SP parameters exist; ensure all required ones are present above.
                using (var con = _dapperContext.CreateConnection())
                {
                    await con.ExecuteAsync(query, prm, commandType: CommandType.StoredProcedure);
                }


                // ensure Pictures safe to iterate
                foreach (var item in p.Pictures ?? Enumerable.Empty<DTOUploadPhotoSign>())
                {
                    // skip empty picture entries
                    if (string.IsNullOrWhiteSpace(item.Picture))
                        continue;

                    // Normalize base64: remove data URI prefix if present and strip whitespace/newlines
                    var raw = item.Picture;
                    var commaIdx = raw.IndexOf(',');
                    if (commaIdx >= 0) raw = raw.Substring(commaIdx + 1);
                    raw = raw.Replace("\r", "").Replace("\n", "").Trim();

                    byte[] photoBytes;
                    try
                    {
                        photoBytes = Convert.FromBase64String(raw);
                    }
                    catch (FormatException)
                    {
                        // either skip invalid images or throw with a useful message.
                        // Here I throw so caller knows which picture failed.
                        throw new Exception($"Invalid base64 image for picture srno={item.srno ?? 0}");
                    }

                    var photoprm = new DynamicParameters();

                    photoprm.Add("@flag1", 1, DbType.Int32);

                    // flag may be string in your DTO — coerce to int safely (adjust default as needed)
                    int flagValue = 0;
                    if (item.flag != null)
                    {
                        if (!int.TryParse(item.flag.ToString(), out flagValue))
                        {
                            // if your SP expects a string flag, change DbType and pass string instead.
                            // For now we coerce to int (fallback 0)
                            flagValue = 0;
                        }
                    }
                    photoprm.Add("@flag", item.flag);

                    photoprm.Add("@Party_Code", newCode, DbType.Int64);

                    // IMPORTANT: pass byte[] as binary, with explicit size to ensure Dapper/SQL treats it as binary
                    photoprm.Add("@Picture", photoBytes, DbType.Binary, ParameterDirection.Input, photoBytes.Length);

                    // other params (use correct types)
                    photoprm.Add("@Code1", 0, DbType.Int32);
                    photoprm.Add("@brnc_code", p.brnc_code, DbType.Decimal);
                    photoprm.Add("@code2", 0, DbType.Int32);
                    photoprm.Add("@srno", 1, DbType.Int32);
                    photoprm.Add("@scan_by", item.Scan_By ?? 0, DbType.Int32);
                    photoprm.Add("@Shr_code1", 0, DbType.Int32);
                    photoprm.Add("@shr_code2", 0, DbType.Int32);
                    photoprm.Add("@Doc_No", 0, DbType.Int32);
                    photoprm.Add("@Description", string.Empty, DbType.String);
                    photoprm.Add("@Msg", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);

                    using (var con = _photodapperContext.CreateConnection())
                    {
                        await con.ExecuteAsync(photoquery, photoprm, commandType: CommandType.StoredProcedure);
                    }
                }


                var result = prm.Get<string>("@msg");
                // return sp's msg (e.g. new CODE or error code sent by SP)
                return result;
            }
            catch (Exception ex)
            {
                // recommended: use ILogger in real app. Here we wrap and rethrow for controller to handle.
                throw new Exception("Failed to save Party Master (SaveFullAsync). See inner exception.", ex);
            }
         }
        // Helper - generate next CODE (you said you have this before)
        private async Task<long> GeneratePartyMasterCode()
        {
            // placeholder: implement your logic to fetch max(CODE) + 1 or sequence
            using (var con = _dapperContext.CreateConnection())
            {
                var sql = "SELECT ISNULL(MAX(CODE),0) + 1 FROM dbo.prtymast";
                var next = await con.ExecuteScalarAsync<long>(sql);
                return next;
            }
        }

        // Helper parsing methods: convert string to long or return 0/dbnull safe value
        private long ParseLongOrDbNull(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;
            return long.TryParse(value, out var v) ? v : 0;
        }

        private long TryLong(string v)
        {
            if (long.TryParse(v, out var x))
                return x;

            return 0;
        }




        //private async Task<Int64> GeneratePartyMasterCode()
        //{
        //    const string query = "select Top 1 CODE from prtymast order by CODE desc";

        //    using (var conn = _dapperContext.CreateConnection())
        //    {
        //        var lastId = await conn.ExecuteScalarAsync<long?>(query);
        //        return (lastId ?? 0) + 1;
        //    }
        //}


    }
}
