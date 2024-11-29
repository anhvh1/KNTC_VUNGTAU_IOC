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
using Com.Gosol.KNTC.BUS.BaoCao;
using static Com.Gosol.KNTC.BUS.KNTC.QuanLyHoSoDonThuBUS;

namespace GO.API.Controllers.KNTC
{
    [Route("api/v2/QuanLyHoSoDonThu")]
    [ApiController]
    public class QuanLyHoSoDonThuController : BaseApiController
    {
        private QuanLyHoSoDonThuBUS _QuanLyHoSoDonThuBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public QuanLyHoSoDonThuController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<QuanLyHoSoDonThuController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._QuanLyHoSoDonThuBUS = new QuanLyHoSoDonThuBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetListPaging")]
        [CustomAuthAttribute(ChucNangEnum.QuanLyHoSoDonThu, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] BasePagingParamsForFilter p)
        {
            try
            {
                IdentityHelper IdentityHelper = new IdentityHelper();
                IdentityHelper.CanBoID = CanBoID;
                IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                IdentityHelper.HuyenID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                IdentityHelper.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                IdentityHelper.UserID = IdentityHelper.NguoiDungID;
                IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);

                int TotalRow = 0;
                IList<DonThuInfo> Data;
                Data = _QuanLyHoSoDonThuBUS.GetBySearch(ref TotalRow, p, IdentityHelper);

                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("ThongTinBoSung")]
        [CustomAuthAttribute(ChucNangEnum.QuanLyHoSoDonThu, AccessLevel.Read)]
        public IActionResult ThongTinBoSung([FromQuery] int? XuLyDonID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                List<BuocInfo> Data;
                Data = _QuanLyHoSoDonThuBUS.ThongTinBoSung(XuLyDonID ?? 0);

                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("ExportExcel")]
        [CustomAuthAttribute(ChucNangEnum.QuanLyHoSoDonThu, AccessLevel.Read)]
        public IActionResult ExportExcel([FromQuery] BasePagingParamsForFilter p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    IdentityHelper IdentityHelper = new IdentityHelper();
                    IdentityHelper.CanBoID = CanBoID;
                    IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    IdentityHelper.HuyenID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    IdentityHelper.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    IdentityHelper.UserID = IdentityHelper.NguoiDungID;
                    IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                    IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                    IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                    IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);

                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);

                    p.PageNumber = 1;
                    p.PageSize = 999999;
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = new BaseResultModel();             
                    Data = _QuanLyHoSoDonThuBUS.ExportExcel(p, ContentRootPath, IdentityHelper);
                    
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;

                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }
        //
    }
}
