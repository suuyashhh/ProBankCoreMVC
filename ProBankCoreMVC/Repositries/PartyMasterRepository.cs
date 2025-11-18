using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using System.Data;
using System.Globalization;

namespace ProBankCoreMVC.Repositries
{
    public class PartyMasterRepository : IPartyMaster
    {
        private readonly DapperContext _dapperContext;

        public PartyMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
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

        public async Task save(DTOPartyMaster partymaster)
        {
            if (partymaster == null) throw new ArgumentNullException(nameof(partymaster));

            try
            {
                var query = "sp_Insert_Update_prtymast";

                // generate new CODE (same as you had)
                Int64 newId = await GeneratePartyMasterCode();

                var parameters = new DynamicParameters();

                // Mode: 1 => Insert (your SP checks @flag)
                parameters.Add("@flag", 1, DbType.Int32);

                // required CODE
                parameters.Add("@CODE", newId, DbType.Int64);

                // Map DTO properties -> SP params (use only DTO properties; use defaults for others)
                parameters.Add("@brnc_code", partymaster.brnc_Code, DbType.Int64);
                parameters.Add("@AcType", partymaster.AcType.ToString(), DbType.String); // SP expects nvarchar(1)
                parameters.Add("@name", partymaster.name ?? string.Empty, DbType.String);
                parameters.Add("@nmprefix", partymaster.name_Prefix ?? string.Empty, DbType.String);
                parameters.Add("@pan_no", partymaster.pan_no ?? string.Empty, DbType.String);
                parameters.Add("@GSTNo", partymaster.GST_No ?? string.Empty, DbType.String);

                parameters.Add("@ADDR1", partymaster.ADDR1 ?? string.Empty, DbType.String);
                parameters.Add("@ADDR2", partymaster.ADDR2 ?? string.Empty, DbType.String);
                parameters.Add("@ADDR3", partymaster.ADDR3 ?? string.Empty, DbType.String);
                // PIN is numeric in SP; DTO has PIN as string - try parse, fallback 0
                if (Int64.TryParse(partymaster.PIN ?? "0", out var pinVal)) parameters.Add("@PIN", pinVal, DbType.Int64);
                else parameters.Add("@PIN", 0, DbType.Int64);

                // nationality/state/district/taluka/city/area
                parameters.Add("@NationalityCode", partymaster.NationalityCode, DbType.Int64);
                parameters.Add("@NATIONALITY", partymaster.NATIONALITY ?? string.Empty, DbType.String);
                parameters.Add("@Statecode", partymaster.StateCode, DbType.Int64);
                parameters.Add("@State", partymaster.State ?? string.Empty, DbType.String);
                parameters.Add("@DistCode", partymaster.DistrictCode, DbType.Int64);
                parameters.Add("@District", partymaster.District ?? string.Empty, DbType.String);
                parameters.Add("@Talukacode", partymaster.TalukaCode, DbType.Int64);
                parameters.Add("@Taluka", partymaster.Taluka ?? string.Empty, DbType.String);
                parameters.Add("@Citycode", partymaster.CityCode, DbType.Int64);
                parameters.Add("@City", partymaster.City ?? string.Empty, DbType.String);
                parameters.Add("@Area_code", partymaster.Area_code, DbType.Int64);
                // Cor* fields
                parameters.Add("@CorADDR1", partymaster.CorAddr1 ?? string.Empty, DbType.String);
                parameters.Add("@CorADDR2", partymaster.CorAddr2 ?? string.Empty, DbType.String);
                parameters.Add("@CorADDR3", partymaster.CorAddr3 ?? string.Empty, DbType.String);
                if (Int64.TryParse(partymaster.CorPincCode ?? "0", out var corPin)) parameters.Add("@CorPIN", corPin, DbType.Int64);
                else parameters.Add("@CorPIN", 0, DbType.Int64);

                parameters.Add("@Cor_NationalityCode", partymaster.Cor_NationalityCode, DbType.Int64);
                parameters.Add("@Cor_NATIONALITY", partymaster.Cor_NATIONALITY ?? string.Empty, DbType.String);
                parameters.Add("@Cor_Statecode", partymaster.Cor_StateCode, DbType.Int64);
                parameters.Add("@Cor_State", partymaster.Cor_State ?? string.Empty, DbType.String);
                parameters.Add("@Cor_DistCode", partymaster.Cor_DistrictCode, DbType.Int64);
                parameters.Add("@Cor_District", partymaster.Cor_District ?? string.Empty, DbType.String);
                parameters.Add("@Cor_Talukacode", partymaster.Cor_TalukaCode, DbType.Int64);
                parameters.Add("@Cor_Taluka", partymaster.Cor_Taluka ?? string.Empty, DbType.String);
                parameters.Add("@Cor_Citycode", partymaster.Cor_CityCode, DbType.Int64);
                parameters.Add("@Cor_City", partymaster.Cor_City ?? string.Empty, DbType.String);
                parameters.Add("@Cor_Area_code", partymaster.Cor_Area_code, DbType.Int64);

                // Phones / contact
                parameters.Add("@PHONE", partymaster.PHONE ?? string.Empty, DbType.String);
                parameters.Add("@PHONE1", partymaster.PHONE1 ?? string.Empty, DbType.String);
                //parameters.Add("@Mobile", partymaster.Mobile ?? string.Empty, DbType.String);
                parameters.Add("@EMAIL_ID", partymaster.EMAIL_ID ?? string.Empty, DbType.String);

                // Personal info
                parameters.Add("@AGE", partymaster.AGE, DbType.Int32);
                parameters.Add("@birthdate", partymaster.birthdate == default(DateTime) ? (DateTime?)null : partymaster.birthdate, DbType.DateTime);
                parameters.Add("@SEX", partymaster.SEX ?? string.Empty, DbType.String);

                parameters.Add("@OCCU", partymaster.OCCU, DbType.Int32);
                parameters.Add("@Family_code", partymaster.Family_code, DbType.Int32);
                parameters.Add("@FATHERNAME", partymaster.FATHERNAME ?? string.Empty, DbType.String);

                // Office / company fields
                parameters.Add("@officename", partymaster.officename ?? string.Empty, DbType.String);
                parameters.Add("@OFFICEADDR1", partymaster.OFFICEADDR1 ?? string.Empty, DbType.String);
                parameters.Add("@OFFICEADDR2", partymaster.OFFICEADDR2 ?? string.Empty, DbType.String);
                parameters.Add("@OFFICEADDR3", partymaster.OFFICEADDR3 ?? string.Empty, DbType.String);
                if (Int64.TryParse(partymaster.OFFICEPIN ?? "0", out var offPin)) parameters.Add("@OFFICEPIN", offPin, DbType.Int64);
                else parameters.Add("@OFFICEPIN", 0, DbType.Int64);
                parameters.Add("@OFFICEPHONE", partymaster.OFFICEPHONE ?? string.Empty, DbType.String);
                parameters.Add("@OFFICEPHONE1", partymaster.OFFICEPHONE1 ?? string.Empty, DbType.String);

                // KYC
                parameters.Add("@KycIdProof", partymaster.KycIdProof, DbType.Int32);
                parameters.Add("@KycIdProof_Code", partymaster.KycIdProof_Code, DbType.Int64);
                parameters.Add("@KycAddrProof", partymaster.KycAddrProof, DbType.Int32);
                parameters.Add("@KycAddrProof_Code", partymaster.KycAddrProof_Code, DbType.Int64);

                // Numeric/other DTO fields
                parameters.Add("@income", partymaster.income, DbType.Int64);
                parameters.Add("@Uniq_ID", partymaster.Uniq_ID, DbType.Int64);

                // Cast / Religion
                parameters.Add("@Cast", partymaster.Cast, DbType.Int64);
                parameters.Add("@Religon", partymaster.Religion, DbType.Int64);

                // Flags from DTO
                parameters.Add("@chkSameadd", partymaster.Chk_SameAddress, DbType.Int32);

                // Provide defaults for SP parameters not present in DTO (so SP signature is satisfied)
                parameters.Add("@MEMBER_NR", DBNull.Value, DbType.String);
                parameters.Add("@MEMBER_NO", 0, DbType.Int32);
                parameters.Add("@zonecode", 0, DbType.Int32);
                parameters.Add("@Send_sms", 0, DbType.Int32);
                parameters.Add("@ST_DIR", "O", DbType.String);
                parameters.Add("@Ref_STDIR", 0, DbType.Int32);
                parameters.Add("@voteridno", partymaster.AdharNo != null ? string.Empty : string.Empty, DbType.String); // keep empty (DTO has AdharNo but not voter; adjust if you want)
                parameters.Add("@AdharNo", partymaster.AdharNo ?? string.Empty, DbType.String);
                parameters.Add("@passportno", string.Empty, DbType.String);
                parameters.Add("@passexpdate", null, DbType.DateTime);
                parameters.Add("@passauth", string.Empty, DbType.String);
                parameters.Add("@otherid", string.Empty, DbType.String);
                parameters.Add("@Driving_License", string.Empty, DbType.String);
                parameters.Add("@Driving_License_ExpDate", null, DbType.DateTime);
                parameters.Add("@rationno", string.Empty, DbType.String);

                // Company/Firm fields not in DTO - supply defaults
                parameters.Add("@COMPREGNO", string.Empty, DbType.String);
                parameters.Add("@COMPREGDT", string.Empty, DbType.String);
                parameters.Add("@COMPBRANCH", string.Empty, DbType.String);
                parameters.Add("@COMPNATURE", string.Empty, DbType.String);
                parameters.Add("@COMPPAIDCAPT", string.Empty, DbType.String);
                parameters.Add("@COMPTURNOVER", 0.0m, DbType.Decimal);
                parameters.Add("@COMPNETWORTH", 0, DbType.Int64);
                parameters.Add("@Propritor1", string.Empty, DbType.String);
                parameters.Add("@Propritor2", string.Empty, DbType.String);

                // Operation metadata
                parameters.Add("@opn_by", string.Empty, DbType.String);
                parameters.Add("@IP", string.Empty, DbType.String);

                // Output message
                parameters.Add("@msg", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);

                using (var connection = _dapperContext.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                }

                // read output message (optional logging)
                var resultMsg = parameters.Get<string>("@msg");
                // if you have logger: _logger.LogInformation("sp_Insert_Update_prtymast msg: {msg}", resultMsg);
                Console.WriteLine($"sp_Insert_Update_prtymast returned msg: {resultMsg}");
            }
            catch (Exception ex)
            {
                // keep throwing up (controller will handle) or log as required
                throw;
            }
        }


        private async Task<Int64> GeneratePartyMasterCode()
        {
            const string query = "select Top 1 CODE from prtymast order by CODE desc";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<long?>(query);
                return (lastId ?? 0) + 1;
            }
        }


    }
}
