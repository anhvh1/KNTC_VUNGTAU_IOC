using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class PhongBanInfo
    {
        public int PhongBanID { get; set; }
        public string TenPhongBan { get; set; }
        public string SoDienThoai { get; set; }
        public string GhiChu { get; set; }
        public int CoQuanID { get; set; }
        public string TenCoQuan { get; set; }

        public bool CQCheck { get; set; }
    }
}
