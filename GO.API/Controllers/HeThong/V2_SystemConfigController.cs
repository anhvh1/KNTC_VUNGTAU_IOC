using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestSharp;

namespace GO.API.Controllers.HeThong
{
    [Route("api/v2/SystemconFigv2")]
    [ApiController]
    public class V2_SystemConfigController : BaseApiController
    {
        private V2_SystemConfigBUS1 _V2_SystemConfigBUS1;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment host;
        private IConfiguration config;
        private IOptions<AppSettings> appSettings;
        public V2_SystemConfigController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<V2_SystemConfigController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            _V2_SystemConfigBUS1 = new V2_SystemConfigBUS1();
            host = hostingEnvironment;
            this.config = config;
            appSettings = Settings;
        }



        [HttpGet]
        [Route("DanhSachThamSoHeThong")]
        [CustomAuthAttribute(ChucNangEnum.ThamSoHeThong, AccessLevel.Read)]
        public IActionResult DanhSach([FromQuery] ThamSoLocDanhMuc p)
        {
            try
            {
                var Result = _V2_SystemConfigBUS1.DanhSach(p);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                Status = -1;
                Message = ConstantLogMessage.API_Error_System;
                return GetActionResult();
            }
        }

        [HttpGet]
        [Route("ChiTietThamSoHeThong")]
        [CustomAuthAttribute(ChucNangEnum.ThamSoHeThong, AccessLevel.Read)]
        public IActionResult ChiTiet([FromQuery] int? SystemConfigID)
        {
            try
            {
                var Result = _V2_SystemConfigBUS1.ChiTiet(SystemConfigID);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                Status = -1;
                Message = ConstantLogMessage.API_Error_System;
                return GetActionResult();
            }
        }

        [HttpPost]
        [Route("ThemMoiThamSoHeThong")]
        [CustomAuthAttribute(ChucNangEnum.ThamSoHeThong, AccessLevel.Create)]
        public IActionResult ThemMoi([FromBody] V2_SystemConfigMODADD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = _V2_SystemConfigBUS1.ThemMoi(item);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception)
            {
                Status = -1;
                Message = ConstantLogMessage.API_Error_System;
                return GetActionResult();
            }
        }


        [HttpPost]
        [Route("CapNhatThamSoHeThong")]
        [CustomAuthAttribute(ChucNangEnum.ThamSoHeThong, AccessLevel.Create)]
        public IActionResult CapNhat([FromBody] V2_SystemConfigMOD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = _V2_SystemConfigBUS1.CapNhat(item);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception)
            {
                Status = -1;
                Message = ConstantLogMessage.API_Error_System;
                return GetActionResult();
            }
        }


        [HttpPost]
        [Route("XoaThamSoHeThong")]
        [CustomAuthAttribute(ChucNangEnum.ThamSoHeThong, AccessLevel.Delete)]
        public IActionResult Xoa([FromBody] int? SystemConfigID)
        {
            if (SystemConfigID == null) return BadRequest();
            var Result = _V2_SystemConfigBUS1.Xoa(SystemConfigID);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
}