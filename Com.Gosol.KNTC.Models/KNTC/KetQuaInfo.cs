using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class KetQuaInfo
    {
        public int KetQuaID { get; set; }
        public int DonDocID { get; set; }
        public int? LanGiaiQuyet { get; set; }
        public int? LoaiKetQuaID { get; set; }
        public int? CanBoID { get; set; }
        public int? CoQuanID { get; set; }
        public DateTime? NgayRaKQ { get; set; }
        public DateTime? NgayThayDoi { get; set; }
        public DateTime? NgayDonDoc { get; set; }
        public DateTime? HanGiaiQuyetMoi { get; set; }
        public DateTime? HanGiaiQuyetCu { get; set; }
        public string LyDoDieuChinh { get; set; }
        public string NgayRaKQStr { get; set; }
        public string NoiDungDonDoc { get; set; }
        public int SoTien { get; set; }
        public int SoDat { get; set; }
        public int? SoNguoiDuocTraQuyenLoi { get; set; }
        public int? SoDoiTuongBiXuLy { get; set; }
        public int? SoDoiTuongDaBiXuLy { get; set; }
        public int XuLyDonID { get; set; }
        public string FileUrl { get; set; }
        public int? PhanTichKQ { get; set; }
        public int? KetQuaGQLan2 { get; set; }
        public int? NguoiUp { get; set; }
        public string TenCanBoUp { get; set; }

        public string TenLoaiKetQua { get; set; }
        public string TenCanBo { get; set; }
        public string TenCoQuan { get; set; }
        public string SoQuyetDinh { get; set; }
        public List<FileHoSoInfo> lstFileKQ { get; set; }

        public int? QuyetDinh { get; set; }
        public decimal? SoTienD { get; set; }
        public decimal? SoDatD { get; set; }
        public string NoiDungQuyetDinh { get; set; }
        public DateTime? ThoiHanThiHanh { get; set; }
        public string ThoiHanThiHanhStr { get; set; }
        public int? CoQuanThiHanh { get; set; }
        public int? VaiTro { get; set; }
        public int? NoiDungID { get; set; }
        public int? CoQuanThiHanhQuyetDinhID { get; set; }
        public string TenCoQuanBanHanh { get; set; }

        public string TomTatNoiDungGQ { get; set; }
        public int? LoaiKhieuToID { get; set; }
        public decimal? SoTienThuHoi { get; set; }
        public decimal? SoDatThuHoi { get; set; }
        public int? SoCaNhan { get; set; }
        public decimal? SoTienCaNhanTraLai { get; set; }
        public decimal? SoDatCaNhanTraLai { get; set; }
        public int? SoToChuc { get; set; }
        public decimal? SoTienToChucTraLai { get; set; }
        public decimal? SoDatToChucTraLai { get; set; }
        public int? SoNguoiBiKienNghiXuLy { get; set; }
        public int? SoCanBoBiXuLy { get; set; }
        public int? SoNguoiChuyenCoQuanDieuTra { get; set; }
        public int? SoCanBoChuyenCoQuanDieuTra { get; set; }
        public int? Lan2 { get; set; }
        public int? LoaiKhieuTo1ID { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
    }
    public class PXLModel
    {
        public int? CanBoID { get; set; }
        public int? XuLyDonID { get; set; }
        public DateTime? HanGiaiQuyetCu { get; set; }
        public int? StateID { get; set; }
        public int? PreStateID { get; set; }
    }
    public class QuyetDinhInfo
    {
        public int KetQuaID { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime? NgayQuyetDinh { get; set; }
        public DateTime? ThoiHanThiHanh { get; set; }
        public string TomTatNoiDungGQ { get; set; }
        public string NgayQuyetDinhStr { get; set; }
        public int? CoQuanBanHanh { get; set; }
        public string TenCoQuanBanHanh { get; set; }
        public string TenCoQuan { get; set; }
        public string TenCanBo { get; set; }
        public int? QuyetDinh { get; set; }
        public int? PhanTichKetQua { get; set; }
        public int? LoaiKetQuaID { get; set; }
        public int? KetQuaGiaiQuyetLan2 { get; set; }
        public List<NoiDungQuyetDinhInfo> ListNoiDungQuyetDinh { get; set; }
        public int? XuLyDonID { get; set; }
        public int? LoaiKhieuToID { get; set; }
        public string fileDataStr { get; set; }
        public int? CanBoID { get; set; }
        public decimal? SoTienThuHoi { get; set; }
        public decimal? SoDatThuHoi { get; set; }
        public int? SoCaNhan { get; set; }
        public decimal? SoTienCaNhanTraLai { get; set; }
        public decimal? SoDatCaNhanTraLai { get; set; }
        public int? SoToChuc { get; set; }
        public decimal? SoTienToChucTraLai { get; set; }
        public decimal? SoDatToChucTraLai { get; set; }
        public int? SoNguoiBiKienNghiXuLy { get; set; }
        public int? SoCanBoBiXuLy { get; set; }
        public int? SoNguoiChuyenCoQuanDieuTra { get; set; }
        public int? SoCanBoChuyenCoQuanDieuTra { get; set; }
        public int? Lan2 { get; set; }
        public List<FileHoSoInfo> ListFileQuyetDinh { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
    }

    public class NoiDungQuyetDinhInfo
    {
        public int NoiDungID { get; set; }
        public int? KetQuaID { get; set; }
        public int? LoaiKetQuaID { get; set; }
        public string TenLoaiKetQua { get; set; }
        public decimal? SoTien { get; set; }
        public decimal? SoDat { get; set; }
        public int? SoNguoiDuocTraQuyenLoi { get; set; }
        public int? SoDoiTuongBiXuLy { get; set; }
        public string NoiDungQuyetDinh { get; set; }
        public DateTime? ThoiHanThiHanh { get; set; }
        public string ThoiHanThiHanhStr { get; set; }
        public int? CanBoID { get; set; }
        public int? TrangThai { get; set; }
        public string TenTrangThai { get; set; }
        public List<CoQuanThiHanhInfo> ListCoQuanThiHanh { get; set; }
    }

    public class CoQuanThiHanhInfo
    {
        public int? CoQuanID { get; set; }
        public int? VaiTro { get; set; } // 1- phụ trách, 2- phối hợp, 3- theo dõi
        public string TenCoQuan { get; set; }
        public string TenVaiTro { get; set; }
    }

    public class DonThuThiHanhInfo
    {
        public int XuLyDonID { get; set; }
        public int KetQuaID { get; set; }
        public int LoaiKhieuTo1ID { get; set; }
        public int CoQuanThiHanh { get; set; }
        public string TenCoQuan { get; set; }
        public string TenCanBo { get; set; }
        public string SoDonThu { get; set; }
        public string TenChuDon { get; set; }
        public string NoiDungDon { get; set; }
        public DateTime ThoiHanThiHanh { get; set; }
        public int TrangThaiID { get; set; } // 1 Chưa thi hành, 2- đang thi hành, 3- đã thi hành
        public string TenTrangThai { get; set; }
        public int DonThuID { get; set; }
        public int? NhomKNID { get; set; }
        public NhomKNInfo NhomKN { get; set; }

        public int? ThiHanhID { get; set; }
        public DateTime? NgayThiHanh { get; set; }
        public DateTime? NgayTiepNhan { get; set; }
        public string GhiChu { get; set; }
        public decimal? SoTienThuHoi { get; set; }
        public decimal? SoDatThuHoi { get; set; }
        public int? SoCaNhan { get; set; }
        public decimal? SoTienCaNhanTraLai { get; set; }
        public decimal? SoDatCaNhanTraLai { get; set; }
        public int? SoToChuc { get; set; }
        public decimal? SoTienToChucTraLai { get; set; }
        public decimal? SoDatToChucTraLai { get; set; }
        public int? SoNguoiBiKienNghiXuLy { get; set; }
        public int? SoCanBoBiXuLy { get; set; }
        public int? SoNguoiChuyenCoQuanDieuTra { get; set; }
        public int? SoCanBoChuyenCoQuanDieuTra { get; set; }
        public QuyetDinhInfo ThongTinQuyetDinhGQ { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
    }

    public class KetQuaPatialInfo : KetQuaInfo
    {
        // trong thi hành
        public int? ThiHanhID { get; set; }
        public DateTime? NgayThiHanh { get; set; }
        public int? LoaiThiHanhID { get; set; }
        public decimal? SoTienDaThu { get; set; }
        public decimal? SoDatDaThu { get; set; }
        public int? SoNguoiDuocTraQuyenLoi_TH { get; set; }
        public int? SoDoiTuongBiXuLy_TH { get; set; }
        public string NoiDungThiHanh { get; set; }
        public string TenCoQuan_ThiHanh { get; set; }
        //file thi hành
        public int? FileHoSoID { get; set; }
        public string TenFile { get; set; }
        public string FileURL { get; set; }
        public DateTime? NgayUp { get; set; }
    }
    public class NoiDungThiHanhInfo
    {
        // trong thi hành
        public int? XuLyDonID { get; set; }
        public int? NoiDungID { get; set; }
        public int? ThiHanhID { get; set; }
        public DateTime? NgayThiHanh { get; set; }
        public string NgayThiHanhStr { get; set; }
        public int? LoaiThiHanhID { get; set; }
        public decimal? SoTienDaThu { get; set; }
        public decimal? SoDatDaThu { get; set; }
        public int? SoNguoiDuocTraQuyenLoi { get; set; }
        public int? SoDoiTuongBiXuLy { get; set; }
        public string NoiDungThiHanh { get; set; }
        public int? CoQuanThiHanh { get; set; }
        public string TenCoQuan { get; set; }
        //file thi hành
        public int? FileHoSoID { get; set; }
        public string TenFile { get; set; }
        public string FileURL { get; set; }
        public DateTime? NgayUp { get; set; }
        public List<FileHoSoInfo> ListFileThiHanh { get; set; }
        public string ListFileString { get; set; }

        public string GhiChu { get; set; }
        public decimal? SoTienThuHoi { get; set; }
        public decimal? SoDatThuHoi { get; set; }
        public int? SoCaNhan { get; set; }
        public decimal? SoTienCaNhanTraLai { get; set; }
        public decimal? SoDatCaNhanTraLai { get; set; }
        public int? SoToChuc { get; set; }
        public decimal? SoTienToChucTraLai { get; set; }
        public decimal? SoDatToChucTraLai { get; set; }
        public int? SoNguoiBiKienNghiXuLy { get; set; }
        public int? SoCanBoBiXuLy { get; set; }
        public int? SoNguoiChuyenCoQuanDieuTra { get; set; }
        public int? SoCanBoChuyenCoQuanDieuTra { get; set; }
        public int? CanBoID { get; set; }

        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
    }
    public class KetQuaThiHanhItemInfo
    {
        public NoiDungQuyetDinhInfo NoiDungQuyetDinh { get; set; }
        public NoiDungThiHanhInfo NoiDungThiHanh { get; set; }
    }

    public class KetQuaThiHanhInfo : DonThuThiHanhInfo
    {
        public List<KetQuaThiHanhItemInfo> ListItemKetQuaThiHanh { get; set; }
    }

    public class DanhSachDonThuDaDuyetKQInfo
    {
        public int TotalRow { get; set; }
        public List<DonThuThiHanhInfo> DanhSachDonThu { get; set; }
    }

    public class ChiTietDonThuKetQuaBanHanhInfo
    {
        public QuyetDinhInfo quyetDinhInfo { get; set; }
        public KetQuaThiHanhInfo ketQuaThiHanhItemInfo { get; set; }
    }
}
