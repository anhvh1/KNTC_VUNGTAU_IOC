using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class LichSuTraCuuModel
    {
        public int TraCuuID { get; set; }
        public string SoDonThu { get; set; }
        public string ChuDon { get; set; }
        public string CCCD { get; set; }
        public DateTime? NgayNopDon { get; set; }
        public DateTime? NgayTraCuu { get; set; }
    }
}
