using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucChucVuMOD : DanhMucChucVuThemMoiMOD
    {
        public int? ChucVuID { get; set; }
    }

    public class DanhMucChucVuThemMoiMOD
    {
        public string MaChucVu { get; set; }
        public string TenChucVu { get; set; }
        public string GhiChu { get; set; }
        public bool TrangThai { get; set; }
    }
}
