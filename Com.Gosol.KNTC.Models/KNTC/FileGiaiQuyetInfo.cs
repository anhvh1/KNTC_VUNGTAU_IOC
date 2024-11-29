using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class FileGiaiQuyetInfo
    {
        public int FileGiaiQuyetID { get; set; }
        public string TenFile { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public string DuongDanFile { get; set; }
        public int TheoDoiXuLyID { get; set; }
        public string NgayCapNhats { get; set; }
        public bool IsBaoMat { get; set; }
        public int NguoiUp { get; set; }
        public int FileID { get; set; }
    }
}
