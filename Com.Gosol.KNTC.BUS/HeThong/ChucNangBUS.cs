using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using System;
using System.Collections.Generic;
using System.Text;
using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Ultilities;
using static Com.Gosol.KNTC.Model.HeThong.ChucNangInfo;

namespace Com.Gosol.KNTC.BUS.HeThong
{
    public class ChucNangBUS
    {
        private ChucNangDAL _ChucNangDAL;

        public ChucNangBUS()
        {
            _ChucNangDAL = new ChucNangDAL();
        }

        public List<ChucNangModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            return _ChucNangDAL.GetPagingBySearch(p, ref TotalRow);
        }

        public List<MenuModel> GetListMenuByNguoiDungID(int NguoidungID)
        {
            return _ChucNangDAL.GetListMenuByNguoiDungID(NguoidungID);
        }

        //V2
        public BaseResultModel DanhSach(ThamSoLocDanhMuc thamSo)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _ChucNangDAL.DanhSach(thamSo);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                Result.Data = null;
            }

            return Result;
        }

        private bool HasUnicodeChar(string? input)
        {
            if (input == null) return false;

            var asciiBytesCount = Encoding.ASCII.GetByteCount(input);
            var unicodBytesCount = Encoding.UTF8.GetByteCount(input);
            return asciiBytesCount != unicodBytesCount;
        }

        public BaseResultModel ThemMoi(DanhMucChucNangThemMoi chucNang)
        {
            var Result = new BaseResultModel();

            if (chucNang.MaChucNang.Contains(" ") || HasUnicodeChar(chucNang.MaChucNang))
            {
                Result.Status = -1;
                Result.Message = "Mã chức năng không được chứa ký tự, khoảng trắng";
            }

            if (_ChucNangDAL.KiemTraChucNang(null, chucNang.TenChucNang, chucNang.MaChucNang))
            {
                Result.Status = -1;
                Result.Message = "Tên, mã chức năng đã tồn tại";
                return Result;
            }

            if (chucNang.ChucNangChaID != null && !_ChucNangDAL.KiemTraCapCha(chucNang.ChucNangChaID))
            {
                Result.Status = -1;
                Result.Message = "ID chức năng cha không hợp lệ";
                return Result;
            }

            try
            {
                Result = _ChucNangDAL.ThemMoi(chucNang);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.ERR_INSERT;
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel DanhSachCapCha()
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _ChucNangDAL.DanhSachCapCha();
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel Xoa(int idChucNang)
        {
            var Result = new BaseResultModel();
            if (_ChucNangDAL.KiemTraFKPhanQuyen(idChucNang))
            {
                Result.Status = -1;
                Result.Message = "Chức năng chứa dữ liệu không được xóa";
                return Result;
            }

            if (_ChucNangDAL.KiemTraFKMenu(idChucNang))
            {
                Result.Status = -1;
                Result.Message = "Chức năng chứa dữ liệu không được xóa";
                return Result;
            }

            if (_ChucNangDAL.KiemTraFKChucNangApDung(idChucNang))
            {
                Result.Status = -1;
                Result.Message = "Chức năng chứa dữ liệu không được xóa";
                return Result;
            }

            if (_ChucNangDAL.KiemTraFKHuongDanSuDung(idChucNang))
            {
                Result.Status = -1;
                Result.Message = "Chức năng chứa dữ liệu không được xóa";
                return Result;
            }

            if (_ChucNangDAL.KiemTraCapCon(idChucNang))
            {
                Result.Status = -1;
                Result.Message = "Chức năng cha chứa dữ liệu không được xóa";
                return Result;
            }

            try
            {
                Result = _ChucNangDAL.Xoa(idChucNang);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.Message;
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ChiTiet(int? idChucNang)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _ChucNangDAL.ChiTiet(idChucNang);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel Sua(DanhMucChucNangSua chucNang)
        {
            var Result = new BaseResultModel();
            if (chucNang.MaChucNang.Contains(" ") || HasUnicodeChar(chucNang.MaChucNang))
            {
                Result.Status = -1;
                Result.Message = "Mã chức năng không được chứa ký tự, khoảng trắng";
            }

            if (_ChucNangDAL.KiemTraCapCha(chucNang.ChucNangID) && chucNang.ChucNangChaID != null)
            {
                Result.Status = -1;
                Result.Message = "Đây là chức năng cha không được sửa lại chức năng cha!";
                return Result;
            }

            if (_ChucNangDAL.KiemTraChucNang(chucNang.ChucNangID, chucNang.TenChucNang, chucNang.MaChucNang))
            {
                Result.Status = -1;
                Result.Message = "Tên, mã chức năng đã tồn tại";
                return Result;
            }

            try
            {
                Result = _ChucNangDAL.Sua(chucNang);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                Result.Data = null;
            }

            return Result;
        }
    }
}