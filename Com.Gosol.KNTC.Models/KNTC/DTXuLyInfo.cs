using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DTXuLyInfo
    {
        public int SoLan { get; set; }

        public DateTime NgayQuaHan { get; set; }

        public int CQChuyenDonID { get; set; }

        public string ChuyenChoCoQuan { get; set; }
        public string SoCongVan { get; set; }

        public DateTime NgayChuyenDon { get; set; }

        public bool ThuocThamQuyen { get; set; }

        public bool DuDieuKien { get; set; }

        public int? CQChuyenDonDenID { get; set; }

        public string NoiDungHuongDan { get; set; }

        public int CQTiepNhanID { get; set; }

        public int CanBoXuLyID { get; set; }

        public int CanBoKyID { get; set; }

        public string CQDaGiaiQuyetID { get; set; }

        public int CQGiaiQuyetTiepID { get; set; }

        public int TrangThaiDonID { get; set; }

        public int PhanTichKQID { get; set; }

        public int CanBoTiepNhanID { get; set; }

        public int CoQuanID { get; set; }

        public string TenCoQuan { get; set; }

        public DateTime NgayThuLy { get; set; }

        public string LyDo { get; set; }

        public int DuAnID { get; set; }

        public string TenHuongXuLy { get; set; }

        public string TenNguonDonDen { get; set; }

        public string NgayQuaHanStr { get; set; }

        public int NgayXuLyConLai { get; set; }

        public DateTime NgayLDDuyetXL { get; set; }

        public string TenCQChuyenDonDen { get; set; }

        public int XuLyDonID { get; set; }

        public int DonThuID { get; set; }

        public string SoDonThu { get; set; }

        public int? NguonDonDen { get; set; }

        public string TenChuDon { get; set; }

        public string NoiDungDon { get; set; }

        public DateTime NgayNhapDon { get; set; }

        public string NgayNhapDonStr { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string TenCBTiepNhan { get; set; }

        public string NgayNhapDons { get; set; }

        public string NgayGiao { get; set; }

        public string HanXuLy { get; set; }

        public string NguoiGiao { get; set; }

        public string TenLoaiKhieuTo { get; set; }
        public int LoaiKhieuTo1ID { get; set; }

        public int NgayXLConLai { get; set; }

        public string HuongXuLy { get; set; }

        public string TenCBXuLy { get; set; }

        public DateTime NgayGui { get; set; }

        public int PhongBanID { get; set; }

        public int CanBoID { get; set; }

        public string DiaChi { get; set; }

        public DateTime? NgayTiepNhan { get; set; }

        public string TrangThai { get; set; }
        public int TrangThaiID { get; set; }
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

        public string SoDon { get; set; }

        public DateTime NgayTiep { get; set; }

        public string NgayTiepStr { get; set; }

        public string HoTen { get; set; }

        public List<DTXuLyInfo> listFileDonThuDaTiepNhan { get; set; }

        public string TenFile { get; set; }

        public string FileUrl { get; set; }

        public bool IsBaoMat { get; set; }

        public int NguoiUp { get; set; }


        public int HuongXuLyID { get; set; }

        public string KetQuaID_Str { get; set; }

        public string TenHuongGiaiQuyet { get; set; }

        public int NgayGQConLai { get; set; }

        public int Count { get; set; }
        public string TrangThaiStr { get; set; }
        public string TrangThaiPhanXuLyStr { get; set; }
        public string TrangThaiXuLyStr { get; set; }
        public string TenCanBoXuLy { get; set; }
        public int TrangThaiQuaHan { get; set; }
        public int PhanXuLyID { get; set; }
        public int TrinhDuThao { get; set; }
        public int? LanhDaoID { get; set; }
        public int? LanhDaoDuyet2ID { get; set; }
        public int? TrangThaiDuyet { get; set; }
        public int? ChuyenGiaiQuyetID { get; set; }
        public string NoiDungBanHanhXM { get; set; }
        public string NoiDungBanHanhGQ { get; set; }
        public int QuyTrinhXLD { get; set; }
        public int QuyTrinhGQ { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public GiaoXacMinhModel GiaoXacMinh { get; set; }
        public List<DoiTuongKNInfo> listDoiTuongKN { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieuTrinhDuThao { get; set; }
        public NhomKNInfo NhomKN { get; set; }


        public int HuongGiaiQuyetID { get; set; }
        public string TrangThaiMoi { get; set; }
        public int TrangThaiIDMoi { get; set; }
        public bool CheckTrangThai { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public int LoaiQuyTrinh { get; set; }
        public int KetQuaID { get; set; }
        public int RutDonID { get; set; }

    }

    public class DuyetXuLyModel
    {
        public int? XuLyDonID { get; set; }
        public int? CanBoID { get; set; }
        public int? LanhDaoID { get; set; }
        public int? TrangThaiPheDuyet { get; set; }
        public string NoiDungPheDuyet { get; set; }
        public string NoiDungBanHanh { get; set; }
        public string NoiDungGiaiQuyet { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public DateTime? NgayBanHanh { get; set; }
        public int? LanhDaoDuyet2ID { get; set; }
        public int? TrangThaiDuyet { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }

    }

    public class ChuyenDonModel
    {
        public int? XuLyDonID { get; set; }
        public int? CoQuanID { get; set; }
        public string CoQuanNgoaiHeThong { get; set; }
        public string CoQuanTheoDoiDonDoc { get; set; }
        public DateTime? NgayChuyen { get; set; }
        public DateTime? HanXuLy { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
    }
}
