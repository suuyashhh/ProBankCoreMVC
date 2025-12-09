using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProBankCoreMVC.Repositries
{
    public class CommanMasterRepository : ICommanMaster
    {
        private readonly DapperContext _dapperContext;

        public CommanMasterRepository(DapperContext dapperContext)
        {

            _dapperContext = dapperContext;
        }


        public async Task<IEnumerable<DTOCommanMaster>> GetAllMaster(string tableName)
        {
            const string spName = "New_SP_Insert2FieldMaster";

            var parameters = new DynamicParameters();
            parameters.Add("@flag", 5);
            parameters.Add("@TblName", tableName);   // e.g. "StaffMast", "DDCITY"
            parameters.Add("@fld1", "CODE");        // assuming common design
            parameters.Add("@fld2", "NAME");

            // Not used for GetAll, but SP expects them – pass null
            parameters.Add("@fld1val", null, DbType.String, ParameterDirection.Input);
            parameters.Add("@fld2val", null, DbType.String, ParameterDirection.Input);

            parameters.Add("@Msg", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.CreateConnection())
            {
                var list = await connection.QueryAsync<DTOCommanMaster>(
                    spName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                // optional: read the message
                var msg = parameters.Get<string>("@Msg");
                foreach (var item in list)
                {
                    item.Msg = msg;
                }

                return list;
            }
        }


        public async Task<DTOCommanMaster> GetCommanMasterById(string tableName, int code)
        {
            const string spName = "New_SP_Insert2FieldMaster";

            var parameters = new DynamicParameters();
            parameters.Add("@flag", 3);
            parameters.Add("@TblName", tableName);     // <- from caller
            parameters.Add("@fld1", "CODE");          // common master design: key column
            parameters.Add("@fld2", "NAME");          // common master design: value column

            parameters.Add("@fld1val", code.ToString(), DbType.String, ParameterDirection.Input);
            parameters.Add("@fld2val", null, DbType.String, ParameterDirection.Input);

            parameters.Add("@Msg", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<DTOCommanMaster>(
                    spName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result != null)
                {
                    result.Msg = parameters.Get<string>("@Msg");
                }

                return result;
            }
        }




        //public async Task<IEnumerable<DTOStaffMaster>> GetStaff(int code)
        //{
        //    var query = @"SELECT code,name,Entry_Date  FROM StaffMast  where code = @code";
        //    try
        //    {
        //        using (var Connection = _dapperContext.CreateConnection())
        //        {
        //            var result = await Connection.QueryAsync<DTOStaffMaster>(query, new { code });
        //            return result;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task Save(string tblName, string name)
        {
            //var query = @"SELECT code,name,Entry_Date  FROM StaffMast";

            try
            {


                var query = "New_SP_Insert2FieldMaster";
                var parameters = new DynamicParameters();
                parameters.Add("@flag", 1);
                parameters.Add("@TblName", tblName);
                parameters.Add("@fld1", "CODE");
                parameters.Add("@fld2", "NAME");
                //parameters.Add("@fld1val",objComman.fld1val);
                parameters.Add("@fld2val", name);
                parameters.Add("@Msg", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);


                using (var connection = _dapperContext.CreateConnection())
                {

                    await connection.QueryAsync<DTOCommanMaster>(query, parameters, commandType: CommandType.StoredProcedure);


                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateCommanMaster(string tableName, int code, string name)
        {
            const string spName = "New_SP_Insert2FieldMaster";

            var parameters = new DynamicParameters();
            parameters.Add("@flag", 2);
            parameters.Add("@TblName", tableName);          // e.g. "StaffMast", "DDCITY"
            parameters.Add("@fld1", "CODE");               // key column name
            parameters.Add("@fld2", "NAME");               // value/description column name

            // Code used only in WHERE; user cannot change it (UI should keep it read-only)
            parameters.Add("@fld1val", code.ToString(), DbType.String, ParameterDirection.Input);

            // New value to update
            parameters.Add("@fld2val", name, DbType.String, ParameterDirection.Input);

            // Output message from SP
            parameters.Add("@Msg", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.CreateConnection())
            {
                await connection.ExecuteAsync(
                    spName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var msg = parameters.Get<string>("@Msg");
                // you can log or handle msg if you want ("2" = updated successfully, as per SP)
            }
        }


        public async Task DeleteCommanMaster(string tableName, int code)
        {
            const string spName = "New_SP_Insert2FieldMaster";

            var parameters = new DynamicParameters();
            parameters.Add("@flag", 4);
            parameters.Add("@TblName", tableName);   // e.g. "StaffMast", "DDCITY"
            parameters.Add("@fld1", "CODE");        // key column
            parameters.Add("@fld2", "NAME");        // value column (not used in delete, but SP expects it)

            parameters.Add("@fld1val", code.ToString(), DbType.String, ParameterDirection.Input);
            parameters.Add("@fld2val", null, DbType.String, ParameterDirection.Input);

            parameters.Add("@Msg", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.CreateConnection())
            {
                await connection.ExecuteAsync(
                    spName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var msg = parameters.Get<string>("@Msg");
                // Optionally inspect msg ("5" for delete) or log it
            }
        }




    }
}
