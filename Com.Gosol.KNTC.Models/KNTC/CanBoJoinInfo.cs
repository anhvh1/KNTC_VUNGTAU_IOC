using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class CanBoJoinInfo
    {
        public int CanBoID { get; set; }

        public string TenCanBo { get; set; }

        public DateTime NgaySinh { get; set; }

        public int GioiTinh { get; set; }

        public string DiaChi { get; set; }

        public int CoQuanID { get; set; }

        public int QuyenKy { get; set; }

        public string TenCoQuan { get; set; }

        public string Email { get; set; }

        public string DienThoai { get; set; }

        public int NguoiDungID { get; set; }

        public int QuanTriDonVi { get; set; }

        public bool XemTaiLieuMat { get; set; }

        public string XemTaiLieuMat_String { get; set; }
    }
}
