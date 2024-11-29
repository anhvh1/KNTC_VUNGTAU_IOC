using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class TiepCongDan_DanKhongDenInfo
    {
        public int? DanKhongDenID { get; set; }
        public int? CanBoID { get; set; }
        public int CoQuanID { get; set; }
        public string TenCanBo { get; set; }
        public int? NguoiTaoID { get; set; }
        public DateTime? NgayTruc { get; set; }
        public string NgayTrucStr { get; set; }
        public string ChucVu { get; set; }
    }
}
