using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.Idics
{
    public class DanhMucThietBiModel
    {
        public string MachineId { get; set; }
        public string AgencyId { get; set; }
        public string StaticIP { get; set; }
        public string DynamicIP { get; set; }
        public bool? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditBy { get; set; }
        public string Location { get; set; }
        public string SecretKey { get; set; }
    }
}
