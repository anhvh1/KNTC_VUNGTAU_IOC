using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class QuanLyQuyTrinhNghiepVuBUS
    {
        public IList<QuyTrinhInfo> GetAllCap()
        {
            IList<QuyTrinhInfo> list = new QuyTrinh().GetAllTenCapByChaID(0);
            return list;
        }

        public IList<QuyTrinhInfo> GetCoQuanByCap(int CapID)
        {
            IList<QuyTrinhInfo> list = new QuyTrinh().GetAllTenCapByChaID(CapID);
            return list;
        }

        public IList<QuyTrinhInfo> GetQuyTrinhByCap(int CapID)
        {
            IList<QuyTrinhInfo> list = new QuyTrinh().GetByCapID(CapID);
            return list;
        }

        public BaseResultModel Save(QuyTrinhInfo Info)
        {
            var Result = new BaseResultModel();
            if (Info.QuyTrinhID > 0)
            {
                int kq = new QuyTrinh().Update(Info);
                if (kq != 0)
                {
                    Result.Data = Info.QuyTrinhID;
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
                int QuyTrinhID = new QuyTrinh().Insert(Info);
                if (QuyTrinhID != 0)
                {
                    Result.Data = QuyTrinhID;
                    Result.Status = 1;
                    Result.Message = Constant.CONTENT_MESSAGE_SUCCESS;

                }
                else
                {
                    Result.Status = 0;
                    Result.Message = Constant.CONTENT_MESSAGE_ERROR;
                }
            }

            return Result;
        }
    }
}
