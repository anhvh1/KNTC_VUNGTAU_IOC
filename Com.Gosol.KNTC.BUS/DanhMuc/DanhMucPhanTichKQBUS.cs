using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Create by duc - 13/10/2022
namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucPhanTichKQBUS
    {
        private DanhMucPhanTichKQDAL danhMucPhanTichKQDAL;
        public DanhMucPhanTichKQBUS()
        {
            danhMucPhanTichKQDAL = new DanhMucPhanTichKQDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucPhanTichKQDAL.DanhSach(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTiet(int? PhanTichKQID)
        {
            var Result = new BaseResultModel();
            if (PhanTichKQID == null || PhanTichKQID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = danhMucPhanTichKQDAL.ChiTiet(PhanTichKQID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel ThemMoi(DanhMucPhanTichKQThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin phân tích kết quả cần thêm!";
                    return Result;
                }
                else if (item == null || item.MaPhanTichKQ == null || string.IsNullOrEmpty(item.MaPhanTichKQ))
                {
                    Result.Status = 0;
                    Result.Message = "Mã phân tích kết quả không được trống!";
                    return Result;
                }
                else if (item.MaPhanTichKQ.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã phân tích kết quả 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenPhanTichKQ == null || string.IsNullOrEmpty(item.TenPhanTichKQ))
                {
                    Result.Status = 0;
                    Result.Message = "Tên phân tích kết quả không được trống";
                    return Result;
                }
                else if (item.TenPhanTichKQ.Length > 100)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên phân tích kết quả 1 - 100 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucPhanTichKQDAL.KiemTraTonTai(item.MaPhanTichKQ.Trim(), 1, null))
                {
                    Result.Status = 0;
                    Result.Message = "Mã phân tích kết quả đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucPhanTichKQDAL.KiemTraTonTai(item.TenPhanTichKQ.Trim(), 2, null))
                {
                    Result.Status = 0;
                    Result.Message = "Tên phân tích kết quả đã tồn tại";
                    return Result;
                }

                // thực hiện thêm mới
                Result = danhMucPhanTichKQDAL.ThemMoi(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel CapNhat(DanhMucPhanTichKQMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null || item.PhanTichKQID is null || item.PhanTichKQID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin phân tích kết quả cần cập nhật!";
                    return Result;
                }
                else if (item == null || item.MaPhanTichKQ == null || string.IsNullOrEmpty(item.MaPhanTichKQ))
                {
                    Result.Status = 0;
                    Result.Message = "Mã phân tích kết quả không được trống!";
                    return Result;
                }
                else if (item.MaPhanTichKQ.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã phân tích kết quả 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenPhanTichKQ == null || string.IsNullOrEmpty(item.TenPhanTichKQ))
                {
                    Result.Status = 0;
                    Result.Message = "Tên phân tích kết quả không được trống";
                    return Result;
                }
                else if (item.TenPhanTichKQ.Length > 100)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên phân tích kết quả 1 - 100 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucPhanTichKQDAL.KiemTraTonTai(item.MaPhanTichKQ.Trim(), 1, item.PhanTichKQID))
                {
                    Result.Status = 0;
                    Result.Message = "Mã phân tích kết quả đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucPhanTichKQDAL.KiemTraTonTai(item.TenPhanTichKQ.Trim(), 2, item.PhanTichKQID))
                {
                    Result.Status = 0;
                    Result.Message = "Tên phân tích kết quả đã tồn tại";
                    return Result;
                }

                // thực hiện cập nhật 
                Result = danhMucPhanTichKQDAL.CapNhat(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel Xoa(int? PhanTichKQID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (PhanTichKQID == null || PhanTichKQID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                //kiểm tra PhanTichKQID có tồn tại trong DB không
                var checkItem = danhMucPhanTichKQDAL.KiemTraTonTai("", 3, PhanTichKQID);
                if (checkItem == false)
                {
                    Result.Status = 0;
                    Result.Message = "Danh mục phân tích kết quả không tồn tại";
                    return Result;
                }

                // Kiểm tra dữ liệu đã phát sinh ở các chức khác chưa - làm sau

                //Thực hiện xóa
                Result = danhMucPhanTichKQDAL.Xoa(PhanTichKQID.Value);
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
