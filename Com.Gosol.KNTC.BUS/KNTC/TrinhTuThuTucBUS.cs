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
    public class TrinhTuThuTucBUS
    {
        public List<TrinhTuThuTucModel> GetBySearch(ref int TotalRow, BasePagingParams p)
        {
            List<TrinhTuThuTucModel> DataSource = new TrinhTuThuTucDAL().GetPagingBySearch(p, ref TotalRow);
            foreach (var item in DataSource)
            {
                item.Thumbnail = new FileDinhKemDAL().GetByNgiepVuID(item.TrinhTuThuTucID, EnumLoaiFile.FileThumbnail.GetHashCode()).FirstOrDefault();
            }
            return DataSource;
        }

        public BaseResultModel Save(TrinhTuThuTucModel Info)
        {
            var Result = new BaseResultModel();
            if (Info.TrinhTuThuTucID > 0)
            {
                try
                {
 
                    new TrinhTuThuTucDAL().Update(Info);
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
                Info.TrinhTuThuTucID = new TrinhTuThuTucDAL().Insert(Info);
                if (Info.TrinhTuThuTucID > 0)
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
            if (Info.TrinhTuThuTucID > 0 && Info.DanhSachFileDinhKemID != null && Info.DanhSachFileDinhKemID.Count > 0)
            {
                new FileDinhKemDAL().UpdateNghiepVuID(Info.DanhSachFileDinhKemID, Info.TrinhTuThuTucID);
            }

            return Result;
        }

        public BaseResultModel UpdateTrangThaiPublic(TrinhTuThuTucModel Info)
        {
            var Result = new BaseResultModel();
            new TrinhTuThuTucDAL().UpdateTrangThaiPublic(Info);
            Result.Message = Constant.CONTENT_MESSAGE_SUCCESS;
            Result.Status = 1;
            return Result;
        }

        public BaseResultModel Delete(int ID)
        {
            var Result = new BaseResultModel();
            if (ID != 0)
            {
                try
                {
                    new TrinhTuThuTucDAL().Delete(ID);
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

        public TrinhTuThuTucModel GetByID(int TrinhTuThuTucID)
        {
            TrinhTuThuTucModel Info = new TrinhTuThuTucModel();
            Info = new TrinhTuThuTucDAL().GetByID(TrinhTuThuTucID);
            Info.Thumbnail = new FileDinhKemDAL().GetByNgiepVuID(Info.TrinhTuThuTucID, EnumLoaiFile.FileThumbnail.GetHashCode()).FirstOrDefault(); 
            Info.DanhSachFileDinhKem = new FileDinhKemDAL().GetByNgiepVuID(Info.TrinhTuThuTucID, EnumLoaiFile.FileTrinhTuThuTuc.GetHashCode());
            return Info;
        }

    }
}
