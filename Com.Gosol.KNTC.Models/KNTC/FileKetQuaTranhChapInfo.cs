using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class KetQuaTranhChapInfo
    {
        public int KetQuaTranhChapID { get; set; }
        public int XuLyDonID { get; set; }
        public int CoQuanID { get; set; }
        public int CanBoID { get; set; }
        public string NDHoaGiai { get; set; }
        public string KetQuaHoaGiai { get; set; }
        public string FileUrl { get; set; }
        public DateTime NgayRaKQ { get; set; }
        public string NgayRaKQ_Str { get; set; }
        public string TenCanBo { get; set; }

        public List<HoiDongHoaGiaiInfo> lstHoiDong { get; set; }
        public List<FileKetQuaTranhChapInfo> lstFileKQ { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
    }

    public class HoiDongHoaGiaiInfo
    {
        public int HoiDongHoaGiaiID { get; set; }
        public int KetQuaTranhChapID { get; set; }
        public string TenCanBo { get; set; }
        public string NhiemVu { get; set; }
    }

    public class FileKetQuaTranhChapInfo
    {
        public int KetQuaTranhChapID { get; set; }
        public int CanBoUpID { get; set; }
        public string TenFile { get; set; }
        public string TomTat { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public string NgayCapNhat_Str { get; set; }
        public string FileUrl { get; set; }
        public int LoaiLog { get; set; }
        public int LoaiFile { get; set; }
        public Boolean IsMaHoa { get; set; }
        public Boolean IsBaoMat { get; set; }
        public string TenCanBo { get; set; }

        public int FileID { get; set; }
        public int NhomFileID { get; set; }
        public string TenNhomFile { get; set; }
        public int ThuTuHienThiNhom { get; set; }
        public int ThuTuHienThiFile { get; set; }
        public string GroupUID { get; set; }
    }
}
