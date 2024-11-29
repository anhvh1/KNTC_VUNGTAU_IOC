using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DonThuGiaiQuyetInfo
    {
        public int PhanGiaiQuyetID { get; set; }
        public int XuLyDonID { get; set; }
        public int PhongBanID { get; set; }
        public int CanBoID { get; set; }
        public int TrangThai { get; set; }
        public int ChaID { get; set; }
        public int CanBoGiaoID { get; set; }

        public int TrangThaiQuaHan { get; set; }
        public string TenTrangThai { get; set; }
        public string SoDonThu { get; set; }
        public int NhomKNID { get; set; }
        public int TiepDanID { get; set; }
        public String NoiDungDon { get; set; }
        public DateTime NgayQuaHan { get; set; }
        public DateTime NgayQuaHanDueDate { get; set; }
        public int SoNgayConLai { get; set; }
        public int DonThuID { get; set; }
        public int TrangThaiGiao { get; set; }

        public DateTime NgayRutDon { get; set; }
        public string LyDoRutDon { get; set; }
        public String TenPhongBan { get; set; }
        public String TenCanBo { get; set; }
        public String TenCanBoGiao { get; set; }
        public string HoTen { get; set; }
        public string HoTenStr { get; set; }
        public DateTime NgayPhanCong { get; set; }
        public DateTime NgayThuLy { get; set; }
        public DateTime? HanGiaiQuyet { get; set; }
        public string NgayThuLys { get; set; }
        public string NgayPhanCongs { get; set; }
        public string HanGiaiQuyets { get; set; }
        public string TenLoaiKhieuTo { get; set; }
        public int NguonDonDen { get; set; }
        public string TenNguonDonDen { get; set; }
        public DateTime NgayGui { get; set; }
        public string NgayGuis { get; set; }
        public DateTime HanGQ { get; set; }
        public string HanGQStr { get; set; }
        public int NgayGQConLai { get; set; }
        public String StateName { get; set; }
        public int StateID { get; set; }
        public string NgayQuaHanStr { get; set; }


        public int TrangThaiGQCapDuoi { get; set; }
        public int PhanGiaiQuyetCapDuoiID { get; set; }

        public int CoQuanID { get; set; }
        public int CoQuanGiaoID { get; set; }
        public int LoaiKhieuTo1ID { get; set; }

        public string TenTrangThaiXuLy { get; set; }
        public string NgayCapNhatStr { get; set; }
        public string DuongDanFile { get; set; }
        public List<string> listDuongDanFile { get; set; }
        public int TheoDoiXuLyID { get; set; }
        public int TrangThaiXuLyID { get; set; }

        public int ExecuteTime { get; set; }
        public string TenCoQuanPhanGQ { get; set; }
        public string TenCoQuanGQ { get; set; }

        public string TenCanBoPhanGQTiep { get; set; }

        public int DaPhanTrongCQ { get; set; }
        public int DaPhanGQTiepTrongCQ { get; set; }
        public int DuocPhanGQTiep { get; set; }
        public int DaPhanCQKhac { get; set; }
        public int DaGiaiQuyetTrongCQ { get; set; }
        public int DaGiaiQuyetTaiCQDuocPhan { get; set; }
        public string GhiChu { get; set; }
        public int RutDonID { get; set; }

        public DateTime HanGQGoc { get; set; }
        public DateTime HanGQMoi { get; set; }
        public DateTime HanGQQTPhucTap { get; set; }

        public int ChuyenGiaiQuyetID { get; set; }

        public string SoCVBaoCaoGQ { get; set; }
        public DateTime NgayCVBaoCaoGQ { get; set; }
        public string NgayCVBaoCaoGQStr { get; set; }
        public string CQNhanDonChuyen { get; set; }
        public string CQNhanVanBanDonDoc { get; set; }
        public int HuongXuLyID { get; set; }
        public string TenHuongGiaiQuyet { get; set; }

        // anhnt
        public string SoDon { get; set; }
        public string TenChuDon { get; set; }
        public DateTime? NgayTiepNhan { get; set; }
        public DateTime? HanXuLy { get; set; }
        public DateTime NgayTiep { get; set; }
        public string NgayTiepStr { get; set; }
        public DateTime NgayNhapDon { get; set; }
        public string NgayNhapDonStr { get; set; }
        public string DiaChi { get; set; }
        public int TrangThaiPhanHoi { get; set; }
        public string NoiDungPhanHoi { get; set; }
        public int PhanLoaiPhanHoi { get; set; }
        public string CanBoPhanHoi { get; set; }
        public int Count { get; set; }
        public int? CQPhoiHopID { get; set; }
        public int? TrangThaiDuyet { get; set; }
        public int TrinhDuThao { get; set; }

        public GiaoXacMinhModel GiaoXacMinh { get; set; }
        public List<BuocXacMinhInfo> BuocXacMinh { get; set; }
        public List<DoiTuongKNInfo> listDoiTuongKN { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }


        public int HuongGiaiQuyetID { get; set; }
        public string TrangThaiMoi { get; set; }
        public int TrangThaiIDMoi { get; set; }
        public bool CheckTrangThai { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public int LoaiQuyTrinh { get; set; }
        public int KetQuaID { get; set; }
        public int LanhDaoDuyet2ID { get; set; }
        public int CoCapNhapDoanToXacMinh { get; set; }

    }

    public class GiaoXacMinhModel
    {
        public int? XuLyDonID { get; set; }
        public int? CoQuanID { get; set; }
        public int? TruongPhongID { get; set; }
        public DateTime? HanGiaiQuyet { get; set; }
        public string SoQuyetDinh { get; set; }
        public string QuyetDinh { get; set; }
        public DateTime? NgayQuyetDinh { get; set; }
        public string CanBoQD { get; set; }
        public string GhiChu { get; set; }
        public List<CQPhoiHopGQInfo> CQPhoiHopGQ { get; set; }
        public List<VaiTroGiaiQuyetInfo> ToXacMinh { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
    }
    //public class QueryFilterInfo
    //{
    //    public int CoQuanID { get; set; }
    //    public int CQChuyenDonDenID { get; set; }
    //    public string KeyWord { get; set; }
    //    public int LoaiKhieuToID { get; set; }
    //    public DateTime TuNgay { get; set; }
    //    public DateTime DenNgay { get; set; }
    //    public int Start { get; set; }
    //    public int End { get; set; }
    //    public string StateName { get; set; }
    //    public string StateName2 { get; set; }
    //    public int StateID { get; set; }
    //    public string TenCoQuan { get; set; }

    //    public int StateOrder { get; set; }
    //    public int CanBoID { get; set; }
    //    public bool IsChuyenVien { get; set; }
    //    public int PhongBanID { get; set; }
    //    public string PrevStateName { get; set; }
    //    public int VaiTro { get; set; }

    //    public bool IsTruongPhong { get; set; }
    //    public bool IsLanhDao { get; set; }

    //    public DateTime TuNgayGoc { get; set; }
    //    public DateTime DenNgayGoc { get; set; }
    //    public DateTime TuNgayMoi { get; set; }
    //    public DateTime DenNgayMoi { get; set; }


    //    public int LoaiRutDon { get; set; }

    //    public int LoaiXL { get; set; }
    //    public int PTKQDung { get; set; }
    //    public int PTKQDungMotPhan { get; set; }
    //    public int PTKQSai { get; set; }
    //    public int LoaiDon { get; set; }
    //    public int TrangThaiKQ { get; set; }
    //    public int HuongXuLyID { get; set; }

    //    public int CapID { get; set; }
    //    public int TinhID { get; set; }
    //    public int? TrangThai { get; set; } // trạng thái đơn thư trong quá trình thi hành quyết định: 1= Chưa ban hành, 2= Chưa thi hành, 3= Đang thi hành, 4= Đã thi hành

    //}
}
