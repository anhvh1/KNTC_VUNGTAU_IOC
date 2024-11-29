using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class ThongKeInfo
    {
        public string STT { get; set; }
        public string Ten { get; set; }
        public int SoLuong { get; set; }
        public double TyLe { get; set; }
        public int Level { get; set; }
        public int Cap { get; set; }

        public string TenChuDon { get; set; }
        public string DiaChi { get; set; }
        public DateTime NgayNhapDon { get; set; }
        public DateTime NgayXuLy { get; set; }

        public int LoaiKhieuToID { get; set; }

        public int CoQuanID { get; set; }
        public int TinhID { get; set; }
        public int HuyenID { get; set; }
        public int CoQuanChuyenID { get; set; }
        public string TenCoQuanChuyen { get; set; }
        public int DonGianTiep { get; set; }
        public int DonTrucTiep { get; set; }
        public int TiepDanKhongDon { get; set; }
        public string TenLoaiKhieuTo { get; set; }
    }
}
