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
namespace GO.API.Controllers.DanhMuc
{
    [Route("api/v2/DanhMucPhongBan")]
    [ApiController]
    public class DanhMucPhongBanController : BaseApiController
    {
        private DanhMucPhongBanBUS danhMucPhongBanBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment host;
        private IConfiguration config;
        private IOptions<AppSettings> appSettings;
        public DanhMucPhongBanController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DanhMucPhongBanController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this.danhMucPhongBanBUS = new DanhMucPhongBanBUS();
            this.host = hostingEnvironment;
            this.config = config;
            this.appSettings = Settings;
        }

        [HttpGet]
        [Route("DanhSachPhongBan")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSach(int? CoQuanID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucPhongBanBUS.DanhSach(CoQuanID);
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
        [Route("ChiTietPhongBan")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult ChiTiet([FromQuery] int? PhongBanID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucPhongBanBUS.ChiTiet(PhongBanID);
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
        [Route("ThemMoiPhongBan")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Create)]
        public IActionResult ThemMoi([FromBody] DanhMucPhongBanThemMoiMOD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = danhMucPhongBanBUS.ThemMoi(item);
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
        [Route("CapNhatPhongBan")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Edit)]
        public IActionResult CapNhat([FromBody] DanhMucPhongBanMOD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = danhMucPhongBanBUS.CapNhat(item);
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
        [Route("XoaPhongBan")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Delete)]
        public IActionResult Xoa([FromBody] int? PhongBanID)
        {
            if (PhongBanID == null) return BadRequest();
            var Result = danhMucPhongBanBUS.Xoa(PhongBanID);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

    }
}
