using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucLoaiKhieuToMOD : DanhSachLoaiKhieuToThemMoiMOD
    {
        public int LoaiKhieuToID { get; set; }
    }

    public class DanhSachLoaiKhieuToThemMoiMOD
    {
        public string TenLoaiKhieuTo { get; set; }
        public int LoaiKhieuToCha { get; set; }
        public int Cap { get; set; }
        public string MappingCode { get; set; }
        public int? ThuTu { get; set; }
        public bool SuDung { get; set; }
    }


    public class DanhSachLoaiKhieuToCapNhatSuDungMOD
    {
        public int LoaiKhieuToID { get; set; }
        public bool SuDung { get; set; }
    }

    public class DanhMucLoaiKhieuToExtenMOD : DanhMucLoaiKhieuToMOD
    {
        public int? Highlight { get; set; }
        public List<DanhMucLoaiKhieuToExtenMOD> DanhMucLoaiKhieuToCon { get; set; }

    }

    public class DanhMucLoaiKhieuToCha
    {
        public int LoaiKhieuToID { get; set; }
        public string TenLoaiKhieuTo { get; set; }
        public int LoaiKhieuToCha { get; set; }
        public int Cap { get; set; }
        public string MappingCode { get; set; }
        public int? ThuTu { get; set; }
        public bool SuDung { get; set; }
    }
}
