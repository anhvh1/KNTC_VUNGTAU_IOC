using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucLoaiKetQuaMOD : DanhMucLoaiKetQuaThemMoiMOD
    {
        public int? LoaiKetQuaID { get; set; }

    }
    public class DanhMucLoaiKetQuaThemMoiMOD
    {
        public string TenLoaiKetQua { get; set; }
        public string MaLoaiKetQua { get; set; }
        public string GhiChu { get; set; }
        public bool TrangThai { get; set; }
    }
}
