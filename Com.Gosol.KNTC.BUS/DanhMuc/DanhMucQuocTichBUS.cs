using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Create by AnhVH - 12/10/2022
namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucQuocTichBUS
    {
        private DanhMucQuocTichDAL danhMucQuocTichDAL;
        public DanhMucQuocTichBUS()
        {
            danhMucQuocTichDAL = new DanhMucQuocTichDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucQuocTichDAL.DanhSach(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTiet(int? QuocTichID)
        {
            var Result = new BaseResultModel();
            if (QuocTichID == null || QuocTichID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = danhMucQuocTichDAL.ChiTiet(QuocTichID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel ThemMoi(DanhMucQuocTichThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin quốc tịch cần thêm!";
                    return Result;
                }
                else if (item == null || item.MaQuocTich == null || string.IsNullOrEmpty(item.MaQuocTich))
                {
                    Result.Status = 0;
                    Result.Message = "Mã quốc tịch không được trống!";
                    return Result;
                }
                else if (item.MaQuocTich.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã quốc tịch 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenQuocTich == null || string.IsNullOrEmpty(item.TenQuocTich))
                {
                    Result.Status = 0;
                    Result.Message = "Tên quốc tịch không được trống";
                    return Result;
                }
                else if (item.TenQuocTich.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên quốc tịch 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucQuocTichDAL.KiemTraTonTai(item.MaQuocTich.Trim(), 1, null))
                {
                    Result.Status = 0;
                    Result.Message = "Mã quốc tịch đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucQuocTichDAL.KiemTraTonTai(item.TenQuocTich.Trim(), 2, null))
                {
                    Result.Status = 0;
                    Result.Message = "Tên quốc tịch đã tồn tại";
                    return Result;
                }

                // thực hiện thêm mới
                Result = danhMucQuocTichDAL.ThemMoi(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel CapNhat(DanhMucQuocTichMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null || item.QuocTichID is null || item.QuocTichID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin quốc tịch cần cập nhật!";
                    return Result;
                }
                else if (item == null || item.MaQuocTich == null || string.IsNullOrEmpty(item.MaQuocTich))
                {
                    Result.Status = 0;
                    Result.Message = "Mã quốc tịch không được trống!";
                    return Result;
                }
                else if (item.MaQuocTich.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã quốc tịch 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenQuocTich == null || string.IsNullOrEmpty(item.TenQuocTich))
                {
                    Result.Status = 0;
                    Result.Message = "Tên quốc tịch không được trống";
                    return Result;
                }
                else if (item.TenQuocTich.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên quốc tịch 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucQuocTichDAL.KiemTraTonTai(item.MaQuocTich.Trim(), 1, item.QuocTichID))
                {
                    Result.Status = 0;
                    Result.Message = "Mã quốc tịch đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucQuocTichDAL.KiemTraTonTai(item.TenQuocTich.Trim(), 2, item.QuocTichID))
                {
                    Result.Status = 0;
                    Result.Message = "Tên quốc tịch đã tồn tại";
                    return Result;
                }

                // thực hiện cập nhật 
                Result = danhMucQuocTichDAL.CapNhat(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel Xoa(int? QuocTichID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (QuocTichID == null || QuocTichID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                //kiểm tra QuocTichID có tồn tại trong DB không
                var checkItem = danhMucQuocTichDAL.KiemTraTonTai("", 3, QuocTichID);
                if (checkItem == false)
                {
                    Result.Status = 0;
                    Result.Message = "Danh mục quốc tịch không tồn tại";
                    return Result;
                }

                // Kiểm tra dữ liệu đã phát sinh ở các chức khác chưa - làm sau

                //Thực hiện xóa
                Result = danhMucQuocTichDAL.Xoa(QuocTichID.Value);
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


    }
}
