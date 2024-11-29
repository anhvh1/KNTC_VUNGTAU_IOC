using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class ChuyenGiaiQuyetInfo
    {
        public int XuLyDonID { get; set; }

        public int CoQuanPhanID { get; set; }

        public int CoQuanGiaiQuyetID { get; set; }

        public string GhiChu { get; set; }
        public string SoQuyetDinh { get; set; }
        public string QuyetDinh { get; set; }
        public DateTime? NgayQuyetDinh { get; set; }
        public DateTime NgayChuyen { get; set; }

        public string NgayChuyen_Str { get; set; }

        public string FileUrl { get; set; }

        public string TenFile { get; set; }

        public string TenCoQuanGQ { get; set; }

        public int SoLuong { get; set; }

        public int StateID { get; set; }

        public string TenCoQuanPhan { get; set; }

        public int CanBoID { get; set; }

        public string TenCanBo { get; set; }

        public int KetQuaID { get; set; }

        public int YKienGiaiQuyetID { get; set; }

        public bool IsBaoMat { get; set; }

        public int ChuyenGiaiQuyetID { get; set; }

        public string TenCoQuanUp { get; set; }

        public int FileID { get; set; }

        public int NhomFileID { get; set; }

        public string TenNhomFile { get; set; }

        public int ThuTuHienThiNhom { get; set; }

        public int ThuTuHienThiFile { get; set; }

        public int FileHoSoID { get; set; }
        public string GroupUID { get; set; }
    }
}
