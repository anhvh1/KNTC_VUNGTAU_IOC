using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.BUS.KNTC;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Com.Gosol.KNTC.DAL.KNTC;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.API.Controllers;
using QuanLyQuyTrinhNghiepVuBUS = Com.Gosol.KNTC.BUS.KNTC.QuanLyQuyTrinhNghiepVuBUS;
using Newtonsoft.Json;

namespace GO.API.Controllers.KNTC
{
    [Route("api/v2/QuanLyQuyTrinhNghiepVu")]
    [ApiController]
    public class QuanLyQuyTrinhNghiepVuController : BaseApiController
    {
        private QuanLyQuyTrinhNghiepVuBUS _QuanLyQuyTrinhNghiepVuBUS;
        private FileDinhKemBUS _FileDinhKemBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public QuanLyQuyTrinhNghiepVuController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<QuanLyQuyTrinhNghiepVuController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._QuanLyQuyTrinhNghiepVuBUS = new QuanLyQuyTrinhNghiepVuBUS();
            this._FileDinhKemBUS = new FileDinhKemBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [Route("GetAllCap")]
        [HttpGet]
        [CustomAuthAttribute(ChucNangEnum.QuyTrinhHeThong, AccessLevel.Read)]
        public IActionResult GetAllCap()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                IList<QuyTrinhInfo> Data;
                Data = _QuanLyQuyTrinhNghiepVuBUS.GetAllCap();
                base.Status = 1;
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [Route("GetCoQuanByCap")]
        [HttpGet]
        [CustomAuthAttribute(ChucNangEnum.QuyTrinhHeThong, AccessLevel.Read)]
        public IActionResult GetCoQuanByCap(int? CapID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                IList<QuyTrinhInfo> Data;
                Data = _QuanLyQuyTrinhNghiepVuBUS.GetCoQuanByCap(CapID ?? 0);
                base.Status = 1;
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [Route("GetQuyTrinhByCap")]
        [HttpGet]
        [CustomAuthAttribute(ChucNangEnum.QuyTrinhHeThong, AccessLevel.Read)]
        public IActionResult GetQuyTrinhByCap(int? CapID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                IList<QuyTrinhInfo> Data;
                Data = _QuanLyQuyTrinhNghiepVuBUS.GetQuyTrinhByCap(CapID ?? 0);
                if(Data.Count > 0)
                {
                    foreach (var item in Data)
                    {
                        var listFile = _FileDinhKemBUS.GetByNgiepVuID(item.QuyTrinhID, EnumLoaiFile.FileQuyTrinhNghiepVu.GetHashCode());
                        if (listFile.Count > 0)
                        {
                            item.ImgUrl = clsCommon.GetServerPath(HttpContext) + listFile[0].FileUrl;
                            item.FileDinhKem = listFile[0];
                        }
                    }
                }
                base.Status = 1;
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [HttpPost]
        [Route("Save")]
        [CustomAuthAttribute(ChucNangEnum.QuyTrinhHeThong, AccessLevel.Edit)]
        public async Task<IActionResult> SaveAsync(IFormFile files, [FromForm] string QuyTrinhStr)
        {
            try
            {
                var Info = JsonConvert.DeserializeObject<QuyTrinhInfo>(QuyTrinhStr);
                var Data = _QuanLyQuyTrinhNghiepVuBUS.Save(Info);

                FileDinhKemModel FileDinhKem = new FileDinhKemModel();
                FileDinhKem.FileType = EnumLoaiFile.FileQuyTrinhNghiepVu.GetHashCode();
                FileDinhKem.NguoiCapNhat = CanBoID;
                FileDinhKem.NghiepVuID = Utils.ConvertToInt32(Data.Data, 0);

                if(Info != null && Info.QuyTrinhID > 0)
                {
                    var listFile = _FileDinhKemBUS.GetByNgiepVuID(Info.QuyTrinhID, EnumLoaiFile.FileQuyTrinhNghiepVu.GetHashCode());
                    _FileDinhKemBUS.Delete(listFile);
                }
                var clsCommon = new Commons();
                var file = await clsCommon.InsertFileAsync(files, FileDinhKem, _host);

                base.Data = Data.Data;
                base.Status = Data.Status;
                base.Message = Data.Message;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }

        }
    }
}
