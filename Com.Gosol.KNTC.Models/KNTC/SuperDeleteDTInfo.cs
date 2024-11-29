using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class SuperDeleteDTInfo
    {
        public int SoLan { get; set; }

        public DateTime NgayQuaHan { get; set; }

        public int CQChuyenDonID { get; set; }

        public string SoCongVan { get; set; }

        public DateTime NgayChuyenDon { get; set; }

        public bool ThuocThamQuyen { get; set; }

        public bool DuDieuKien { get; set; }

        public int HuongGiaiQuyetID { get; set; }

        public string NoiDungHuongDan { get; set; }

        public int CQTiepNhanID { get; set; }

        public int CanBoXuLyID { get; set; }

        public int CanBoKyID { get; set; }

        public int CQDaGiaiQuyetID { get; set; }

        public int CQGiaiQuyetTiepID { get; set; }

        public int TrangThaiDonID { get; set; }

        public int PhanTichKQID { get; set; }

        public int CanBoTiepNhanID { get; set; }

        public int CoQuanID { get; set; }

        public DateTime NgayThuLy { get; set; }

        public string LyDo { get; set; }

        public int DuAnID { get; set; }

        public string TenHuongXuLy { get; set; }

        public string TenNguonDonDen { get; set; }

        public string NgayQuaHanStr { get; set; }

        public int NgayXuLyConLai { get; set; }

        public int XuLyDonID { get; set; }

        public int DonThuID { get; set; }

        public string SoDonThu { get; set; }

        public int NguonDonDen { get; set; }

        public string TenChuDon { get; set; }

        public string NoiDungDon { get; set; }

        public DateTime NgayNhapDon { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string TenCBTiepNhan { get; set; }

        public string NgayNhapDons { get; set; }

        public string NgayGiao { get; set; }

        public string HanXuLy { get; set; }

        public string NguoiGiao { get; set; }

        public string TenLoaiKhieuTo { get; set; }

        public int NgayXLConLai { get; set; }

        public string HuongXuLy { get; set; }

        public string TenCBXuLy { get; set; }

        public DateTime NgayGui { get; set; }

        public int PhongBanID { get; set; }

        public int CanBoID { get; set; }

        public string DiaChi { get; set; }

        public string NgayTiepNhan { get; set; }

        public string TrangThai { get; set; }

        public string StateName { get; set; }

        public int StateID { get; set; }

        public int CanBoPhanXLID { get; set; }

        public int NextStateID { get; set; }

        public int CBDuocChonXL { get; set; }

        public int QTTiepNhanDon { get; set; }

        public string TenTinh { get; set; }

        public string TenHuyen { get; set; }

        public string TenXa { get; set; }

        public string DiaChiCT { get; set; }

        public string NguonDonDens { get; set; }

        public string NgayGuis { get; set; }

        public DateTime NgayXuLy { get; set; }

        public string MaHoSoMotCua { get; set; }

        public string SoBienNhanMotCua { get; set; }

        public string NgayHenTraMotCuaStr { get; set; }

        public int TransitionID { get; set; }

        public string HoTenStr { get; set; }

        public int DoiTuongBiKNID { get; set; }

        public int NhomKNID { get; set; }

        public string TenCoQuan { get; set; }
        public List<DoiTuongKNInfo> listDoiTuongKN { get; set; }
    }
}
