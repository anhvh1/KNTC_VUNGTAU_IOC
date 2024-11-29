using Castle.Core.Internal;
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
    public class DanhMucCoQuanDonViBUS_v2
    {
        private DanhMucCoQuanDAL_v2 DanhMucCoQuanDAL_v2;
        public DanhMucCoQuanDonViBUS_v2()
        {
            this.DanhMucCoQuanDAL_v2 = new DanhMucCoQuanDAL_v2();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc_v2 p)
        {
            var Result = new BaseResultModel();
            try
            {
               
                 Result = DanhMucCoQuanDAL_v2.DanhSach(p);
  
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        
        //them moi 
        public BaseResultModel ThemMoi( AddDanhMucCoQuanMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin đơn vị cần thêm!";
                    return Result;
                }
                else if (item == null ||item.CoQuanChaID is null ||item.CoQuanChaID == null)
                {
                    Result.Status = 0;
                    Result.Message = " ID cơ quan cha không được trống!";
                    return Result;
                }
                else if (item == null || item.CapID == null)
                {
                    Result.Status = 0;
                    Result.Message = "Cấp không được để trống";
                    return Result;
                }
                else if (item.TenCoQuan == null || string.IsNullOrEmpty(item.TenCoQuan))
                {
                    Result.Status = 0;
                    Result.Message = "Tên cơ quan không được trống";
                    return Result;
                }
                else if(item == null || item.TinhID is null || item.TinhID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Tỉnh không được để trống";
                    return Result;
                }
                //else if (item == null || item.HuyenID is null || item.HuyenID < 1)
                //{
                //    Result.Status = 0;
                //    Result.Message = "Huyện  không được để trống";
                //    return Result;
                //}
                //else if (item == null || item.XaID is null || item.XaID < 1)
                //{
                //    Result.Status = 0;
                //    Result.Message = "Xã không được để trống";
                //    return Result;
                //}

                // CHECK 

                if (DanhMucCoQuanDAL_v2.KiemTraTonTai(item.MaCQ.Trim(), 1, null))
                {
                    Result.Status = 0;
                    Result.Message = "Mã Co Quan đã tồn tại";
                    return Result;
                }

                // thực hiện thêm mới
                Result = DanhMucCoQuanDAL_v2.ThemMoi(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        // xoa
        public BaseResultModel Xoa(int? ID)
        {
            
            var Result = new BaseResultModel();
            try
            {     
                // kiểm tra frontend có truyền tham số lên không
                if (ID == null || ID <1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                //kiểm tra có tồn tại trong DB không
                //var checkItem = DanhMucCoQuanDAL_v2.KiemTraTonTai("", 3, null);
                if (DanhMucCoQuanDAL_v2.KiemTraTonTai("", 3,ID) == false)
                {
                    Result.Status = 0;
                    Result.Message = "Danh mục cơ quan không tồn tại";
                    return Result;
                }

                // Kiểm tra dữ liệu đã phát sinh ở các chức khác chưa - làm sau

                //Thực hiện xóa
                Result = DanhMucCoQuanDAL_v2.Xoa(ID.Value);
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
        // tim kiem ten co quan 
        public BaseResultModel SearchName(string Name)
        {
            var Result = new BaseResultModel();
            if (Name == null )
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = DanhMucCoQuanDAL_v2.SearchName(Name);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }
        // update
        public BaseResultModel CapNhat(UpdateDanhMucCoQuanMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null || item.CoQuanID == null || item.CoQuanID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin cơ quan cần cập nhật!";
                    return Result;
                }
                else if (item == null || item.TenCoQuan == null || string.IsNullOrEmpty(item.TenCoQuan))
                {
                    Result.Status = 0;
                    Result.Message = "Tên cơ quan không được trống!";
                    return Result;
                }
                else if (item.TenCoQuan.Length > 100)
                {
                    Result.Status = 0;
                    Result.Message = "Độ dài của ten cơ quan 1 - 100 ký tự";
                    return Result;
                }
                // kiểm tra 
                if (DanhMucCoQuanDAL_v2.KiemTraTonTai(item.TenCoQuan.Trim(), 1, null))
                {
                    Result.Status = 0;
                    Result.Message = "Tên Đơn vị đã tồn tại";
                    return Result;
                }
                


                // thực hiện cập nhật 
                Result = DanhMucCoQuanDAL_v2.CapNhat(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }
        //-----------
        public BaseResultModel ChiTiet(int? CoQuanID)
        {
            var Result = new BaseResultModel();
            if (CoQuanID == null || CoQuanID < 1)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = DanhMucCoQuanDAL_v2.ChiTiet(CoQuanID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        //------------
        public BaseResultModel DanhSachCap(ThamSoLocDanhMuc_v2 p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = DanhMucCoQuanDAL_v2.DanhSachCap(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel DanhSachCap_HDSD(ThamSoLocDanhMuc_v2 p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = DanhMucCoQuanDAL_v2.DanhSachCap_HDSD(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel CacCapCoQuan(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = DanhMucCoQuanDAL_v2.CacCapCoQuan(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel DanhSachCoQuanCha(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = DanhMucCoQuanDAL_v2.DanhSachCoQuanCha(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

    }
}
