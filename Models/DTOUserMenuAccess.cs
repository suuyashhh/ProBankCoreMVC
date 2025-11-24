// Models/DTOMenuMasterItem.cs
namespace Models
{
    public class DTOUserMenuAccess
    {
        public long UserGrad { get; set; }          // UserGrade.Code
        public int ProgrameId { get; set; } = 1;    // Programe_ID (default 1 like old page)
        public List<long> SelectedMenuIds { get; set; } = new();  // MenuIDs with Show_YN = 'Y'
    }
    public class DTOUserGrade
    {
        public long Code { get; set; }
        public string Name { get; set; }
    }
    public class DTOMenuMasterItem
    {
        public long MenuId { get; set; }
        public string MenuName { get; set; }

        public int ProgrameId { get; set; }
        public long MainMenuId { get; set; }

        public int SeqNo1 { get; set; }
        public int SeqNo2 { get; set; }
        public int SeqNo3 { get; set; }
        public int SeqNo4 { get; set; }
        public int SeqNo5 { get; set; }
    }
}
