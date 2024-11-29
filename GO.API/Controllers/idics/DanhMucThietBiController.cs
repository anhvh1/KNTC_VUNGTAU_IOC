using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.Idics;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.Idics;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GO.API.Controllers.idics
{
    [Route("api/v1/DanhMucThietBi")]
    [ApiController]
    public class DanhMucThietBiController : BaseApiController
    {
        DanhMucThietBiBUS _DanhMucThietBiBus;
        public DanhMucThietBiController(Microsoft.AspNetCore.Hosting.IHostingEnvironment HostingEnvironment, IOptions<AppSettings> Settings, ILogHelper _LogHelper, ILogger<DanhMucThietBiController> logger) : base(_LogHelper, logger)
        {
            _DanhMucThietBiBus = new DanhMucThietBiBUS();
        }

        [HttpPost]
        //[CustomAuthAttribute(ChucNangEnum.DanhMucThietBi, AKNTCessLevel.Create)]
        [Route("Insert")]
        public IActionResult Insert(DanhMucThietBiModel DanhMucThietBiModel)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.DM_ChucVu_ThemChucVu, EnumLogType.Insert, () =>
                //{
                var Result = _DanhMucThietBiBus.Insert(DanhMucThietBiModel);
                base.Status = Result.Status;
                base.Data = Result.Data;
                base.Message = Result.Message;
                return base.GetActionResult();
                //});
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
        //[CustomAuthAttribute(ChucNangEnum.DanhMucThietBi, AKNTCessLevel.Edit)]
        [Route("Update")]
        public IActionResult Update(DanhMucThietBiModel DanhMucThietBiModel)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.DM_ChucVu_SuaChucVu, EnumLogType.Update, () =>
                //{
                var Result = _DanhMucThietBiBus.Update(DanhMucThietBiModel);
                base.Status = Result.Status;
                base.Message = Result.Message;
                return base.GetActionResult();
                //});
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
        [Route("GetByID")]
        public IActionResult GetByID(string MachineId)
        {
            try
            {
                var Data = _DanhMucThietBiBus.GetByID(MachineId);
                if (Data != null)
                {
                    base.Message = ConstantLogMessage.API_Success;
                    base.Data = Data;
                }
                else
                {
                    base.Message = ConstantLogMessage.API_NoData;
                }
                base.Status = 1;

                return base.GetActionResult();
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        [Route("GetPagingBySearch")]
        public IActionResult GetPagingBySearch([FromQuery] BasePagingParams p)
        {
            try
            {
                var Result = _DanhMucThietBiBus.GetPagingBySearch(p);
                base.Data = Result.Data;
                base.Status = Result.Status;
                base.Message = Result.Message;
                base.TotalRow = Result.TotalRow;
                return base.GetActionResult();
            }
            catch (Exception)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }
    }
}
