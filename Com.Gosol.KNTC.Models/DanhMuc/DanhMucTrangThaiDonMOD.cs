using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucTrangThaiDonMOD : DanhMucTrangThaiDonThemMoiMOD
    {
        public int? TrangThaiDonID { get; set; }

    }
    public class DanhMucTrangThaiDonThemMoiMOD
    {
        public string TenTrangThaiDon { get; set; }
        public string MaTrangThaiDon { get; set; }
        public string GhiChu { get; set; }
        public bool TrangThai { get; set; }
    }
}
