using Com.Gosol.KNTC.Models.BaoCao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.HeThong
{
    public class NguoiDungModel
    {
        public int NguoiDungID { get; set; }
        public string TenNguoiDung { get; set; }
        public string MatKhau { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
        public int CanBoID { get; set; }
        public int CoQuanID { get; set; }
        public int CoQuanChaID { get; set; }
        public int PhongBanID { get; set; }
        public string TenCanBo { get; set; }
        public int RoleID { get; set; }
        public int CapID { get; set; }
        public int? CapUBND { get; set; }
        public int TinhID { get; set; }
        public int HuyenID { get; set; }
        public string TenHuyen { get; set; }
        public int XaID { get; set; }
        public string TenXa { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public string RoleName { get; set; }
        public int GioiTinh { get; set; }
        public string Token { get; set; }
        public int CapCoQuan { get; set; }
        public int VaiTro { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string AnhHoSo { get; set; }
        public int QuanLyThanNhan { get; set; }
        public bool DoiMatKhauLanDau { get; set; }
        public DateTime? ThoiGianLogin { get; set; }
        public int? SoLanLogin { get; set; }
        public bool? LaCanBo { get; set; }
        public Boolean XemTaiLieuMat { get; set; }
        public DateTime? expires_at { get; set; }
        public string MaCoQuan { get; set; }
        public string TenCoQuan { get; set; }
        public bool? SuDungQuyTrinhPhucTap { get; set; }
        public bool? SuDungQuyTrinhGQPhucTap { get; set; }
        public bool? SuDungQTVanThuTiepDan { get; set; }
        public bool? SuDungQTVanThuTiepNhanDon { get; set; }
        public int? QuyTrinhGianTiep { get; set; }
        public int? ChuTichUBND { get; set; }
        public int? ChanhThanhTra { get; set; }
        public bool? BanTiepDan { get; set; }
        public bool? CapThanhTra { get; set; }
        public string id_token { get; set; } // token nhận về từ sso
        public int? TrangThaiGiaoXacMinh { get; set; }
        public bool? isAdmin { get; set; }
        public int? CapHanhChinh { get; set; } // phân cấp cơ quan theo quản lý hành chính theo EnumCapHanhChinh
        public List<CapInfo> ListCap { get; set; }
        public NguoiDungModel() { }
        public NguoiDungModel(int NguoiDungID, string TenNguoiDung)
        {
            this.NguoiDungID = NguoiDungID;
            this.TenNguoiDung = TenNguoiDung;
        }
    }

}
