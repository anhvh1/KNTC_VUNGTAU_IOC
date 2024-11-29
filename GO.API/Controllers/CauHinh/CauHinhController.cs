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
    [Route("api/v2/CauHinh")]
    [ApiController]
    public class CauHinhController : BaseApiController
    {
        private CauHinhBUS _CauHinhBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        public CauHinhController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, ILogHelper _LogHelper, ILogger<CauHinhController> logger) : base(_LogHelper, logger)
        {
            this._host = hostingEnvironment;
            _CauHinhBUS= new CauHinhBUS();
        }

        [HttpGet]
        [Route("DanhSachCauHinhTheoPhanLoai")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_ThamSoHeThong, AKNTCessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] string? phanLoai)
        {
            try
            {
                    IList<CauHinhModel> Data;
                    Data = _CauHinhBUS.DanhSachCauHinhTheoPhanLoai(phanLoai);
                    base.Status = 1;
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
        // abc
    }
}