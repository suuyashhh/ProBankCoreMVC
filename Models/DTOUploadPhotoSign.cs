using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DTOUploadPhotoSign
    {

        /* PhotoSign DB*/

        public int? srno { get; set; }
        public int? Scan_By { get; set; }
        public int? Party_Code { get; set; }
        public string? Picture { get; set; }
        public string? flag { get; set; }
    }
}
