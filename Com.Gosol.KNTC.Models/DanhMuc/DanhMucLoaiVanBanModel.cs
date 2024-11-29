using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucLoaiVanBanModel
    {
        public int LoaiVanBanID { get; set; }
        [Required] public string TenVanBan { get; set; }
        public string MaVanBan { get; set; }
        public string GhiChu { get; set; }
        public bool? TrangThai { get; set; }
    }

    public class ThemDanhMucLoaiVanBanModel
    {
        [Required] public string TenVanBan { get; set; }
        public string MaVanBan { get; set; }
        public string GhiChu { get; set; }
        public bool? TrangThai { get; set; }
    }

    public class XoaThemDanhMucLoaiVanBanModel
    {
        public int LoaiVanBanID { get; set; }
    }
}