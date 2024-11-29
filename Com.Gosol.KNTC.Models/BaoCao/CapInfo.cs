using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.BaoCao
{
    public class CapInfo
    {
        public int CapID { get; set; }
        public string TenCap { get; set; }
        public string CapQuanLy { get; set; }
        public int ThuTu { get; set; }
    }

    public class PhamViModel
    {
        public string Key { get; set; }
        public int Value { get; set; }     
        public PhamViModel(string Key, int Value)
        {
            this.Key = Key;
            this.Value = Value;
        } 
    }
}
