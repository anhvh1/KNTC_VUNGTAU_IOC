using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.DAL.HeThong;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Com.Gosol.KNTC.Ultilities;

namespace Com.Gosol.KNTC.BUS.HeThong
{
    public interface ISystemLogBUS
    {
        int Insert(SystemLogModel systemLogInfo);
        List<SystemLogPartialModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow);
        public List<SystemLogPartialModel> GetPagingByQuanTriDuLieu(BasePagingParams p, ref int TotalRow);
        public XDocument CreateLogFile(string SystemLogPath, string TuNgay, string DenNgay);
    }
    public class SystemLogBUS : ISystemLogBUS
    {
        private SystemLogDAL _SystemLogDAL;
        public SystemLogBUS()
        {
            _SystemLogDAL = new SystemLogDAL();
        }
        public int Insert(SystemLogModel systemLogInfo)
        {
            return _SystemLogDAL.Insert(systemLogInfo);
        }
        public List<SystemLogPartialModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            return _SystemLogDAL.GetPagingBySearch(p, ref TotalRow);
        }

        public List<SystemLogPartialModel> GetPagingByQuanTriDuLieu(BasePagingParams p, ref int TotalRow)
        {
            return _SystemLogDAL.GetPagingByQuanTriDuLieu(p, ref TotalRow);
        }
        public XDocument CreateLogFile(string SystemLogPath, string TuNgay, string DenNgay)
        {
            return _SystemLogDAL.CreateLogFile(SystemLogPath, TuNgay, DenNgay);
        }
        
        public BaseResultModel DanhSach(NhatKyHeThongThamSo thamSo)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _SystemLogDAL.DanhSach(thamSo);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                Result.Data = null;
            }

            return Result;
        }
    }
}
