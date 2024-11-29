using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class BaoCaoVuViecDongNguoiInfo
    {
        public String DonVi { get; set; }
        public int CoQuanID { get; set; }
        public int DonKN { get; set; }
        public int DonTC { get; set; }
        public int DonKNPA { get; set; }
        public int XLDKhieuKienDN { get; set; }
        public int CapID { get; set; }

        public String CssClass { get; set; }
        public int DonNhieuNguoiDungTen { get; set; }
    }

    public class ChiTietVuViecDongNguoiInfo
    {
        public string SoDon { get; set; }
        public DateTime NgayNhapDon { get; set; }
        public string NgayNhapDonStr { get; set; }
        public string TenChuDon { get; set; }
        public string DiaChi { get; set; }
        public string NoiDungDon { get; set; }
        public string TenLoaiKhieuTo { get; set; }
        public int CoQuanID { get; set; }
        public int CapID { get; set; }
        public int SoLuong { get; set; }
        public int STT { get; set; }
        public int NhomKNID { get; set; }
        public int KetQuaID { get; set; }
        public int XuLyDonID { get; set; }
        public int DonThuID { get; set; }
        public string TenHuongGiaiQuyet { get; set; }
        public string KetQuaID_Str { get; set; }
        public int HuongXuLyID { get; set; }
        public int StateID { get; set; }

    }
}
