using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class TKTheoNoiPhatSinhInfo
    {
        public string STT { get; set; }
        public string Ten { get; set; }
        public int TongSo { get; set; }
        public int SLKhieuNai { get; set; }
        public int SLToCao { get; set; }
        public int SLKienNghi { get; set; }
        public int SLPhanAnh { get; set; }

        public int Level { get; set; }
        public int DiaPhuongID { get; set; }
        public int TinhID { get; set; }
        public int HuyenID { get; set; }
        public int XaID { get; set; }
    }
}
