using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Controllers;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GO.API.Controllers.KNTC
{
    [Route("api/v2/ChiTietDonThu")]
    [ApiController]
    public class ChiTietDonThuController : BaseApiController
    {
        private ChiTietDonThuBUS _ChiTietDonThuBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public ChiTietDonThuController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<ChiTietDonThuController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._ChiTietDonThuBUS = new ChiTietDonThuBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetChiTietDonThu")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetChiTietDonThu([FromQuery] int DonThuID, int XuLyDonID)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    //int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);

                    var Data = _ChiTietDonThuBUS.GetChiTietDonThu(DonThuID, XuLyDonID, CanBoID, serverPath);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        [Route("GetChiTietDonThuDaTiepNhan")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetChiTietDonThuDaTiepNhan([FromQuery] int DonThuID, int XuLyDonID)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    //int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);

                    var Data = _ChiTietDonThuBUS.GetChiTietDonThu(DonThuID, XuLyDonID, CanBoID, serverPath);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        [Route("DanhMucTenFile")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhMucTenFile()
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    string ContentRootPath = _host.ContentRootPath;

                    var Data = _ChiTietDonThuBUS.DanhMucTenFile();
                    base.Status = 1;
                    base.Data = Data;                 
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        [Route("DanhMucLoaiKhieuTo")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhMucLoaiKhieuTo()
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    string ContentRootPath = _host.ContentRootPath;

                    var Data = _ChiTietDonThuBUS.GetLoaiKhieuTos();
                    base.Status = 1;
                    base.Data = Data;                 
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }
    }
}
