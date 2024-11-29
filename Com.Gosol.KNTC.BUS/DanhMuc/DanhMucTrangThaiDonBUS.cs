using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucTrangThaiDonBUS
    {
        private DanhMucTrangThaiDonDAL danhMucTrangThaiDonDAL;
        public DanhMucTrangThaiDonBUS()
        {
            danhMucTrangThaiDonDAL = new DanhMucTrangThaiDonDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = danhMucTrangThaiDonDAL.DanhSach(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTiet(int? TrangThaiDonID)
        {
            var Result = new BaseResultModel();
            if (TrangThaiDonID == null || TrangThaiDonID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = danhMucTrangThaiDonDAL.ChiTiet(TrangThaiDonID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel ThemMoi(DanhMucTrangThaiDonThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin trạng thái đơn cần thêm!";
                    return Result;
                }
                else if (item == null || item.MaTrangThaiDon == null || string.IsNullOrEmpty(item.MaTrangThaiDon))
                {
                    Result.Status = 0;
                    Result.Message = "Mã trạng thái đơn không được trống!";
                    return Result;
                }
                else if (item.MaTrangThaiDon.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã trạng thái đơn 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenTrangThaiDon == null || string.IsNullOrEmpty(item.TenTrangThaiDon))
                {
                    Result.Status = 0;
                    Result.Message = "Tên trạng thái đơn không được trống";
                    return Result;
                }
                else if (item.TenTrangThaiDon.Length > 50)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên trạng thái đơn 1 - 50 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucTrangThaiDonDAL.KiemTraTonTai(item.MaTrangThaiDon.Trim(), 1, null))
                {
                    Result.Status = 0;
                    Result.Message = "Mã trạng thái đơn đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucTrangThaiDonDAL.KiemTraTonTai(item.TenTrangThaiDon.Trim(), 2, null))
                {
                    Result.Status = 0;
                    Result.Message = "Tên trạng thái đơn đã tồn tại";
                    return Result;
                }

                // thực hiện thêm mới
                Result = danhMucTrangThaiDonDAL.ThemMoi(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel CapNhat(DanhMucTrangThaiDonMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null || item.TrangThaiDonID is null || item.TrangThaiDonID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin trạng thái đơn cần cập nhật!";
                    return Result;
                }
                else if (item == null || item.MaTrangThaiDon == null || string.IsNullOrEmpty(item.MaTrangThaiDon))
                {
                    Result.Status = 0;
                    Result.Message = "Mã trạng thái đơn không được trống!";
                    return Result;
                }
                else if (item.MaTrangThaiDon.Length > 20)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã trạng thái đơn 1 - 20 ký tự";
                    return Result;
                }
                else if (item.TenTrangThaiDon == null || string.IsNullOrEmpty(item.TenTrangThaiDon))
                {
                    Result.Status = 0;
                    Result.Message = "Tên trạng thái đơn không được trống";
                    return Result;
                }
                else if (item.TenTrangThaiDon.Length > 50)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên trạng thái đơn 1 - 50 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                if (danhMucTrangThaiDonDAL.KiemTraTonTai(item.MaTrangThaiDon.Trim(), 1, item.TrangThaiDonID))
                {
                    Result.Status = 0;
                    Result.Message = "Mã trạng thái đơn đã tồn tại";
                    return Result;
                }

                // kiểm tra trùng tên
                if (danhMucTrangThaiDonDAL.KiemTraTonTai(item.TenTrangThaiDon.Trim(), 2, item.TrangThaiDonID))
                {
                    Result.Status = 0;
                    Result.Message = "Tên trạng thái đơn đã tồn tại";
                    return Result;
                }

                // thực hiện cập nhật 
                Result = danhMucTrangThaiDonDAL.CapNhat(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel Xoa(int? TrangThaiDonID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (TrangThaiDonID == null || TrangThaiDonID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                //kiểm tra TrangThaiDonID có tồn tại trong DB không
                var checkItem = danhMucTrangThaiDonDAL.KiemTraTonTai("", 3, TrangThaiDonID);
                if (checkItem == false)
                {
                    Result.Status = 0;
                    Result.Message = "Danh mục trạng thái đơn không tồn tại";
                    return Result;
                }

                // Kiểm tra dữ liệu đã phát sinh ở các chức khác chưa - làm sau

                //Thực hiện xóa
                Result = danhMucTrangThaiDonDAL.Xoa(TrangThaiDonID.Value);
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
