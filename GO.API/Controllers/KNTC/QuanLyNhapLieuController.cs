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

namespace GO.API.Controllers.KNTC
{
    [Route("api/v2/QuanLyNhapLieu")]
    [ApiController]
    public class QuanLyNhapLieuController : BaseApiController
    {
        private QuanLyNhapLieuBUS _QuanLyNhapLieuBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public QuanLyNhapLieuController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<QuanLyNhapLieuController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._QuanLyNhapLieuBUS = new QuanLyNhapLieuBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetListPaging")]
        [CustomAuthAttribute(ChucNangEnum.ThongKeNhapLieu, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] BasePagingParamsForFilter p)
        {
            try
            {
                int TotalRow = 0;
                BaoCaoModel Data;
                Data = _QuanLyNhapLieuBUS.GetBySearch(ref TotalRow, p);

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
        [CustomAuthAttribute(ChucNangEnum.ThongKeNhapLieu, AccessLevel.Read)]
        public IActionResult ExportExcel([FromQuery] BasePagingParamsForFilter p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var Data = _QuanLyNhapLieuBUS.ExportExcel(p, CanBoID, ContentRootPath);
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                //base.GetActionResult();
                //throw;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }
    }
}
