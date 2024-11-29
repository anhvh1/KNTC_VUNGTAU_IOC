using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using System;

//Create by AnhVH - 12/10/2022
namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucHuongGiaiQuyetBUS
    {
        private DanhMucHuongGiaiQuyetDAL _danhMucHuongGiaiQuyetDAL;

        public DanhMucHuongGiaiQuyetBUS()
        {
            _danhMucHuongGiaiQuyetDAL = new DanhMucHuongGiaiQuyetDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc thamSoLocDanhMuc)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucHuongGiaiQuyetDAL.DanhSach(thamSoLocDanhMuc);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ChiTietHuongGiaiQuyet(int huongGiaiQuyetID)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucHuongGiaiQuyetDAL.ChiTiet(huongGiaiQuyetID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThemMoi(ThemDanhMucHuongGiaiQuyetModel huongGiaiQuyet)
        {
            var Result = new BaseResultModel();

            if (huongGiaiQuyet.TenHuongGiaiQuyet.Trim().Length > 100)
            {
                Result.Status = 0;
                Result.Message = "Tên hướng giải quyết không quá 100 ký tự";
                return Result;
            }

            try
            {
                Result = _danhMucHuongGiaiQuyetDAL.ThemMoi(huongGiaiQuyet);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XoaHuongGiaiQuyet(int huongGiaiQuyetID)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucHuongGiaiQuyetDAL.XoaHuongGiaiQuyet(huongGiaiQuyetID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel SuaHuongGiaiQuyet(DanhMucHuongGiaiQuyetModel huongGiaiQuyet)
        {
            var Result = new BaseResultModel();

            if (huongGiaiQuyet.TenHuongGiaiQuyet.Trim().Length > 100)
            {
                Result.Status = 0;
                Result.Message = "Tên hướng giải quyết không quá 100 ký tự";
                return Result;
            }

            try
            {
                Result = _danhMucHuongGiaiQuyetDAL.SuaHuongGiaiQuyet(huongGiaiQuyet);
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