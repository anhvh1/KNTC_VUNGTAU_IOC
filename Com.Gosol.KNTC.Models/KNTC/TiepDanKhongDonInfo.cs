using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class TiepDanKhongDonInfo
    {
        public int TiepDanKhongDonID { get; set; }
        public int? DanKhongDenID { get; set; }
        public int? LoaiTiepDanID { get; set; }
        public string TenLoaiTiepDan { get; set; }
        public DateTime NgayTiep { get; set; }
        public string NgayTiepStr { get; set; }
        public string NgayNhapDonStr { get; set; }
        public string NoiDungTiep { get; set; }
        public bool VuViecCu { get; set; }
        public int CanBoTiepID { get; set; }
        public int DonThuID { get; set; }
        public int CoQuanID { get; set; }
        public Boolean GapLanhDao { get; set; }
        public DateTime NgayGapLanhDao { get; set; }
        public int XuLyDonID { get; set; }
        public int LeTanChuyenID { get; set; }
        public string SoDon { get; set; }
        public string SoDonThu { get; set; }
        public string KetQuaTiep { get; set; }

        public string TenLoaiKhieuTo1 { get; set; }
        public string TenLoaiKhieuTo2 { get; set; }
        public string TenLoaiKhieuTo3 { get; set; }

        public int SoLuong { get; set; }
        public int LanTiep { get; set; }
        public string TenLanhDaoTiep { get; set; }

        //join
        public int NhomKNID { get; set; }
        public int NguonDonDen { get; set; }
        public string TenNguonDonDen { get; set; }
        public string TenCanBoTiep { get; set; }
        public string TenLoaiDoiTuong { get; set; }
        public string CMND { get; set; }
        public string HoTen { get; set; }
        public string TenChuDon { get; set; }
        public string DiaChi { get; set; }
        public string TenLoaiKhieuTo { get; set; }
        public string TenCQDaGiaiQuyet { get; set; }
        public int HuongGiaiQuyetID { get; set; }
        public int SoNguoi { get; set; }
        public string NgayXuLyStr { get; set; }

        //trinm
        public string TenCanBo { get; set; }
        public string TenCoQuan { get; set; }
        public string TrangThaiVuViec { get; set; }
        public string NoiDungDon { get; set; }
        public string CQDaGiaiQuyetID { get; set; }
        //public int NhomKNID { get; set; }
        public List<DoiTuongKNInfo> listDoiTuongKN { get; set; }
        public int LoaiKhieuTo1ID { get; set; }
        public int LoaiKhieuTo2ID { get; set; }
        public int LoaiKhieuTo3ID { get; set; }
        public string HuongXuLy { get; set; }
        // File
        public List<TiepDanKhongDonInfo> listFileTiepDanGianTiep { get; set; }
        public string TenFile { get; set; }
        public string FileUrl { get; set; }
        public bool IsBaoMat { get; set; }
        public int NguoiUp { get; set; }
        public NhomKNInfo NhomKN { get; set; }
        public List<ThanhPhanThamGiaInfo> ThanhPhanThamGia { get; set; }
        public int? CanBoXuLyID { get; set; }
        public int? CanBoTiepNhanID { get; set; }
        public int RutDonID { get; set; }
    }
}
