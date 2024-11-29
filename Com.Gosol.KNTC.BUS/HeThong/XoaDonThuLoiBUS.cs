using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.HeThong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.HeThong
{
    public class XoaDonThuLoiBUS
    {
        private XoaDonThuLoiDAL XoaDonThuLoiDAL;
        public XoaDonThuLoiBUS()
        {
            XoaDonThuLoiDAL = new XoaDonThuLoiDAL();
        }

        public BaseResultModel DanhSachDonThu(thamsodonthuloi /*ThamSoLocDanhMuc*/ p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = XoaDonThuLoiDAL.DanhSachDonThuLoi(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }
        //--------------
        
        public BaseResultModel ChiTiet(int? DonThuID , int? XuLyDonID)
        {
            var Result = new BaseResultModel();
            if (DonThuID == null )
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                return Result;
            }
            try
            {
                Result = XoaDonThuLoiDAL.ChiTiet(DonThuID, XuLyDonID);
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
        public BaseResultModel XoaDonThu(int? DonThuID, int? XuLyDonID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (DonThuID == null || DonThuID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }
                else if (XuLyDonID == null || XuLyDonID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }


                //Thực hiện xóa
                Result = XoaDonThuLoiDAL.XoaDonThu(DonThuID.Value, XuLyDonID.Value);
            }
            catch (Exception ex)
            {
                Result.Status = -1;

                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }
        //--------------

        public BaseResultModel XoaDonThuLoi(Thamsoxoa thamsoxoa)
        {
            var Result = new BaseResultModel();
            try
            {
             
                //Thực hiện xóa
                Result = XoaDonThuLoiDAL.XoaDonThuLoi(thamsoxoa);
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
