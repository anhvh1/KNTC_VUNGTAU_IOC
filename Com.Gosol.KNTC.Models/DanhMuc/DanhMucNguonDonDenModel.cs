using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucNguonDonDenModel
    {
        public int NguonDonDenID { get; set; }
        [Required]
        public string TenNguonDonDen { get; set; }
        public string MaNguonDonDen { get; set; }
        public string GhiChu { get; set; }
        public bool? TrangThai { get; set; } 
    }
    
    public class ThemDanhMucNguonDonDenModel
    {
        [Required]
        public string TenNguonDonDen { get; set; }
        public string MaNguonDonDen { get; set; }
        public string GhiChu { get; set; }
        public bool? TrangThai { get; set; }
    }

    public class XoaDanhMucNguonDonDenModel
    {
        public int NguonDonDenID { get; set; }
    }
}
