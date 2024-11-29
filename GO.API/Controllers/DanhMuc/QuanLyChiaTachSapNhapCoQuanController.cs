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
using Com.Gosol.KNTC.BUS.DanhMuc;

namespace GO.API.Controllers.KNTC
{
    [Route("api/v2/QuanLyChiaTachSapNhapCoQuan")]
    [ApiController]
    public class QuanLyChiaTachSapNhapCoQuanController : BaseApiController
    {
        private QuanLyChiaTachSapNhapCoQuanBUS _QuanLyChiaTachSapNhapCoQuanBUS;

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public QuanLyChiaTachSapNhapCoQuanController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<QuanLyChiaTachSapNhapCoQuanController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._QuanLyChiaTachSapNhapCoQuanBUS = new QuanLyChiaTachSapNhapCoQuanBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetListPaging")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] BasePagingParamsForFilter p)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                List<QL_CQSatNhapInfo> Data;
                Data = _QuanLyChiaTachSapNhapCoQuanBUS.GetBySearch(ref TotalRow, p);
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
        [Route("DanhSachLichSu")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSachLichSu([FromQuery] BasePagingParamsForFilter p)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                List<QL_CQSatNhapInfo> Data;
                Data = _QuanLyChiaTachSapNhapCoQuanBUS.GetBySearch(ref TotalRow, p);
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

        [HttpPost]
        [Route("Save")]
        [CustomAuthAttribute(ChucNangEnum.QuanLyChiaTachSapNhapCoQuan, AccessLevel.Edit)]
        public IActionResult Save(QL_CQSatNhapInfo Info)
        {
            try
            {
                var Data = new BaseResultModel();
                Info.NgayThucHien = DateTime.Now;
                Info.NguoiThucHienID = CanBoID;
                if (Info.laSapNhap == true)
                {
                    Data = _QuanLyChiaTachSapNhapCoQuanBUS.Insert_SN(Info);
                }
                else Data = _QuanLyChiaTachSapNhapCoQuanBUS.Insert_CT(Info);
                
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
        [HttpPost]
        [Route("Delete")]
        [CustomAuthAttribute(ChucNangEnum.QuanLyChiaTachSapNhapCoQuan, AccessLevel.Delete)]
        public IActionResult Delete(QL_CQSatNhapInfo Info)
        {
            try
            {
                var Data = _QuanLyChiaTachSapNhapCoQuanBUS.Delete(Info);
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

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetAllByCap, EnumLogType.GetList, () =>
                {
               
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    IList<CoQuanInfo> Data;
                    Data = _QuanLyChiaTachSapNhapCoQuanBUS.GetAllCQ();
                    if (Data.Count == 0)
                    {
                        base.Status = 1;
                        base.Message = ConstantLogMessage.API_NoData;
                        return base.GetActionResult();
                    }
                    base.Status = Data.Count > 0 ? 1 : 0;
                    base.Data = Data;
                    base.TotalRow = Data.Count;
                    return base.GetActionResult();
                });
            }
            catch
            {
                base.Status = -1;
                return base.GetActionResult();
            }
        }
    }
}
