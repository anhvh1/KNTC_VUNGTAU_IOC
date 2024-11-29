using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class CoQuanInfo
    {
        public int CoQuanID { get; set; }
        public String TenCoQuan { get; set; }
        public int CoQuanChaID { get; set; }
        public string TenCoQuanCha { get; set; }
        public int CapID { get; set; }
        public int ThamQuyenID { get; set; }
        public int TinhID { get; set; }
        public int HuyenID { get; set; }
        public int XaID { get; set; }
        public int Level { get; set; }
        public bool CapUBND { get; set; }
        public bool CapThanhTra { get; set; }
        public bool SuDungPM { get; set; }
        public string MaCQ { get; set; }
        public bool SuDungQuyTrinh { get; set; }
        public bool QuyTrinhVanThuTiepNhan { get; set; }
        public bool SuDungQuyTrinhGQ { get; set; }
        public bool QTVanThuTiepDan { get; set; }
        public bool CQCoHieuLuc { get; set; }
        public bool Disable { get; set; }
        public int TTChiaTachSapNhap { get; set; }
        public int ChiaTachSapNhapDenCQID { get; set; }
        public string TenCQChiaTachSapNhapDen { get; set; }
        public int hasChild { get; set; }
        public int IsPhuTrach { get; set; }
        public string TenHuyen { get; set; }
        public bool? BanTiepDan { get; set; }
        public DateTime? NgayThucHien { get; set; }
        public string TenNguoiThucHien { get; set; }
    }
}
