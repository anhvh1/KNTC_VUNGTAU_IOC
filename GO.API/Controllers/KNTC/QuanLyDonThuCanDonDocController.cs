using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.BUS.KNTC;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Com.Gosol.KNTC.DAL.KNTC;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.API.Controllers;

namespace GO.API.Controllers.KNTC
{
    [Route("api/v2/QuanLyDonThuCanDonDoc")]
    [ApiController]
    public class QuanLyDonThuCanDonDocController : BaseApiController
    {
        private QuanLyDonThuCanDonDocBUS _QuanLyDonThuCanDonDocBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public QuanLyDonThuCanDonDocController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<QuanLyDonThuCanDonDocController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._QuanLyDonThuCanDonDocBUS = new QuanLyDonThuCanDonDocBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetListPaging")]
        [CustomAuthAttribute(ChucNangEnum.DSDonThuCanDonDoc, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] BasePagingParamsForFilter p)
        {
            try
            {
                IdentityHelper IdentityHelper = new IdentityHelper();
                IdentityHelper.CanBoID = CanBoID;
                IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                IdentityHelper.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                IdentityHelper.UserID = IdentityHelper.NguoiDungID;
                IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);
                IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);

                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                int TotalRow = 0;
                IList<DonThuDonDocInfo> Data;
                Data = _QuanLyDonThuCanDonDocBUS.GetBySearch(ref TotalRow, p, IdentityHelper);

                if (Data != null && Data.Count > 0)
                {
                   
                }

                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ex.Message;
                //base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }
        }

        //[Route("GetByID")]
        //[HttpGet]
        //public IActionResult GetByID(int QuanLyDonThuCanDonDocID)
        //{
        //    try
        //    {

        //        var clsCommon = new Commons();
        //        string serverPath = clsCommon.GetServerPath(HttpContext);
        //        QuanLyDonThuCanDonDocModel Data;
        //        Data = _QuanLyDonThuCanDonDocBUS.GetByID(QuanLyDonThuCanDonDocID);
        //        if (Data.Thumbnail != null && Data.Thumbnail.FileUrl != null)
        //        {
        //            Data.Thumbnail.FileUrl = serverPath + Data.Thumbnail.FileUrl;
        //        }
        //        if (Data.DanhSachFileDinhKem != null && Data.DanhSachFileDinhKem.Count > 0)
        //        {
        //            foreach (var item in Data.DanhSachFileDinhKem)
        //            {
        //                item.FileUrl = serverPath + item.FileUrl;
        //            }
        //        }
        //        base.Status = 1;
        //        base.Data = Data;
        //        return base.GetActionResult();

        //    }
        //    catch (Exception)
        //    {
        //        base.Status = -1;
        //        base.GetActionResult();
        //        throw;
        //    }
        //}


        [HttpPost]
        [Route("RaVanBanDonDoc")]
        [CustomAuthAttribute(ChucNangEnum.DSDonThuCanDonDoc, AccessLevel.Edit)]
        public IActionResult RaVanBanDonDoc(KetQuaInfo KetQuaInfo)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                {
                    var Data = _QuanLyDonThuCanDonDocBUS.RaVanBanDonDoc(KetQuaInfo);
                    base.Data = Data.Data;
                    base.Status = Data.Status;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }

        }

        
    }
}
