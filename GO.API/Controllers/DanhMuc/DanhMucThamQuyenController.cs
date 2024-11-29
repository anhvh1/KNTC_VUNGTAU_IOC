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
    [Route("api/v2/DanhMucThamQuyen")]
    [ApiController]
    public class DanhMucThamQuyenController : BaseApiController
    {
        private DanhMucThamQuyenBUS danhMucThamQuyenBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment host;
        private IConfiguration config;
        private IOptions<AppSettings> appSettings;
        public DanhMucThamQuyenController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DanhMucThamQuyenController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this.danhMucThamQuyenBUS = new DanhMucThamQuyenBUS();
            this.host = hostingEnvironment;
            this.config = config;
            this.appSettings = Settings;
        }



        [HttpGet]
        [Route("DanhSachThamQuyen")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSach([FromQuery] ThamSoLocDanhMuc p)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucThamQuyenBUS.DanhSach(p);
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
        [Route("ChiTietThamQuyen")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult ChiTiet([FromQuery] int? ThamQuyenID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucThamQuyenBUS.ChiTiet(ThamQuyenID);
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
        [Route("ThemMoiThamQuyen")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucThamQuyen, AccessLevel.Create)]
        public IActionResult ThemMoi([FromBody] DanhMucThamQuyenThemMoiMOD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = danhMucThamQuyenBUS.ThemMoi(item);
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
        [Route("CapNhatThamQuyen")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucThamQuyen, AccessLevel.Edit)]
        public IActionResult CapNhat([FromBody] DanhMucThamQuyenMOD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = danhMucThamQuyenBUS.CapNhat(item);
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
        [Route("XoaThamQuyen")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucThamQuyen, AccessLevel.Delete)]
        public IActionResult Xoa([FromBody] int? ThamQuyenID)
        {
            if (ThamQuyenID == null) return BadRequest();
            var Result = danhMucThamQuyenBUS.Xoa(ThamQuyenID);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
}