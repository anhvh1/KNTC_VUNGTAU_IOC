using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucNhomFileModel
    {
        public int NhomFileID { get; set; }
        public string TenNhomFile { get; set; }
        public int ThuTuHienThi { get; set; }
        public bool TrangThaiSuDung { get; set; }
        
    }

    public class DanhMucFileModel
    {
        public int FileID { get; set; }
        public string TenFile { get; set; }
        public int ThuTuHienThi { get; set; }
        public string TenNhomFile { get; set; }
        public bool TrangThaiSuDung { get; set; }
        public int NhomFileID { get; set; }
        public string TenChucNang { get; set; }
    }

    public class ThamSoFileModel : ThamSoLocDanhMuc
    {
        public int? ChucNangID { get; set; }
        public int? NhomFileID { get; set; }
    }

    public class FileChiTietModel
    {
        public int FileID { get; set; }
        public string TenFile { get; set; }
        public int ThuTuHienThi { get; set; }
        public string TenNhomFile { get; set; }
        public bool TrangThaiSuDung { get; set; }
        public int NhomFileID { get; set; }
        public List<DMChucNangModel> ChucNangApDung { get; init; } = new List<DMChucNangModel>();
    }

    public class ThemFileModel
    {
        public string TenFile { get; set; }
        public int ThuTuHienThi { get; set; }
        public bool TrangThaiSuDung { get; set; }
        public int NhomFileID { get; set; }
        public List<int> ChucNangApDungID { get; init; } = new List<int>();
    }

    public class UpdateFileModel : ThemFileModel
    {
        public int FileID { get; set; }
    }

    public class XoaFileModel
    {
        public int FileID { get; set; }
    } 
    public class ChucNangFileModel
    {
        public int? ChucNangID { get; set; }
        public string TenChucNang { get; set; }
    }

    public class DMChucNangModel
    {
        public int ChucNangID { get; set; }
        public string TenChucNang { get; set; }
    }

    public class ThemChucNangModel
    {
        public int FileID { get; set; }
        public int ChucNangID { get; set; }
    }

    public class DeleteFileModel
    {
        public int NhomFileID { get; set; }
    }

    public class ThemNhomFileModel
    {
        [Required] public string TenNhomFile { get; set; }
        [Required] public int ThuTuHienThi { get; set; }
        [Required] public bool TrangThaiSuDung { get; set; }
    }
}