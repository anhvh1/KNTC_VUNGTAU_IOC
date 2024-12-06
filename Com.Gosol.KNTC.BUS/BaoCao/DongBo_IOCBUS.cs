using Com.Gosol.KNTC.DAL.BaoCao;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.BaoCao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.BaoCao
{
    public class DongBo_IOCBUS
    {
        private DongBo_IOCDAL _dongBo_IOCDAL;
        public DongBo_IOCBUS()
        {
            _dongBo_IOCDAL = new DongBo_IOCDAL();   
        }
        public BaseResultModel Insert_BC_2a(List<ThongKeBC_2a_DongBo_IOC_Request> item, int nguoiDungId)
        {
            var result = new BaseResultModel(); 
            result = _dongBo_IOCDAL.Insert_BC_2a(item, nguoiDungId);
            return result;
        }
        public BaseResultModel Update_BC_2a(List<ThongKeBC_2a_DongBo_IOC_UpdateRequest> item, int nguoiDungId)
        {
            var result = new BaseResultModel();
            result = _dongBo_IOCDAL.Update_BC_2a(item, nguoiDungId);
            return result;
        }
        public List<ThongKeBC_2a_DongBo_IOC> GetList_2a (FilterDongBo_IOC p)
        {
            return _dongBo_IOCDAL.GetListBySearch(p);
        }

        public BaseResultModel Insert_BC_2b(List<ThongKeBC_2b_DongBo_IOC_Request> item, int nguoiDungId)
        {
            var result = new BaseResultModel();
            result = _dongBo_IOCDAL.Insert_BC_2b(item, nguoiDungId);
            return result;
        }
        public BaseResultModel Update_BC_2b(List<ThongKeBC_2b_DongBo_IOC_UpdateRequest> item, int nguoiDungId)
        {
            var result = new BaseResultModel();
            result = _dongBo_IOCDAL.Update_BC_2b(item, nguoiDungId);
            return result;
        }
        public List<ThongKeBC_2b_DongBo_IOC> GetList_2b(FilterDongBo_IOC p)
        {
            return _dongBo_IOCDAL.GetListBySearch_BC2b(p);
        }
    }
}
