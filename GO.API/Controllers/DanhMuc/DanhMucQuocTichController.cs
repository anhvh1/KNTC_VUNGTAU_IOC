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

namespace Com.Gosol.KNTC.API.Controllers.DanhMuc
{
    [Route("api/v2/DanhMucQuocTich")]
    [ApiController]
    public class DanhMucQuocTichController : BaseApiController
    {
        private DanhMucQuocTichBUS danhMucQuocTichBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment host;
        private IConfiguration config;
        private IOptions<AppSettings> appSettings;
        public DanhMucQuocTichController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DanhMucQuocTichController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this.danhMucQuocTichBUS = new DanhMucQuocTichBUS();
            this.host = hostingEnvironment;
            this.config = config;
            this.appSettings = Settings;
        }



        [HttpGet]
        [Route("DanhSachQuocTich")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSach([FromQuery] ThamSoLocDanhMuc p)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucQuocTichBUS.DanhSach(p);
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

        [HttpGet]
        [Route("ChiTietQuocTich")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult ChiTiet([FromQuery] int? QuocTichID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucQuocTichBUS.ChiTiet(QuocTichID);
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


        [HttpPost]
        [Route("ThemMoiQuocTich")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucQuocTich, AccessLevel.Create)]
        public IActionResult ThemMoi([FromBody] DanhMucQuocTichThemMoiMOD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = danhMucQuocTichBUS.ThemMoi(item);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }


        [HttpPost]
        [Route("CapNhatQuocTich")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucQuocTich, AccessLevel.Edit)]
        public IActionResult CapNhat([FromBody] DanhMucQuocTichMOD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = danhMucQuocTichBUS.CapNhat(item);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }


        [HttpPost]
        [Route("XoaQuocTich")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucQuocTich, AccessLevel.Delete)]
        public IActionResult Xoa([FromBody] int? QuocTichID)
        {
            if (QuocTichID == null) return BadRequest();
            var Result = danhMucQuocTichBUS.Xoa(QuocTichID);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
}