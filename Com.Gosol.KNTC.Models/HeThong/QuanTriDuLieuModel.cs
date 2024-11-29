using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.HeThong
{
    public class QuanTriDuLieuModel
    {
        public string TenFile { get; set; }
        public QuanTriDuLieuModel(string tenFile) { this.TenFile = tenFile; }
    }

    public class FileTextMOD
    {
        public string ThoiGian { get; set; }
        public string ThaoTac { get; set; }
        public string NguoiThucHien { get; set; }
    }

}
