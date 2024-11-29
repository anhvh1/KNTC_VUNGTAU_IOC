using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using System;

//Create by TienKM - 14/10/2022
namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucNguonDonDenBUS
    {
        private DanhMucNguonDonDenDAL _danhMucNguonDonDenDAL;

        public DanhMucNguonDonDenBUS()
        {
            _danhMucNguonDonDenDAL = new DanhMucNguonDonDenDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc thamSoLocDanhMuc)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucNguonDonDenDAL.DanhSach(thamSoLocDanhMuc);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ChiTiet(int NguonDonDenID)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucNguonDonDenDAL.ChiTiet(NguonDonDenID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ThemMoi(ThemDanhMucNguonDonDenModel NguonDonDen)
        {
            var Result = new BaseResultModel();

            if (NguonDonDen.TenNguonDonDen.Trim().Length > 50)
            {
                Result.Status = 0;
                Result.Message = "Tên nguồn đơn đến không quá 50 ký tự";
                return Result;
            }

            try
            {
                Result = _danhMucNguonDonDenDAL.ThemMoi(NguonDonDen);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel XoaNguonDonDen(int NguonDonDenID)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucNguonDonDenDAL.XoaNguonDonDen(NguonDonDenID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }
        public BaseResultModel SuaNguonDonDen(DanhMucNguonDonDenModel NguonDonDen)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucNguonDonDenDAL.SuaNguonDonDen(NguonDonDen);
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
