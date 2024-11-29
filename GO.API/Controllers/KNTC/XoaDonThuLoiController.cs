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
using XoaDonThuLoiBUS = Com.Gosol.KNTC.BUS.KNTC.XoaDonThuLoiBUS;

namespace GO.API.Controllers.KNTC
{
    [Route("api/v2/XoaDonThuLoi")]
    [ApiController]
    public class XoaDonThuLoiController : BaseApiController
    {
        private XoaDonThuLoiBUS _XoaDonThuLoiBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public XoaDonThuLoiController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<XoaDonThuLoiController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._XoaDonThuLoiBUS = new XoaDonThuLoiBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetListPaging")]
        [CustomAuthAttribute(ChucNangEnum.XoaDonThuLoi, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] TiepDanParamsForFilter p)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                if(!UserRole.CheckAdmin(NguoiDungID))
                {
                    p.CoQuanID = CoQuanID;
                }
                int TotalRow = 0;
                IList<SuperDeleteDTInfo> Data;
                Data = _XoaDonThuLoiBUS.GetBySearch(ref TotalRow, p, CanBoID);
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

        [HttpPost]
        [Route("Delete")]
        [CustomAuthAttribute(ChucNangEnum.XoaDonThuLoi, AccessLevel.Delete)]
        public IActionResult Delete(TiepNhanDonDeleteParams p)
        {
            try
            {
                var Data = _XoaDonThuLoiBUS.Delete(p.DonThuID ?? 0, p.XuLyDonID ?? 0, p.DoiTuongBiKNID ?? 0 , p.NhomKNID ?? 0, CanBoID);
                base.Data = Data.Data;
                base.Status = Data.Status;
                base.Message = Data.Message;

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
