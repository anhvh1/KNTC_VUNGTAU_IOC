using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class QL_CQSatNhapInfo
    {
        public int CoQuanMoiID { get; set; }
        public string TenCoQuanMoi { get; set; }
        public String ChiaTachSapNhap { get; set; }
        public int CoQuanCuID { get; set; }
        public string TenCoQuanCu { get; set; }
        public int TrangThai { get; set; }
        public bool laSapNhap { get; set; }
        public int CountTotal { get; set; }
        public string TenNguoiThucHien { get; set; }
        public int? NguoiThucHienID { get; set; }
        public DateTime? NgayThucHien { get; set; }
        public List<QL_CQSatNhapInfo> DanhSachCoQuan { get; set; }
    }
}
