using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class RutDonInfo
    {
        public int RutDonID { get; set; }
        public DateTime? NgayRutDon { get; set; }
        public String LyDo { get; set; }
        public int XuLyDonID { get; set; }
        public String FileQD { get; set; }

        public int NhomKNID { get; set; }
        public String TenLoaiKhieuTo1 { get; set; }
        public String TenLoaiKhieuTo2 { get; set; }
        public String TenLoaiKhieuTo3 { get; set; }
        public int CoQuanID { get; set; }
        public String NoiDungDon { get; set; }
        public string TenCoQuan { get; set; }

        public string SoDon { get; set; }
        public string TenChuDon { get; set; }
        public string HoTenStr { get; set; }
        public string TrangThaiRut { get; set; }
        public string TenLoaiKhieuTo { get; set; }
        public int DonThuID { get; set; }
        public string StateName { get; set; }

        public string TenCanBo { get; set; }
        public int NguoiUp { get; set; }
        public string FileUrl { get; set; }
        public string TenCoQuanUp { get; set; }
        public DateTime NgayUp { get; set; }
        public string NgayUps { get; set; }
        public bool IsBaoMat { get; set; }
        public string TenFile { get; set; }

        public int FileID { get; set; }
        public int NhomFileID { get; set; }
        public string TenNhomFile { get; set; }
        public int ThuTuHienThiNhom { get; set; }
        public int ThuTuHienThiFile { get; set; }
        public int FileRutDonID { get; set; }
    }
}
