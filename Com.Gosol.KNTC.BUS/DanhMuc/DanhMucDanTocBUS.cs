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
    public class DanhMucDanTocBUS
    {
        private DanhMucDanTocDAL danhMucDanTocDAL;
        public DanhMucDanTocBUS()
        {
            danhMucDanTocDAL = new DanhMucDanTocDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucDanTocDAL.DanhSach(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTiet(int? danTocID)
        {
            var Result = new BaseResultModel();
            if (danTocID == null || danTocID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = danhMucDanTocDAL.ChiTiet(danTocID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel ThemMoi(DanhMucDanTocThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin dân tộc cần thêm!";
                    return Result;
                }
                else if (item == null || item.MaDanToc == null || string.IsNullOrEmpty(item.MaDanToc))
                {
                    Result.Status = 0;
                    Result.Message = "Mã dân tộc không được trống!";
                    return Result;
                }
                else if (item.MaDanToc.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã dân tộc 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenDanToc == null || string.IsNullOrEmpty(item.TenDanToc))
                {
                    Result.Status = 0;
                    Result.Message = "Tên dân tộc không được trống";
                    return Result;
                }
                else if (item.TenDanToc.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên dân tộc 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucDanTocDAL.KiemTraTonTai(item.MaDanToc.Trim(), 1, null))
                {
                    Result.Status = 0;
                    Result.Message = "Mã dân tộc đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucDanTocDAL.KiemTraTonTai(item.TenDanToc.Trim(), 2, null))
                {
                    Result.Status = 0;
                    Result.Message = "Tên dân tộc đã tồn tại";
                    return Result;
                }

                // thực hiện thêm mới
                Result = danhMucDanTocDAL.ThemMoi(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel CapNhat(DanhMucDanTocMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null || item.DanTocID is null || item.DanTocID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin dân tộc cần cập nhật!";
                    return Result;
                }
                else if (item == null || item.MaDanToc == null || string.IsNullOrEmpty(item.MaDanToc))
                {
                    Result.Status = 0;
                    Result.Message = "Mã dân tộc không được trống!";
                    return Result;
                }
                else if (item.MaDanToc.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã dân tộc 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenDanToc == null || string.IsNullOrEmpty(item.TenDanToc))
                {
                    Result.Status = 0;
                    Result.Message = "Tên dân tộc không được trống";
                    return Result;
                }
                else if (item.TenDanToc.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên dân tộc 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucDanTocDAL.KiemTraTonTai(item.MaDanToc.Trim(), 1, item.DanTocID))
                {
                    Result.Status = 0;
                    Result.Message = "Mã dân tộc đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucDanTocDAL.KiemTraTonTai(item.TenDanToc.Trim(), 2, item.DanTocID))
                {
                    Result.Status = 0;
                    Result.Message = "Tên dân tộc đã tồn tại";
                    return Result;
                }

                // thực hiện cập nhật 
                Result = danhMucDanTocDAL.CapNhat(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel Xoa(int? danTocID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (danTocID == null || danTocID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                //kiểm tra DanTocID có tồn tại trong DB không
                var checkItem = danhMucDanTocDAL.KiemTraTonTai("", 3, danTocID);
                if (checkItem == false)
                {
                    Result.Status = 0;
                    Result.Message = "Danh mục dân tộc không tồn tại";
                    return Result;
                }

                // Kiểm tra dữ liệu đã phát sinh ở các chức khác chưa - làm sau

                //Thực hiện xóa
                Result = danhMucDanTocDAL.Xoa(danTocID.Value);
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
