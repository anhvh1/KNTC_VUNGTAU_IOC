using Com.Gosol.KNTC.Models.HeThong;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class RutDon_V2Model
    {
        public int XuLyDonID { get; set; }
        public string LyDoRutDon { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; } 
    }
    public class ChiTietRutDon 
    {      
        public ChiTietRutDon()
        {
            DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
        }
        public int RutDonID { get; set; }
        public string LyDoRutDon { get; set; }
        public int CanBoID { get; set; }
        public string TenCanBo { get; set; }
        public DateTime? NgayCapNhap { get; set; }
        public int XuLyDonID { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; } 
    }
  

}
