using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DongBo_LogInfo
    {
        public int id { get; set; }
        public int XyLyDonID { get; set; }
        public int CanBoID { get; set; }
        public string TenBuocDongBo { get; set; }
        public int TrangThai { get; set; }
        public DateTime CreateDate { get; set; }
        public int TrangThaiDonThu { get; set; }
        public int DongBoID { get; set; }
        public List<DonThuInfo> DanhSachDonThu { get; set; }

        /*dong bo*/
        public int LoaiDongBo { get; set; }
        public string NgayTrongTuan { get; set; }
        public string GioDongBo { get; set; }
        public string URl { get; set; }
        public string Password { get; set; }
    }
}
