using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class TDXLDonThuInfo
    {
        public int DonThuID { get; set; }
        public int XuLyDonID { get; set; }
        public int SoDonThu { get; set; }
        public int NhomKNID { get; set; }
        public String NoiDungDon { get; set; }
        public DateTime ThoiHan { get; set; }
        public String GhiChu { get; set; }
        public int PhanGiaiQuyetID { get; set; }
        public String TenCanBo { get; set; }
        public int LanGiaiQuyet { get; set; }

        public int LoaiKhieuTo1ID { get; set; }
    }
}
