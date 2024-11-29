using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class PhanXuLyInfo
    {
        public int PhanXuLyID { get; set; }
        public int XuLyDonID { get; set; }
        public int PhongBanID { get; set; }
        public int CanBoID { get; set; }
        public DateTime NgayPhan { get; set; }
        public string GhiChu { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
    }
}
