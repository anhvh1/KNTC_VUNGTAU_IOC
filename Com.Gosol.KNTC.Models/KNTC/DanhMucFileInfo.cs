using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DanhMucFileInfo
    {
        public int FileID { get; set; }
        public string TenFile { get; set; }
        public int ThuTuHienThi { get; set; }
        public bool TrangThaiSuDung { get; set; }
        public int NhomFileID { get; set; }
        public string TenNhomFile { get; set; }
        public int ChucNangID { get; set; }
        public string TenChucNang { get; set; }
        public List<int> DanhSachChucNangID { get; set; }
        public List<string> DanhSachTenChucNang { get; set; }
        public int FileChucNangID { get; set; }
    }

    public class DanhMucNhomFileInfo
    {
        public int NhomFileID { get; set; }
        public string TenNhomFile { get; set; }
        public int ThuTuHienThi { get; set; }
        public bool TrangThaiSuDung { get; set; }
    }
 
    public class DanhMucNhomFilePatialInfo
    {
        public List<DanhMucNhomFileInfo> DanhSachFileInfo { get; set; }
        public int TotalRow { get; set; }
    }

    public class DanhMucFilePatialInfo
    {
        public List<DanhMucFileInfo> DanhSachFileInfo { get; set; }
        public int TotalRow { get; set; }
    }

    public class DanhMucFileChucNangApDungInfo
    {
        public int FileChucNangID { get; set; }
        public int FileID { get; set; }
        public int ChucNangID { get; set; }
        public string TenChucNang { get; set; }
    }
}
