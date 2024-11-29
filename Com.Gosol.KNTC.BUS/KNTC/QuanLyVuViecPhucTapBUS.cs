using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class QuanLyVuViecPhucTapBUS
    {
        public List<DTXuLyInfo> GetBySearch(ref int TotalRow, BasePagingParamsForFilter p, IdentityHelper IdentityHelper, bool DaDanhDau)
        {
            QueryFilterInfo info = new QueryFilterInfo();
            info.KeyWord = p.Keyword;
            info.LoaiKhieuToID = p.LoaiKhieuToID ?? 0;
            info.CanBoID = IdentityHelper.CanBoID ?? 0;
            info.CoQuanID = IdentityHelper.CoQuanID ?? 0;
            //if (AccessControl.User.HasPermission(ChucNangEnum.XemDuLieuDonThu, AccessLevel.Read))
            //{
            //    info.CoQuanID = Utils.ConvertToInt32(Utils.ConvertToInt32(coQuanID, 0), 0);
            //    info.CanBoID = 0;

            //}          
            List<DTXuLyInfo> lst_DonThu = new List<DTXuLyInfo>();
            lst_DonThu = new DTXuLy().GetDTVuViecPhucTap(info, DaDanhDau).ToList();
           
            return lst_DonThu;
        }

        public BaseResultModel DanhDauDonThuLaVuViecPhucTap(List<int> DanhSachDonThuID)
        {
            var Result = new BaseResultModel();
            int val = new DTXuLy().DanhDauLaVuViecPhucTap(DanhSachDonThuID);
            if(val > 0)
            {
                Result.Status = 1;
                Result.Message = "Đánh dấu vụ việc phức tạp thành công";
            }
            else
            {
                Result.Status = 0;
                Result.Message = "Đánh dấu vụ việc phức tạp thất bại";
            }
            return Result;
        }
    }
}
