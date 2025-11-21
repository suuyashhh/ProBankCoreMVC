using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class CityMasterRepository : ICityMaster
    {
        private readonly DapperContext _dapperContext;

        public CityMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOCityMaster>> GetAllCity()
        {
            const string query = @"SELECT ID, Name FROM CityMast";
            try
            {
                using var con = _dapperContext.CreateConnection();
                var result = await con.QueryAsync<DTOCityMaster>(query);
                return result.ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTOCityMaster> GetCityById(DTOCityMaster objList)
        {
            const string query = @"
               SELECT 
                    city.Code       AS CITY_CODE,
                    city.CountryCode AS COUNTRY_CODE,
                    city.StateCode AS STATE_CODE,
                    city.DistCode AS DIST_CODE,
                    city.TalukaCode TALUKA_CODE,
                    city.Name       AS CITY_NAME,
                    city.PinCode    AS pin,
                	 c.Name AS COUNTRY_NAME,
                	 s.Name AS STATE_NAME,
                	 d.Name AS DIST_NAME,
                	 t.name AS TALUKA_NAME

                FROM CityMast  as city
                join CountryMast AS c
                ON c.Code=city.CountryCode
                JOIN StateMast AS s
                ON s.Code=city.StateCode AND s.Country_Code=city.CountryCode
                JOIN DistrictMast AS d
                ON d.Code=city.DistCode AND d.Country_Code=city.CountryCode AND d.State_Code=city.StateCode
                JOIN talkmast AS t
                ON t.code=city.TalukaCode AND t.Country_Code=city.CountryCode AND t.State_Code=city.StateCode AND t.Dist_code=city.DistCode

                WHERE  
                    city.CountryCode = @CountryCode AND
                    city.StateCode   = @StateCode AND
                    city.DistCode    = @DistCode AND
                    city.TalukaCode  = @TalukaCode AND
                    city.Code        = @Code;";

            using var conn = _dapperContext.CreateConnection();
            var result = await conn.QueryFirstOrDefaultAsync<DTOCityMaster>(query, new
            {
                CountryCode = objList.COUNTRY_CODE,
                StateCode = objList.STATE_CODE,
                DistCode = objList.DIST_CODE,
                TalukaCode = objList.TALUKA_CODE,
                Code = objList.CITY_CODE
            });

            return result;
        }

        public async Task Save(DTOCityMaster objList)
        {
            const string query = @"
                INSERT INTO CityMast
                (Code, CountryCode, StateCode, DistCode, TalukaCode, Name, PinCode, Entry_Date)
                VALUES (@Code, @CountryCode, @StateCode, @DistCode, @TalukaCode, @Name, @PinCode, @Entry_Date);";

            try
            {
                long newCode = await GenerateCityCode(
                    objList.COUNTRY_CODE,
                    objList.STATE_CODE,
                    objList.DIST_CODE,
                    objList.TALUKA_CODE
                );

                using var con = _dapperContext.CreateConnection();
                await con.ExecuteAsync(query, new
                {
                    Code = newCode,
                    CountryCode = objList.COUNTRY_CODE,
                    StateCode = objList.STATE_CODE,
                    DistCode = objList.DIST_CODE,
                    TalukaCode = objList.TALUKA_CODE,
                    Name = objList.CITY_NAME,
                    PinCode = objList.pin,
                    Entry_Date = objList.Entry_Date
                });

                // optionally, set the DTO CITY_CODE so controller can return it
                objList.CITY_CODE = (int)newCode;
            }
            catch
            {
                throw;
            }
        }

        public async Task Update(DTOCityMaster objList)
        {
            const string query = @"
                UPDATE CityMast
                SET
                    CountryCode = @CountryCode,
                    StateCode   = @StateCode,
                    DistCode    = @DistCode,
                    TalukaCode  = @TalukaCode,
                    Name        = @Name,
                    PinCode     = @PinCode,
                    Entry_Date  = @Entry_Date
                WHERE
                    Code = @Code
                    AND CountryCode = @CountryCode
                    AND StateCode   = @StateCode
                    AND DistCode    = @DistCode
                    AND TalukaCode  = @TalukaCode;";

            try
            {
                using var con = _dapperContext.CreateConnection();
                await con.ExecuteAsync(query, new
                {
                    Code = objList.CITY_CODE,
                    CountryCode = objList.COUNTRY_CODE,
                    StateCode = objList.STATE_CODE,
                    DistCode = objList.DIST_CODE,
                    TalukaCode = objList.TALUKA_CODE,
                    Name = objList.CITY_NAME,
                    PinCode = objList.pin,
                    Entry_Date = objList.Entry_Date
                });
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(DTOCityMaster objList)
        {
            const string query = @"
                DELETE FROM CityMast
                WHERE Code = @Code
                  AND CountryCode = @CountryCode
                  AND StateCode = @StateCode
                  AND DistCode = @DistCode
                  AND TalukaCode = @TalukaCode;";

            try
            {
                using var con = _dapperContext.CreateConnection();
                var rows = await con.ExecuteAsync(query, new
                {
                    Code = objList.CITY_CODE,
                    CountryCode = objList.COUNTRY_CODE,
                    StateCode = objList.STATE_CODE,
                    DistCode = objList.DIST_CODE,
                    TalukaCode = objList.TALUKA_CODE
                });

                return rows > 0;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<DTOCityMaster>> GetAllDependencies()
        {
            const string query = @"
                SELECT
                    c.PinCode AS pin,
                    c.Code        AS CITY_CODE,
                    c.Name        AS CITY_NAME,
                    t.Code        AS TALUKA_CODE,
                    t.Name        AS TALUKA_NAME,
                    d.Code        AS DIST_CODE,
                    d.Name        AS DIST_NAME,
                    s.Code        AS STATE_CODE,
                    s.Name        AS STATE_NAME,
                    CTRY.Code     AS COUNTRY_CODE,
                    CTRY.Name     AS COUNTRY_NAME,
                    CONCAT(
                      RIGHT('000' + ISNULL(CAST(CTRY.Code AS VARCHAR(10)), '0'), 3),
                      RIGHT('000' + ISNULL(CAST(s.Code    AS VARCHAR(10)), '0'), 3),
                      RIGHT('00000' + ISNULL(CAST(d.Code    AS VARCHAR(10)), '0'), 5),
                      RIGHT('00000' + ISNULL(CAST(t.Code    AS VARCHAR(10)), '0'), 5),
                      RIGHT('00000' + ISNULL(CAST(c.Code    AS VARCHAR(10)), '0'), 5)
                    ) AS UniqCode
                FROM CityMast AS c
                LEFT JOIN talkmast   AS t    ON t.Code      = c.TalukaCode
                                          AND t.Dist_Code = c.DistCode
                                          AND t.State_Code= c.StateCode
                                          AND t.Country_Code = c.CountryCode
                LEFT JOIN DistrictMast AS d  ON d.Code      = c.DistCode
                                          AND d.State_Code = c.StateCode
                                          AND d.Country_Code = c.CountryCode
                LEFT JOIN StateMast    AS s  ON s.Code      = c.StateCode
                                          AND s.Country_Code = c.CountryCode
                LEFT JOIN CountryMast  AS CTRY ON CTRY.Code = c.CountryCode
                ORDER BY c.Name;

            ";

            try
            {
                using var connection = _dapperContext.CreateConnection();
                var cities = (await connection.QueryAsync<DTOCityMaster>(query)).ToList();
                return cities;
            }
            catch
            {
                throw;
            }
        }

        private async Task<long> GenerateCityCode(long countryCode, long stateCode, long distCode, long talukaCode)
        {
            const string query = @"
                SELECT TOP 1 Code 
                FROM CityMast
                WHERE CountryCode = @CountryCode
                  AND StateCode = @StateCode
                  AND DistCode = @DistCode
                  AND TalukaCode = @TalukaCode
                ORDER BY Code DESC;";

            using var conn = _dapperContext.CreateConnection();
            var lastId = await conn.ExecuteScalarAsync<long?>(query, new
            {
                CountryCode = countryCode,
                StateCode = stateCode,
                DistCode = distCode,
                TalukaCode = talukaCode
            });

            return (lastId ?? 0) + 1;
        }
    }
}
