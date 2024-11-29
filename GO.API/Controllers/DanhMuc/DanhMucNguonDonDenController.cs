using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

//Created by TienKM 14/10/2022

namespace Com.Gosol.KNTC.API.Controllers.DanhMuc
{
    [Route("api/v2/DanhMucNguonDonDen")]
    [ApiController]
    public class DanhMucNguonDonDenController : BaseApiController
    {

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment host;
        private IConfiguration _config;
        private IOptions<AppSettings> _appSettings;
        private DanhMucNguonDonDenBUS _danhMucNguonDonDenBUS;

        public DanhMucNguonDonDenController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DanhMucDanTocController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this.host = hostingEnvironment;
            this._config = config;
            this._appSettings = Settings;
            _danhMucNguonDonDenBUS = new DanhMucNguonDonDenBUS();
        }

        [HttpGet]
        [Route("DanhSachNguonDonDen")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSach([FromQuery] ThamSoLocDanhMuc thamSoLocDanhMuc)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _danhMucNguonDonDenBUS.DanhSach(thamSoLocDanhMuc);
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
        
        [HttpGet]
        [Route("ChiTietNguonDonDen")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult ChiTiet(int NguonDonDenID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _danhMucNguonDonDenBUS.ChiTiet(NguonDonDenID);
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
        [Route("ThemMoiNguonDonDen")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucNguonDonDen, AccessLevel.Create)]
        public IActionResult ThemMoi([FromBody]ThemDanhMucNguonDonDenModel NguonDonDen)
        {
            try
            {
                var result = _danhMucNguonDonDenBUS.ThemMoi(NguonDonDen);
                return Ok(result);

            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }
        [HttpPost]
        [Route("CapNhatNguonDonDen")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucNguonDonDen, AccessLevel.Edit)]
        public IActionResult SuaNguonDonDen(DanhMucNguonDonDenModel NguonDonDen)
        {
            try
            {
                var result = _danhMucNguonDonDenBUS.SuaNguonDonDen(NguonDonDen);
                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }
        [HttpPost]
        [Route("XoaNguonDonDen")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucNguonDonDen, AccessLevel.Delete)]
        public IActionResult XoaNguonDonDen([FromBody] XoaDanhMucNguonDonDenModel NguonDonDen)
        {
            try
            {
                var Result = _danhMucNguonDonDenBUS.XoaNguonDonDen(NguonDonDen.NguonDonDenID);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }

    }
}