using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using System;

//Create by TienKM - 13/10/2022
namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucLoaiVanBanBUS
    {
        private readonly DanhMucLoaiVanBanDAL _danhMucLoaiVanBanDAL;

        public DanhMucLoaiVanBanBUS()
        {
            _danhMucLoaiVanBanDAL = new DanhMucLoaiVanBanDAL();
        }

        public BaseResultModel DanhSach(ThamSoLocDanhMuc thamSoLocDanhMuc)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucLoaiVanBanDAL.DanhSach(thamSoLocDanhMuc);
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
                Result = _danhMucLoaiVanBanDAL.ChiTiet(NguonDonDenID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }
        public BaseResultModel ThemMoi(ThemDanhMucLoaiVanBanModel vanBan)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucLoaiVanBanDAL.ThemMoi(vanBan);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel XoaVanBan(int VanBanID)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucLoaiVanBanDAL.XoaVanBan(VanBanID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }
        public BaseResultModel SuaVanBan(DanhMucLoaiVanBanModel vanBan)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucLoaiVanBanDAL.SuaVanBan(vanBan);
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
