using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class CaNhanBiKNInfo
    {
        public int? CaNhanBiKNID { get; set; }
        public String NgheNghiep { get; set; }
        public string NoiCongTac { get; set; }
        public string ChucVu { get; set; }
        public int? ChucVuID { get; set; }
        public int? GioiTinh { get; set; }
        public int? QuocTichID { get; set; }
        public int? DanTocID { get; set; }
        public int DoiTuongBiKNID { get; set; }
        public string TenChucVu { get; set; }
        public string TenDanToc { get; set; }
        public string TenQuocTich { get; set; }
    }
}
