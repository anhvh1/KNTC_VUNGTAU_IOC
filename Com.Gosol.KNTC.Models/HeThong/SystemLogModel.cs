using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.HeThong
{
    public class SystemLogModel
    {
        public int SystemLogid { get; set; }
        public int CanBoID { get; set; }
        public string LogInfo { get; set; }
        public DateTime LogTime { get; set; }
        public int LogType { get; set; }
        public string TenCanBo { get; set; }
    }
    public class SystemLogPartialModel : SystemLogModel
    {
        public string TenCoQuan { get; set; }
    }
    
    // V2
    public class NhatKyHeThongThamSo 
    {
        public string Keyword { get; set; } = "";
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public LogType? LogType { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class NhatKyHeThongModel
    {
        public int SystemLogID { get; set; }
        public int CanBoID { get; set; }
        public string LogInfo { get; set; }
        public DateTime LogTime { get; set; }
        public int LogType { get; set; }
        public string TenCanBo { get; set; }
    }

    public enum LogType
    {
        Create = 1,
        Edit = 2,
        Delete = 3,
        Other = 4
    }
}
