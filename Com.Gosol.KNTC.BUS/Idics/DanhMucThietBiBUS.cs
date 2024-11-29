using Com.Gosol.KNTC.DAL.Idics;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.Idics;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.Idics
{
    public class DanhMucThietBiBUS
    {
        private DanhMucThietBiDAL _DanhMucThietBiDAL;
        public DanhMucThietBiBUS()
        {
            _DanhMucThietBiDAL = new DanhMucThietBiDAL();
        }
        public BaseResultModel GetPagingBySearch(BasePagingParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                int TotalRow = 0;

                var Data = _DanhMucThietBiDAL.GetPagingBySearch(p, ref TotalRow);
                if (Data.Count > 0)
                {
                    Result.Data = Data;
                }
                else
                {
                    Result.Message = ConstantLogMessage.API_NoData;
                }

                Result.Status = 1;
                Result.Message = "";
                Result.TotalRow = TotalRow;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                //Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }
        public BaseResultModel Insert(DanhMucThietBiModel DanhMucThietBiModel)
        {
            return _DanhMucThietBiDAL.Insert(DanhMucThietBiModel);
        }
        public BaseResultModel Update(DanhMucThietBiModel DanhMucThietBiModel)
        {
            return _DanhMucThietBiDAL.Update(DanhMucThietBiModel);
        }
        public DanhMucThietBiModel GetByID(string MachineId)
        {
            return _DanhMucThietBiDAL.GetByID(MachineId);
        }
    }
}
