using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using System.Data;

namespace ProBankCoreMVC.Repositries
{
    public class ThreeFieldMasterRepository : IThreeFieldMaster
    {
        private readonly DapperContext _dapperContext;

        public ThreeFieldMasterRepository(DapperContext dapperContext)
        {

            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOThreeFieldMaster>> GetAllThreeFieldMaster(string tableName)
        {
            const string spName = "SP_Insert3FieldMaster";

            var parameters = new DynamicParameters();
            parameters.Add("@flag", 5);
            parameters.Add("@TblName", tableName);
            parameters.Add("@fld1", "CODE");
            parameters.Add("@fld2", "NAME");
            parameters.Add("@fld3", "ENTRY_DATE");

            // not used for GetAll
            parameters.Add("@fld1val", null, DbType.String, ParameterDirection.Input);
            parameters.Add("@fld2val", null, DbType.String, ParameterDirection.Input);
            parameters.Add("@fld3val", null, DbType.String, ParameterDirection.Input);

            parameters.Add("@Msg", dbType: DbType.String, size: 80, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.CreateConnection())
            {
                var list = (await connection.QueryAsync<DTOThreeFieldMaster>(
                    spName,
                    parameters,
                    commandType: CommandType.StoredProcedure)).ToList();

                var msg = parameters.Get<string>("@Msg");
                foreach (var item in list)
                    item.Msg = msg;

                return list;
            }
        }

        public async Task<DTOThreeFieldMaster?> GetThreeFieldMasterById(string tableName, int code)
        {
            const string spName = "SP_Insert3FieldMaster";

            var parameters = new DynamicParameters();
            parameters.Add("@flag", 3);
            parameters.Add("@TblName", tableName);
            parameters.Add("@fld1", "CODE");
            parameters.Add("@fld2", "NAME");
            parameters.Add("@fld3", "ENTRY_DATE");

            parameters.Add("@fld1val", code.ToString(), DbType.String, ParameterDirection.Input);
            parameters.Add("@fld2val", null, DbType.String, ParameterDirection.Input);
            parameters.Add("@fld3val", null, DbType.String, ParameterDirection.Input);

            parameters.Add("@Msg", dbType: DbType.String, size: 80, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<DTOThreeFieldMaster>(
                    spName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result != null)
                    result.Msg = parameters.Get<string>("@Msg");

                return result;
            }
        }

        public async Task Save(DTOThreeFieldMaster obj)
        {
            const string spName = "SP_Insert3FieldMaster";

            var parameters = new DynamicParameters();

            parameters.Add("@flag", obj.flag);                    // 1=Insert, 2=Update, 4=Delete etc.
            parameters.Add("@TblName", obj.TblName);
            parameters.Add("@fld1", obj.fld1 ?? "CODE");
            parameters.Add("@fld2", obj.fld2 ?? "NAME");
            parameters.Add("@fld3", obj.fld3 ?? "ENTRY_DATE");

            // fld1val is OUTPUT now (for insert you usually pass null in, SP will generate it)
            parameters.Add("@fld1val", obj.fld1val, DbType.String, ParameterDirection.InputOutput);
            parameters.Add("@fld2val", obj.fld2val, DbType.String, ParameterDirection.Input);
            parameters.Add("@fld3val", obj.fld3val, DbType.String, ParameterDirection.Input);

            parameters.Add("@Msg", dbType: DbType.String, size: 80, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.CreateConnection())
            {
                await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);

                // read back generated code and message
                obj.fld1val = parameters.Get<string>("@fld1val");   // new CODE when flag=1
                obj.Msg = parameters.Get<string>("@Msg");
            }
        }

        // UPDATE
        public async Task<DTOThreeFieldMaster> Update(DTOThreeFieldMaster dto)
        {
            const string spName = "SP_Insert3FieldMaster";

            var parameters = new DynamicParameters();
            parameters.Add("@flag", 2); // update
            parameters.Add("@TblName", dto.TblName ?? throw new ArgumentNullException(nameof(dto.TblName)));
            parameters.Add("@fld1", dto.fld1 ?? "CODE");
            parameters.Add("@fld2", dto.fld2 ?? "NAME");
            parameters.Add("@fld3", dto.fld3 ?? "ENTRY_DATE");

            // fld1val must be the key value to update
            parameters.Add("@fld1val", dto.fld1val, DbType.String, ParameterDirection.Input);
            parameters.Add("@fld2val", dto.fld2val, DbType.String, ParameterDirection.Input); // new name
            parameters.Add("@fld3val", dto.fld3val, DbType.String, ParameterDirection.Input); // optional new entry date

            parameters.Add("@Msg", dbType: DbType.String, size: 80, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.CreateConnection())
            {
                await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);

                dto.Msg = parameters.Get<string>("@Msg");
                return dto;
            }
        }

        // DELETE
        public async Task<DTOThreeFieldMaster> Delete(string tableName, string code)
        {
            const string spName = "SP_Insert3FieldMaster";

            var parameters = new DynamicParameters();
            parameters.Add("@flag", 4); // delete
            parameters.Add("@TblName", tableName ?? throw new ArgumentNullException(nameof(tableName)));
            parameters.Add("@fld1", "CODE");         // assumption: key column name
            parameters.Add("@fld2", "NAME");         // not used for delete
            parameters.Add("@fld3", "ENTRY_DATE");   // not used for delete

            parameters.Add("@fld1val", code, DbType.String, ParameterDirection.Input);
            parameters.Add("@fld2val", null, DbType.String, ParameterDirection.Input);
            parameters.Add("@fld3val", null, DbType.String, ParameterDirection.Input);

            parameters.Add("@Msg", dbType: DbType.String, size: 80, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.CreateConnection())
            {
                await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);

                var result = new DTOThreeFieldMaster
                {
                    TblName = tableName,
                    fld1 = "CODE",
                    fld1val = code,
                    Msg = parameters.Get<string>("@Msg")
                };

                return result;
            }
        }


    }
}
