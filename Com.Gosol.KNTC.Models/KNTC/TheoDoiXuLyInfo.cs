using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class TheoDoiXuLyInfo
    {
        public int TheoDoiXuLyID { get; set; }
        public int TrangThaiXuLyID { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public int XuLyDonID { get; set; }
        public string GhiChu { get; set; }
        public string StringNgayCapNhat { get; set; }
        public String TenTrangThaiXuLy { get; set; }
        public string DuongDanFile { get; set; }
        public int CanBoID { get; set; }
        public string TenCanBo { get; set; }
        public string TenCapBoUp { get; set; }
        public string TenCoQuan { get; set; }
        public int BuocXacMinhID { get; set; }
        public string TenBuoc { get; set; }
        public int SoFile { get; set; }
        public bool IsBaoMat { get; set; }
        public string TomTat { get; set; }
        public string TenFile { get; set; }
        public string TenCoQuanUp { get; set; }
        public string NoiDung { get; set; }

        public int FileID { get; set; }
        public int NhomFileID { get; set; }
        public string TenNhomFile { get; set; }
        public int ThuTuHienThiNhom { get; set; }
        public int ThuTuHienThiFile { get; set; }
        public int FileGiaiQuyetID { get; set; }
        public int FileDonThuCanDuyetGiaiQuyetID { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
    }
}
