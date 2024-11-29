using Com.Gosol.KNTC.Models.DanhMuc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Gosol.KNTC.Models.HeThong
{
    public class HeThongCanBoModel
    {
        [Required]
        public int CanBoID { get; set; }
        public string TenCanBo { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int? GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public int? ChucVuID { get; set; }
        public int? QuyenKy { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public int? PhongBanID { get; set; }
        public int? CoQuanID { get; set; }
        public int? RoleID { get; set; }
        public int? QuanTriDonVi { get; set; }
        public int? CoQuanCuID { get; set; }
        public int? CanBoCuID { get; set; }
        public int? XemTaiLieuMat { get; set; }
        public string AnhHoSo { get; set; }
        public string AnhChamCong { get; set; }
        public int? IsStatus { get; set; }
        public string HoKhau { get; set; }
        public string MaCB { get; set; }
        public int ThanNhanID { get; set; }
        public string HoTenThanNhan { get; set; }
        public int? CapQuanLy { get; set; }
        public List<int> DanhSachChucVuID { get; set; }
        public List<string> DanhSachTenChucVu { get; set; }
        public List<int> DanhSachNhomNguoiDungID { get; set; }
        public int? TrangThaiID { get; set; }
        public int? State { get; set; }
        public int? NguoiDungID { get; set; }
        public string SSOID { get; set; }
        public string TenNguoiDung { get; set; }
        public string MatKhau { get; set; }
        public int? VaiTro { get; set; }
        public string TenCoQuan { get; set; }
        public int CapCoQuanID { get; set; }
        public string CMND { get; set; }
        public string NoiCap { get; set; }
        public DateTime? NgayCap { get; set; }
        public int SoLuongAnh { get; set; }
        public DateTime? NgayBatDauLam { get; set; }
        public DateTime? NgayKetThucLam { get; set; }
        public int AnhDaiDienID { get; set; }
        public int FaceID { get; set; }
        public decimal Similarity { get; set; }
        public DateTime NgayGioChamCong { get; set; }
        public int NguoiTao { get; set; }
        public Boolean? TuLayAnh { get; set; }
        public string TicKet { get; set; }
        public string TrangThaiSoLuongAnh { get; set; }
        public int? TrangThaiTaiKhoan { get; set; }
        public List<NhomNguoiDungModel> DanhSachNhomNguoiDung { get; set; }
        public List<int> DanhSachCaLamViecID { get; set; }
        public bool? LaCanBo { get; set; }
        public int? ChuTichUBND { get; set; }
        public HeThongCanBoModel() { }
        public HeThongCanBoModel(int CanBoID, string TenCanBo, DateTime NgaySinh, int GioiTinh, string DiaChi, int ChucVuID, int QuyenKy, string Email, string DienThoai,
            int PhongBanID, int CoQuanID, int RoleID, int QuanTridonVi, int CoQuanCuID, int CanBoCuID, int XemTaiLieuMat, string AnhHoSo, string HoKhau,
            string MaCB, int CapQuanLy, int TrangThaiID, int NguoiDungID, int VaiTro,int CapCoQuanID)
        {
            this.CanBoID = CanBoID;
            this.TenCanBo = TenCanBo;
            this.NgaySinh = NgaySinh;
            this.GioiTinh = GioiTinh;
            this.DiaChi = DiaChi;
            this.ChucVuID = ChucVuID;
            this.QuyenKy = QuyenKy;
            this.Email = Email;
            this.DienThoai = DienThoai;
            this.PhongBanID = PhongBanID;
            this.CoQuanID = CoQuanID;
            this.RoleID = RoleID;
            this.QuanTriDonVi = QuanTridonVi;
            this.CoQuanCuID = CoQuanCuID;
            this.CanBoCuID = CanBoCuID;
            this.XemTaiLieuMat = XemTaiLieuMat;
            //this.IsStatus = IsStatus;
            this.AnhHoSo = AnhHoSo;
            this.HoKhau = HoKhau;
            this.MaCB = MaCB;
            this.CapQuanLy = CapQuanLy;
            this.TrangThaiID = TrangThaiID;
            this.NguoiDungID = NguoiDungID;
            this.VaiTro = VaiTro;
            this.CapCoQuanID = CapCoQuanID;
        }

        public HeThongCanBoModel(int CanBoID, string TenCanBo)
        {
            this.CanBoID = CanBoID;
            this.TenCanBo = TenCanBo;
        }
    }
    public class HeThongCanBoPartialModel : HeThongCanBoModel
    {
        public string TenChucVu { get; set; }
        
        public string TenTrangThai { get; set; }
        public string TenCapQuanLy { get; set; }
        public List<string> NguyenNhan { get; set; }
    }
    public class HeThongCanBoShortModel : HeThongCanBoModel
    {
        public int CanBoID { get; set; }
        public int ThanNhanID { get; set; }
        public string TenCanBo { get; set; }
        public string HoTenThanNhan { get; set; }
        public string TenDotKeKhai { get; set; }
        public HeThongCanBoShortModel() { }
    }
    public class CanBoChuVu
    {
        public int CanBoID { get; set; }
        public int ChucVuID { get; set; }
        public bool KeKhaiHangNam { get; set; }
        public int CapQuanLy { get; set; }

        public int TrangThaiID { get; set; }
        public int CoQuanID { get; set; }
    }
    public class Files
    {
        public string files { get; set; }
        public Files()
        {


        }
        public Files(string files)
        {
            this.files = files;
        }
    }

    public class ThongTinDonViModel
    {
        public string TenCanBo { get; set; }
        public string TenCoQuan { get; set; }
        public string TenCoQuanCha { get; set; }
        public int CanBoID { get; set; }
        public int CoQuanID { get; set; }
        public int CoQuanChaID { get; set; }
    }

    public class ThongTinCanBoModel
    {
        public HeThongCanBoModel ThongTinCanBo { get; set; }
    }

    public class CanBoResult
    {
        public int CanBoID { get; set; }
        public string TenCanBo { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int? GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public string MaCB { get; set; }
        public int? TrangThaiID { get; set; }
        public int? NguoiDungID { get; set; }
        public string TenNguoiDung { get; set; }
        public string CMND { get; set; }
        public bool? LaCanBo { get; set; }
        public CanBoResult(HeThongCanBoModel CanBoModel)
        {
            this.CanBoID = CanBoModel.CanBoID;
            this.TenCanBo = CanBoModel.TenCanBo;
            this.NgaySinh = CanBoModel.NgaySinh;
            this.GioiTinh = CanBoModel.GioiTinh;
            this.DiaChi = CanBoModel.DiaChi;
            this.Email = CanBoModel.Email;
            this.DienThoai = CanBoModel.DienThoai;
            this.MaCB = CanBoModel.MaCB;
            this.TrangThaiID = CanBoModel.TrangThaiID;
            this.NguoiDungID = CanBoModel.NguoiDungID;
            this.TenNguoiDung = CanBoModel.TenNguoiDung;
            this.CMND = CanBoModel.CMND;
            this.LaCanBo = CanBoModel.LaCanBo;
        }
    }
}
