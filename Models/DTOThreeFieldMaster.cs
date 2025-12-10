using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DTOThreeFieldMaster
    {
        // Inputs used to call the SP
        public int flag { get; set; }
        public string TblName { get; set; }
        public string fld1 { get; set; }        // column name e.g. "CODE"
        public string fld2 { get; set; }        // column name e.g. "NAME"
        public string fld3 { get; set; }        // column name e.g. "ENTRY_DATE"

        public string fld1val { get; set; }     // input value for CODE (used for get-by-id/update/delete)
        public string fld2val { get; set; }     // input value for NAME (insert/update)
        public string fld3val { get; set; }     // input value for ENTRY_DATE (insert/update)

        // Output columns returned by SP (these must match SP aliases)
        public string Name { get; set; }           // maps to [Name] returned by SP
        public DateTime? EntryDate { get; set; }   // maps to [EntryDate] returned by SP
        public string Code { get; set; }           // maps to [Code] returned by SP

        // message output
        public string Msg { get; set; }
    }



}
