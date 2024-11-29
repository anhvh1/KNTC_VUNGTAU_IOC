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
    [Route("api/v2/DanhMucTrangThaiDon")]
    [ApiController]
    public class DanhMucTrangThaiDonController : BaseApiController
    {
        private DanhMucTrangThaiDonBUS danhMucTrangThaiDonBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment host;
        private IConfiguration config;
        private IOptions<AppSettings> appSettings;
        public DanhMucTrangThaiDonController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DanhMucTrangThaiDonController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this.danhMucTrangThaiDonBUS = new DanhMucTrangThaiDonBUS();
            this.host = hostingEnvironment;
            this.config = config;
            this.appSettings = Settings;
        }



        [HttpGet]
        [Route("DanhSachTrangThaiDon")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSach([FromQuery] ThamSoLocDanhMuc p)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucTrangThaiDonBUS.DanhSach(p);
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
        [Route("ChiTietTrangThaiDon")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult ChiTiet([FromQuery] int? TrangThaiDonID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucTrangThaiDonBUS.ChiTiet(TrangThaiDonID);
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
        [Route("ThemMoiTrangThaiDon")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucTrangThaiDon, AccessLevel.Create)]
        public IActionResult ThemMoi([FromBody] DanhMucTrangThaiDonThemMoiMOD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = danhMucTrangThaiDonBUS.ThemMoi(item);
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
        [Route("CapNhatTrangThaiDon")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucTrangThaiDon, AccessLevel.Edit)]
        public IActionResult CapNhat([FromBody] DanhMucTrangThaiDonMOD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = danhMucTrangThaiDonBUS.CapNhat(item);
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
        [Route("XoaTrangThaiDon")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucTrangThaiDon, AccessLevel.Delete)]
        public IActionResult Xoa([FromBody] int? TrangThaiDonID)
        {
            if (TrangThaiDonID == null) return BadRequest();
            var Result = danhMucTrangThaiDonBUS.Xoa(TrangThaiDonID);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
}