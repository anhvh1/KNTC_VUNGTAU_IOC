using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2016.Excel;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class LichTiepDanBUS
    {
        public List<LichTiepDanInfo> GetBySearch(ref int TotalRow, LichTiepDanParams p)
        {
            List<LichTiepDanInfo> DataSource = new List<LichTiepDanInfo>();
            int currentPage = p.PageNumber;
            string keyword = p.Keyword ?? "";
            int start = (currentPage - 1) * p.PageSize;
            int end = currentPage * p.PageSize;
            int coQuanID = p.CoQuanID ?? 0;
            keyword = "%" + keyword + "%";
            string parmKeyword = keyword;
            DataSource = new LichTiepDan().GetLichTiepBySearch(keyword, coQuanID, start, end, ref TotalRow);

            return DataSource;
        }

        public BaseResultModel Save(LichTiepDanInfo Info)
        {
            var Result = new BaseResultModel();
            if(Info.IDLichTiep > 0)
            {
                try
                {
                    Info.EditDate = DateTime.Now;
                    new LichTiepDan().Update(Info);
                    Result.Message = Constant.CONTENT_MESSAGE_SUCCESS;
                    Result.Status = 1;
                }
                catch
                {
                    Result.Message = Constant.CONTENT_MESSAGE_ERROR;
                    Result.Status = 0;
                }
            }
            else
            {
                int status = new LichTiepDan().Insert(Info);

                if (status > 0)
                {
                    Result.Message = Constant.MESSAGE_INSERT_SUCCESS;
                    Result.Status = 1;
                }
                else
                {
                    Result.Message = Constant.MESSAGE_INSERT_ERROR;
                    Result.Status = 0;
                }
            }
            
            return Result;
        }

        public BaseResultModel UpdateTrangThaiPublish(LichTiepDanInfo Info)
        {
            var Result = new BaseResultModel();
            new LichTiepDan().UpdateTrangThaiPublish(Info);
            Result.Message = Constant.CONTENT_MESSAGE_SUCCESS;
            Result.Status = 1;
            return Result;
        }

        public BaseResultModel Delete(int IDLoaiTin)
        {
            var Result = new BaseResultModel();
            if (IDLoaiTin != 0)
            {
                try
                {
                    new LichTiepDan().Delete(IDLoaiTin);
                    Result.Message = Constant.CONTENT_DELETE_SUCCESS;
                    Result.Status = 1;
                }
                catch
                {
                    Result.Message = Constant.CONTENT_DELETE_ERR;
                    Result.Status = 1;
                   
                }
            }

            return Result;
        }

        public LichTiepDanInfo GetByID(int IDLichTiep)
        {
            LichTiepDanInfo loaiTinInfo = new LichTiepDanInfo();
            loaiTinInfo = new LichTiepDan().GetLichTiepByID(IDLichTiep);
            return loaiTinInfo;
        }

        public List<LichTiepDanInfo> GetDataLichTiepDan(int coquanid, int thang, int nam)
        {
            List<LichTiepDanInfo> result = new List<LichTiepDanInfo>();
            result = new LichTiepDan().GetLichTiepByCoQuanAndThangNam(coquanid, thang, nam);
          
            return result;
        }
    }
}
