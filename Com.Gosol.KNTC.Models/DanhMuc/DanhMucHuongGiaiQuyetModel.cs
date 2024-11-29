using System.ComponentModel.DataAnnotations;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucHuongGiaiQuyetModel
    {
        public int HuongGiaiQuyetID { get; set; }
        [Required]
        public string TenHuongGiaiQuyet { get; set; }
        public string MaHuongGiaiQuyet { get; set; }
        public string GhiChu { get; set; }
        public bool? TrangThai { get; set; }
    }

    public class ThemDanhMucHuongGiaiQuyetModel
    {
        [Required]
        public string TenHuongGiaiQuyet { get; set; }
        public string MaHuongGiaiQuyet { get; set; }
        public string GhiChu { get; set; }
        public bool? TrangThai { get; set; }
    }

    public class XoaDanhMucHuongGiaiQuyetModel
    {
        public int HuongGiaiQuyetID { get; set; }
    }

}
