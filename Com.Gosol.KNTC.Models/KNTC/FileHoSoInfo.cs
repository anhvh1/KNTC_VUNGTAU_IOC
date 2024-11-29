using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class FileHoSoInfo
    {
        public int FileHoSoID { get; set; }
        public int DonDocID { get; set; }
        public int GiaHanGiaiQuyetID { get; set; }
        public String TenFile { get; set; }
        public String TomTat { get; set; }
        public DateTime NgayUp { get; set; }
        public string NgayUp_str { get; set; }
        public int NguoiUp { get; set; }
        public string NgayUps { get; set; }
        public String FileURL { get; set; }
        public int XuLyDonID { get; set; }
        public int DonThuID { get; set; }
        public bool IsBaoMat { get; set; }
        public int ChuyenGiaiQuyetID { get; set; }
        public int KetQuaID { get; set; }
        public int FileRutDonID { get; set; }
        public int ThiHanhID { get; set; }
        public string FileBase64 { get; set; }
        public int CanBoID { get; set; }
        public string TenCanBo { get; set; }
        public int XemTaiLieuMat { get; set; }
        public string TenCoQuanUp { get; set; }
        public string GroupUID { get; set; }

        public bool IsMaHoa { get; set; }
        public int LoaiFile { get; set; }
        public int YKienGiaiQuyetID { get; set; }
        public int FilePhanXuLyID { get; set; }
        public string IsBaoMatString { get; set; }
        public int TheoDoiXuLyID { get; set; }
        public int DMBuocXacMinhID { get; set; }
        public int YKienXuLyID { get; set; }

        public int FileID { get; set; }
        public int NhomFileID { get; set; }
        public string TenNhomFile { get; set; }
        public int ThuTuHienThiNhom { get; set; }
        public int ThuTuHienThiFile { get; set; }
        public string CANBOTHEM { get; set; }
        public string NDFILE { get; set; }
        public int Type { get; set; }
        public DateTime? NgayCapNhat { get; set; }
    }
}
