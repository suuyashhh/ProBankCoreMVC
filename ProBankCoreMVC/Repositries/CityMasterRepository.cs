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

        public async Task<IEnumerable<DTOCityMaster>> GetAllCities()
        {
            var query = @"
                              select
                              Id As CITY_UNIC_ID,CODE As CITY_CODE,NAME As CITY_NAME  
                              from CityMast ";
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTOCityMaster>(query);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<DTOCityMaster> GetDependencyByCityId(int cityUnicId)
        {
            var query = @"
                SELECT
                    Id AS CITY_UNIC_ID,
                    CountryCode AS COUNTRY_CODE,
                    StateCode AS STATE_CODE,
                    DistCode AS DIST_CODE,
                    TalukaCode AS TALUKA_CODE,
                    Code AS CITY_CODE,
                    Name AS CITY_NAME,
                    PinCode AS PIN_CODE
                FROM CityMast
                WHERE Id = @cityUnicId";

            try
            {

                using (var connection = _dapperContext.CreateConnection())
                {
                    var city = await connection.QueryFirstOrDefaultAsync<DTOCityMaster>(query, new { cityUnicId });

                    var country = (await _countryMastRepo.GetCountryById(city.COUNTRY_CODE));
                    var state =(await _stateMasterRepo.GetStateById(city.COUNTRY_CODE,city.STATE_CODE));
                    var dist =(await _distMasterRepo.GetDistrictById(city.COUNTRY_CODE,city.STATE_CODE,city.DIST_CODE));
                    var talk =(await _talukaMasterRepo.GetTalukaById(city.COUNTRY_CODE,city.STATE_CODE,city.DIST_CODE,city.TALUKA_CODE));
                    var area =(await _areaMasterRepo.GetAreaById(city.COUNTRY_CODE,city.STATE_CODE,city.DIST_CODE,city.TALUKA_CODE,city.CITY_CODE));

                    city.COUNTRY_NAME = country.COUNTRY_NAME;
                    city.STATE_NAME = state.STATE_NAME;
                    city.DIST_NAME = dist.DIST_NAME;
                    city.TALUKA_NAME = talk.TALUKA_NAME;
                    city.AREA_NAME = area.AREA_NAME;
                    city.AREA_CODE = area.AREA_CODE;

                    return city;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }



    }
}
