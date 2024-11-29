using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DonThuInfo
    {
        public int DonThuID { get; set; }
        public string TenCoQuanGiaiQuyet { get; set; }
        public int NhomKNID { get; set; }
        public int PreState { get; set; }
        public int DoiTuongBiKNID { get; set; }
        public int LoaiKhieuToID { get; set; }
        public int LoaiKhieuTo1ID { get; set; }
        public int LoaiKhieuTo2ID { get; set; }
        public int LoaiKhieuTo3ID { get; set; }

        public string NoiDungDon { get; set; }

        public Boolean TrungDon { get; set; }
        //public int CoQuanID { get; set; }
        public string DiaChiPhatSinh { get; set; }
        public int? TinhID { get; set; }
        public int? HuyenID { get; set; }
        public int? XaID { get; set; }

        public String TenLoaiDoiTuong { get; set; }
        public int SoLanTrung { get; set; }

        //for show List
        public DateTime NgayNhapDon { get; set; }
        public string NgayNhapDonStr { get; set; }
        public DateTime NgayPhan { get; set; }
        public string NgayPhanStr { get; set; }
        public DateTime NgayTiep { get; set; }
        public string HoTen { get; set; }
        public string HoTenStr { get; set; }
        public string DiaChi { get; set; }
        public string DiaChiCT { get; set; }
        public string TenLoaiKhieuTo { get; set; }
        public string TenCanBo { get; set; }
        public string TenCoQuan { get; set; }
        public string TenXa { get; set; }
        public string TenHuyen { get; set; }
        public string TenTinh { get; set; }
        public int SoLan { get; set; }
        public int LanGiaiQuyet { get; set; }
        public int TrangThaiDonID { get; set; }
        public int CoQuanID { get; set; }
        public int XuLyDonID { get; set; }
        public int SoLuong { get; set; }
        public string CMND { get; set; }
        public string SoDonThu { get; set; }
        public DateTime NgayVietDon { get; set; }
        public DateTime NgayQuaHanGQ { get; set; }
        public int CanBoTiepNhapID { get; set; }
        public String LyDo { get; set; }
        public string NgayQuaHanGQStr { get; set; }
        public string TenPhongBanTiepNhan { get; set; }
        public DateTime NgayXuLy { get; set; }
        public string NgayXuLyStr { get; set; }
        public DateTime HanXuLy { get; set; }
        public string HanXuLyStr { get; set; }
        public string TenCanBoXuLy { get; set; }
        public DateTime NgayChuyenDon { get; set; }
        public DateTime NgayChuyenDonSangCQKhac { get; set; }
        public string NgayChuyenDonSangCQKhacStr { get; set; }
        public string CoQuanChuyenDonDi { get; set; }
        public string NgayChuyenDonStr { get; set; }
        public int VaiTro { get; set; }
        public string VaiTroStr { get; set; }
        public string CQDaGiaiQuyetID { get; set; }
        public string TenPhongBanXuLy { get; set; }
        public string HuongXuLy { get; set; }
        public string SoCongVan { get; set; }
        public int StateID { get; set; }
        public int HuongGiaiQuyetID { get; set; }
        public string StateName { get; set; }
        public int CapID { get; set; }

        public String TenLoaiKhieuTo1 { get; set; }
        public String TenLoaiKhieuTo2 { get; set; }
        public String TenLoaiKhieuTo3 { get; set; }

        public string TenHuongGiaiQuyet { get; set; }
        public string NoiDungHuongDan { get; set; }
        public string TenCanBoTiepNhan { get; set; }
        public string KetQuaGiaiQuyet { get; set; }

        public string CQDaGiaiQuyet { get; set; }
        public string TenLoaiKetQua { get; set; }

        public int NguonDonDen { get; set; }
        public string TenNguonDonDen { get; set; }
        public string TenCQChuyenDonDen { get; set; }
        public string TenCoQuanTiepNhan { get; set; }
        public string TenCoQuanGQ { get; set; }
        public string TenCoQuanXL { get; set; }

        public string MaHoSoMotCua { get; set; }
        public string SoBienNhanMotCua { get; set; }
        public string NgayHenTraMotCuaStr { get; set; }
        public string SoCVBaoCaoGQ { get; set; }
        public DateTime NgayCVBaoCaoGQ { get; set; }
        public string NgayCVBaoCaoGQStr { get; set; }
        public string TenCQPhan { get; set; }

        public string YKienXuLy { get; set; }
        public string YKienGiaiQuyet { get; set; }
        public int DonThuGocID { get; set; }

        public int TrangThaiDongBo { get; set; }

        public DateTime HanGiaiQuyetCu { get; set; } // Hạn giải quyết phucth => phuocqh
        public DateTime HanGiaiQuyetMoi { get; set; } // Hạn giải quyết chivt => trangdtt
        public DateTime HanGiaiQuyetFrist { get; set; } // Hạn giải quyết anvt => phucth
        public DateTime NgayXuLyDon { get; set; } // Ngày xử lý đơn
        public string HanGiaiQuyetCu_Str { get; set; }
        public string HanGiaiQuyetMoi_Str { get; set; }
        public string HanGiaiQuyetFrist_Str { get; set; }
        public string NgayXuLyDon_Str { get; set; }

        public string TenCoQuanPhoiHop_Str { get; set; }
        public int LanGQ { get; set; }

        public string TenCQDaGiaiQuyet { get; set; }

        public DateTime NgayGiao { get; set; }
        public string NgayGiao_Str { get; set; }

        public int Count { get; set; }
        public int? TrinhDuThao { get; set; }
        public DateTime? NgayBanHanh { get; set; }
        public string TenCanBoBanHanh { get; set; }
        public string TenCoQuanBanHanh { get; set; }
        public List<DoiTuongKNInfo> listDoiTuongKN { get; set; }

        public List<DanhSachHoSoTaiLieu> FileCQGiaiQuyet { get; set; }
        public List<DanhSachHoSoTaiLieu> FileKQTiep { get; set; }
        public List<DanhSachHoSoTaiLieu> FileKQGiaiQuyet { get; set; }

        public int LanhDaoDuyet1ID { get; set; }
        public int LanhDaoDuyet2ID { get; set; }


    }
}
