// ProBankCoreMVC/Repositries/UserMenuAccessRepository.cs
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class UserMenuAccessRepository : IUserMenuAccess
    {
        private readonly DapperContext _dapperContext;

        public UserMenuAccessRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOUserGrade>> GetUserGradesAsync()
        {
            const string query = @"SELECT Code, Name 
                                   FROM UserGrade 
                                   ORDER BY Code";

            using (var conn = _dapperContext.CreateConnection())
            {
                var result = await conn.QueryAsync<DTOUserGrade>(query);
                return result;
            }
        }

        public async Task<IEnumerable<DTOMenuMasterItem>> GetMenuMasterAsync(int programeId)
        {
            const string query = @"
                SELECT 
                    Menu_ID     AS MenuId,
                    Menu_Name   AS MenuName,
                    Programe_ID AS ProgrameId,
                    Main_Menu_ID AS MainMenuId,
                    ISNULL(Seq_No1, 0) AS SeqNo1,
                    ISNULL(Seq_No2, 0) AS SeqNo2,
                    ISNULL(Seq_No3, 0) AS SeqNo3,
                    ISNULL(Seq_No4, 0) AS SeqNo4,
                    ISNULL(Seq_No5, 0) AS SeqNo5
                FROM MenuMaster
                WHERE Programe_ID = @ProgrameId
                ORDER BY Main_Menu_ID, Seq_No1, Seq_No2, Seq_No3, Seq_No4, Seq_No5";

            using (var conn = _dapperContext.CreateConnection())
            {
                var result = await conn.QueryAsync<DTOMenuMasterItem>(query, new { ProgrameId = programeId });
                return result;
            }
        }
        public async Task<IEnumerable<long>> GetSelectedMenuIdsAsync(long userGrad, int programeId)
        {
            const string query = @"
                SELECT MenuID 
                FROM UserGradeDefaultMenu
                WHERE Programe_ID = @ProgrameId 
                  AND UserGrad = @UserGrad
                  AND Show_YN = 'Y'";

            using (var conn = _dapperContext.CreateConnection())
            {
                var result = await conn.QueryAsync<long>(query, new { ProgrameId = programeId, UserGrad = userGrad });
                return result;
            }
        }

        public async Task SaveUserMenuAccessAsync(DTOUserMenuAccess model)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        const string deleteQuery = @"
                            DELETE FROM UserGradeDefaultMenu
                            WHERE Programe_ID = @ProgrameId 
                              AND UserGrad = @UserGrad";

                        await conn.ExecuteAsync(deleteQuery,
                            new { ProgrameId = model.ProgrameId, UserGrad = model.UserGrad },
                            transaction: tran);

                        const string menuQuery = @"
                            SELECT Menu_ID 
                            FROM MenuMaster
                            WHERE Programe_ID = @ProgrameId";

                        var allMenuIds = (await conn.QueryAsync<long>(menuQuery,
                            new { ProgrameId = model.ProgrameId },
                            transaction: tran)).ToList();

                        var selectedSet = new HashSet<long>(model.SelectedMenuIds ?? Enumerable.Empty<long>());

                        const string insertQuery = @"
                            INSERT INTO UserGradeDefaultMenu (UserGrad, Programe_ID, MenuID, Show_YN)
                            VALUES (@UserGrad, @ProgrameId, @MenuId, @ShowYN)";

                        foreach (var menuId in allMenuIds)
                        {
                            var showYn = selectedSet.Contains(menuId) ? "Y" : "N";

                            await conn.ExecuteAsync(insertQuery,
                                new
                                {
                                    UserGrad = model.UserGrad,
                                    ProgrameId = model.ProgrameId,
                                    MenuId = menuId,
                                    ShowYN = showYn
                                },
                                transaction: tran);
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
