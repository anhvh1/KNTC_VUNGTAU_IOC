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
    public class PhanQuyenBUS 
    {
        private ChucNangDAL _ChucNangDAL;
        private PhanQuyenDAL _PhanQuyenDAL;
        public PhanQuyenBUS()
        {
            _ChucNangDAL = new ChucNangDAL();
            _PhanQuyenDAL = new PhanQuyenDAL();
        }
        public List<ChucNangModel> GetListChucNangByNguoiDungID(int NguoiDungID)
        {
            List<ChucNangModel> result = _ChucNangDAL.GetListChucNangByNguoiDungID(NguoiDungID);

            return result;
        }

        public List<NhomNguoiDungModel> NhomNguoiDung_GetPagingBySearch(BasePagingParamsForFilter p, int? CoQuanID, int NguoiDungID, ref int TotalRow)
        {
            return _PhanQuyenDAL.NhomNguoiDung_GetPagingBySearch(p, CoQuanID, NguoiDungID, ref TotalRow);
        }
        public BaseResultModel NhomNguoiDung_Insert(NhomNguoiDungModel NhomNguoiDungModel, int NguoiDungID, int CoQuanID)
        {
            return _PhanQuyenDAL.NhomNguoiDung_Insert(NhomNguoiDungModel, NguoiDungID, CoQuanID);
        }

        public BaseResultModel NhomNguoiDung_GetForUpdate(int? NhomNguoiDungID, int NguoiDungID, int CoQuanID)
        {
            return _PhanQuyenDAL.NhomNguoiDung_GetForUpdate(NhomNguoiDungID, NguoiDungID, CoQuanID);
        }

        public BaseResultModel NhomNguoiDung_GetChiTietByID(int? NhomNguoiDungID, int NguoiDungID, int CoQuanID)
        {
            return _PhanQuyenDAL.NhomNguoiDung_GetChiTietByID(NhomNguoiDungID, NguoiDungID, CoQuanID);
        }

        public BaseResultModel NhomNguoiDung_Delete(int? NhomNguoiDungID, int CoQuanID, int NguoiDungID)
        {
            return _PhanQuyenDAL.NhomNguoiDung_Delete(NhomNguoiDungID, CoQuanID, NguoiDungID);
        }

        public BaseResultModel NhomNguoiDung_Update(NhomNguoiDungModel NhomNguoiDungModel, int NguoiDungID, int CoQuanID)
        {
            return _PhanQuyenDAL.NhomNguoiDung_Update(NhomNguoiDungModel, NguoiDungID, CoQuanID);
        }

        public BaseResultModel NguoiDung_NhomNguoiDung_Insert(NguoiDungNhomNguoiDungModel NguoiDungNhomNguoiDungModel)
        {
            return _PhanQuyenDAL.NguoiDung_NhomNguoiDung_Insert(NguoiDungNhomNguoiDungModel);
        }

        public BaseResultModel NguoiDung_NhomNguoiDung_Insert_Multi(NguoiDungNhomNguoiDungModel NguoiDungNhomNguoiDungModel)
        {
            return _PhanQuyenDAL.NguoiDung_NhomNguoiDung_Insert_Multi(NguoiDungNhomNguoiDungModel);
        }

        public BaseResultModel NguoiDung_NhomNguoiDung_Delete(int? NguoiDungID, int? NhomNguoiDungID)
        {
            return _PhanQuyenDAL.NguoiDung_NhomNguoiDung_Delete(NguoiDungID, NhomNguoiDungID);
        }

        public BaseResultModel PhanQuyen_Insert(PhanQuyenModel PhanQuyenModel, int? CoQuanID, int? NguoiDungID)
        {
            return _PhanQuyenDAL.PhanQuyen_Insert(PhanQuyenModel, CoQuanID, NguoiDungID);
        }
        public BaseResultModel PhanQuyen_InsertMulti(List<PhanQuyenModel> PhanQuyenModel, int? CoQuanID, int? NguoiDungID)
        {
            return _PhanQuyenDAL.PhanQuyen_InsertMulti(PhanQuyenModel, CoQuanID, NguoiDungID);
        }

        public BaseResultModel PhanQuyen_Delete(int? PhanQuyenID, int? CoQuanID, int? NguoiDungID)
        {
            return _PhanQuyenDAL.PhanQuyen_Delete(PhanQuyenID, CoQuanID, NguoiDungID);
        }

        public BaseResultModel PhanQuyen_UpdateMulti(List<PhanQuyenModel> PhanQuyenModel, int? CoQuanID, int? NguoiDungID)
        {
            return _PhanQuyenDAL.PhanQuyen_UpdateMulti(PhanQuyenModel, CoQuanID, NguoiDungID);
        }

        public List<DanhMucCoQuanDonViModel> DanhMuKNTCoQuan_GetAllFoPhanQuyen(int CoQuanID, int NguoiDungID)
        {
            return _PhanQuyenDAL.DanhMuKNTCoQuan_GetAllFoPhanQuyen(CoQuanID, NguoiDungID);
        }

        public List<HeThongCanBoModel> HeThongCanBo_GetAllByListCoQuanID(int NhomNguoiDungID, int CoQuanID, int NguoiDungID)
        {
            return _PhanQuyenDAL.HeThongCanBo_GetAllByListCoQuanID(NhomNguoiDungID, CoQuanID, NguoiDungID);
        }

        public List<HeThongNguoiDungModel> HeThongNguoiDung_GetAllByListCoQuanID(int NhomNguoiDungID)
        {
            return _PhanQuyenDAL.HeThongNguoiDung_GetAllByListCoQuanID(NhomNguoiDungID);
        }

        public List<HeThongNguoiDungModel> HeThong_NguoiDung_GetListBy_NhomNguoiDungID(int NhomNguoiDungID, int NguoiDungID)
        {
            return _PhanQuyenDAL.HeThong_NguoiDung_GetListBy_NhomNguoiDungID(NhomNguoiDungID, NguoiDungID);
        }

        public List<NhomNguoiDungModel> NhomNguoiDung_GetAll(BasePagingParamsForFilter p, int? CoQuanID, int NguoiDungID, ref int TotalRow)
        {
            return _PhanQuyenDAL.NhomNguoiDung_GetAll(p, CoQuanID, NguoiDungID, ref TotalRow);
        }

        public List<ChucNangModel> ChucNang_GetQuyenDuocThaoTacTrongNhom(int NhomNguoiDungID, int CoQuanID, int NguoiDungID)
        {
            return _PhanQuyenDAL.ChucNang_GetQuyenDuocThaoTacTrongNhom(NhomNguoiDungID, CoQuanID, NguoiDungID);
        }
    }
}