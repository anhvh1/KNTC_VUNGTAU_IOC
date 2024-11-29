using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using System;
using System.IO;
using System.Text;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;


//Create by TienKM - 13/10/2022
namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucBieuMauBUS
    {
        private DanhMucBieuMauDAL _danhMucBieuMauDAL;

        public DanhMucBieuMauBUS()
        {
            _danhMucBieuMauDAL = new DanhMucBieuMauDAL();
        }

        public BaseResultModel DanhSach(DanhMucBieuMauThamSo thamSo, string root)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucBieuMauDAL.DanhSach(thamSo, root);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel DanhSachCap()
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucBieuMauDAL.DanhSachCap();
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ChiTiet(int BieuMauID, string serverPath)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucBieuMauDAL.ChiTiet(BieuMauID, serverPath);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel LichSuChiTiet(int BieuMauID)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucBieuMauDAL.LichSuBieuMauChiTiet(BieuMauID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel SuaBieuMau(SuaBieuMauModel bieuMau)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucBieuMauDAL.SuaBieuMau(bieuMau, null);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.ERR_UPDATE;
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel VaidateSua(SuaBieuMauModel bieuMau)
        {
            var Result = new BaseResultModel();

            if (!_danhMucBieuMauDAL.KiemTraBieuMauTonTai(bieuMau.BieuMauID, null, null, null))
            {
                Result.Status = -1;
                Result.Message = $"Biểu mẫu không tồn tại";
                return Result;
            }

            if (bieuMau.TenBieuMau == null || bieuMau.TenBieuMau.Length < 1)
            {
                Result.Status = 0;
                Result.Message = "Tên biểu mẫu không được để trống";
                return Result;
            }

            if (HasUnicodeChar(bieuMau.MaPhieuIn) || bieuMau.MaPhieuIn.Contains(" "))
            {
                Result.Status = 0;
                Result.Message = "Mã biểu mẫu không được có ký tự unicode, khoảng trắng";
                return Result;
            }

            Result.Status = 1;
            return Result;
        }
        
        public BaseResultModel VaidateThemMoi(ThemBieuMauModel bieuMau)
        {
            var Result = new BaseResultModel();

            if (string.IsNullOrEmpty(bieuMau.TenBieuMau))
            {
                Result.Status = 0;
                Result.Message = "Tên biểu mẫu không được để trống";
                return Result;
            }

            if (HasUnicodeChar(bieuMau.MaPhieuIn) || bieuMau.MaPhieuIn.Contains(" "))
            {
                Result.Status = 0;
                Result.Message = "Mã biểu mẫu không được có ký tự unicode, khoảng trắng";
                return Result;
            }

            Result.Status = 1;
            return Result;
        }
        public BaseResultModel VaidateFile(IFormFile file)
        {
            var Result = new BaseResultModel();
            if (file.ContentType != "application/msword" && file.ContentType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                Result.Status = -1;
                Result.Message = "Chỉ hỗ trợ upload file .doc, .docx";

                return Result;
            }    

            Result.Status = 1;
            return Result;
        }

        public BaseResultModel ThemBieuMau(ThemBieuMauModel bieuMau)
        {
            var Result = new BaseResultModel();

            //Kiểm tra có trùng tên biểu mẫu và cấp
            if (_danhMucBieuMauDAL.KiemTraBieuMauTonTai(null, bieuMau.TenBieuMau, null, bieuMau.CapID))
            {
                Result.Status = 0;
                Result.Message = $"Tên biểu mẫu đã tồn tại";
                return Result;
            }

            //Kiểm tra trùng MaPhieuIn
            if (_danhMucBieuMauDAL.KiemTraBieuMauTonTai(null, null, bieuMau.MaPhieuIn, null))
            {
                Result.Status = 0;
                Result.Message = $"Mã phiếu in đã tồn tại";
                return Result;
            }

            try
            {
                Result = _danhMucBieuMauDAL.ThemBieuMau(bieuMau);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.ERR_INSERT;//Constant.ERR_INSERT
                Result.Data = null;
            }

            return Result;
        }

        private bool HasUnicodeChar(string? input)
        {
            if (input == null) return false;

            var asciiBytesCount = Encoding.ASCII.GetByteCount(input);
            var unicodBytesCount = Encoding.UTF8.GetByteCount(input);
            return asciiBytesCount != unicodBytesCount;
        }

        public BaseResultModel XoaBieuMau(int bieuMauID, string rootPath)
        {
            var Result = new BaseResultModel();
            try
            {
                if (_danhMucBieuMauDAL.KiemTraID(bieuMauID))
                {
                    var model = (DanhMucBieuMauChiTietModel)ChiTiet(bieuMauID, "").Data;
                    if (model.FileUrl != null && model.FileUrl != "")
                    {
                        var path = rootPath + "\\UploadFiles\\FileBieuMau\\" + model.FileUrl;
                        System.IO.File.Delete(path);
                    }

                    Result = _danhMucBieuMauDAL.XoaBieuMau(bieuMauID);
                }
                else
                {
                    Result.Status = 0;
                    Result.Message = "Biểu mẫu không tồn tại";
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.ERR_DELETE;
                Result.Data = null;
            }

            return Result;
        }
    }
}