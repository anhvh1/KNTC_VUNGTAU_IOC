using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Create by NamNH - 13/10/2022

namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucPhongBanBUS
    {
        private DanhMucPhongBanDAL danhMucPhongBanDAL;
        public DanhMucPhongBanBUS()
        {
            danhMucPhongBanDAL = new DanhMucPhongBanDAL();
        }

        public BaseResultModel DanhSach(int? CoQuanID)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucPhongBanDAL.DanhSach(CoQuanID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTiet(int? PhongBanID)
        {
            var Result = new BaseResultModel();
            if (PhongBanID == null || PhongBanID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = danhMucPhongBanDAL.ChiTiet(PhongBanID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel ThemMoi(DanhMucPhongBanThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin phòng ban cần thêm!";
                    return Result;
                }
                else if (item.TenPhongBan == null || string.IsNullOrEmpty(item.TenPhongBan))
                {
                    Result.Status = 0;
                    Result.Message = "Tên phòng ban không được trống";
                    return Result;
                }
                else if (item.TenPhongBan.Length > 200)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên phòng ban không quá 200 ký tự";
                    return Result;
                }
                else if (item.CoQuanID == null || item.CoQuanID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Cơ quan ID không được trống";
                    return Result;
                }

                /*// kiểm tra trùng mã số
                if (danhMucPhongBanDAL.KiemTraTonTai(item.SoDienThoai.Trim(), 1, null))
                {
                    Result.Status = 0;
                    Result.Message = "Số điện thoại đã tồn tại";
                    return Result;
                }*/

                // kiểm tra trùng tên
                if (danhMucPhongBanDAL.KiemTraTonTai(item.TenPhongBan.Trim(), 2, null, item.CoQuanID))
                {
                    Result.Status = 0;
                    Result.Message = "Tên phòng ban đã tồn tại";
                    return Result;
                }

                // thực hiện thêm mới
                Result = danhMucPhongBanDAL.ThemMoi(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel CapNhat(DanhMucPhongBanMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null || item.PhongBanID == null || item.PhongBanID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin phòng ban cần cập nhật!";
                    return Result;
                }
                else if (item.TenPhongBan == null || string.IsNullOrEmpty(item.TenPhongBan))
                {
                    Result.Status = 0;
                    Result.Message = "Tên phòng ban không được trống";
                    return Result;
                }
                else if (item.TenPhongBan.Length > 200)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên phòng ban không quá 200 ký tự";
                    return Result;
                }
                else if (item.CoQuanID == null)
                {
                    Result.Status = 0;
                    Result.Message = "Cơ quan ID không được trống";
                    return Result;
                }

                /*// kiểm tra trùng mã số
                if (danhMucPhongBanDAL.KiemTraTonTai(item.SoDienThoai.Trim(), 1, item.PhongBanID))
                {
                    Result.Status = 0;
                    Result.Message = "Số điện thoại đã tồn tại";
                    return Result;
                }*/

                // kiểm tra trùng tên
                if (danhMucPhongBanDAL.KiemTraTonTai(item.TenPhongBan.Trim(), 2, item.PhongBanID, item.CoQuanID))
                {
                    Result.Status = 0;
                    Result.Message = "Tên phòng ban đã tồn tại";
                    return Result;
                }

                // thực hiện cập nhật 
                Result = danhMucPhongBanDAL.CapNhat(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel Xoa(int? PhongBanID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (PhongBanID == null || PhongBanID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                //kiểm tra PhongBanID có tồn tại trong DB không
                var checkItem = danhMucPhongBanDAL.KiemTraTonTai("", 3, PhongBanID, null);
                if (checkItem == false)
                {
                    Result.Status = 0;
                    Result.Message = "Danh mục phòng ban không tồn tại";
                    return Result;
                }

                // Kiểm tra dữ liệu đã phát sinh ở các chức khác chưa - làm sau

                //Thực hiện xóa
                Result = danhMucPhongBanDAL.Xoa(PhongBanID.Value);
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
