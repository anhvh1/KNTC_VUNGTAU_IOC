using Com.Gosol.KNTC.Models.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class LoaiKhieuToInfo
    {
        public int LoaiKhieuToID { get; set; }

        public string TenLoaiKhieuTo { get; set; }

        public int LoaiKhieuToCha { get; set; }

        public int Cap { get; set; }

        public int DonGianTiep { get; set; }

        public int DonTrucTiep { get; set; }

        public int TiepDanKhongDon { get; set; }
        public int Tong { get; set; }

        public int hasChild { get; set; }

        public bool SuDung { get; set; }
        public List<LoaiKhieuToInfo> Children { get; set; }
    }
}
