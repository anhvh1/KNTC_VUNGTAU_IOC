using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucQuocTichMOD : DanhMucQuocTichThemMoiMOD
    {
        public int? QuocTichID { get; set; }

    }
    public class DanhMucQuocTichThemMoiMOD
    {
        public string MaQuocTich { get; set; }
        public string TenQuocTich { get; set; }
        public string GhiChu { get; set; }
        public bool TrangThai { get; set; }
    }
}
