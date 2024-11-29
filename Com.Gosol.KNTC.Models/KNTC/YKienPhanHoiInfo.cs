using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class VaiTroGiaiQuyetInfo
    {
        public int XuLyDonID { get; set; }
        public int CanBoID { get; set; }
        public int VaiTro { get; set; }
        public int ChuyenGiaiQuyetID { get; set; }
        public int HoatDong { get; set; }
        public string TenCanBo { get; set; }
    }
    public class YKienPhanHoiInfo
    {
        public int XuLyDonID { get; set; }
        public int CanBoID { get; set; }
        public string TenCanBo { get; set; }
        public string NoiDungPhanHoi { get; set; }

    }
}
