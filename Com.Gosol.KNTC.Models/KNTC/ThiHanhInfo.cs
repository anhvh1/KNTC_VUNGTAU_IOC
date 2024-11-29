using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class ThiHanhInfo
    {
        public int ThiHanhID { get; set; }
        public DateTime NgayThiHanh { get; set; }
        public string NgayThiHanhStr { get; set; }
        public string CQThiHanh { get; set; }
        public int TienDaThu { get; set; }
        public int DatDaThu { get; set; }
        public int XuLyDonID { get; set; }
        public int KetQuaID { get; set; }
        public String TenLoaiKetQua { get; set; }
        public String TenCoQuanThiHanh { get; set; }

        public int CoQuanTHID { get; set; }
        public string FileKetQua { get; set; }
        public string FileDinhKem { get; set; }
        public List<FileHoSoInfo> lstFileTH { get; set; }

    }

}
