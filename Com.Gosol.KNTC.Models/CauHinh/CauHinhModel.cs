using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.HeThong
{
    public class CauHinhItemModel
    {
        public int id { get; set; }
        public string MaSo { get; set; }
        public string GiaTri { get; set; }
    }
    public class CauHinhModel : CauHinhItemModel
    {
        //Là nhóm cha của cột Nhóm: 1 - Cấu hình cấu trúc layout, 2 - Các màu cơ bản, 3 - Thông tin đơn vị sử dụng phần mềm
        public string PhanLoai { get; set; }
        //là con của cột Phân loại, là chi tiết của phân loại
        public string Nhom { get; set; }
    }



}
