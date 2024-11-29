using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucThamQuyenMOD : DanhMucThamQuyenThemMoiMOD
    {
        public int? ThamQuyenID { get; set; }

    }
    public class DanhMucThamQuyenThemMoiMOD
    {
        public string MaThamQuyen { get; set; }
        public string TenThamQuyen { get; set; }
        public string GhiChu { get; set; }
        public bool TrangThai { get; set; }
    }
}
