using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Controllers.DanhMuc;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.DanhMuc;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GO.API.Controllers.HeThong
{
    [Route("api/v2/HeThongXoaDonThuLoi")]
    [ApiController]
    public class XoaDonThuLoiController : BaseApiController
    {
        private XoaDonThuLoiBUS XoaDonThuLoiBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment host;
        private IConfiguration config;
        private IOptions<AppSettings> appSettings;
        public XoaDonThuLoiController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<XoaDonThuLoiController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this.XoaDonThuLoiBUS = new XoaDonThuLoiBUS();
            this.host = hostingEnvironment;
            this.config = config;
            this.appSettings = Settings;
        }


        //[HttpGet]
        //[Route("DanhSachDonThuLoi")]
        ////[CustomAuthAttribute(ChucNangEnum.GoManager, AccessLevel.Read)]
        //public IActionResult DanhSachDonThu([FromQuery] thamsodonthuloi /*ThamSoLocDanhMuc*/ p)
        //{
        //    try
        //    {
        //        var Result = XoaDonThuLoiBUS.DanhSachDonThu(p);
        //        if (Result != null) return Ok(Result);
        //        else return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        base.Status = -1;
        //        base.Message = ConstantLogMessage.API_Error_System;
        //        return base.GetActionResult();
        //    }
        //}

        //[HttpGet]
        //[Route("ChiTietDonThu")]
        ////[CustomAuthAttribute(ChucNangEnum.GoManager, AccessLevel.Read)]
        //public IActionResult ChiTiet([FromQuery] int DonThuID , int XuLyDonID)
        //{
        //    try
        //    {
        //        var Result = XoaDonThuLoiBUS.ChiTiet(DonThuID, XuLyDonID);
        //        if (Result != null) return Ok(Result);
        //        else return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        base.Status = -1;
        //        base.Message = ConstantLogMessage.API_Error_System;
        //        return base.GetActionResult();
        //    }
        //}
        //[HttpPost]
        //[Route("XoaDonThuLoi")]
        ////[CustomAuthAttribute(ChucNangEnum.GoManager, AccessLevel.Delete)]
        //public IActionResult XoaDonThu([FromQuery] int DonThuID, int XuLyDonID)
        //{
        //    if (DonThuID == null || XuLyDonID == null) return BadRequest();
        //    var Result = XoaDonThuLoiBUS.XoaDonThu(DonThuID,XuLyDonID);
        //    if (Result != null) return Ok(Result);
        //    else return NotFound();
        //}

        //[HttpPost]
        //[Route("XoaDonThuLoi_01")]
        ////[CustomAuthAttribute(ChucNangEnum.GoManager, AccessLevel.Delete)]
        //public IActionResult XoaDonThuLoi([FromBody] Thamsoxoa thamsoxoa)
        //{
        //    if (thamsoxoa.DonThuID == null || thamsoxoa.XuLyDonID == null) return BadRequest();
        //    var Result = XoaDonThuLoiBUS.XoaDonThuLoi(thamsoxoa);
        //    if (Result != null) return Ok(Result);
        //    else return NotFound();
        //}

    }
}
