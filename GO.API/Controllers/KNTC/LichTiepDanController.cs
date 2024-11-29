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

namespace GO.API.Controllers.KNTC
{
    [Route("api/v2/LichTiepDan")]
    [ApiController]
    public class LichTiepDanController : BaseApiController
    {
        private LichTiepDanBUS _LichTiepDanBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public LichTiepDanController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<LichTiepDanController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._LichTiepDanBUS = new LichTiepDanBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetListPaging")]
        [CustomAuthAttribute(ChucNangEnum.LichTiepDan, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] LichTiepDanParams p)
        {
            try
            {
                int TotalRow = 0;
                IList<LichTiepDanInfo> Data;
                var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                //p.CoQuanID = CoQuanID;
                Data = _LichTiepDanBUS.GetBySearch(ref TotalRow, p);
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

        [Route("GetByID")]
        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetByID(int IDLichTiep)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                LichTiepDanInfo Data;
                Data = _LichTiepDanBUS.GetByID(IDLichTiep);
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
        [CustomAuthAttribute(ChucNangEnum.LichTiepDan, AccessLevel.Edit)]
        public IActionResult Save(LichTiepDanInfo LichTiepDanInfo)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                {
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    LichTiepDanInfo.Editer = CanBoID;
                    LichTiepDanInfo.CreateDate = DateTime.Now;
                    var Data = _LichTiepDanBUS.Save(LichTiepDanInfo);
                    base.Data = Data.Data;
                    base.Status = Data.Status;
                    base.Message = Data.Message;
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
        [Route("UpdateTrangThaiPublic")]
        [CustomAuthAttribute(ChucNangEnum.LichTiepDan, AccessLevel.Edit)]
        public IActionResult UpdateTrangThaiPublic(LichTiepDanInfo LichTiepDanInfo)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                {
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    LichTiepDanInfo.Editer = CanBoID;
                    LichTiepDanInfo.CreateDate = DateTime.Now;
                    var Data = _LichTiepDanBUS.UpdateTrangThaiPublish(LichTiepDanInfo);
                    base.Data = Data.Data;
                    base.Status = Data.Status;
                    base.Message = Data.Message;
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
        [Route("Delete")]
        [CustomAuthAttribute(ChucNangEnum.LichTiepDan, AccessLevel.Delete)]
        public IActionResult Delete(BaseDeleteParams p)
        {
            try
            {
                if (p.ListID != null && p.ListID.Count > 0)
                {
                    foreach (var item in p.ListID)
                    {
                        var Data = _LichTiepDanBUS.Delete(item);
                        base.Data = Data.Data;
                        base.Status = Data.Status;
                        base.Message = Data.Message;
                    }
                }

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
        [Route("XemLichTiepDan")]
        public IActionResult XemLichTiepDan([FromQuery] LichTiepDanParams p)
        {
            try
            {
                int TotalRow = 0;
                IList<LichTiepDanInfo> Data;
                Data = _LichTiepDanBUS.GetBySearch(ref TotalRow, p);
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
    }
}
