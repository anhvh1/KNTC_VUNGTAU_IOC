using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.HeThong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class V2_SystemConfigBUS1
    {
        private V2_SystemConFigDAL _V2_SystemConFigDAL;
        public V2_SystemConfigBUS1()
        {
            _V2_SystemConFigDAL = new V2_SystemConFigDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _V2_SystemConFigDAL.DanhSach(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTiet(int? SystemConfigID)
        {
            var Result = new BaseResultModel();
            if (SystemConfigID == null || SystemConfigID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = _V2_SystemConFigDAL.ChiTiet(SystemConfigID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        

        public BaseResultModel ThemMoi(V2_SystemConfigMODADD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập tham số hệ thống thông tin  cần thêm!";
                    return Result;
                }
                else if (item == null || item.ConfigKey == null || string.IsNullOrEmpty(item.ConfigKey))
                {
                    Result.Status = 0;
                    Result.Message = "Tham số đơn không được trống!";
                    return Result;
                }
                else if (item.ConfigKey.Length > 200)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tham số hệ thống 1 - 200 ký tự";
                    return Result;
                }
                else if (item.ConfigValue == null || string.IsNullOrEmpty(item.ConfigValue))
                {
                    Result.Status = 0;
                    Result.Message = "Tên trạng giá trị được trống";
                    return Result;
                }
                else if (item.ConfigValue.Length > 200)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của giá trị 1 - 50 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

              /*  // kiểm tra trùng mã số
                if (V2_SystemConFigDAL.KiemTraTonTai(item.ConfigValue.Trim(), 1, null))
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
                }*/

                // thực hiện thêm mới
                Result = _V2_SystemConFigDAL.ThemMoi(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        public BaseResultModel CapNhat(V2_SystemConfigMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null || item.SystemConfigID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin tham số  cần cập nhật!";
                    return Result;
                }
                else if (item == null || item.ConfigKey == null || string.IsNullOrEmpty(item.ConfigKey))
                {
                    Result.Status = 0;
                    Result.Message = "Mã tham số không được trống!";
                    return Result;
                }
                else if (item.ConfigKey.Length > 200)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của mã tham số 1 - 20 ký tự";
                    return Result;
                }
                else if (item.ConfigValue == null || string.IsNullOrEmpty(item.ConfigValue))
                {
                    Result.Status = 0;
                    Result.Message = "Tên giá trị không được trống";
                    return Result;
                }
                else if (item.ConfigValue.Length > 200)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của tên giá trị 1 - 50 ký tự";
                    return Result;
                }
                else if (item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được trống";
                    return Result;
                }

                // kiểm tra trùng mã số
                /*if (V2_SystemConfigMODADD.KiemTraTonTai(item.MaTrangThaiDon.Trim(), 1, item.TrangThaiDonID))
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
*/
                // thực hiện cập nhật 
                Result = _V2_SystemConFigDAL.CapNhat(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel Xoa(int? SystemConfigID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (SystemConfigID == null || SystemConfigID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                /*  //kiểm tra TrangThaiDonID có tồn tại trong DB không
                  var checkItem = V2_SystemConFigDAL.KiemTraTonTai("", 3, SystemConfigID);
                  if (checkItem == false)
                  {
                      Result.Status = 0;
                      Result.Message = "Danh mục trạng thái đơn không tồn tại";
                      return Result;
                  }*/

                // Kiểm tra dữ liệu đã phát sinh ở các chức khác chưa - làm sau

                //Thực hiện xóa
                Result = _V2_SystemConFigDAL.Xoa(SystemConfigID.Value);
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
