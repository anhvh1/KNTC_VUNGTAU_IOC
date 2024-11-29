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
    [Route("api/v2/TrinhTuThuTuc")]
    [ApiController]
    public class TrinhTuThuTucController : BaseApiController
    {
        private TrinhTuThuTucBUS _TrinhTuThuTucBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public TrinhTuThuTucController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<TrinhTuThuTucController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._TrinhTuThuTucBUS = new TrinhTuThuTucBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetListPaging")]
        //[CustomAuthAttribute(ChucNangEnum.QuanLyPhanQuyen, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] BasePagingParams p)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                int TotalRow = 0;
                IList<TrinhTuThuTucModel> Data;
                Data = _TrinhTuThuTucBUS.GetBySearch(ref TotalRow, p);
                if(Data != null && Data.Count > 0)
                {
                    foreach (var item in Data)
                    {
                        if (item.Thumbnail != null && item.Thumbnail.FileUrl != null)
                        {
                            item.Thumbnail.FileUrl = serverPath + item.Thumbnail.FileUrl;
                        }
                    }
                }    
               
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
        public IActionResult GetByID(int TrinhTuThuTucID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                TrinhTuThuTucModel Data;
                Data = _TrinhTuThuTucBUS.GetByID(TrinhTuThuTucID);
                if(Data.Thumbnail != null && Data.Thumbnail.FileUrl != null)
                {
                    Data.Thumbnail.FileUrl = serverPath + Data.Thumbnail.FileUrl;
                }
                if (Data.DanhSachFileDinhKem != null && Data.DanhSachFileDinhKem.Count > 0)
                {
                    foreach (var item in Data.DanhSachFileDinhKem)
                    {
                        item.FileUrl = serverPath + item.FileUrl;
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
        [CustomAuthAttribute(ChucNangEnum.TrinhTuThuTuc, AccessLevel.Edit)]
        public IActionResult Save(TrinhTuThuTucModel TrinhTuThuTucModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                {
                    TrinhTuThuTucModel.NguoiTaoID = CanBoID;
                    var Data = _TrinhTuThuTucBUS.Save(TrinhTuThuTucModel);
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
        [CustomAuthAttribute(ChucNangEnum.TrinhTuThuTuc, AccessLevel.Edit)]
        public IActionResult UpdateTrangThai(TrinhTuThuTucModel TrinhTuThuTucModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                {
                    TrinhTuThuTucModel.NguoiTaoID = CanBoID;
                    var Data = _TrinhTuThuTucBUS.UpdateTrangThaiPublic(TrinhTuThuTucModel);
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
        [CustomAuthAttribute(ChucNangEnum.TrinhTuThuTuc, AccessLevel.Delete)]
        public IActionResult Delete(BaseDeleteParams p)
        {
            try
            {
                if (p.ListID != null && p.ListID.Count > 0)
                {
                    foreach (var item in p.ListID)
                    {
                        var Data = _TrinhTuThuTucBUS.Delete(item);
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
        [Route("XemTrinhTuThuTuc")]
        public IActionResult XemTrinhTuThuTuc([FromQuery] BasePagingParams p)
        {
            try
            {
                int TotalRow = 0;
                IList<TrinhTuThuTucModel> Data;
                Data = _TrinhTuThuTucBUS.GetBySearch(ref TotalRow, p);
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
