using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.DanhMuc;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class DashBoardBUS
    {
        public DashBoardModel GetDuLieuDashBoard(DashBoardParams p)
        {
            return new DashBoardDAL().GetDuLieuDashBoard_New(p);
        }
        public List<CoQuanInfo> GetCoQuanByPhamViID(string PhamViID, int CapID, int TinhID, int HuyenID)
        {
            return new DashBoardDAL().GetCoQuanByPhamViID(PhamViID, CapID, TinhID, HuyenID);
        }
        public SoLieuModel DashBoard_ChuTichUBND(CanhBaoParams p)
        {
            return new DashBoardDAL().DashBoard_ChuTichUBND(p);
        }

        public SoLieuModel DashBoard_CanhBaoPheDuyetKetQuaXuLy(CanhBaoParams p)
        {
            return new DashBoardDAL().DashBoard_CanhBaoPheDuyetKetQuaXuLy(p);
        }
        
        public SoLieuModel DashBoard_CanhBaoXuLyDon(CanhBaoParams p)
        {
            return new DashBoardDAL().DashBoard_CanhBaoXuLyDon(p);
        } 
        public SoLieuModel DashBoard_CanhBaoCapNhatGiaoXacMinh(CanhBaoParams p)
        {
            return new DashBoardDAL().DashBoard_CanhBaoCapNhatGiaoXacMinh(p);
        }
        public SoLieuModel DashBoard_CanhBaoCapNhatQuyetDinhGiaiQuyet(CanhBaoParams p)
        {
            return new DashBoardDAL().DashBoard_CanhBaoCapNhatQuyetDinhGiaiQuyet(p);
        } 
        public SoLieuModel DashBoard_CanhBaoGiaiQuyetDon(CanhBaoParams p)
        {
            return new DashBoardDAL().DashBoard_CanhBaoGiaiQuyetDon(p);
        }
        public SoLieuModel DashBoard_CanhBaoGiaiQuyetDon_TP(CanhBaoParams p)
        {
            return new DashBoardDAL().DashBoard_CanhBaoGiaiQuyetDon_TP(p);
        }

        public SoLieuModel GetDataDashBoard_By_User(CanhBaoParams p)
        {
            return new DashBoardDAL().GetDataDashBoard_By_User(p);
        }
    }
}
