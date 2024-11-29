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
    [Route("api/v2/QuanLyCauHinhNoiDungEmail")]
    [ApiController]
    public class QuanLyCauHinhNoiDungEmailController : BaseApiController
    {
        private QuanLyCauHinhNoiDungEmailBUS _QuanLyCauHinhNoiDungEmailBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public QuanLyCauHinhNoiDungEmailController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<QuanLyCauHinhNoiDungEmailController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._QuanLyCauHinhNoiDungEmailBUS = new QuanLyCauHinhNoiDungEmailBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetListPaging")]
        [CustomAuthAttribute(ChucNangEnum.QLEmail, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] BasePagingParams p, int? LoaiEmailID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<QL_EmailInfo> Data;
                Data = _QuanLyCauHinhNoiDungEmailBUS.GetBySearch(ref TotalRow, p, LoaiEmailID);
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
                throw ex;
            }
        }

        [HttpGet]
        [Route("DMLoaiEmail")]
        [CustomAuthAttribute(ChucNangEnum.QLEmail, AccessLevel.Read)]
        public IActionResult DMLoaiEmail([FromQuery] BasePagingParams p)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<QL_DmEmailInfo> Data;
                Data = _QuanLyCauHinhNoiDungEmailBUS.DMLoaiEmail(ref TotalRow, p);
                base.Status = 1;
                base.TotalRow = Data.Count;
                base.Data = Data;
                return base.GetActionResult();

            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }
        }

        [HttpPost]
        [Route("Save")]
        [CustomAuthAttribute(ChucNangEnum.QLEmail, AccessLevel.Edit)]
        public IActionResult SaveQuanLyCauHinhNoiDungEmail(QL_EmailInfo Info)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                { 
                    var Data = _QuanLyCauHinhNoiDungEmailBUS.Save(Info);
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

        [Route("GetByID")]
        [HttpGet]
        [CustomAuthAttribute(ChucNangEnum.QLEmail, AccessLevel.Read)]
        public IActionResult GetByID(int? EmailID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                QL_EmailInfo Data;
                Data = _QuanLyCauHinhNoiDungEmailBUS.GetByID(EmailID ?? 0);
                base.Status = 1;
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }



        [HttpPost]
        [Route("Delete")]
        [CustomAuthAttribute(ChucNangEnum.QLEmail, AccessLevel.Delete)]
        public IActionResult Delete(BaseDeleteParams p)
        {
            try
            {
                if (p.ListID != null && p.ListID.Count > 0)
                {
                    foreach (var item in p.ListID)
                    {
                        var Data = _QuanLyCauHinhNoiDungEmailBUS.Delete(item);
                        base.Data = Data.Data;
                        base.Status = Data.Status;
                        base.Message = Data.Message;
                    }
                }

                return base.GetActionResult();
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
