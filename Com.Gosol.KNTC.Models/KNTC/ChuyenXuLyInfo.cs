using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class ChuyenXuLyInfo
    {
        public int ChuyenXuLyID { get; set; }
        public int XuLyDonID { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public string NgayCapNhats { get; set; }
        public string TheoDoiVBDi { get; set; }
        public string TheoDoiVBDen { get; set; }
        public string FileDinhKem { get; set; }

        public int CQGuiID { get; set; }
        public int CQNhanID { get; set; }

        public int CoQuanID { get; set; }

        //Other
        public int DonThuID { get; set; }
        public string SoDonThu { get; set; }
        public string HoTen { get; set; }
        public string DiaChiCT { get; set; }
        public string NoiDungDon { get; set; }
        public DateTime NgayChuyen { get; set; }
        public string NgayChuyens { get; set; }
        public string TenCoQuanNhan { get; set; }
        public String TenCanBoGiaiQuyet { get; set; }

        public int NguonDonDen { get; set; }
        public string NguonDonDens { get; set; }
        public string TenLoaiKhieuTo { get; set; }
        public DateTime NgayThuLy { get; set; }
        public int NhomKNID { get; set; }
        public DateTime NgayQuaHan { get; set; }
        public DateTime? HanGiaiQuyet { get; set; }

        public string TenCanBoXuLy { get; set; }
        public DateTime NgayPhan { get; set; }
        public DateTime NgayHetHan { get; set; }

        public int SoLanTiep { get; set; }
        public int? Lan2 { get; set; }
        public int SoLuongNguoiKNTC { get; set; }
        public string KetQuaID { get; set; }
        public string KetQuaTranhChapID_Str { get; set; }
        public int KetQuaTranhChapID { get; set; }

        public int KetQuaBanHanhID { get; set; }

        public int CQGiaoID { get; set; }
        public DateTime HanGQGoc { get; set; }
        public DateTime HanGQMoi { get; set; }


        // anhnt
        public string SoDon { get; set; }
        public DateTime NgayTiep { get; set; }
        public string NgayTiepStr { get; set; }
        public string NgayNhapDonStr { get; set; }
        public string TenChuDon { get; set; }
        public string DiaChi { get; set; }
        public int LoaiKhieuTo1ID { get; set; }
        public int PhanTichKQ { get; set; }

        //
        public string TenCoQuanGiaiQuyet { get; set; }

        public string TenTrangThai { get; set; }
        public int TrangThai { get; set; }
        public bool? TrangThaiKhoa { get; set; }

        //ajax
        public string ThuocThamQuyen { get; set; }
        public string NgayPhanCongStr { get; set; }
        public string TrangThaiStr { get; set; }
        public string TrangThaiCssClass { get; set; }
        public string TenChuDonStr { get; set; }
        public string NguonDonDenStr { get; set; }
        public string HanXuLyStr { get; set; }
        public NhomKNInfo NhomKN { get; set; }
        public List<DoiTuongKNInfo> listDoiTuongKN { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }

        public int HuongGiaiQuyetID { get; set; }
        public string TrangThaiMoi { get; set; }
        public int TrangThaiIDMoi { get; set; }
        public bool CheckTrangThai { get; set; }
        public int LoaiQuyTrinh { get; set; }
        public int StateID { get; set; }
        public int? TrangThaiDuyet { get; set; }
        public int TrinhDuThao { get; set; }
        public int? ChuyenGiaiQuyetID { get; set; }
        public int? LanhDaoDuyet2ID { get; set; }
        public DateTime? NgayTiepNhan { get; set; }
        public int ThiHanhID { get; set; }
        public int RutDonID { get; set; }
    }
}
