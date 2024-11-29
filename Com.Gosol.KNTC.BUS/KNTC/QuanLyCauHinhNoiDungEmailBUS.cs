using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class QuanLyCauHinhNoiDungEmailBUS
    {
        public IList<QL_EmailInfo> GetBySearch(ref int TotalRow, BasePagingParams p, int? LoaiEmailID)
        { 
            string keyword = "%" + (p.Keyword ?? "") + "%";
            int currentPage = p.PageNumber;
            if (currentPage == 0)
            {
                currentPage = 1;
            }
            int start = (currentPage - 1) * p.PageSize;
            int end = currentPage * p.PageSize;
            List<QL_EmailInfo> lst = new QL_Email().GetBySear(keyword, start, end, LoaiEmailID);

            return lst;
        }

        public IList<QL_DmEmailInfo> DMLoaiEmail(ref int TotalRow, BasePagingParams p)
        {
            string keyword = "%" + p.Keyword + "%";
            int currentPage = p.PageNumber;
            if (currentPage == 0)
            {
                currentPage = 1;
            }
            int start = (currentPage - 1) * p.PageSize;
            int end = currentPage * p.PageSize;
            List<QL_DmEmailInfo> lst_DmEmail = new List<QL_DmEmailInfo>();
            lst_DmEmail = new QL_Email().GetLoaiEmail("%%", 0, 0);

            return lst_DmEmail;
        }

        public QL_EmailInfo GetByID(int EmailID)
        {
            QL_EmailInfo info = new QL_Email().GetByID(EmailID);
            return info;
        }

        public BaseResultModel Delete(int EmailID)
        {
            var Result = new BaseResultModel();
            var kq = new QL_Email().Delete(EmailID);
            if (kq > 0)
            {
                Result.Status = 1;
                Result.Message = Constant.CONTENT_DELETE_SUCCESS;
            }
            else
            {
                Result.Status = 0;
                Result.Message = Constant.CONTENT_DELETE_ERROR;
            }
            return Result;
        }

        public BaseResultModel Save(QL_EmailInfo Info)
        {
            var Result = new BaseResultModel();
            if (Info.EmailID > 0)
            {
                int kq = new QL_Email().Update(Info);
                if (kq != 0)
                {
                    Result.Status = 1;
                    Result.Message = Constant.CONTENT_MESSAGE_SUCCESS;
                }
                else
                {
                    Result.Status = 0;
                    Result.Message = Constant.CONTENT_MESSAGE_ERROR;
                }
            }
            else
            {
                int cbID = new QL_Email().Insert(Info);
                if (cbID != 0)
                {
                    Info.EmailID = cbID;
                    Result.Status = 1;
                    Result.Message = Constant.CONTENT_MESSAGE_SUCCESS;
                   
                }
                else
                {
                    Result.Status = 0;
                    Result.Message = Constant.CONTENT_MESSAGE_ERROR;
                }
            }

            Result.Data = Info;
            return Result;
        }

    }
}
