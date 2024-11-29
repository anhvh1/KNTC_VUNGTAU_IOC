using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class BaoCaoChuyenGQInfo
    {
        public string TenCoQuan { get; set; }
        public int CoQuanGiaiQuyetID { get; set; }
        public int SoLuong { get; set; }

        public int SLDangGQ { get; set; }
        public int SLChuaBanHanhQD { get; set; }
        public int SLDaBanHanhQD { get; set; }
        public int SLDonDoc1Lan { get; set; }
        public int SLDonDocNhieuLan { get; set; }
        public int SLDonChuaBaoCao { get; set; }
        public int SLDonDaBaoCao { get; set; }
    }
}
