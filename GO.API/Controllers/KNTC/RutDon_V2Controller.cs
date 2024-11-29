using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Controllers;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.BUS.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Office2010.Excel;
using GroupDocs.Viewer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GO.API.Controllers.KNTC
{
    [Route("api/v2/RutDon_V2")]
    [ApiController]
    public class RutDon_V2Controller : BaseApiController
    {
        private RutDon_V2BUS _rutDon_V2BUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        private FileDinhKemBUS _fileDinhKemBUS;
        public RutDon_V2Controller(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<RutDon_V2Controller> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._rutDon_V2BUS = new RutDon_V2BUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
            this._fileDinhKemBUS = new FileDinhKemBUS();
        }
        [HttpPost, Route("Insert")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        public  IActionResult Insert([FromBody] RutDon_V2Model rutDon_V2Model)
        {
            try
            {
                var canBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                if (canBoID == 0)
                {
                    base.Status = 0;
                    base.Message = "Thiếu cán bộ id";
                    return base.GetActionResult();
                }
                else
                {
                    var result = _rutDon_V2BUS.Insert(rutDon_V2Model, canBoID);
                    base.Status = result.Status;
                    base.Message = result.Message;
                    return base.GetActionResult();

                }
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }
        [HttpGet, Route("ChiTiet")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult ChiTiet(int xuLyDonID)
        {
            try
            {
                var Data = _rutDon_V2BUS.GetByXuLyDonID(xuLyDonID);

                if (Data == null || Data.RutDonID < 1)
                {
                    base.Message = "Không có dữ liệu";
                    base.Status = 0;
                    base.Data = new List<ChiTietRutDon>();
                }
                else
                {
                    //var cmClass = new Commons();
                    base.Message = " ";
                    base.Status = 1;                   
                    base.Data = Data;
                }

                return base.GetActionResult();
                //});
            }
            catch (Exception e)
            {
                base.Status = -1;
                base.Message = e.ToString();
                return base.GetActionResult();
                throw;
            }
        }
    }
}
