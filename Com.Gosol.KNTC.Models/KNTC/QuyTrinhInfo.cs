using Com.Gosol.KNTC.Models.HeThong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class QuyTrinhInfo
    {
        public int QuyTrinhID { get; set; }
        public int CapID { get; set; }
        public string TenQuyTrinh { get; set; }
        public string ImgUrl { get; set; }
        public string TenCap { get; set; }
        public FileDinhKemModel FileDinhKem { get; set; }
    }
}
