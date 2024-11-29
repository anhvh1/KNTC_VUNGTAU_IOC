using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.HeThong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using static Com.Gosol.KNTC.Model.HeThong.ChucNangInfo;

namespace Com.Gosol.KNTC.BUS.HeThong
{
    public class HuongDanSuDungBUS
    {
        private HuongDanSuDungDAL _HuongDanSuDungDAL;
        public HuongDanSuDungBUS()
        {
            _HuongDanSuDungDAL = new HuongDanSuDungDAL();
        }

        public BaseResultModel Delete(List<int> ListHuongDanSuDungID, int CanBoID)
        {
            return _HuongDanSuDungDAL.Delete(ListHuongDanSuDungID, CanBoID);
        }

        public HuongDanSuDungModel GetByID(int HuongDanSuDungID)
        {
            return _HuongDanSuDungDAL.GetByID(HuongDanSuDungID);
        }

        public HuongDanSuDungModel GetByMaChucNang(string MaChucNang, int CapID)
        {
            return _HuongDanSuDungDAL.GetByMaChucNang(MaChucNang, CapID);
        }

        public List<HuongDanSuDungModel> GetPagingBySearch(BasePagingParamsForFilter p, ref int TotalRow)
        {
            return _HuongDanSuDungDAL.GetPagingBySearch(p, ref TotalRow);
        }



        public BaseResultModel Update(HuongDanSuDungModel HuongDanSuDungModel, int CanBoID)
        {
            return _HuongDanSuDungDAL.Update(HuongDanSuDungModel, CanBoID);
        }

        #region V2
        public BaseResultModel CheckFile(IFormFile file)
        {
            BaseResultModel model = new BaseResultModel();
            if (file.ContentType != "application/pdf")
            {
                model.Status = -1;
                model.Message = "Chỉ được upload file pdf";
                return model;
            }

            if ((file.Length / 1024) > 10240)
            {
                model.Status = -1;
                model.Message = "Dung lượng file tối đa là 10MB";
                return model;
            }

            model.Status = 1;
            return model;
        }

        public BaseResultModel ValidateInsert(HuongDanSuDungModel HuongDanSuDungModel)
        {
            var Result = new BaseResultModel();

            if (HuongDanSuDungModel.TieuDe == null || HuongDanSuDungModel.TieuDe.Trim().Length < 1)
            {
                Result.Status = 0;
                Result.Message = "Tiêu đề không được trống";
                return Result;
            }
            //if (HuongDanSuDungModel.ChucNangID == null)
            //{
            //    Result.Status = 0;
            //    Result.Message = "Chức năng ID không được trống";
            //    return Result;
            //}

            //if (!_HuongDanSuDungDAL.KiemTraChucNang((int)HuongDanSuDungModel.ChucNangID))
            //{
            //    Result.Status = 0;
            //    Result.Message = "Chức năng ID không hợp lệ";
            //    return Result;
            //}
            //if (_HuongDanSuDungDAL.KiemTraChucNangTonTai((int)HuongDanSuDungModel.ChucNangID))
            //{
            //    Result.Status = 0;
            //    Result.Message = "Chức năng ID đã tồn tại";
            //    return Result;
            //}

            //if (_HuongDanSuDungDAL.KiemTraTieuDeTonTai(HuongDanSuDungModel.TieuDe, null))
            //{
            //    Result.Status = 0;
            //    Result.Message = "Tiêu đề đã tồn tại";
            //    return Result;
            //}

            Result.Status = 1;

            return Result;
        }
        public BaseResultModel ValidateUpdate(HuongDanSuDungModel HuongDanSuDungModel)
        {
            var Result = new BaseResultModel();

            if (!_HuongDanSuDungDAL.KiemTraID(HuongDanSuDungModel.HuongDanSuDungID))
            {
                Result.Status = 0;
                Result.Message = "Hướng dẫn sử dụng không tồn tại";
                return Result;
            }

            if (HuongDanSuDungModel.TieuDe == null || HuongDanSuDungModel.TieuDe.Trim().Length < 1)
            {
                Result.Status = 0;
                Result.Message = "Tiêu đề không được trống";
                return Result;
            }


            //if (_HuongDanSuDungDAL.KiemTraTieuDeTonTai(HuongDanSuDungModel.TieuDe, HuongDanSuDungModel.HuongDanSuDungID))
            //{
            //    Result.Status = 0;
            //    Result.Message = "Tiêu đề đã tồn tại";
            //    return Result;
            //}

            Result.Status = 1;

            return Result;
        }

        public BaseResultModel Insert(HuongDanSuDungModel HuongDanSuDungModel, int CanBoID)
        {
            var Result = new BaseResultModel();

            try
            {
                Result = _HuongDanSuDungDAL.Insert(HuongDanSuDungModel, CanBoID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.Message;
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel DanhSach_v2(HuongDanSuDungParams thamSo, string rootPath)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _HuongDanSuDungDAL.DanhSach(thamSo, rootPath);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.Message;
                Result.Data = null;
            }

            return Result;
        }
        public BaseResultModel ChiTiet(int HuongDanSuDungID, string rootPath)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _HuongDanSuDungDAL.ChiTiet(HuongDanSuDungID, rootPath);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.Message;
                Result.Data = null;
            }

            return Result;
        }
        public BaseResultModel DanhSach_ChucNang()
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _HuongDanSuDungDAL.DanhSachChucNang();
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.Message;
                Result.Data = null;
            }

            return Result;
        }

        public List<MenuModel> GetListMenuByNguoiDungID(int NguoidungID, int CapID)
        {
            return _HuongDanSuDungDAL.GetListMenuByNguoiDungID(NguoidungID, CapID);
        }

        public BaseResultModel Xoa_v2(int HuongDanSuDungID, IHostingEnvironment host)
        {
            var Result = new BaseResultModel();
            try
            {
                if (_HuongDanSuDungDAL.KiemTraID(HuongDanSuDungID))
                {
                    //Xóa File cũ
                    var model = _HuongDanSuDungDAL.ChiTiet(HuongDanSuDungID, "");
                    var file = ((HuongDanSuDungModel_v2)model.Data).TenFileHeThong;
                    var path = host.ContentRootPath+ "\\UploadFiles\\FileHuongDanSuDung\\"+file;
                    System.IO.File.Delete(path);

                    Result = _HuongDanSuDungDAL.Xoa(HuongDanSuDungID);
                }
                else
                {
                    Result.Status = 0;
                    Result.Message = "ID không tồn tại";
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.Message;
                Result.Data = null;
            }

            return Result;
        }

        #endregion
    }
}