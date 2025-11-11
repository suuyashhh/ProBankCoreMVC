using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class CityMasterRepository : ICityMaster
    {
        private readonly DapperContext _dapperContext;
        private readonly ICountryMaster _countryMastRepo;
        private readonly IStateMaster _stateMasterRepo;
        private readonly IDistrictMaster _distMasterRepo;
        private readonly ITalukaMaster _talukaMasterRepo;
        private readonly IAreaMaster _areaMasterRepo;

        public CityMasterRepository(DapperContext dapperContext, ICountryMaster countryMastRepo,IStateMaster stateMasterRepo, IDistrictMaster distMasterRepo, ITalukaMaster talukaMasterRepo, IAreaMaster areaMasterRepo)
        {
            _dapperContext = dapperContext;
            _countryMastRepo = countryMastRepo;
            _stateMasterRepo = stateMasterRepo;
            _distMasterRepo = distMasterRepo;
            _talukaMasterRepo = talukaMasterRepo;
            _areaMasterRepo = areaMasterRepo;
        }       


        public async Task<IEnumerable<DTOCityMaster>> GetAllDependencies()
        {
            const string query = @"
SELECT
    c.TRN_NO        AS CITY_CODE,
    c.CITY_NAME     AS CITY_NAME,
    t.TRN_NO        AS TALUKA_CODE,
    t.TALUKA_NAME   AS TALUKA_NAME,
    d.TRN_NO        AS DIST_CODE,
    d.DIST_NAME     AS DIST_NAME,
    s.TRN_NO        AS STATE_CODE,
    s.STATE_NAME    AS STATE_NAME,
	CTRY.TRN_NO     AS COUNTRY_CODE,
	CTRY.COUNTRY_NAME AS COUNTRY_NAME
FROM DB_A36730_ayushconstruction.dbo.MST_CITY AS c
LEFT JOIN DB_A36730_ayushconstruction.dbo.MST_TALUKA AS t 
    ON t.TRN_NO = c.TALUKA_CODE
LEFT JOIN DB_A36730_ayushconstruction.dbo.MST_DISTICT AS d 
    ON d.TRN_NO = t.DIST_CODE
LEFT JOIN DB_A36730_ayushconstruction.dbo.MST_STATE AS s 
    ON s.TRN_NO = d.STATE_CODE
LEFT JOIN DB_A36730_ayushconstruction.dbo.MST_COUNTRY AS CTRY 
    ON CTRY.TRN_NO = s.COUNTRY_CODE
ORDER BY c.CITY_NAME;
;
";

            try
            {
                using var connection = _dapperContext.CreateConnection();
                // Single query returns all columns mapped into DTOCityMaster by name
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
