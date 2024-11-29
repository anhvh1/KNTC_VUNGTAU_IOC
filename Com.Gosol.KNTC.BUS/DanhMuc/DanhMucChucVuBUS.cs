using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Create by NamHN - 13/10/2022

namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucChucVuBUS
    {
        private DanhMucChucVuDAL danhMucChucVuDAL;
        public DanhMucChucVuBUS()
        {
            danhMucChucVuDAL = new DanhMucChucVuDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucChucVuDAL.DanhSach(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTiet(int? ChucVuID)
        {
            var Result = new BaseResultModel();
            if (ChucVuID == null || ChucVuID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = danhMucChucVuDAL.ChiTiet(ChucVuID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel ThemMoi(DanhMucChucVuThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin chức vụ cần thêm!";
                    return Result;
                }
                else if (item == null || item.MaChucVu == null || string.IsNullOrEmpty(item.MaChucVu))
                {
                    Result.Status = 0;
                    Result.Message = "Mã chức vụ không được trống!";
                    return Result;
                }
                else if (item.MaChucVu.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã chức vụ 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenChucVu == null || string.IsNullOrEmpty(item.TenChucVu))
                {
                    Result.Status = 0;
                    Result.Message = "Tên chức vụ không được trống";
                    return Result;
                }
                else if (item.TenChucVu.Length > 50)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên chức vụ không quá 50 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucChucVuDAL.KiemTraTonTai(item.MaChucVu.Trim(), 1, null))
                {
                    Result.Status = 0;
                    Result.Message = "Mã chức vụ đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucChucVuDAL.KiemTraTonTai(item.TenChucVu.Trim(), 2, null))
                {
                    Result.Status = 0;
                    Result.Message = "Tên chức vụ đã tồn tại";
                    return Result;
                }

                // thực hiện thêm mới
                Result = danhMucChucVuDAL.ThemMoi(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel CapNhat(DanhMucChucVuMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null || item.ChucVuID is null || item.ChucVuID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin chức vụ cần cập nhật!";
                    return Result;
                }
                else if (item == null || item.MaChucVu == null || string.IsNullOrEmpty(item.MaChucVu))
                {
                    Result.Status = 0;
                    Result.Message = "Mã chức vụ không được trống!";
                    return Result;
                }
                else if (item.MaChucVu.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã chức vụ 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenChucVu == null || string.IsNullOrEmpty(item.TenChucVu))
                {
                    Result.Status = 0;
                    Result.Message = "Tên chức vụ không được trống";
                    return Result;
                }
                else if (item.TenChucVu.Length > 50)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên chức vụ không quá 50 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucChucVuDAL.KiemTraTonTai(item.MaChucVu.Trim(), 1, item.ChucVuID))
                {
                    Result.Status = 0;
                    Result.Message = "Mã chức vụ đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucChucVuDAL.KiemTraTonTai(item.TenChucVu.Trim(), 2, item.ChucVuID))
                {
                    Result.Status = 0;
                    Result.Message = "Tên chức vụ đã tồn tại";
                    return Result;
                }

                // thực hiện cập nhật 
                Result = danhMucChucVuDAL.CapNhat(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel Xoa(int? ChucVuID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (ChucVuID == null || ChucVuID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                //kiểm tra ChucVuID có tồn tại trong DB không
                var checkItem = danhMucChucVuDAL.KiemTraTonTai("", 3, ChucVuID);
                if (checkItem == false)
                {
                    Result.Status = 0;
                    Result.Message = "Danh mục chức vụ không tồn tại";
                    return Result;
                }

                // Kiểm tra dữ liệu đã phát sinh ở các chức khác chưa - làm sau

                //Thực hiện xóa
                Result = danhMucChucVuDAL.Xoa(ChucVuID.Value);
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
