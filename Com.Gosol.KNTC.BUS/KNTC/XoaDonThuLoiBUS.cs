using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Ultilities;
using Com.Gosol.KNTC.Models.TiepDan;
using Com.Gosol.KNTC.Models.HeThong;
using DocumentFormat.OpenXml.EMMA;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class XoaDonThuLoiBUS
    {
        public IList<SuperDeleteDTInfo> GetBySearch(ref int TotalRow, TiepDanParamsForFilter p, int? CanBoID)
        {
            int currentPage = p.PageNumber;
            if (currentPage == 0)
            {
                currentPage = 1;
            }

            QueryFilterInfo info = new QueryFilterInfo();   
            info.KeyWord = p.Keyword ?? "";
            info.Start = (currentPage - 1) * p.PageSize;
            info.End = currentPage * p.PageSize;
            info.LoaiKhieuToID = p.LoaiKhieuToID ?? 0;
            info.TuNgay = p.TuNgay ?? DateTime.MinValue;
            info.DenNgay = p.DenNgay ?? DateTime.MinValue;
            info.CanBoID = CanBoID ?? 0;
            info.CoQuanID = p.CoQuanID ?? 0;

            var Data = new SupperDeleteDT().GetSoTiepNhanGianTiep_GetALL(info);
            TotalRow = new SupperDeleteDT().Count_SoTiepNhanGianTiep_GetAll(info);
            return Data;
        }

        public BaseResultModel Delete(int donthuid, int xuLyDonID, int doiTuongBiKNID, int nhomKNID, int CanBoID)
        {
            var Result = new BaseResultModel();
            SystemLogInfo systeminfo = new SystemLogInfo();
            List<SuperDeleteDTInfo> list = new SupperDeleteDT().ListXuLyDonID(donthuid);
            int kq = 0;
            if (donthuid != 0)
            {
                try
                {
                    new SupperDeleteDT().Delete_AllXuLyDon(donthuid, xuLyDonID);
                    kq = new SupperDeleteDT().Delete_AllDonThu(donthuid, xuLyDonID, doiTuongBiKNID, nhomKNID);
                    Result.Message = Constant.CONTENT_DELETE_SUCCESS;
                    Result.Status = 1;
                  
                    systeminfo.CanBoID = CanBoID;
                    systeminfo.LogType = (int)LogType.Delete;
                    systeminfo.LogInfo = "Xóa đơn thư";
                    systeminfo.LogTime = Utils.ConvertToDateTime(DateTime.Now, DateTime.MinValue);
                    new SystemLog().Insert(systeminfo);
                }
                catch (Exception ex)
                {
                    Result.Message = Constant.CONTENT_DELETE_ERROR + " " + ex.Message;
                    Result.Status = 0;
                }
            }

            return Result;
        }

    }
}
