using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucDanTocModel
    {
        public int DanTocID { get; set; }
        public string TenDanToc { get; set; }

        public DanhMucDanTocModel(int danTocID, string tenDanToc)
        {
            this.DanTocID = danTocID;
            this.TenDanToc = tenDanToc;
        } 
        
        public DanhMucDanTocModel() { }
    }

    public class AddDanhMucDanTocModel
    {
        public string? MaDanToc { get; set; }
        public string? TenDanToc { get; set; }
        public string? GhiChu { get; set; }
        public int? TrangThai { get; set; }
    }

    public class DetailDanhMucDanTocModel
    {
        public int DanTocID { get; set; }
        public string? MaDanToc { get; set; }
        public string? TenDanToc { get; set; }
        public string? GhiChu { get; set; }
        public int? TrangThai { get; set; }
    }
}
