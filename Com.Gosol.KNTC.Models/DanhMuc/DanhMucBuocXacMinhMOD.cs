using Com.Gosol.KNTC.Models.HeThong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Create by NamNH - 24/10/2022
namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucBuocXacMinhMOD
    {
        public int? BuocXacMinhID { get; set; }
        public string TenBuoc { get; set; }
        public int? LoaiDon { get; set; }
        public bool IsDinhKemFile { get; set; }
        public string GhiChu { get; set; }
        public string TenFile { get; set; }
        public int? OrderBy { get; set; }
        public bool SuDung { get; set; }
    }

    public class DanhMucBuocXacMinhThemMoiMOD
    {
        public string TenBuoc { get; set; }
        public int? LoaiDon { get; set; }
        public bool IsDinhKemFile { get; set; }
        public string GhiChu { get; set; }
        public int? OrderBy { get; set; }
        public bool SuDung { get; set; }
        public List<FileDinhKemModel> FileMau { get; set; }
    }

    public class DanhMucBuocXacMinhCapNhatMOD
    {
        public int? BuocXacMinhID { get; set; }
        public string TenBuoc { get; set; }
        public int? LoaiDon { get; set; }
        public bool IsDinhKemFile { get; set; }
        public string GhiChu { get; set; }
        public int? OrderBy { get; set; }
        public bool SuDung { get; set; }
        public List<FileDinhKemModel> FileMau { get; set; }
    }

    public class FileMauMOD : FileMauThemMoiMOD
    {
        public int? FileDanhMucBuocXacMinhID { get; set; }
    }

    public class FileMauThemMoiMOD
    {
        public string TenFile { get; set; }
        public string NgayUp { get; set; }
        public string FileURL { get; set; }
        //public int Loai { get; set; }
    }

    public class CapNhatFileMOD
    {
        public int FileDanhMucBuocXacMinhID { get; set; }
        public string TenFile { get; set; }
        public string TenFileGoc { get; set; }
        public string NgayUp { get; set; }
        public string FileURL { get; set; }
    }

    public class ThemMoiFileMauMOD
    {
        public List<ThemMoiFileMOD> themMoiFileMOD { get; set; }
    }

    public class ThemMoiFileMOD
    {
        public string TenFile { get; set; }
        public string? NgayUp { get; set; }
        public string TenFileGoc { get; set; }
        public string FileURL { get; set; }
        public int DMBuocXacMinhID { get; set; }
    }

    public class FileDMBuocXacMinhMOD
    {
        public int FileDanhMucBuocXacMinhID { get; set; }
        public string TenFile { get; set; }
        public string NgayUp { get; set; }
        public int NguoiUp { get; set; }
        public string FileURL { get; set; }
    }
}

