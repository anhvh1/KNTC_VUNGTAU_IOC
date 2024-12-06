using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.BaoCao;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GO.API.Controllers.BacCao
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class DongBo_IOCController : BaseApiController
    {
        private DongBo_IOCBUS _dongBo_IOCBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public DongBo_IOCController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DongBo_IOCController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._dongBo_IOCBUS = new DongBo_IOCBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }
        [HttpPost]
        [Route("Insert_BC_2a")]
        [CustomAuthAttribute(ChucNangEnum.TongHopKetQuaTiepCongDan, AccessLevel.Create)]
        public IActionResult Insert_BC_2a([FromBody] List<ThongKeBC_2a_DongBo_IOC_Request> model)
        {
            try
            {
                return CreateActionResult("Thêm mới dữ liệu IOC", EnumLogType.Insert, () =>
                {
                    int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var result = _dongBo_IOCBUS.Insert_BC_2a(model,nguoiDungID);
                    base.Status = result.Status;
                    base.Message = result.Message;
                    return base.GetActionResult();
                });
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
        [Route("Update_BC_2a")]
        [CustomAuthAttribute(ChucNangEnum.TongHopKetQuaTiepCongDan, AccessLevel.Edit)]
        public IActionResult Update_BC_2a([FromBody] List<ThongKeBC_2a_DongBo_IOC_UpdateRequest> model)
        {
            try
            {
                return CreateActionResult("cập nhập dữ liệu IOC", EnumLogType.Update, () =>
                {
                    int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var result = _dongBo_IOCBUS.Update_BC_2a(model, nguoiDungID);
                    base.Status = result.Status;
                    base.Message = result.Message;
                    return base.GetActionResult();
                });
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
        [Route("GetList_BC_2a")]
        [CustomAuthAttribute(ChucNangEnum.TongHopKetQuaTiepCongDan, AccessLevel.Read)]
        public IActionResult GetList_BC_2a([FromQuery] FilterDongBo_IOC p)
        {
            try
            {
                return CreateActionResult("Get list dữ liệu IOC", EnumLogType.GetList, () =>
                {
                    int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var result = _dongBo_IOCBUS.GetList_2a(p);
                    base.Status = 1;
                    base.Message = "Lấy danh sách thành công";
                    base.Data = result;
                    return base.GetActionResult();
                });
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
        [Route("Insert_BC_2b")]
        [CustomAuthAttribute(ChucNangEnum.TongHopKetQuaTiepCongDan, AccessLevel.Create)]
        public IActionResult Insert_BC_2b([FromBody] List<ThongKeBC_2b_DongBo_IOC_Request> model)
        {
            try
            {
                return CreateActionResult("Thêm mới dữ liệu IOC", EnumLogType.Insert, () =>
                {
                    int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var result = _dongBo_IOCBUS.Insert_BC_2b(model, nguoiDungID);
                    base.Status = result.Status;
                    base.Message = result.Message;
                    return base.GetActionResult();
                });
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
        [Route("Update_BC_2b")]
        [CustomAuthAttribute(ChucNangEnum.TongHopKetQuaTiepCongDan, AccessLevel.Edit)]
        public IActionResult Update_BC_2b([FromBody] List<ThongKeBC_2b_DongBo_IOC_UpdateRequest> model)
        {
            try
            {
                return CreateActionResult("cập nhập dữ liệu IOC", EnumLogType.Update, () =>
                {
                    int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var result = _dongBo_IOCBUS.Update_BC_2b(model, nguoiDungID);
                    base.Status = result.Status;
                    base.Message = result.Message;
                    return base.GetActionResult();
                });
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
        [Route("GetList_BC_2b")]
        [CustomAuthAttribute(ChucNangEnum.TongHopKetQuaTiepCongDan, AccessLevel.Read)]
        public IActionResult GetList_BC_2b([FromQuery] FilterDongBo_IOC p)
        {
            try
            {
                return CreateActionResult("Get list dữ liệu IOC", EnumLogType.GetList, () =>
                {
                    int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var result = _dongBo_IOCBUS.GetList_2b(p);
                    base.Status = 1;
                    base.Message = "Lấy danh sách thành công";
                    base.Data = result;
                    return base.GetActionResult();
                });
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
