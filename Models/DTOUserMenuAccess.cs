// Models/DTOMenuMasterItem.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models
{
    public class DTOUserMenuAccess
    {
        public long UserGrad { get; set; }          // UserGrade.Code
        public int ProgrameId { get; set; } = 1;    // Programe_ID (default 1 like old page)
        public List<long> SelectedMenuIds { get; set; } = new();  // MenuIDs with Show_YN = 'Y'
        public string? user_img { get; set; }
    }
    public class DTOUserMenuAccessMultiple
    {
        // Matches TS field "selectedusergradelist" (case-insensitive binding),
        // attribute ensures perfect mapping if you prefer exact name.
        [JsonPropertyName("selectedusergradelist")]
        public List<long> SelectedUserGradeList { get; set; } = new();

        public int ProgrameId { get; set; } = 1;

        public List<long> SelectedMenuIds { get; set; } = new();
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
        public string PageName { get; set; }
        public string ComponentKey { get; set; }
        public string Title { get; set; }

        public int ProgrameId { get; set; }
        public int viewId { get; set; }
        public long MainMenuId { get; set; }

        public int SeqNo1 { get; set; }
        public int SeqNo2 { get; set; }
        public int SeqNo3 { get; set; }
        public int SeqNo4 { get; set; }
        public int SeqNo5 { get; set; }
    }
}
