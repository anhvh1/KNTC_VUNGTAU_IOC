using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Controllers;
using Com.Gosol.KNTC.API.Controllers.DanhMuc;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.DanhMuc;
using Com.Gosol.KNTC.BUS.TiepDan;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.TiepDan;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GO.API.Controllers.TiepDan
{
    [Route("api/v2/TiepDan")]
    [ApiController]
    public class TiepDanTrucTiepController : BaseApiController
    {
        private TiepDanTrucTiepBUS tiepDanTrucTiepBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration config;
        private IOptions<AppSettings> appSettings;

        public TiepDanTrucTiepController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<TiepDanTrucTiepController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this.tiepDanTrucTiepBUS = new TiepDanTrucTiepBUS();
            this._host = hostingEnvironment;
            this.config = config;
            this.appSettings = Settings;
        }

        [HttpPost]
        [Route("ThemMoiTiepDan")]
        public IActionResult ThemMoi([FromForm] string TiepDanStr)
        {
            try
            {
                var TiepDan = JsonConvert.DeserializeObject<TiepDanTrucTiepMOD>(TiepDanStr);
                var clsComon = new Commons();
                if (TiepDan == null) return BadRequest();
                var Result = tiepDanTrucTiepBUS.ThemMoiDoiTuongKN(TiepDan);
                var Result1 = tiepDanTrucTiepBUS.ThemMoiNhomKN(TiepDan);
                if (Result is null) return NotFound();
                else return Ok(Result);
                if (Result1 is null) return NotFound();
                else return Ok(Result);
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("DanhSachLoaiDoiTuongKN")]
        //[CustomAuthAttribute(ChucNangEnum.GoManager, AccessLevel.Read)]
        public IActionResult DanhSachLoaiDoiTuongKN()
        {
            try
            {
                var Result = tiepDanTrucTiepBUS.DanhSachLoaiDoiTuongKN();
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }
    }
}
