using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucPhanTichKQMOD : DanhMucPhanTichKQThemMoiMOD
    {
        public int? PhanTichKQID { get; set; }

    }
    public class DanhMucPhanTichKQThemMoiMOD
    {
        public string MaPhanTichKQ { get; set; }
        public string TenPhanTichKQ { get; set; }
        public string GhiChu { get; set; }
        public bool TrangThai { get; set; }
    }
}
