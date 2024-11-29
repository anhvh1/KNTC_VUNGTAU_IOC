using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.DAL.TiepDan;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.TiepDan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.TiepDan
{
    public class TiepDanTrucTiepBUS
    {
        private TiepDanTrucTiepDAL TiepDanTrucTiepDAL;
        public TiepDanTrucTiepBUS()
        {
            TiepDanTrucTiepDAL = new TiepDanTrucTiepDAL();
        }

        public BaseResultModel ThemMoiDoiTuongKN(TiepDanTrucTiepMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                int i = 0;
                // validate data
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin cần thêm!";
                    return Result;
                }
                else if (item.DoiTuongKN[i].HoTen == null || string.IsNullOrEmpty(item.DoiTuongKN[i].HoTen))
                {
                    Result.Status = 0;
                    Result.Message = "Họ tên không được trống";
                    return Result;
                }

                i++;
                // Thực hiện thêm mới
                Result = TiepDanTrucTiepDAL.ThemMoiDoiTuongKN(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ThemMoiNhomKN(TiepDanTrucTiepMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                /*int i = 0;
                // validate data
                if (item == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin cần thêm!";
                    return Result;
                }
                else if (item.DoiTuongKN[i].HoTen == null || string.IsNullOrEmpty(item.DoiTuongKN[i].HoTen))
                {
                    Result.Status = 0;
                    Result.Message = "Họ tên không được trống";
                    return Result;
                }

                i++;*/
                // Thực hiện thêm mới
                Result = TiepDanTrucTiepDAL.ThemMoiNhomKN(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel DanhSachLoaiDoiTuongKN()
        {
            var Result = new BaseResultModel();
            try
            {
                Result = TiepDanTrucTiepDAL.DanhSachLoaiDoiTuongKN();
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
