using System.Data;
using System.Globalization;
using Dapper;
using Microsoft.Data.SqlClient;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using static Models.DTOPartyMaster;

namespace ProBankCoreMVC.Repositries
{
    public class PartyMasterRepository : IPartyMaster
    {
        private readonly DapperContext _dapperContext;
        private readonly PhotoDapperContext _photodapperContext;

        public PartyMasterRepository(DapperContext dapperContext, PhotoDapperContext photoDapperContext)
        {
            _dapperContext = dapperContext;
            _photodapperContext = photoDapperContext;
        }


        public async Task<List<DTOPartyMaster.CustomerSummary>> GetCustomers(int branchCode, string? search = null)
        {
            search = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

            string query;

            if (search == null)
            {
                query = @"
            SELECT TOP (10) CODE, brnc_Code AS brnc_code, name, Mobile
            FROM prtymast
            WHERE brnc_Code = @BranchCode
            ORDER BY code DESC;
        ";
            }
            else
            {
                query = @"
            SELECT CODE, brnc_Code AS brnc_code, name, PHONE1
            FROM prtymast
            WHERE brnc_Code = @BranchCode
              AND (
                    (TRY_CAST(@Search AS BIGINT) IS NOT NULL AND CODE = TRY_CAST(@Search AS BIGINT))
                    OR (TRY_CAST(@Search AS BIGINT) IS NULL AND UPPER(name) LIKE CONCAT('%', UPPER(@Search), '%'))
                  )
            ORDER BY name;
        ";
            }

            using (var con = _dapperContext.CreateConnection())
            {
                var result = await con.QueryAsync<DTOPartyMaster.CustomerSummary>(
                    query,
                    new { BranchCode = branchCode, Search = search }
                );

                return result.AsList();
            }
        }


        public async Task<DTOPartyMaster> GetCustomerById(int Cust_Code)
        {
            var query = @"
              SELECT P.CODE,AcType,P.name,name_Prefix AS nmprefix,pan_no,GST_No AS GSTNo,ADDR1,ADDR2,ADDR3,PIN,NationalityCode,NATIONALITY,Statecode,State,DistrictCode AS DistCode,District,Talukacode,Taluka,Citycode,City,
Area_code,Area,Chk_SameAddress AS chkSameadd,CorADDR1,CorADDR2,CorADDR3,CorPincCode AS CorPIN,Cor_NationalityCode,Cor_NATIONALITY,Cor_Statecode,Cor_State,Cor_DistrictCode AS Cor_DistCode,Cor_District,Cor_Talukacode,
Cor_Taluka,Cor_Citycode,Cor_City,Cor_Area_code,Cor_Area,PHONE,PHONE1,Mobile,Mobile1,zonecode,Send_sms,AGE,birthdate,SEX,O.name As OCCU_Name, OCCU,Family_code,MEMBER_NR,MEMBER_NO,ST_DIR,S.name as Ref_STDIR_Name,Ref_STDIR,
AdharNo,voteridno,passportno,passexpdate,passauth,otherid,Driving_License,Driving_License_ExpDate,rationno,FATHERNAME,officename,OFFICEADDR1,OFFICEADDR2,
OFFICEADDR3,OFFICEPIN,OFFICEPHONE,OFFICEPHONE1,EMAIL_ID,Name_Bneficiary,AccountNo_Bneficiary,IFSCODE_Bneficiary,BankName_Bneficiary,BrName_Bneficiary,COMPREGNO,
COMPREGDT,COMPBRANCH,COMPNATURE,COMPPAIDCAPT,COMPTURNOVER,COMPNETWORTH,propname1 AS Propritor1,propname2 AS Propritor2,C.name as Cast_Name,Cast,R.Name as Religon_Name,Religion AS Religon,KycAddrProof,KA.NAME As KycAddrProof_Name,KycAddrProof_Code,KycIdProof,KI.NAME As KycIdProof_Name,
KycIdProof_Code,opn_by
      FROM prtymast as P
	  join ReligionMast as R
	  on R.code = P.Religion
	  join castmast as C
	  on C.code = P.Cast
	  join occumast as O
	  on O.code = P.OCCU
	  join StaffMast as S 
	  on S.code = P.Ref_STDIR
	  join KycAddrmast as KA 
	  on KA.CODE = P.KycAddrProof_Code
	  join KycIdmast as KI
	  on KI.CODE = P.KycIdProof_Code
      WHERE P.CODE = @CustCode;
    ";

            try
            {
                using (var con = _dapperContext.CreateConnection())
                {
                    var customer = await con.QueryFirstOrDefaultAsync<DTOPartyMaster>(query, new { CustCode = Cust_Code });

                    if (customer == null)
                    {
                        return null;
                    }

                    customer.Pictures = new List<DTOUploadPhotoSign>();

                    var photoSql = @"
                SELECT ID, code1, brnc_code, code2, srno, flag, Scan_By, Scan_Date, Authorise_By, Authorise_Date,
                       Party_Code, Shr_code1, Shr_code2, Picture, Entry_Date, Description, Doc_No, Picture_Description
                FROM picture
                WHERE Party_Code = @PartyCode
                ORDER BY srno;
            ";

                    using (var pcon = _photodapperContext.CreateConnection())
                    {
                        var photos = await pcon.QueryAsync(photoSql, new { PartyCode = Cust_Code });

                        foreach (var row in photos)
                        {
                            byte[]? pictureBytes = null;
                            try
                            {
                                var dict = (IDictionary<string, object?>)row as IDictionary<string, object?>;
                                if (dict != null && dict.ContainsKey("Picture") && dict["Picture"] is byte[] b)
                                    pictureBytes = b;
                                else if (row.Picture is byte[] bb)
                                    pictureBytes = bb;
                            }
                            catch
                            {
                                try { pictureBytes = (byte[])row.Picture; } catch { pictureBytes = null; }
                            }

                            string? pictureBase64 = null;
                            if (pictureBytes != null && pictureBytes.Length > 0)
                            {
                                pictureBase64 = "data:image/jpeg;base64," + Convert.ToBase64String(pictureBytes);
                            }

                            var dto = new DTOUploadPhotoSign
                            {
                                srno = row.srno != null ? (int?)Convert.ToInt32(row.srno) : null,
                                Scan_By = row.Scan_By != null ? (int?)Convert.ToInt32(row.Scan_By) : null,
                                Party_Code = row.Party_Code != null ? (int?)Convert.ToInt32(row.Party_Code) : null,
                                flag = row.flag != null ? row.flag.ToString() : null,
                                Picture = pictureBase64
                            };

                            customer.Pictures.Add(dto);
                        }
                    }

                    return customer;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load customer (CODE={Cust_Code}). See inner exception.", ex);
            }
        }


        public async Task<string> Save(DTOPartyMaster p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p));

