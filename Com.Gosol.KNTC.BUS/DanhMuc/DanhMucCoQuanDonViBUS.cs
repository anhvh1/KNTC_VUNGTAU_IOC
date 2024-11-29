using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucCoQuanDonViBUS 
    {

        private DanhMucCoQuanDonViDAL _DanhMucCoQuanDonViDAL;
        public DanhMucCoQuanDonViBUS()
        {
            this._DanhMucCoQuanDonViDAL = new DanhMucCoQuanDonViDAL();
        }
        public int Insert(DanhMucCoQuanDonViModel DanhMucCoQuanDonViModel, ref int CoQuanID,int NguoiDungID, ref string Message, int? CoQuanDangNhapID)
        {
            int val = 0;
            try
            {
                val = _DanhMucCoQuanDonViDAL.Insert(DanhMucCoQuanDonViModel, ref CoQuanID, NguoiDungID, ref Message,CoQuanDangNhapID);
                return val;
            }
            catch (Exception ex)
            {
                return val;
                throw ex;
            }
        }
        public int Update(DanhMucCoQuanDonViModel DanhMucCoQuanDonViModel, ref string Message)
        {
            int val = 0;
            try
            {
                val = _DanhMucCoQuanDonViDAL.Update(DanhMucCoQuanDonViModel, ref Message);
                return val;
            }
            catch (Exception ex)
            {
                return val;
                throw ex;
            }
        }

        public int UpdateQuyTrinh(DanhMucCoQuanDonViModel DanhMucCoQuanDonViModel, ref string Message)
        {
            int val = 0;
            try
            {
                val = _DanhMucCoQuanDonViDAL.UpdateQuyTrinh(DanhMucCoQuanDonViModel, ref Message);
                return val;
            }
            catch (Exception ex)
            {
                return val;
                throw ex;
            }
        }

        public int Insert_New(DanhMucCoQuanDonViModel DanhMucCoQuanDonViModel, ref int CoQuanID, int NguoiDungID, ref string Message, int? CoQuanDangNhapID)
        {
            int val = 0;
            try
            {
                val = _DanhMucCoQuanDonViDAL.Insert_New(DanhMucCoQuanDonViModel, ref CoQuanID, NguoiDungID, ref Message, CoQuanDangNhapID);
                return val;
            }
            catch (Exception ex)
            {
                return val;
                throw ex;
            }
        }
        public int Update_New(DanhMucCoQuanDonViModel DanhMucCoQuanDonViModel, ref string Message)
        {
            int val = 0;
            try
            {
                val = _DanhMucCoQuanDonViDAL.Update_New(DanhMucCoQuanDonViModel, ref Message);
                return val;
            }
            catch (Exception ex)
            {
                return val;
                throw ex;
            }
        }
        public Dictionary<int, string> Delete(List<int> ListCoQuanID)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            try
            {
                dic = _DanhMucCoQuanDonViDAL.Delete(ListCoQuanID);
                return dic;
            }
            catch (Exception ex)
            {
                return dic;
                throw ex;
            }
        }
        public List<DanhMucCoQuanDonViModel> FilterByName(string TenCoQuan)
        {
            return _DanhMucCoQuanDonViDAL.FilterByName(TenCoQuan);
        }
        public DanhMucCoQuanDonViPartialNew GetByID(int CoQuanID)
        {
            return _DanhMucCoQuanDonViDAL.GetByID(CoQuanID);
        }
        public DanhMucCoQuanDonViPartialNew GetByID_New(int CoQuanID)
        {
            return _DanhMucCoQuanDonViDAL.GetByID_New(CoQuanID);
        }
        public List<DanhMucCoQuanDonViModel> GetListByidAndCap()
        {
            return _DanhMucCoQuanDonViDAL.GetListByidAndCap();
        }
        public List<DanhMucCoQuanDonViModelPartial> GetAllByCap(int ID, int Cap, string Keyword)
        {
            return _DanhMucCoQuanDonViDAL.GetAllByCap(ID, Cap, Keyword);
        }

        public List<DanhMucCoQuanDonViModelPartial> GetALL(int ID, int CapCoQuanID, string Keyword)
        {
            return _DanhMucCoQuanDonViDAL.GetALL(ID, CapCoQuanID, Keyword);
        }

        public List<DanhMucCoQuanDonViModel> GetListByUser(int CoQuanID, int NguoiDungID)
        {
            return _DanhMucCoQuanDonViDAL.GetListByUser(CoQuanID, NguoiDungID);
        }

        public List<CoQuanInfo> GetListCoQuanByUser(int CoQuanID, int NguoiDungID)
        {
            var list = new CoQuan().GetAllCoQuan().ToList();
            if(CoQuanID > 0 && !UserRole.CheckAdmin(NguoiDungID))
            {
                list = list.Where(x => x.CoQuanID == CoQuanID).ToList();
            }
            return list;
        }

        public int ImportFile(string FilePath, ref string Message, int NguoiDungID)
        {
            return _DanhMucCoQuanDonViDAL.ImportFile(FilePath, ref  Message, NguoiDungID);
        }

        public List<DanhMucCoQuanDonViModel> GetListByUser_FoPhanQuyen(int CoQuanID, int NguoiDungID)
        {
            return _DanhMucCoQuanDonViDAL.GetListByUser_FoPhanQuyen(CoQuanID, NguoiDungID);
        }

        public List<DanhMucCoQuanDonViModel> GetByUser_FoPhanQuyen(int CoQuanID, int NguoiDungID, string KeyWord)
        {
            return _DanhMucCoQuanDonViDAL.GetByUser_FoPhanQuyen(CoQuanID, NguoiDungID, KeyWord);
        }
        public BaseResultModel CheckMaCQ(int? CoQuanID, string MaCQ)
        {
            return _DanhMucCoQuanDonViDAL.CheckMaCQ(CoQuanID, MaCQ);
        }

        public List<DanhMucCoQuanDonViModel> GetAll(int? CoQuanID, int? NguoiDungID)
        {
            return _DanhMucCoQuanDonViDAL.GetAll(CoQuanID,NguoiDungID);
        }

        public DanhMucCoQuanDonViModel GetByTicKet(string TicKet)
        {
            return _DanhMucCoQuanDonViDAL.GetByTicKet(TicKet);
        }
        public string GenerateTicKet(string key, string plainText)
        {
            return _DanhMucCoQuanDonViDAL.GenerateTicKet(key, plainText);
        }
        public DanhMucCoQuanDonViModel GetCoQuanByCanBoID(int CanBoID)
        {
            return _DanhMucCoQuanDonViDAL.GetCoQuanByCanBoID(CanBoID);
        }
        public int UpdateNgayReset(DanhMucCoQuanDonViModel DanhMuKNTCoQuanDonViModel, ref string Message)
        {
            return _DanhMucCoQuanDonViDAL.UpdateNgayReset(DanhMuKNTCoQuanDonViModel, ref Message);
        }
        public DateTime? GetNgayReset(int CoQuanID)
        {
            return _DanhMucCoQuanDonViDAL.GetNgayReset(CoQuanID);
        }
    }
}
