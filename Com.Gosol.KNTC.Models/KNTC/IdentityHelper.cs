using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class IdentityHelper
    {
        public bool? SuDungQTVanThuTiepNhanDon { get; set; }
        public bool? SuDungQuyTrinhPhucTap { get; set; }
        public bool? SuDungQuyTrinhGQPhucTap { get; set; }
        public bool? SuDungQTVanThuTiepDan { get; set; }
        public int? QuyTrinhGianTiep { get; set; }
        public int? UserID { get; set; }//NguoiDungID
        public int? NguoiDungID { get; set; }
        public int? CanBoID { get; set; }
        public int? CapID { get; set; }
        public int? CoQuanID { get; set; }
        public int? CoQuanChaID { get; set; }
        public int? PhongBanID { get; set; }
        public int? RoleID { get; set; }
        public bool? CapUBND { get; set; }
        public bool? BanTiepDan { get; set; }
        public int PageSize { get; set; }
        public string MaCoQuan { get; set; }
        public int? ChuTichUBND { get; set; }
        public int? ChanhThanhTra { get; set; }
        public int? CapThanhTra { get; set; }
        public int? HuyenID { get; set; }
        public int? TinhID { get; set; }
        public int? XaID { get; set; }
        public int? CapHanhChinh { get; set; }
    }
}
