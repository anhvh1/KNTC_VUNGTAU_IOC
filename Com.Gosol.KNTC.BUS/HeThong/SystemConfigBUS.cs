using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.KNTC.BUS.HeThong
{

    public class SystemConfigBUS
    {
        SystemConfigDAL _SystemConfigDAL;
        public SystemConfigBUS()
        {
            _SystemConfigDAL = new SystemConfigDAL();
        }
        public BaseResultModel Insert(SystemConfigModel SystemConfigModel)
        {
            return _SystemConfigDAL.Insert(SystemConfigModel);
        }
        public BaseResultModel Update(SystemConfigModel SystemConfigModel)
        {
            return _SystemConfigDAL.Update(SystemConfigModel);
        }
        public BaseResultModel Delete(List<int> ListSystemConfigID)
        {
            return _SystemConfigDAL.Delete(ListSystemConfigID);
        }
        public SystemConfigModel GetByID(int SystemConfigID)
        {
            return _SystemConfigDAL.GetByID(SystemConfigID);
        }
        public SystemConfigModel GetByKey(string ConfigKey)
        {
            return _SystemConfigDAL.GetByKey(ConfigKey);
        }
        public List<SystemConfigModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            return _SystemConfigDAL.GetPagingBySearch(p, ref TotalRow);
        }


    }
}
