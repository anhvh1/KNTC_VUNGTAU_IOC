using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class LeTanChuyenInfo
    {
        public int LeTanChuyenID { get; set; }
        public int DonThuID { get; set; }
        public int LeTanID { get; set; }
        public string NoiDungTiep { get; set; }
        public DateTime NgayDangKy { get; set; }
        public DateTime NgayTiep { get; set; }
        public bool DaTiep { get; set; }
        public int LanTiep { get; set; }
        public int STT { get; set; }
        public int CoQuanID { get; set; }

        public int XuLyDonID { get; set; }
        public int NhomKNID { get; set; }
        public int SoNguoi { get; set; }
        public String TenCoQuan { get; set; }
        public String TenLoaiKhieuTo1 { get; set; }
        public String TenLoaiKhieuTo2 { get; set; }
        public String TenLoaiKhieuTo3 { get; set; }
    }
}
