using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using System;
using System.Collections.Generic;

namespace Com.Gosol.KNTC.BUS.HeThong
{
    public class HeThongCanBoBUS 
    {
        public HeThongCanBoBUS()
        {
        }
        public string GenerationMaCanBo(int CoQuanID)
        {
            return new HeThongCanBoDAL().GenerationMaCanBo(CoQuanID);
        }
        //Insert
        public int Insert(HeThongCanBoModel HeThongCanBoModel, ref int CanBoID, ref string Message, int? CoQuanDangNhapID, int? NguoiDungID, int? CanBoDangNhapID)
        {
            int val = 0;
            try
            {
                return new HeThongCanBoDAL().Insert(HeThongCanBoModel, ref CanBoID, ref Message, CoQuanDangNhapID, NguoiDungID, CanBoDangNhapID);
            }
            catch (Exception ex)
            {
                return val;
                throw ex;
            }
        }

        public int InsertNguoiDangKy(HeThongCanBoModel HeThongCanBoModel, ref string Message)
        {
            int val = 0;
            try
            {
                return new HeThongCanBoDAL().InsertNguoiDangKy(HeThongCanBoModel, ref Message);
            }
            catch (Exception ex)
            {
                return val;
                throw ex;
            }
        }
        //Update
        public int Update(HeThongCanBoModel HeThongCanBoModel, ref string Message, int? NguoiDungID)
        {
            int val = 0;
            try
            {
                val = new HeThongCanBoDAL().Update(HeThongCanBoModel, ref Message, NguoiDungID);
                return val;
            }
            catch (Exception ex)
            {
                return val;
                throw ex;
            }
        }

        // Delete
        public List<string> Delete(List<int> ListCanBoID, int? NguoiDungID)
        {
            List<string> dic = new List<string>();
            try
            {
                dic = new HeThongCanBoDAL().Delete(ListCanBoID, NguoiDungID);
                return dic;
            }
            catch (Exception ex)
            {
                return dic;
                throw ex;
            }
        }

        // Get By id
        public HeThongCanBoModel GetCanBoByID(int CanBoID)
        {

            try
            {
                return new HeThongCanBoDAL().GetCanBoByID(CanBoID);

            }
            catch (Exception ex)
            {
                return new HeThongCanBoModel();
                throw ex;
            }
        }

        // Get list by paging and search
        public List<HeThongCanBoModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int? CoQuanID, int? TrangThaiID, int CoQuan_ID, int NguoiDungID, string host)
        {
            try
            {
                return new HeThongCanBoDAL().GetPagingBySearch(p, ref TotalRow, CoQuanID, TrangThaiID, CoQuan_ID, NguoiDungID, host);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    
       
        public List<HeThongCanBoShortModel> GetThanNhanByCanBoID(int CanBoID)
        {
            return new HeThongCanBoDAL().GetThanNhanByCanBoID(CanBoID);
        }

        public List<HeThongCanBoModel> GetAllCanBoByCoQuanID(int CoQuanID, int CoQuan_ID)
        {
            return new HeThongCanBoDAL().GetAllCanBoByCoQuanID(CoQuanID, CoQuan_ID);
        }

        public List<HeThongCanBoModel> GetAllByCoQuanID(int CoQuanID)
        {
            return new HeThongCanBoDAL().GetAllByCoQuanID(CoQuanID);
        }

        public List<HeThongCanBoModel> GetAllInCoQuanCha(int CoQuanID)
        {
            return new HeThongCanBoDAL().GetAllInCoQuanCha(CoQuanID);
        }

        public ThongTinDonViModel HeThongCanBo_GetThongTinCoQuan(int CanBoID, int NguoiDungID)
        {
            return new HeThongCanBoDAL().HeThongCanBo_GetThongTinCoQuan(CanBoID, NguoiDungID);
        }

        public List<HeThongCanBoModel> GetListCanBoPhuHopVoiThoiGianCaLamviec(TimeSpan? ThoiGianBatDau, TimeSpan? ThoiGianKetThuc, int CoQuanID, int? CoQuanID_Filter)
        {
            return new HeThongCanBoDAL().GetListCanBoPhuHopVoiThoiGianCaLamviec(ThoiGianBatDau, ThoiGianKetThuc, CoQuanID, CoQuanID_Filter);
        }
        public HeThongCanBoModel GetByID(int? CanBoID)
        {
            return new HeThongCanBoDAL().GetByID(CanBoID);
        }

        public List<HeThongCanBoModel> GetAllForLichSuChamCong(int? CoQuanDangNhapID, int? NguoiDungID)
        {
            return new HeThongCanBoDAL().GetAllForLichSuChamCong(CoQuanDangNhapID, NguoiDungID);
        }

        public bool CheckCanBoCoAnhDaiDien(int CanBoID)
        {
            return new HeThongCanBoDAL().CheckCanBoCoAnhDaiDien(CanBoID);
        }

        public int InsertAnhDaiDienFromAnhTraningModel(string UrlAnh)
        {
            return new HeThongCanBoDAL().InsertAnhDaiDienFromAnhTraningModel(UrlAnh);
        }
    }
}
