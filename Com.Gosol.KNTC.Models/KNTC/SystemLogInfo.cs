using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class SystemLogInfo
    {
        public int SystemLogID { get; set; }
        public int CanBoID { get; set; }
        public string LogInfo { get; set; }
        public DateTime LogTime { get; set; }
        public int LogType { get; set; }

        public string TenCanBo { get; set; }
    }
}
