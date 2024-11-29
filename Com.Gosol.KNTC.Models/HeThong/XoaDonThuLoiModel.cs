using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.HeThong
{
    public class XoaDonThuLoiModel
    {

        public int? DonThuID { get; set; }
        public string SoDonThu { get; set; }
        public string TenCoQuan { get; set; }
        public string TenChuDon { get; set; }
        public string NoiDungDon { get; set; }

        public int KhieuToID { get; set; }

        public int CoQuanID { get; set; }
        public string TenLoaiKhieuTo { get; set; }
        public DateTime NgayNhapDon { get; set; }

    }

    public class ChiTietDonThuLoi
    {
        
        public string SoDonThu { get; set; }
        
        public string HoTen { get; set; }

        public int GioiTinh { get; set; }

        public int SoDienThoai { get; set; }

        public string TenDanToc { get; set; }

        public string DiaChi { get; set; }

        public string TenQuocTich { get; set; }

        public string CMND { get; set; }
        
        public string NoiDungDon { get; set; }
        public string NgayThuLy { get; set; }
        public int NhomKNID { get; set; }

        public string LyDo { get; set; }

        public string HanGiaiQuyet { get; set; }

        public int SoLan { get; set; }

        public int LanGiaiQuyet { get; set; }

        public string NoiDungHuongDan { get; set; }

        public string TenCanBoTiepNhan { get; set; }

        

        public string NguonDonDen { get; set; }
        
        public string TenTrangThaiDon { get; set; }
        public string TenCQChuyenDonDen { get; set; }

        public string MaHoSoMotCua { get; set; }
        public int SoBienNhanMotCua { get; set; }

        public string TenKhieuTo { get; set; }

        public string TenKhieuTo1 { get; set; }

        public string TenKhieuTo2 { get; set; }
        public string TenKhieuTo3 { get; set; }

        public string TenHuongGiaiQuyet { get; set; }
        public string TenThamQuyen { get; set; }

        public string NgayNhapDon { get; set; }
         
        public string TenPhongBanTiepNhan { get; set; }
        public string TenLoaiKetQua { get; set; }

        public string TenCoQuanGQ { get; set; }

        public string TenCoQuanXuLy { get; set; }

        public string TenCoQuanTiepNhan { get;set; }
        public string TenCoQuanBanHanh { get; set; }

        public string TenPhongBanXuLy { get; set; }

        public string TenCanBoXuLy { get; set; }

    }

    public class thamsodonthuloi 
    {
        public string Keyword { get; set; } = "";
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        
        public int? LoaiKhieuToID { get; set; }

        public int? CoQuanID { get; set; }

       

        public DateTime? TuNgay { get; set; }

        public DateTime? DenNgay { get; set; }
        public string TenCoQuan { get; set; }
    }
    public class Thamsoxoa 
    {
        public int DonThuID { get; set; }

        public int XuLyDonID { get; set; }
    }

    
}
