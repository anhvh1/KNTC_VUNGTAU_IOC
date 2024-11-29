using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workflow.Model;
using Workflow;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class RutDonBUS
    {
        public BaseResultModel SaveRutDon(IdentityHelper IdentityHelper, RutDonInfo info)
        {
            var Result = new BaseResultModel();
            if (info.XuLyDonID < 0)
            {
                Result.Status = 0;
                Result.Message = "Chưa chọn đơn thư";
                return Result;
            }
            //info.LyDo = lydorutdon;
            //info.NgayRutDon = ngayrutdon;
            //info.XuLyDonID = xulydonid;
            var val = new RutDonDAL().Insert(info);
            string commandCode = "RutDon";
            WorkflowInstance.Instance.ExecuteCommand(info.XuLyDonID, IdentityHelper.CanBoID ?? 0, commandCode, DateTime.Now, info.LyDo ?? "");


            return Result;
        }

    }
}
