using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Create by Duc - 17/10/2022
namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucLoaiKetQuaBUS
    {
        private DanhMucLoaiKetQuaDAL danhMucLoaiKetQuaDAL;
        public DanhMucLoaiKetQuaBUS()
        {
            danhMucLoaiKetQuaDAL = new DanhMucLoaiKetQuaDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucLoaiKetQuaDAL.DanhSach(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTiet(int? LoaiKetQuaID)
        {
            var Result = new BaseResultModel();
            if (LoaiKetQuaID == null || LoaiKetQuaID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = danhMucLoaiKetQuaDAL.ChiTiet(LoaiKetQuaID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel ThemMoi(DanhMucLoaiKetQuaThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin loại kết quả cần thêm!";
                    return Result;
                }
                else if (item == null || item.TenLoaiKetQua == null || string.IsNullOrEmpty(item.TenLoaiKetQua))
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại kết quả không được trống!";
                    return Result;
                }
                else if (item.MaLoaiKetQua.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã loại kết quả 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenLoaiKetQua == null || string.IsNullOrEmpty(item.TenLoaiKetQua))
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại kết quả không được trống";
                    return Result;
                }
                else if (item.TenLoaiKetQua.Length > 50)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên loại kết quả 1 - 50 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucLoaiKetQuaDAL.KiemTraTonTai(item.MaLoaiKetQua.Trim(), 1, null))
                {
                    Result.Status = 0;
                    Result.Message = "Mã loại kết quả đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucLoaiKetQuaDAL.KiemTraTonTai(item.TenLoaiKetQua.Trim(), 2, null))
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại kết quả đã tồn tại";
                    return Result;
                }

                // thực hiện thêm mới
                Result = danhMucLoaiKetQuaDAL.ThemMoi(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel CapNhat(DanhMucLoaiKetQuaMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null || item.LoaiKetQuaID is null || item.LoaiKetQuaID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin loại kết quả cần cập nhật!";
                    return Result;
                }
                else if (item == null || item.TenLoaiKetQua == null || string.IsNullOrEmpty(item.TenLoaiKetQua))
                {
                    Result.Status = 0;
                    Result.Message = "Mã loại kết quả không được trống!";
                    return Result;
                }
                else if (item.MaLoaiKetQua.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã loại kết quả 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenLoaiKetQua == null || string.IsNullOrEmpty(item.TenLoaiKetQua))
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại kết quả không được trống";
                    return Result;
                }
                else if (item.TenLoaiKetQua.Length > 50)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên loại kết quả 1 - 50 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucLoaiKetQuaDAL.KiemTraTonTai(item.MaLoaiKetQua.Trim(), 1, item.LoaiKetQuaID))
                {
                    Result.Status = 0;
                    Result.Message = "Mã loại kết quả đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucLoaiKetQuaDAL.KiemTraTonTai(item.TenLoaiKetQua.Trim(), 2, item.LoaiKetQuaID))
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại kết quả đã tồn tại";
                    return Result;
                }

                // thực hiện cập nhật 
                Result = danhMucLoaiKetQuaDAL.CapNhat(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel Xoa(int? LoaiKetQuaID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (LoaiKetQuaID == null || LoaiKetQuaID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                //kiểm tra LoaiKetQuaID có tồn tại trong DB không
                var checkItem = danhMucLoaiKetQuaDAL.KiemTraTonTai("", 3, LoaiKetQuaID);
                if (checkItem == false)
                {
                    Result.Status = 0;
                    Result.Message = "Danh mục loại kết quả không tồn tại";
                    return Result;
                }

                // Kiểm tra dữ liệu đã phát sinh ở các chức khác chưa - làm sau

                //Thực hiện xóa
                Result = danhMucLoaiKetQuaDAL.Xoa(LoaiKetQuaID.Value);
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