            try
            {
                var query = "sp_Insert_Update_prtymast";
                var photoquery = "sp_Upload_Photo_and_Sign";

                long newCode = await GeneratePartyMasterCode();

                var prm = new DynamicParameters();

                // ALWAYS INSERT
                prm.Add("@flag", 1, DbType.Int32);
                prm.Add("@CODE", 0);

                // ---------- ALL YOUR ORIGINAL PARAMETERS ----------
                prm.Add("@brnc_code", p.brnc_code, DbType.Decimal);
                prm.Add("@AcType", p.AcType ?? string.Empty);
                prm.Add("@name", p.name ?? string.Empty);
                prm.Add("@nmprefix", p.nmprefix ?? string.Empty);
                prm.Add("@pan_no", p.pan_no ?? string.Empty);
                prm.Add("@GSTNo", p.GSTNo ?? string.Empty);

                prm.Add("@ADDR1", p.ADDR1 ?? string.Empty);
                prm.Add("@ADDR2", p.ADDR2 ?? string.Empty);
                prm.Add("@ADDR3", p.ADDR3 ?? string.Empty);
                prm.Add("@PIN", p.PIN ?? 0);

                prm.Add("@NationalityCode", p.NationalityCode ?? 0);
                prm.Add("@NATIONALITY", p.NATIONALITY ?? string.Empty);

                prm.Add("@Statecode", p.Statecode ?? 0);
                prm.Add("@State", p.State ?? string.Empty);
                prm.Add("@DistCode", p.DistCode ?? 0);
                prm.Add("@District", p.District ?? string.Empty);
                prm.Add("@Talukacode", p.Talukacode ?? 0);
                prm.Add("@Taluka", p.Taluka ?? string.Empty);
                prm.Add("@Citycode", p.Citycode ?? 0);
                prm.Add("@City", p.City ?? string.Empty);
                prm.Add("@Area_code", p.Area_code ?? 0);
                prm.Add("@Area", p.Area ?? string.Empty);
                prm.Add("@chkSameadd", p.chkSameadd ?? 0);

                prm.Add("@CorADDR1", p.CorADDR1 ?? string.Empty);
                prm.Add("@CorADDR2", p.CorADDR2 ?? string.Empty);
                prm.Add("@CorADDR3", p.CorADDR3 ?? string.Empty);
                prm.Add("@CorPIN", p.CorPIN ?? 0);
                prm.Add("@Cor_NationalityCode", p.Cor_NationalityCode ?? 0);
                prm.Add("@Cor_NATIONALITY", p.Cor_NATIONALITY ?? string.Empty);
                prm.Add("@Cor_Statecode", p.Cor_Statecode ?? 0);
                prm.Add("@Cor_State", p.Cor_State ?? string.Empty);
                prm.Add("@Cor_DistCode", p.Cor_DistCode ?? 0);
                prm.Add("@Cor_District", p.Cor_District ?? string.Empty);
                prm.Add("@Cor_Talukacode", p.Cor_Talukacode ?? 0);
                prm.Add("@Cor_Taluka", p.Cor_Taluka ?? string.Empty);
                prm.Add("@Cor_Citycode", p.Cor_Citycode ?? 0);
                prm.Add("@Cor_City", p.Cor_City ?? string.Empty);
                prm.Add("@Cor_Area_code", p.Cor_Area_code ?? 0);
                prm.Add("@Cor_Area", p.Cor_Area ?? string.Empty);

                prm.Add("@PHONE", p.PHONE ?? string.Empty);
                prm.Add("@PHONE1", p.PHONE1 ?? string.Empty);
                prm.Add("@Mobile", p.Mobile ?? string.Empty);
                prm.Add("@Mobile1", p.Mobile1 ?? string.Empty);
                prm.Add("@zonecode", p.zonecode ?? 0);
                prm.Add("@Send_sms", p.Send_sms ?? false);

                prm.Add("@AGE", p.AGE ?? 0);
                prm.Add("@birthdate", p.birthdate);
                prm.Add("@SEX", p.SEX ?? string.Empty);
                prm.Add("@OCCU", p.OCCU ?? 0);
                prm.Add("@Family_code", p.Family_code ?? 0);
                prm.Add("@MEMBER_NR", p.MEMBER_NR ?? string.Empty);
                prm.Add("@MEMBER_NO", p.MEMBER_NO ?? 0);

                prm.Add("@ST_DIR", p.ST_DIR ?? "O");
                prm.Add("@Ref_STDIR", p.Ref_STDIR ?? 0);

                prm.Add("@AdharNo", p.AdharNo ?? string.Empty);
                prm.Add("@voteridno", p.voteridno ?? string.Empty);
                prm.Add("@passportno", p.passportno ?? string.Empty);
                prm.Add("@passexpdate", p.passexpdate);
                prm.Add("@passauth", p.passauth ?? string.Empty);
                prm.Add("@otherid", p.otherid ?? string.Empty);
                prm.Add("@Driving_License", p.Driving_License ?? string.Empty);
                prm.Add("@Driving_License_ExpDate", p.Driving_License_ExpDate);
                prm.Add("@rationno", p.rationno ?? string.Empty);

                prm.Add("@FATHERNAME", p.FATHERNAME ?? string.Empty);
                prm.Add("@officename", p.officename ?? string.Empty);
                prm.Add("@OFFICEADDR1", p.OFFICEADDR1 ?? string.Empty);
                prm.Add("@OFFICEADDR2", p.OFFICEADDR2 ?? string.Empty);
                prm.Add("@OFFICEADDR3", p.OFFICEADDR3 ?? string.Empty);

                prm.Add("@OFFICEPIN", p.OFFICEPIN ?? 0);
                prm.Add("@OFFICEPHONE", p.OFFICEPHONE ?? string.Empty);
                prm.Add("@OFFICEPHONE1", p.OFFICEPHONE1 ?? string.Empty);
                prm.Add("@EMAIL_ID", p.EMAIL_ID ?? string.Empty);

                prm.Add("@Name_Bneficiary", p.Name_Bneficiary ?? string.Empty);
                prm.Add("@AccountNo_Bneficiary", p.AccountNo_Bneficiary ?? string.Empty);
                prm.Add("@IFSCODE_Bneficiary", p.IFSCODE_Bneficiary ?? string.Empty);
                prm.Add("@BankName_Bneficiary", p.BankName_Bneficiary ?? string.Empty);
                prm.Add("@BrName_Bneficiary", p.BrName_Bneficiary ?? string.Empty);

                prm.Add("@COMPREGNO", p.COMPREGNO ?? string.Empty);
                prm.Add("@COMPREGDT", p.COMPREGDT ?? string.Empty);
                prm.Add("@COMPBRANCH", p.COMPBRANCH ?? string.Empty);
                prm.Add("@COMPNATURE", p.COMPNATURE ?? string.Empty);
                prm.Add("@COMPPAIDCAPT", p.COMPPAIDCAPT ?? string.Empty);
                prm.Add("@COMPTURNOVER", p.COMPTURNOVER ?? 0);
                prm.Add("@COMPNETWORTH", p.COMPNETWORTH ?? 0);
                prm.Add("@Propritor1", p.Propritor1 ?? string.Empty);
                prm.Add("@Propritor2", p.Propritor2 ?? string.Empty);

                prm.Add("@Cast", p.Cast ?? 0);
                prm.Add("@Religon", p.Religon ?? 0);

                prm.Add("@KycAddrProof", p.KycAddrProof ?? false);
                prm.Add("@KycAddrProof_Code", p.KycAddrProof_Code ?? 0);
                prm.Add("@KycIdProof", p.KycIdProof ?? false);
                prm.Add("@KycIdProof_Code", p.KycIdProof_Code ?? 0);

                prm.Add("@KycIdProof_DocNo", p.KycIdProof_DocNo ?? string.Empty);
                prm.Add("@KycAddrProof_DocNo", p.KycAddrProof_DocNo ?? string.Empty);

                prm.Add("@opn_by", p.opn_by ?? string.Empty);
                prm.Add("@IP", p.IP ?? string.Empty);

                // SP OUTPUT
                prm.Add("@msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);

                long finalPartyCode;

                using (var con = _dapperContext.CreateConnection())
                {
                    await con.ExecuteAsync(query, prm, commandType: CommandType.StoredProcedure);

                    // READ OUTPUT
                    var msg = prm.Get<string>("@msg");

                    if (!long.TryParse(msg, out finalPartyCode) || finalPartyCode <= 0)
                    {
                        return "Error: PartyMaster not saved.";
                    }
                }

                // ----------------------------------------------------
                //  ONLY INSERT PHOTOS IF prtymast INSERT WAS SUCCESSFUL
                // ----------------------------------------------------
                foreach (var item in p.Pictures ?? Enumerable.Empty<DTOUploadPhotoSign>())
                {
                    if (string.IsNullOrWhiteSpace(item.Picture)) continue;

                    // Base64
                    var raw = item.Picture;
                    var idx = raw.IndexOf(',');
                    if (idx > 0) raw = raw[(idx + 1)..];
                    raw = raw.Replace("\r", "").Replace("\n", "").Trim();

                    byte[] photoBytes = Convert.FromBase64String(raw);

                    var photoprm = new DynamicParameters();
                    photoprm.Add("@flag1", 1);
                    photoprm.Add("@flag", item.flag);
                    photoprm.Add("@Party_Code", finalPartyCode);
                    photoprm.Add("@Picture", photoBytes, DbType.Binary, size: photoBytes.Length);

                    photoprm.Add("@Code1", 0);
                    photoprm.Add("@brnc_code", p.brnc_code);
                    photoprm.Add("@code2", 0);
                    photoprm.Add("@srno", item.srno ?? 1);
                    photoprm.Add("@scan_by", item.Scan_By ?? 0);
                    photoprm.Add("@Shr_code1", 0);
                    photoprm.Add("@shr_code2", 0);
                    photoprm.Add("@Doc_No", 0);
                    photoprm.Add("@Description", "");
                    photoprm.Add("@Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    using (var con2 = _photodapperContext.CreateConnection())
                    {
                        await con2.ExecuteAsync(photoquery, photoprm, commandType: CommandType.StoredProcedure);
                    }
                }

                return finalPartyCode.ToString();
            }
            catch (SqlException ex)
            {
                return "SQL ERROR: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.Message;
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
