using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2016.Excel;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class DonThuDaTiepNhanBUS
    {
        public IList<DTXuLyInfo> GetBySearch(ref int TotalRow, BasePagingParamsForFilter p, IdentityHelper IdentityHelper)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();
            int currentPage = p.PageNumber;
            if (currentPage == 0)
            {
                currentPage = 1;
            }
          
            QueryFilterInfo info = new QueryFilterInfo();
            info.CoQuanID = IdentityHelper.CoQuanID ?? 0;
            info.KeyWord = p.Keyword;
            info.Start = (currentPage - 1) * p.PageSize;
            info.End = currentPage * p.PageSize;
            info.LoaiKhieuToID = p.LoaiKhieuToID ?? 0;
            info.TuNgay = p.TuNgay ?? DateTime.MinValue;
            info.DenNgay = p.DenNgay ?? DateTime.MinValue;
            info.CanBoID = IdentityHelper.CanBoID ?? 0;
          
            //int roleID = IdentityHelper.RoleID ?? 0;
            //if (roleID == (int)RoleEnum.LanhDao)
            //{
                info.CanBoID = 0;
            //}

            try
            {
                ListInfo = new DTXuLy().GetDTDaTiepNhan(info, IdentityHelper.CanBoID ?? 0,ref TotalRow);
            }
            catch (Exception e)
            {
            }
     
            return ListInfo;
        }
    }
}
