using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Model.HeThong
{
    public class CanBoInfo
    {
        public int CanBoID { get; set; }
        public string TenCanBo { get; set; }
        public DateTime NgaySinh { get; set; }
        public string NgaySinhStr { get; set; }
        public int GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public int ChuoiNhaThuocid { get; set; }
        public int NhaThuocid { get; set; }
        public int Khoid { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string TenChuoiNhaThuoc { get; set; }
        public string TenNhaThuoc { get; set; }
        public int LoaiHinhThuc { get; set; }
        public int NhaThuocParentid { get; set; }
        public int NhaThuocParentid2 { get; set; }

        public int CoQuanID { get; set; }
        public int CoQuanChaID { get; set; }
        public int ChuTichUBND { get; set; }
        public int QuyenKy { get; set; }
        public int ChucVuID { get; set; }
        public int CheckCapNhat { get; set; }
        public int PhongBanID { get; set; }
        public string TenCoQuan { get; set; }
        public int VaiTroXacMinh { get; set; }
        public string TenChucVu { get; set; }
        public int QuanTriDonVi { get; set; }
        public bool XemTaiLieuMat { get; set; }
        public string XemTaiLieuMat_String { get; set; }
        public int State { get; set; }
        public string TenPhongBan { get; set; }
    }
    public class NhanVienDTO
    {
        public int NhanVienid { get; set; }
        public string TenNhanVien { get; set; }

    }
}
