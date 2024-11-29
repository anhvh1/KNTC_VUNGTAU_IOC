using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class BuocXacMinhInfo
    {
        public int BuocXacMinhID { get; set; }
        public int FileDanhMucBuocXacMinhID { get; set; }
        public string TenBuoc { get; set; }
        public int LoaiDon { get; set; }
        public string TenLoaiDon { get; set; }
        public Boolean IsDinhKemFile { get; set; }
        public string IsDinhKemFile_str { get; set; }
        public string GhiChu { get; set; }
        public string FileUrl { get; set; }
        public int OrderBy { get; set; }
        public string OrderBy_Str { get; set; }
        public int IsBaoMatInt { get; set; }
        public DateTime NgayUp { get; set; }
        public string NgayUp_Str { get; set; }
        public string TenFile { get; set; }
        public int NguoiUp { get; set; }
        public string TenCanBo { get; set; }
        public string TenCoQuan { get; set; }
        public string TenTrangThai { get; set; }
        public int? TrangThai { get; set; }
        public bool IsBaoMat { get; set; }
        public List<BuocXacMinhInfo> listFileDMBuocXacMinh { get; set; }
        public bool SuDung { get; set; }
        //
        public int? CanBoID { get; set; }
        public int? XuLyDonID { get; set; }
        public int? TheoDoiXuLyID { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
    }
}
