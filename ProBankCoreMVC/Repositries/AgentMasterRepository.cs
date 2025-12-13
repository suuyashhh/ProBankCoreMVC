using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class AgentMasterRepository : IAgentMaster
    {
        private readonly DapperContext _dapperContext;

        public AgentMasterRepository(DapperContext dapperContext)
        {

            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOAgentMaster>> GetAllAgent()
        {
            var query = @"SELECT ID,brnc_code, code, NAME, code1, lockflag, drcode1, PrintYn, AuthHour, NextAcc, NextRect, SbGlc, SbSlc, MaxAMount, TRF_CODE1, TRF_CODE2, Machine_type, MNAME, RINT_RATE, LINT_RATE, TDSglc, TDSslc, NoOfHolidays, PASSWORD, MobileNo, RadyToCash, Pyg_Amt_Digit, OLDCode, Entry_Date, Party_Code, Join_date, Resign_Date,Active_YN, mob_no, OTP, OTP_ValidTime, auth,Coll_Start_Date,Coll_End_Date FROM agntmast;";
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTOAgentMaster>(query);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> GetAgentById(int ID)
        {
            var query = @"SELECT ID, brnc_code, code, Machine_type, Pyg_Amt_Digit, NoOfHolidays, 
                        Party_Code, OTP, NAME, PASSWORD, MobileNo, RadyToCash, OLDCode, 
                        Active_YN, mob_no, auth, Coll_Start_Date, Coll_End_Date, 
                        OTP_ValidTime, Resign_Date, Join_date, Entry_Date 
                  FROM agntmast 
                  WHERE ID = @ID";
            try
            {
                using (var Connection = _dapperContext.CreateConnection())
                {
                    var result = await Connection.QueryFirstOrDefaultAsync<DTOAgentMaster>(query, new { ID });
                    return new
                    {
                        result.ID,
                        result.brnc_code,
                        result.code,
                        result.Machine_type,
                        result.Pyg_Amt_Digit,
                        result.NoOfHolidays,
                        result.Party_Code,
                        result.OTP,
                        result.NAME,
                        result.PASSWORD,
                        result.MobileNo,
                        result.RadyToCash,
                        result.OLDCode,
                        result.Active_YN,
                        result.mob_no,
                        result.auth,
                        result.Coll_Start_Date,
                        result.Coll_End_Date,
                        result.OTP_ValidTime,
                        result.Resign_Date,
                        result.Join_date,
                        result.Entry_Date
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<long> Save(DTOAgentMaster Agent)
        {
            const string query = @"
        INSERT INTO agntmast
        (brnc_code, code, Machine_type, Party_Code, NAME, MobileNo, Active_YN, Resign_Date, Join_date)
        VALUES
        (@brnc_code, @code, @Machine_type, @Party_Code, @NAME, @MobileNo, @Active_YN, @Resign_Date, @Join_date)";

            long newID = await GenerateAgentCode(Agent.brnc_code);

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    brnc_code = Agent.brnc_code,
                    code = newID,
                    Machine_type = Agent.Machine_type,
                    Party_Code = Agent.Party_Code,
                    NAME = Agent.NAME,
                    MobileNo = Agent.MobileNo,
                    Active_YN = Agent.Active_YN,
                    Resign_Date = Agent.Resign_Date,
                    Join_date = Agent.Join_date
                });
            }

            
            return newID;
        }



        public async Task Update(DTOAgentMaster Agent)
        {
            const string query = @"
            UPDATE agntmast
            SET
                Machine_type   = @Machine_type,
                MobileNo       = @MobileNo,
                Active_YN      = @Active_YN,
                Resign_Date    = @Resign_Date,
                Join_date      = @Join_date
            WHERE ID = @ID";


            try
            {
                using (var conn = _dapperContext.CreateConnection())
                {
                    await conn.ExecuteAsync(query, new
                    {
                        ID = Agent.ID,
                        brnc_code = Agent.brnc_code,
                        code = Agent.code,
                        Machine_type = Agent.Machine_type,
                        Pyg_Amt_Digit = Agent.Pyg_Amt_Digit,
                        NoOfHolidays = Agent.NoOfHolidays,
                        Party_Code = Agent.Party_Code,
                        OTP = Agent.OTP,
                        NAME = Agent.NAME,
                        PASSWORD = Agent.PASSWORD,
                        MobileNo = Agent.MobileNo,
                        RadyToCash = Agent.RadyToCash,
                        OLDCode = Agent.OLDCode,
                        Active_YN = Agent.Active_YN,
                        auth = Agent.auth,
                        Coll_Start_Date = Agent.Coll_Start_Date,
                        Coll_End_Date = Agent.Coll_End_Date,
                        OTP_ValidTime = Agent.OTP_ValidTime,
                        Resign_Date = Agent.Resign_Date,
                        Join_date = Agent.Join_date,
                        Entry_Date = Agent.Entry_Date ?? DateTime.Now
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task Delete(int ID)
        {
            const string query = @"DELETE FROM agntmast  WHERE ID = @ID";

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new { ID = ID, });
            }
        }





        private async Task<long> GenerateAgentCode(int brnc_code)
        {
            const string query = @"select top 1 code from agntmast where brnc_code = @Branch_Code order by code Desc";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<long?>(query, new { Branch_Code = brnc_code });
                return (lastId ?? 0) + 1;
            }
        }







    }
}
