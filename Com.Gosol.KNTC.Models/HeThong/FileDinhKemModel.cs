using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.HeThong
{
    public class FileDinhKemModel
    {
        public string TenFileGoc;

        public int FileID { get; set; }
        public int? NghiepVuID { get; set; }
        public int? DMTenFileID { get; set; }
        public string TenFile { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int? NguoiCapNhat { get; set; }
        public int FileType { get; set; }
        public int? TrangThai { get; set; }
        public string FolderPath { get; set; }
        public string FileUrl { get; set; }
        public string NoiDung { get; set; }
        public bool? IsBaoMat { get; set; }
        public bool? IsMaHoa { get; set; }
        public string GroupUID { get; set; }
        public string TenFileHeThong { get; set; }
        public FileDinhKemModel()
        {

        }
        public FileDinhKemModel(int FileID)
        {
            this.FileID = FileID;
        }
    }

    public class FileLogInfo
    {
        public int FileID { get; set; }
        public int LoaiLog { get; set; }
        public int LoaiFile { get; set; }
        public bool IsMaHoa { get; set; }
        public bool IsBaoMat { get; set; }
    }



    #region file sau khi triển khai vĩnh phúc, cần sửa lại cho đúng và đủ
    /// <summary>
    /// Creted by AnhVH
    /// ,05/04/2024
    /// tạo mới cho cấu trúc mới của file
    /// </summary>
    public class TaiLieuModel
    {
        public string GroupUID { get; set; }

        public string TenTaiLieu { get; set; }


    }
    public class FileModel
    {
        public int FileID { get; set; }
        public string TenFile { get; set; }
        public string FileBase64 { get; set; }
        public DateTime NguoiUp { get; set; }


    }

    #endregion
}

