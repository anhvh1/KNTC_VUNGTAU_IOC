using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucBieuMauModel
    {
        public int BieuMauID { get; set; }
        [Required] public string TenBieuMau { get; set; }
        public string MaPhieuIn { get; set; }
        public bool? DuocSuDung { get; set; }
        public int CapID { get; set; }
        public string FileUrl { get; set; }
        public string TenCap { get; set; }
        public string UrlView { get; set; }
    }

    public class XoaBieuMau
    {
        public int BieuMauID { get; set; }
    }

    public class Tinh
    {
        public int TinhID { get; set; }
        public string TenTinh { get; set; }
        public string TenDayDu { get; set; }

        public List<Huyen> DanhSachHuyen { get; } = new List<Huyen>();
    }
    public class Huyen
    {
        public int HuyenID { get; set; }
        public string TenHuyen { get; set; }
        public string TenDayDu { get; set; }

        public List<Xa> DanhSachXa { get; } = new List<Xa>();
    }
    public class Xa
    {
        public int XaID { get; set; }
        public string TenXa { get; set; }
        public string TenDayDu { get; set; }
    }

    public class DanhMucBieuMauChiTietModel : DanhMucBieuMauModel
    {
        public string TenCap { get; set; }
    }

    public class DanhMucBieuMauThamSo : ThamSoLocDanhMuc
    {
        public int? CapID { get; set; }
    }

    public class SuaBieuMauModel
    {

        [Required] public int BieuMauID { get; set; }
        [Required] public string MaPhieuIn { get; set; }
        [Required] public string TenBieuMau { get; set; }
        [Required] public bool? DuocSuDung { get; set; }
        [Required] public int? CapID { get; set; }
        [Required] public int? CanBoID { get; set; }
        [Required] public string FileUrl { get; set; }
    }

    public class LichSuBieuMauModel
    {
        public int LichSuID { get; set; }
        public int BieuMauID { get; set; }
        public DateTime ThoiGianCapNhat { get; set; }
        public int CanBoID { get; set; }
        public string TenCanBo { get; set; }
    }

    public enum IDCapBieuMau
    {
        TinhHuyen = 4, // Cấp tỉnh/huyện
        Xa = 3 // Cấp xã
    }

    public class CapBieuMau
    {
        public string TenCap { get; set; }
        public IDCapBieuMau CapID { get; set; }
    }

    public class ThemBieuMauModel
    {
        [Required] public int CapID { get; set; }

        [Required]
        /*public int CanBoID { get; set; }
        [Required]*/
        public string MaPhieuIn { get; set; }
        [Required] public string TenBieuMau { get; set; }
        [Required] public bool DuocSuDung { get; set; }

        public string FileUrl { get; set; }
    }
}