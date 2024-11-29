using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucPhongBanMOD : DanhMucPhongBanThemMoiMOD
    {
        public int PhongBanID { get; set; }
    }

    public class DanhMucPhongBanThemMoiMOD
    {
        public string TenPhongBan { get; set; }
        public string SoDienThoai { get; set; }
        public string GhiChu { get; set; }
        public int CoQuanID { get; set; }
    }

    public class DanhSachPhongBanCoQuanID
    {
        public int CoQuanID { get; set; }
    }
}
