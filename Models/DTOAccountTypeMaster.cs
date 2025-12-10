using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DTOAccountTypeMaster
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Allow_Mobile_App { get; set; }
        public string Allow_Mobile_App_Trn { get; set; }
        public string AdharCard { get; set; }
        public string PanCard { get; set; }
        public string GST { get; set; }
    }
}
