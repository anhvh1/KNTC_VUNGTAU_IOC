using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucDanTocMOD : DanhMucDanTocThemMoiMOD
    {
        public int? DanTocID { get; set; }

    }
    public class DanhMucDanTocThemMoiMOD
    {
        public string MaDanToc { get; set; }
        public string TenDanToc { get; set; }
        public string GhiChu { get; set; }
        public bool TrangThai { get; set; }
    }
}
