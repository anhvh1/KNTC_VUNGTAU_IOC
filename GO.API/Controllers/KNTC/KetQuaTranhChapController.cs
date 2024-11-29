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
using Com.Gosol.KNTC.BUS.BaoCao;
using static Com.Gosol.KNTC.BUS.KNTC.KetQuaTranhChapBUS;

namespace GO.API.Controllers.KNTC
{
    [Route("api/v2/KetQuaTranhChap")]
    [ApiController]
    public class KetQuaTranhChapController : BaseApiController
    {
        private KetQuaTranhChapBUS _KetQuaTranhChapBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public KetQuaTranhChapController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<KetQuaTranhChapController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._KetQuaTranhChapBUS = new KetQuaTranhChapBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetListPaging")]
        [CustomAuthAttribute(ChucNangEnum.NhapKetQuaTranhChap, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] BasePagingParamsForFilter p)
        {
            try
            {
                IdentityHelper IdentityHelper = new IdentityHelper();
                IdentityHelper.CanBoID = CanBoID;
                IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                IdentityHelper.HuyenID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                IdentityHelper.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                IdentityHelper.UserID = IdentityHelper.NguoiDungID;
                IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);

                int TotalRow = 0;
                IList<ChuyenXuLyInfo> Data;
                Data = _KetQuaTranhChapBUS.GetBySearch(ref TotalRow, p, IdentityHelper);

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
            }
        }

        [Route("GetByID")]
        [HttpGet]
        [CustomAuthAttribute(ChucNangEnum.NhapKetQuaTranhChap, AccessLevel.Read)]
        public IActionResult GetByID([FromQuery] int? XuLyDonID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                KetQuaTranhChapInfo Data;
                Data = _KetQuaTranhChapBUS.GetByID(XuLyDonID ?? 0);
                if (Data != null && Data.DanhSachHoSoTaiLieu != null && Data.DanhSachHoSoTaiLieu.Count > 0)
                {
                    foreach (var item in Data.DanhSachHoSoTaiLieu)
                    {
                        if (item.FileDinhKem != null && item.FileDinhKem.Count > 0)
                        {
                            foreach (var f in item.FileDinhKem)
                            {
                                f.FileUrl = serverPath + f.FileUrl;
                            }
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
        [CustomAuthAttribute(ChucNangEnum.NhapKetQuaTranhChap, AccessLevel.Edit)]
        public IActionResult Save(KetQuaTranhChapInfo KetQuaTranhChapInfo)
        {
            try
            {
                int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                KetQuaTranhChapInfo.CanBoID = CanBoID;
                KetQuaTranhChapInfo.CoQuanID = CoQuanID;
                var Data = _KetQuaTranhChapBUS.NhapKetQuaTranhChap(KetQuaTranhChapInfo);
                base.Data = Data.Data;
                base.Status = Data.Status;
                base.Message = Data.Message;
                return base.GetActionResult();

            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ex.Message;
                //base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }

        }

    }
}
