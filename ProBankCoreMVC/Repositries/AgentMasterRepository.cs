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

        public async Task<DTOStaffMaster> GetAgentById(int code)
        {
            var query = @"ID, brnc_code, code, Machine_type, Pyg_Amt_Digit, NoOfHolidays, Party_Code, OTP, NAME, PASSWORD, MobileNo, RadyToCash, OLDCode, Active_YN, mob_no, auth, Coll_Start_Date, Coll_End_Date, OTP_ValidTime, Resign_Date, Join_date, Entry_Date from agntmast where ID = @ID";

            try
            {
                using (var Connection = _dapperContext.CreateConnection())
                {
                    var result = await Connection.QueryFirstOrDefaultAsync<DTOStaffMaster>(query, new { code = code });

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }




    }
}
