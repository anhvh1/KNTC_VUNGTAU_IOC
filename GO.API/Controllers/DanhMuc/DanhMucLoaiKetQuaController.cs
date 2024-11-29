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
    [Route("api/v2/DanhMucLoaiKetQua")]
    [ApiController]
    public class DanhMucLoaiKetQuaController : BaseApiController
    {
        private DanhMucLoaiKetQuaBUS danhMucLoaiKetQuaBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment host;
        private IConfiguration config;
        private IOptions<AppSettings> appSettings;
        public DanhMucLoaiKetQuaController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DanhMucLoaiKetQuaController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this.danhMucLoaiKetQuaBUS = new DanhMucLoaiKetQuaBUS();
            this.host = hostingEnvironment;
            this.config = config;
            this.appSettings = Settings;
        }



        [HttpGet]
        [Route("DanhSachLoaiKetQua")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSach([FromQuery] ThamSoLocDanhMuc p)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucLoaiKetQuaBUS.DanhSach(p);
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
        [Route("ChiTietLoaiKetQua")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult ChiTiet([FromQuery] int? LoaiKetQuaID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucLoaiKetQuaBUS.ChiTiet(LoaiKetQuaID);
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
        [Route("ThemMoiLoaiKetQua")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucLoaiKetQua, AccessLevel.Create)]
        public IActionResult ThemMoi([FromBody] DanhMucLoaiKetQuaThemMoiMOD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = danhMucLoaiKetQuaBUS.ThemMoi(item);
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
        [Route("CapNhatLoaiKetQua")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucLoaiKetQua, AccessLevel.Edit)]
        public IActionResult CapNhat([FromBody] DanhMucLoaiKetQuaMOD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = danhMucLoaiKetQuaBUS.CapNhat(item);
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
        [Route("XoaLoaiKetQua")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucLoaiKetQua, AccessLevel.Delete)]
        public IActionResult Xoa([FromBody] int? LoaiKetQuaID)
        {
            if (LoaiKetQuaID == null) return BadRequest();
            var Result = danhMucLoaiKetQuaBUS.Xoa(LoaiKetQuaID);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
}