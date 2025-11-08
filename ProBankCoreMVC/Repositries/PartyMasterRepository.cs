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

        public async Task SavePartyMast(DTOPartyMaster partymaster)
        {
            try
            {
                var query = "sp_master";
                using (var connection = _dapperContext.CreateConnection())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            //Int64 newPatientId = await GeneratePartyMasterCode(partymaster.COM_ID);

                            // Step 1: Save partymaster
                            //var parameters = new DynamicParameters();
                            //parameters.Add("@Action", QueryConstant.InsertCasePaper);
                            //parameters.Add("@TRN_NO", newPatientId);
                            //parameters.Add("@PATIENT_NAME", partymaster.PATIENT_NAME);
                            //parameters.Add("@GENDER", partymaster.GENDER);
                            //parameters.Add("@CON_NUMBER", partymaster.CON_NUMBER);
                            //parameters.Add("@ADDRESS", partymaster.ADDRESS);
                            //parameters.Add("@DOCTOR_CODE", partymaster.DOCTOR_CODE);
                            //parameters.Add("@DATE", partymaster.DATE);
                            //parameters.Add("@STATUS_CODE", partymaster.STATUS_CODE);
                            //parameters.Add("@TOTAL_AMOUNT", partymaster.TOTAL_AMOUNT);
                            //parameters.Add("@TOTAL_PROFIT", partymaster.TOTAL_PROFIT);
                            //parameters.Add("@DISCOUNT", partymaster.DISCOUNT);
                            //parameters.Add("@COM_ID", partymaster.COM_ID);
                            //parameters.Add("@PAYMENT_AMOUNT", partymaster.PAYMENT_AMOUNT);
                            //parameters.Add("@PAYMENT_METHOD", partymaster.PAYMENT_METHOD);
                            //parameters.Add("@COLLECTION_TYPE", partymaster.COLLECTION_TYPE);
                            //parameters.Add("@PAYMENT_STATUS", partymaster.PAYMENT_STATUS);
                            //parameters.Add("@CRT_BY", partymaster.CRT_BY);

                            //await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);

                            //if (partymaster.MatIs != null && partymaster.MatIs.Any())
                            //{
                            //    var testTable = new DataTable();
                            //    testTable.Columns.Add("TEST_CODE", typeof(Int64));
                            //    testTable.Columns.Add("TRN_NO", typeof(Int64));
                            //    testTable.Columns.Add("SR_NO", typeof(int));
                            //    testTable.Columns.Add("PRICE", typeof(decimal));
                            //    testTable.Columns.Add("LAB_PRICE", typeof(decimal));
                            //    testTable.Columns.Add("COM_ID", typeof(int));

                            //    int srNo = 1;
                            //    foreach (var test in partymaster.MatIs)
                            //    {
                            //        testTable.Rows.Add(
                            //            test.TEST_CODE,
                            //            newPatientId,
                            //            srNo++,
                            //            test.PRICE,
                            //            test.LAB_PRICE,
                            //            partymaster.COM_ID
                            //        );
                            //    }

                            //    var testParams = new DynamicParameters();
                            //    testParams.Add("@Action", "InsertCasePaperTests");
                            //    testParams.Add("@TestItems", testTable.AsTableValuedParameter("dbo.TestTableType"));

                            //    await connection.ExecuteAsync(query, testParams, transaction, commandType: CommandType.StoredProcedure);
                            //}

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //private async Task<Int64> GeneratePartyMasterCode(string comId)
        //{
        //    // Format: yyMMdd (e.g., 250507 for May 7, 2025)
        //    string datePart = DateTime.UtcNow.AddHours(5.5).ToString("yyMMdd", CultureInfo.InvariantCulture);
        //    string dateComboKey = datePart + comId;

        //    string query = "SELECT TOP 1 CODE FROM [prtymast] ORDER BY CODE DESC";

        //    using (var conn = _dapperContext.CreateConnection())
        //    {
        //        string lastId = await conn.ExecuteScalarAsync<string>(query, new { key = dateComboKey });

        //        int nextNum = 1;
        //        if (!string.IsNullOrEmpty(lastId))
        //        {
        //            // Extract the numeric suffix after date+comId (assumes 9-character prefix)
        //            if (int.TryParse(lastId.Substring(9), out int lastNum))
        //                nextNum = lastNum + 1;
        //        }

        //        // Final TRN_NO: date + comId + nextNum (e.g., 250507101)
        //        return long.Parse(dateComboKey + nextNum);
        //    }
        //}


    }
}
