using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Ultilities;
using Castle.Core.Internal;
using Com.Gosol.KNTC.Security;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class QuanLyChiaTachSapNhapCoQuanBUS
    {
        public List<QL_CQSatNhapInfo> GetBySearch(ref int TotalRow, BasePagingParamsForFilter p)
        {
            
            string keyword = "%" + p.Keyword + "%";
            int currentPage = p.PageNumber;
            if (currentPage == 0)
            {
                currentPage = 1;
            }

            int start = (currentPage - 1) * p.PageSize;
            int end = currentPage * p.PageSize;

            List<QL_CQSatNhapInfo> lst = new QL_CQSatNhap().GetBySear(start, end, keyword);
            if(lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    item.DanhSachCoQuan = GetByCoQuanMoiID(item.CoQuanMoiID).ToList();
                    if (item.ChiaTachSapNhap.ToUpper() == "SÁP NHẬP")
                    {
                        item.laSapNhap = true;
                        var cq = new CoQuan().GetCoQuanByID(item.CoQuanMoiID);
                        item.NgayThucHien = cq.NgayThucHien;
                        item.TenNguoiThucHien = cq.TenNguoiThucHien;
                        if (cq.Disable == true)
                        {
                            item.TrangThai = 2;
                        }
                        else item.TrangThai = 1;
                        if (item.DanhSachCoQuan.Count > 0)
                        {
                            foreach (var info in item.DanhSachCoQuan)
                            {
                                info.laSapNhap = true;
                            }
                        }
                    }
                    else
                    {
                        item.laSapNhap = false;
                        item.CoQuanCuID = item.CoQuanMoiID;
                        var tempTenCQ = item.TenCoQuanCu;
                        item.TenCoQuanCu = item.TenCoQuanMoi;
                        item.TenCoQuanMoi = tempTenCQ;
                        if (item.DanhSachCoQuan.Count > 0)
                        {
                            foreach (var cq in item.DanhSachCoQuan)
                            {
                                var temp = cq.CoQuanMoiID;
                                cq.CoQuanMoiID = cq.CoQuanCuID;
                                cq.TenCoQuanMoi = cq.TenCoQuanCu;
                                cq.CoQuanCuID = temp;
                                cq.laSapNhap = false;
                            }
                        }
                        var info = new CoQuan().GetCoQuanByID(item.CoQuanCuID);
                        item.NgayThucHien = info.NgayThucHien;
                        item.TenNguoiThucHien = info.TenNguoiThucHien;
                        if (info.Disable == true)
                        {
                            item.TrangThai = 2;
                        }
                        else item.TrangThai = 1;
                    }
                }
            }
            TotalRow = new QL_CQSatNhap().CountSear(keyword);
            return lst;
        }

        public BaseResultModel Insert_SN(QL_CQSatNhapInfo Info)
        {
            BaseResultModel result = new BaseResultModel();

            try
            {
                if(Info != null && Info.DanhSachCoQuan != null)
                {
                    int del = new QL_CQSatNhap().delete_SatNhap(Info.CoQuanMoiID);
                    foreach (var data in Info.DanhSachCoQuan)
                    {
                        int val = new QL_CQSatNhap().Insert_SatNhap(data.CoQuanCuID, Info.CoQuanMoiID, data.TrangThai, Info.NgayThucHien, Info.NguoiThucHienID);
                        if (val != 0)
                        {
                            if (data.CoQuanCuID != Info.CoQuanMoiID)
                            {
                                List<CanBoInfo> lst_cb = new CanBo().GetByCoQuanID(data.CoQuanCuID).ToList();
                                if (lst_cb.Count > 0)
                                {
                                    for (var i = 0; i < lst_cb.Count; i++)
                                    {
                                        int check = new QL_CQSatNhap().Insert_CB_US(lst_cb[i].CanBoID, Info.CoQuanMoiID, data.CoQuanCuID);
                                    }
                                }
                            }
                            result.Status = 1;
                            result.Message = "Sáp nhập cơ quan thành công!";
                        }
                        else
                        {
                            result.Status = 0;
                            result.Message = "Sáp nhập cơ quan thất bại!";
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
            }


            return result;
        }

        public BaseResultModel Insert_CT(QL_CQSatNhapInfo Info)
        {
            BaseResultModel result = new BaseResultModel();
          
            if(Info != null && Info.DanhSachCoQuan != null)
            {
                foreach (var data in Info.DanhSachCoQuan)
                {
                    int val = new QL_CQSatNhap().Insert_ChiaTach(Info.CoQuanCuID, data.CoQuanMoiID, Info.TrangThai, Info.NgayThucHien, Info.NguoiThucHienID);
                    if (val != 0)
                    {
                        result.Status = 1;
                        result.Message = "Chia tách cơ quan thành công!";
                    }
                    else
                    {
                        result.Status = 0;
                        result.Message = "Chia tách cơ quan thất bại!";
                    }
                }
            }
           
            return result;
        }

        public IList<QL_CQSatNhapInfo> GetByCoQuanMoiID(int ID)
        {
            IList<QL_CQSatNhapInfo> lst = new List<QL_CQSatNhapInfo>();
            try
            {
                lst = new QL_CQSatNhap().GetByCoQuanMoiID(ID);
               
            }
            catch (Exception ex)
            {
               
            }
            return lst;
        }

        public BaseResultModel Delete(QL_CQSatNhapInfo Info)
        {
            BaseResultModel result = new BaseResultModel();
            if (Info != null)
            {
                int kq = 0;
                try
                {
                    if (Info.laSapNhap == true)
                    {
                        kq = new QL_CQSatNhap().delete_SatNhap(Info.CoQuanMoiID);
                    }
                    else
                    {
                        kq = new QL_CQSatNhap().delete_ChiaTach(Info.CoQuanCuID);
                    }
                    if (kq != 0)
                    {
                        result.Message = Constant.CONTENT_DELETE_SUCCESS;
                        result.Status = 1;

                    }
                    else
                    {
                        result.Message = Constant.CONTENT_DELETE_ERROR;
                        result.Status = 0;
                    }
                }
                catch
                {
                    result.Message = Constant.CONTENT_DELETE_ERROR;
                    result.Status = -1;

                }
            }
            return result;
        }

        public List<CoQuanInfo> GetAllCQ()
        {
            List<CoQuanInfo> lst = new CoQuan().GetCoQuans_All().ToList();
            return lst;
        }
    }
}
