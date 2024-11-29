using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.Security;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Com.Gosol.KNTC.API.Controllers.HeThong
{
    [Route("api/v2/SystemLog")]
    [ApiController]
    public class SystemLogController : BaseApiController
    {
        private SystemConfigBUS _SystemConfigBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private SystemLogBUS _SystemLogBUS;
        public SystemLogController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, ILogHelper _LogHelper, ILogger<SystemLogController> logger) : base(_LogHelper, logger)
        {
            this._SystemLogBUS = new SystemLogBUS();
            this._host = hostingEnvironment;
        }

        [HttpGet]
        [Route("GetListPaging")]
        [CustomAuthAttribute(ChucNangEnum.HeThong, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] BasePagingParams p)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_SystemLog_GetListPaging, EnumLogType.GetList, () =>
                // {
                int TotalRow = 0;
                IList<SystemLogPartialModel> Data;
                Data = _SystemLogBUS.GetPagingBySearch(p, ref TotalRow);
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;

                return base.GetActionResult();
                //});
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }

        }

        [HttpGet]
        [Route("GetPagingByQuanTriDuLieu")]
        [CustomAuthAttribute(ChucNangEnum.HeThong, AccessLevel.Read)]
        public IActionResult GetPagingByQuanTriDuLieu([FromQuery] BasePagingParams p)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_SystemLog_GetListPaging, EnumLogType.GetList, () =>
                //{
                int TotalRow = 0;
                IList<SystemLogPartialModel> Data;
                Data = _SystemLogBUS.GetPagingByQuanTriDuLieu(p, ref TotalRow);
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;

                return base.GetActionResult();
                //});
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }

        }
        
        [HttpGet]
        [Route("DanhSach")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        public IActionResult DanhSach([FromQuery] NhatKyHeThongThamSo thamSo)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var data = _SystemLogBUS.DanhSach(thamSo);
                return Ok(data);
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ex.ToString();
                return base.GetActionResult();
            }
        }


        [HttpGet]
        [Route("CreateLogFile")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        public IActionResult CreateLogFile()
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.KK_KeKhai_CreateLogFile, EnumLogType.Other, () =>
                //{
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                DirectoryInfo d = new DirectoryInfo(_host.ContentRootPath + "\\LogConfig");
                FileInfo[] File = d.GetFiles("*.xml");
                string[] NgayCuoiCung = new string[] { };
                if (File.Length > 0)
                {
                    var LastFileName = File.ToList().LastOrDefault().FullName;
                    NgayCuoiCung = LastFileName.Split("_");
                    if ((int.Parse(DateTime.Now.ToString("yyyyMMdd")) - int.Parse(NgayCuoiCung[1].ToString().Substring(0, NgayCuoiCung[1].ToString().LastIndexOf(".")))) < int.Parse(_SystemConfigBUS.GetByKey("Exp_LogFile").ConfigValue))
                    {
                        return base.GetActionResult();
                    }
                }

                string SavePath = _host.ContentRootPath + "\\LogConfig\\SystemLogFile_" + DateTime.Now.ToString("yyyyMMdd") + ".xml";
                using (FileStream stream = System.IO.File.Create(SavePath))
                {
                    //byte[] byteArray = Convert.FromBase64String(file.files);
                    //stream.Write(byteArray, 0, byteArray.Length);
                }
                _SystemLogBUS.CreateLogFile(SavePath, DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                base.Status = 1;
                base.TotalRow = TotalRow;
                return base.GetActionResult();
                //});
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }

        }
    }
}