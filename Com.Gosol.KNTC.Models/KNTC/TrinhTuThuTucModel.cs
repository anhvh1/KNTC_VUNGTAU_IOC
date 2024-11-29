using Com.Gosol.KNTC.Models.HeThong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class TrinhTuThuTucModel
    {
        public int TrinhTuThuTucID { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? NguoiTaoID { get; set; }
        public bool? Public { get; set; }
        public string TenNguoiTao { get; set; }
        public FileDinhKemModel Thumbnail { get; set; }
        public List<FileDinhKemModel> DanhSachFileDinhKem { get; set; }
        public List<int> DanhSachFileDinhKemID { get; set; }
        
    }
}
