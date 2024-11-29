using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class NhomKNInfo
    {
        public int NhomKNID { get; set; }
        public String TenCQ { get; set; }
        public int? SoLuong { get; set; }
        public int? LoaiDoiTuongKNID { get; set; }
        public string StringLoaiDoiTuongKN { get; set; }
        public int? ThongTinNguoiDaiDien { get; set; }

        public String DiaChiCQ { get; set; }
        public Boolean? DaiDienPhapLy { get; set; }
        public Boolean? DuocUyQuyen { get; set; }
        public List<DoiTuongKNInfo> DanhSachDoiTuongKN { get; set; }
    }
}
