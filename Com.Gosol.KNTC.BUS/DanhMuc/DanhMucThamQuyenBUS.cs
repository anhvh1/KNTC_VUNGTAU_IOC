using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Create by NamNH - 14/10/2022
namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucThamQuyenBUS
    {
        private DanhMucThamQuyenDAL danhMucThamQuyenDAL;
        public DanhMucThamQuyenBUS()
        {
            danhMucThamQuyenDAL = new DanhMucThamQuyenDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucThamQuyenDAL.DanhSach(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTiet(int? ThamQuyenID)
        {
            var Result = new BaseResultModel();
            if (ThamQuyenID == null || ThamQuyenID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = danhMucThamQuyenDAL.ChiTiet(ThamQuyenID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel ThemMoi(DanhMucThamQuyenThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin thẩm quyền cần thêm!";
                    return Result;
                }
                else if (item == null || item.MaThamQuyen == null || string.IsNullOrEmpty(item.MaThamQuyen))
                {
                    Result.Status = 0;
                    Result.Message = "Mã thẩm quyền không được trống!";
                    return Result;
                }
                else if (item.MaThamQuyen.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã thẩm quyền 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenThamQuyen == null || string.IsNullOrEmpty(item.TenThamQuyen))
                {
                    Result.Status = 0;
                    Result.Message = "Tên thẩm quyền không được trống";
                    return Result;
                }
                else if (item.TenThamQuyen.Length > 50)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên thẩm quyền không quá 50 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucThamQuyenDAL.KiemTraTonTai(item.MaThamQuyen.Trim(), 1, null))
                {
                    Result.Status = 0;
                    Result.Message = "Mã thẩm quyền đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucThamQuyenDAL.KiemTraTonTai(item.TenThamQuyen.Trim(), 2, null))
                {
                    Result.Status = 0;
                    Result.Message = "Tên thẩm quyền đã tồn tại";
                    return Result;
                }

                // thực hiện thêm mới
                Result = danhMucThamQuyenDAL.ThemMoi(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel CapNhat(DanhMucThamQuyenMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null || item.ThamQuyenID is null || item.ThamQuyenID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin thẩm quyền cần cập nhật!";
                    return Result;
                }
                else if (item == null || item.MaThamQuyen == null || string.IsNullOrEmpty(item.MaThamQuyen))
                {
                    Result.Status = 0;
                    Result.Message = "Mã thẩm quyền không được trống!";
                    return Result;
                }
                else if (item.MaThamQuyen.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã thẩm quyền 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenThamQuyen == null || string.IsNullOrEmpty(item.TenThamQuyen))
                {
                    Result.Status = 0;
                    Result.Message = "Tên thẩm quyền không được trống";
                    return Result;
                }
                else if (item.TenThamQuyen.Length > 50)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên thẩm quyền không quá 50 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucThamQuyenDAL.KiemTraTonTai(item.MaThamQuyen.Trim(), 1, item.ThamQuyenID))
                {
                    Result.Status = 0;
                    Result.Message = "Mã thẩm quyền đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucThamQuyenDAL.KiemTraTonTai(item.TenThamQuyen.Trim(), 2, item.ThamQuyenID))
                {
                    Result.Status = 0;
                    Result.Message = "Tên thẩm quyền đã tồn tại";
                    return Result;
                }

                // thực hiện cập nhật 
                Result = danhMucThamQuyenDAL.CapNhat(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel Xoa(int? ThamQuyenID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (ThamQuyenID == null || ThamQuyenID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                //kiểm tra ThamQuyenID có tồn tại trong DB không
                var checkItem = danhMucThamQuyenDAL.KiemTraTonTai("", 3, ThamQuyenID);
                if (checkItem == false)
                {
                    Result.Status = 0;
                    Result.Message = "Danh mục thẩm quyền không tồn tại";
                    return Result;
                }

                // Kiểm tra dữ liệu đã phát sinh ở các chức khác chưa - làm sau

                //Thực hiện xóa
                Result = danhMucThamQuyenDAL.Xoa(ThamQuyenID.Value);
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
