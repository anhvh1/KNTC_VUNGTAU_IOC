using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DuLieuDongBoModel
    {
        public List<DonThuInfo> DanhSachDonThu { get; set; }
        public List<DonThuInfo> LichSuDongBo { get; set; }
    }

    //public class DuLieuMapping
    //{
    //    public string TypeApi { get; set; }
    //    public List<ApiGateway.objMapping.ObjMapInfo> DuLieu { get; set; }
    //}
    
}
