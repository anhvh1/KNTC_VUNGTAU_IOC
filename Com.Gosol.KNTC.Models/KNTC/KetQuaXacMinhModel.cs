using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class KetQuaXacMinhModel
    {
        public string SoDonThu { get; set; }
        public GiaoXacMinhModel GiaoXacMinh { get; set; }
        public List<BuocXacMinhInfo> BuocXacMinh { get; set; }
        public NhomKNInfo NhomKN { get; set; }
    }

    public class BaoCaoXacMinhModel
    {
        public int? TrangThaiPheDuyet { get; set; }
        public int? XuLyDonID { get; set; }
        public string NoiDung { get; set; }
        public DateTime? HanGiaiQuyet { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
        public int? LoaiQuyTrinh { get; set; }

    }
}
