using Com.Gosol.KNTC.Models.HeThong;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Model.HeThong
{
    public class ChucNangInfo
    {
        public string TenChucNang { get; set; }
        public int ChucNangid { get; set; }
        public int Xem { get; set; }
        public int Them { get; set; }
        public int Sua { get; set; }
        public int Xoa { get; set; }
        public int Quyen { get; set; }
        public ChucNangInfo()
        {
        }

        public ChucNangInfo(int chucNangid, string tenChucNang, int quyen)
        {
            ChucNangid = chucNangid;
            TenChucNang = tenChucNang;
            Quyen = quyen;
            Xem = Them = Sua = Xoa = 0;         
            switch (quyen)
            {
                case 15:
                    Xem = 1;
                    Them = 1;
                    Sua = 1;
                    Xoa = 1;
                    break;
                case 14:
                    Them = 1;
                    Sua = 1;
                    Xoa = 1;
                    break;
                case 13:
                    Xem = 1;
                    Sua = 1;
                    Xoa = 1;
                    break;
                case 12:
                    Sua = 1;
                    Xoa = 1;
                    break;
                case 11:
                    Xem = 1;
                    Them = 1;
                    Xoa = 1;
                    break;
                case 10:
                    Them = 1;
                    Xoa = 1;
                    break;
                case 9:
                    Xem = 1;
                    Xoa = 1;
                    break;
                case 8:
                    Xoa = 1;
                    break;
                case 7:
                    Xem = 1;
                    Them = 1;
                    Sua = 1;
                    break;
                case 6:
                    Them = 1;
                    Sua = 1;
                    break;
                case 5:
                    Xem = 1;
                    Sua = 1;
                    break;
                case 4:
                    Sua = 1;
                    break;
                case 3:
                    Xem = 1;
                    Them = 1;
                    break;
                case 2:
                    Them = 1;
                    break;
                case 1:
                    Xem = 1;                        
                    break;             
            }

        }

        public class MenuModel
        {
            public int? MenuID { get; set; }
            public string TenMenu { get; set; }
            public string MaMenu { get; set; }
            public int? MenuChaID { get; set; }
            public string TenMenuCha { get; set; }
            public int? ThuTuSapXep { get; set; }
            public string Icon { get; set; }
            public bool? HienThi { get; set; }
            public bool? ViewOnly { get; set; }
            public bool? isHover { get; set; }
            public List<MenuModel> Children { get; set; }
        }
    }
    
    public class DanhMucChucNangModel
    {
        public int ChucNangID { get; set; }
        public string TenChucNang { get; set; }
        public int? ChucNangChaID { get; set; } = null;
        public string MaChucNang { get; set; }
        public int ThuTuSapXep { get; set; }
        public bool HienThiTrenMenu { get; set; }
        public string Icon { get; set; }
        public string TT { get; set; }
    }

    public class ChucNangHDSDModel 
    {
        public int ChucNangID { get; set; }
        public string TenChucNang { get; set; }
        public bool Highlight { get; set; }
    }

    public class DanhMucChucNangThemMoi
    {
        [Required]
        public string TenChucNang { get; set; }
        public int? ChucNangChaID { get; set; }
        public string MaChucNang { get; set; }
        public int? ThuTuSapXep { get; set; }
        public bool? HienThiTrenMenu { get; set; }
        public string Icon { get; set; }
    } 
    
    public class DanhMucChucNangSua : DanhMucChucNangThemMoi
    {
        [Required]
        public int ChucNangID { get; set; }
    }

    public class XoaChucNang
    {
        [Required]
        public int ChucNangID { get; set; }
    }
}
