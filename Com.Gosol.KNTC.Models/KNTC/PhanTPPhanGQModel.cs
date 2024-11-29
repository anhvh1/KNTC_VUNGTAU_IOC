using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class PhanTPPhanGQModel
    {
        public int PhanTPPhanGQID { get; set; }
        public int XuLyDonID { get; set; }
        public int PhongBanID { get; set; }
        public int CanBoID { get; set; }
        public DateTime NgayPhanGQ { get; set; }
        public string TenCanBo { get; set; }
    }
}
