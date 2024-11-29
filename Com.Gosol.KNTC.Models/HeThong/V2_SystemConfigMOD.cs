using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.HeThong
{
    public class V2_SystemConfigMOD
    {
        public int SystemConfigID { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public string Description { get; set; }
        public bool TrangThai { get; set; }
    }
    public class V2_SystemConfigMODADD
    {
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public string Description { get; set; }
        public bool TrangThai { get; set; }
    }
}
