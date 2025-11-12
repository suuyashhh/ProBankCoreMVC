using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class CityMasterRepository : ICityMaster
    {
        private readonly DapperContext _dapperContext;

        public CityMasterRepository(DapperContext dapperContext, ICountryMaster countryMastRepo, IStateMaster stateMasterRepo, IDistrictMaster distMasterRepo, ITalukaMaster talukaMasterRepo, IAreaMaster areaMasterRepo)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOCityMaster>> GetAllCity()
        {
            var query = @"
                            select ID,Name from CityMast";
            try
            {
                using (var con = _dapperContext.CreateConnection())
                {
                    var result=await con.QueryAsync<DTOCityMaster>(query);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

//        public async Task<IEnumerable<DTOCityMaster>> save(int CountryCode, int StateCode, int DistCode, int TalukaCode, int Code, string Name, int PinCode, string Entry_Date)
//        {
//            var query = @"
//                        insert Into CityMast
//                        (ID,CountryCode,StateCode,DistCode,TalukaCode,Code,Name,PinCode,Entry_Date)
//                        values()
//";
//            try
//            {
//                using(var con = _dapperContext.CreateConnection())
//                {
//                    var result= await con.QueryAsync<DTOCityMaster>(query, new { CountryCode = countryCode, StateCode= tateCode })
//                }
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

        public async Task<IEnumerable<DTOCityMaster>> GetAllDependencies()
        {
            const string query = @"
            SELECT
                c.PinCode AS pin,
                c.Code        AS CITY_CODE,
                c.Name     AS CITY_NAME,
                t.code        AS TALUKA_CODE,
                t.name   AS TALUKA_NAME,
                d.Code        AS DIST_CODE,
                d.Name     AS DIST_NAME,
                s.Code        AS STATE_CODE,
                s.Name    AS STATE_NAME,
            	CTRY.Code     AS COUNTRY_CODE,
            	CTRY.Name AS COUNTRY_NAME
            FROM CityMast AS c
            LEFT JOIN talkmast AS t 
                ON t.code = c.TalukaCode AND T.Dist_code = C.DistCode AND t.State_Code= c.StateCode and t.Country_Code = c.CountryCode
            LEFT JOIN DistrictMast AS d 
                ON d.Code = C.DistCode AND d.State_Code= c.StateCode and d.Country_Code = c.CountryCode
            LEFT JOIN StateMast AS s 
                ON s.Code= c.StateCode and d.Country_Code = c.CountryCode
            LEFT JOIN CountryMast AS CTRY 
                ON CTRY.Code = c.CountryCode
            ORDER BY c.Name;
            ";

            try
            {
                using var connection = _dapperContext.CreateConnection();
                var cities = (await connection.QueryAsync<DTOCityMaster>(query)).ToList();
                return cities;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
