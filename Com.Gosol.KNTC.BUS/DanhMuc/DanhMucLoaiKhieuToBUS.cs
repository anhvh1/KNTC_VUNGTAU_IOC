using Castle.Core.Internal;
using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Create by NamNH - 17/10/2022

namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucLoaiKhieuToBUS
    {
        private DanhMucLoaiKhieuToDAL danhMucLoaiKhieuToDAL;
        public DanhMucLoaiKhieuToBUS()
        {
            danhMucLoaiKhieuToDAL = new DanhMucLoaiKhieuToDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucLoaiKhieuToDAL.DanhSach(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel DanhSachLoaiCha(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucLoaiKhieuToDAL.DanhSachLoaiCha(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTiet(int? LoaiKhieuToID)
        {
            var Result = new BaseResultModel();
            if (LoaiKhieuToID == null || LoaiKhieuToID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = danhMucLoaiKhieuToDAL.ChiTiet(LoaiKhieuToID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ThemMoi(DanhSachLoaiKhieuToThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin loại khiếu tố cần thêm!";
                    return Result;
                }
                else if (item.TenLoaiKhieuTo == null || string.IsNullOrEmpty(item.TenLoaiKhieuTo))
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại khiếu tố không được trống!";
                    return Result;
                }
                else if (item.TenLoaiKhieuTo.Length > 200)
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại khiếu tố từ 1 - 200 ký tự!";
                    return Result;
                }
                else if (item.LoaiKhieuToCha == null)
                {
                    Result.Status = 0;
                    Result.Message = "Loại khiếu tố cha không được trống";
                    return Result;
                }
                else if (item.MappingCode == null || string.IsNullOrEmpty(item.MappingCode))
                {
                    Result.Status = 0;
                    Result.Message = "Mã loại khiếu tố không được trống";
                    return Result;
                }
                else if (item.SuDung == null)
                {
                    Result.Status = 0;
                    Result.Message = "Sử dụng không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucLoaiKhieuToDAL.KiemTraTonTai(item.MappingCode.Trim(), 1, null,item.LoaiKhieuToCha))
                {
                    Result.Status = 0;
                    Result.Message = "Mã loại khiếu tố đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucLoaiKhieuToDAL.KiemTraTonTai(item.TenLoaiKhieuTo.Trim(), 2, null, item.LoaiKhieuToCha))
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại khiếu tố đã tồn tại";
                    return Result;
                }

                // thực hiện thêm mới
                Result = danhMucLoaiKhieuToDAL.ThemMoi(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel CapNhat(DanhMucLoaiKhieuToMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null || item.LoaiKhieuToID == null || item.LoaiKhieuToID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin loại khiếu tố cần cập nhật!";
                    return Result;
                }
                else if (item == null || item.TenLoaiKhieuTo == null || string.IsNullOrEmpty(item.TenLoaiKhieuTo))
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại khiếu tố không được trống!";
                    return Result;
                }
                else if (item.TenLoaiKhieuTo.Length > 200)
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại khiếu tố từ 1 - 200 ký tự!";
                    return Result;
                }
                else if (item.MappingCode == null || string.IsNullOrEmpty(item.MappingCode))
                {
                    Result.Status = 0;
                    Result.Message = "Mã loại khiếu tố không được trống";
                    return Result;
                }
                else if (item.SuDung == null)
                {
                    Result.Status = 0;
                    Result.Message = "Sử dụng không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucLoaiKhieuToDAL.KiemTraTonTai(item.MappingCode.Trim(), 1, item.LoaiKhieuToID, item.LoaiKhieuToCha))
                {
                    Result.Status = 0;
                    Result.Message = "Mã loại khiếu tố đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucLoaiKhieuToDAL.KiemTraTonTai(item.TenLoaiKhieuTo.Trim(), 2, item.LoaiKhieuToID, item.LoaiKhieuToCha))
                {
                    Result.Status = 0;
                    Result.Message = "Tên loại khiếu tố đã tồn tại";
                    return Result;
                }

                // thực hiện cập nhật 
                Result = danhMucLoaiKhieuToDAL.CapNhat(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel CapNhatSuDung(DanhSachLoaiKhieuToCapNhatSuDungMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item.SuDung == null)
                {
                    Result.Status = 0;
                    Result.Message = "Sử dụng không được trống";
                    return Result;
                }


                // thực hiện cập nhật 
                Result = danhMucLoaiKhieuToDAL.CapNhatSuDung(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }
        public BaseResultModel Xoa(int? LoaiKhieuToID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (LoaiKhieuToID == null || LoaiKhieuToID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                //kiểm tra DanTocID có tồn tại trong DB không
                var checkItem = danhMucLoaiKhieuToDAL.KiemTraTonTai("", 3, LoaiKhieuToID, null);
                if (checkItem == false)
                {
                    Result.Status = 0;
                    Result.Message = "Danh mục loại khiếu tố không tồn tại";
                    return Result;
                }

                var checkItem2 = danhMucLoaiKhieuToDAL.KiemTraTonTai("", 4, LoaiKhieuToID, null);
                if (checkItem2 == true)
                {
                    Result.Status = 0;
                    Result.Message = "Có cấp con!. Không thể xóa danh mục loại khiểu tố.";
                    return Result;
                }

                // Kiểm tra dữ liệu đã phát sinh ở các chức khác chưa - làm sau

                //Thực hiện xóa
                Result = danhMucLoaiKhieuToDAL.Xoa(LoaiKhieuToID.Value);
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
