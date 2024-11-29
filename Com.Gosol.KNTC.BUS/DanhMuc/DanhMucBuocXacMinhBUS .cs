using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Document = Spire.Doc.Document;


// Create by NamNH - 24/10/2022
namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucBuocXacMinhBUS
    {
        private DanhMucBuocXacMinhDAL danhMucBuocXacMinhDAL;
        public DanhMucBuocXacMinhBUS()
        {
            danhMucBuocXacMinhDAL = new DanhMucBuocXacMinhDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc p, int? LoaiDon)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucBuocXacMinhDAL.DanhSach(p, LoaiDon);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTiet(int? BuocXacMinhID)
        {
            var Result = new BaseResultModel();
            if (BuocXacMinhID == null || BuocXacMinhID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = danhMucBuocXacMinhDAL.ChiTiet(BuocXacMinhID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel ThemMoi(DanhMucBuocXacMinhThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin bước xác minh cần thêm!";
                    return Result;
                }
                else if (item.TenBuoc == null || string.IsNullOrEmpty(item.TenBuoc))
                {
                    Result.Status = 0;
                    Result.Message = "Tên bước xác minh không được trống";
                    return Result;
                }
                else if (item.TenBuoc.Length > 200)
                {
                    Result.Status = 0;
                    Result.Message = "Tên bước xác minh không quá 200 ký tự";
                    return Result;
                }
                else if (item.LoaiDon == null || item.LoaiDon < 0)
                {
                    Result.Status = 0;
                    Result.Message = "Loại đơn không được trống";
                    return Result;
                }


                // thực hiện thêm mới
                Result = danhMucBuocXacMinhDAL.ThemMoi(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel CheckFile(IFormFile file)
        {
            BaseResultModel model = new BaseResultModel();
            /*if (file.ContentType != "application/pdf")
            {
                model.Status = -1;
                model.Message = "Chỉ được upload file pdf";
                return model;
            }*/

            if ((file.Length / 1024) > 10240)
            {
                model.Status = -1;
                model.Message = "Dung lượng file tối đa là 10MB";
                return model;
            }

            model.Status = 1;
            return model;
        }

        public BaseResultModel CapNhat(DanhMucBuocXacMinhCapNhatMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null || item.BuocXacMinhID == null || item.BuocXacMinhID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin bước xác minh cần cập nhật!";
                    return Result;
                }
                else if (item.TenBuoc == null || string.IsNullOrEmpty(item.TenBuoc))
                {
                    Result.Status = 0;
                    Result.Message = "Tên bước xác minh không được trống";
                    return Result;
                }
                else if (item.TenBuoc.Length > 200)
                {
                    Result.Status = 0;
                    Result.Message = "Tên bước xác minh không quá 200 ký tự";
                    return Result;
                }
                else if (item.LoaiDon == null || item.LoaiDon < 0)
                {
                    Result.Status = 0;
                    Result.Message = "Loại đơn không được trống";
                    return Result;
                }

                // thực hiện cập nhật 
                Result = danhMucBuocXacMinhDAL.CapNhat(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel Xoa(int? BuocXacMinhID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (BuocXacMinhID == null || BuocXacMinhID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                //kiểm tra BuocXacMinhID có tồn tại trong DB không
                var checkItem = danhMucBuocXacMinhDAL.KiemTraTonTai("", 3, BuocXacMinhID);
                if (checkItem == false)
                {
                    Result.Status = 0;
                    Result.Message = "Danh mục phân tích kết quả không tồn tại";
                    return Result;
                }

                // Kiểm tra dữ liệu đã phát sinh ở các chức khác chưa - làm sau

                //Thực hiện xóa
                Result = danhMucBuocXacMinhDAL.Xoa(BuocXacMinhID.Value);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                //Result.Message = Constant.API_Error_System;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public async Task<BaseResultModel> SaveFile(IFormFile file, string folderName)
        {
            var Result = new BaseResultModel();
            var fileName = $"{Path.GetFileName(file.FileName)}";
            string route = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(route))
            {
                Directory.CreateDirectory(route);
            }

            string fileRoute = Path.Combine(route, fileName);

            using (FileStream fileStream = File.Create(fileRoute))
            {
                try
                {
                    await file.OpenReadStream().CopyToAsync(fileStream);
                    Result.Status = 1;
                    Result.Message = "Successful!";
                }catch (Exception ex)
                {
                    Result.Status = -1;
                    Result.Message = "Error!";
                }
            }

            return Result;
        }


        //================================================================//
        public BaseResultModel DanhSachFile(int? DMBuocXacMinhID)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucBuocXacMinhDAL.DanhSachFile(DMBuocXacMinhID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel DanhSachFileID(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucBuocXacMinhDAL.DanhSachFileID(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTietFile(int? FileDanhMucBuocXacMinhID, string rootPath)
        {
            var Result = new BaseResultModel();
            if (FileDanhMucBuocXacMinhID == null || FileDanhMucBuocXacMinhID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = danhMucBuocXacMinhDAL.ChiTietFile(FileDanhMucBuocXacMinhID, "");
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ThemMoiFile(ThemMoiFileMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucBuocXacMinhDAL.ThemMoiFile(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel CapNhatFile(CapNhatFileMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucBuocXacMinhDAL.CapNhatFile(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel XoaFile(int FileDanhMucBuocXacMinhID)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucBuocXacMinhDAL.XoaFile(FileDanhMucBuocXacMinhID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }
    }
}
