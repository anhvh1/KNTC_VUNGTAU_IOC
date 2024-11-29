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
using AutoMapper;
using System.Collections.Generic;

namespace GO.API.Controllers.KNTC
{
    [Route("api/v2/DongBoDuLieu")]
    [ApiController]
    public class DongBoDuLieuController : BaseApiController
    {
        private DongBoDuLieuBUS _DongBoDuLieuBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public DongBoDuLieuController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DongBoDuLieuController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._DongBoDuLieuBUS = new DongBoDuLieuBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetDuLieuDongBo")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] BasePagingParams p)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                DuLieuDongBoModel Data = _DongBoDuLieuBUS.GetBySearch(p);
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
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


        [HttpGet]
        [Route("GetLoaiDanhMucAnhXa")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetLoaiDanhMucAnhXa()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                List<ApiGateway.objMapping.ObjMap> Data = _DongBoDuLieuBUS.GetLoaiDanhMucAnhXa();
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
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

        [HttpGet]
        [Route("TaiDuLieuVaCallAPI")]
        [CustomAuthAttribute(ChucNangEnum.DongBoDuLieu, AccessLevel.Read)]
        public IActionResult TaiDuLieuVaCallAPI([FromQuery] string TypeApi)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Data = _DongBoDuLieuBUS.TaiDuLieuVaCallAPI(TypeApi);
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data.Data;
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

        [HttpPost]
        [Route("DanhDauDongBo")]
        [CustomAuthAttribute(ChucNangEnum.DongBoDuLieu, AccessLevel.Edit)]
        public IActionResult DanhDauDongBo(DongBo_LogInfo LogInfo)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Data = _DongBoDuLieuBUS.DanhDauDongBo(LogInfo, CanBoID);
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data.Data;
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

        [HttpPost]
        [Route("CapNhatDuLieuMapping")]
        [CustomAuthAttribute(ChucNangEnum.DongBoDuLieu, AccessLevel.Edit)]
        public IActionResult CapNhatDuLieuMapping(DuLieuMapping DuLieuMapping)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Data = _DongBoDuLieuBUS.CapNhatDuLieuMapping(DuLieuMapping);
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data.Data;
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

        [HttpPost]
        [Route("CapNhatTrangThaiDon")]
        [CustomAuthAttribute(ChucNangEnum.DongBoDuLieu, AccessLevel.Edit)]
        public IActionResult CapNhatTrangThaiDon(DongBo_LogInfo LogInfo)
        {
            try
            {
                var Data = _DongBoDuLieuBUS.CapNhatTrangThaiDon(LogInfo);
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data.Data;
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
