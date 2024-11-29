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
    [Route("api/v2/GiaoXacMinh")]
    [ApiController]
    public class GiaoXacMinhController : BaseApiController
    {
        private GiaoXacMinhBUS _GiaoXacMinhBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public GiaoXacMinhController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<GiaoXacMinhController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._GiaoXacMinhBUS = new GiaoXacMinhBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetListPaging")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] XuLyDonParamsForFilter p)
        {
            try
            {
                //return CreateActionResult(false, ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                //{
                IdentityHelper IdentityHelper = new IdentityHelper();
                IdentityHelper.CanBoID = CanBoID;
                IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                IdentityHelper.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                IdentityHelper.UserID = IdentityHelper.NguoiDungID;
                IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);
                IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                IdentityHelper.CapThanhTra = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapThanhTra").Value, 0);
                IdentityHelper.CapUBND = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "CapUBND").Value, false);
                IdentityHelper.ChuTichUBND = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "ChuTichUBND").Value, 0);
                IdentityHelper.CapHanhChinh = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapHanhChinh").Value, 0);
                IdentityHelper.BanTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "BanTiepDan").Value, false);
                int TotalRow = 0;
                IList<DonThuGiaiQuyetInfo> Data;
                //int Loai 
                if (p.LoaiKhieuToID == 2)
                {
                    p.LoaiKhieuToID = 8;//khieu nai
                }
                else if (p.LoaiKhieuToID == 3)
                {
                    p.LoaiKhieuToID = 9;//kien nghi, phan anh
                }
                Data = _GiaoXacMinhBUS.GetBySearch(ref TotalRow, p, IdentityHelper);
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
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

        //[HttpPost]
        //[Route("Save")]
        ////[CustomAuthAttribute(ChucNangEnum.QuanLyPhanQuyen, AccessLevel.Create)]
        //public IActionResult SaveGiaoXacMinh(TiepDanInfo TiepDanInfo)
        //{
        //    try
        //    {
        //        return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
        //        {
        //            IdentityHelper IdentityHelper = new IdentityHelper();
        //            IdentityHelper.CanBoID = CanBoID;
        //            IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
        //            IdentityHelper.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
        //            IdentityHelper.UserID = IdentityHelper.NguoiDungID;
        //            IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
        //            IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
        //            IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
        //            IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
        //            IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);

        //            if (TiepDanInfo.NgayNhapDon == null) TiepDanInfo.NgayNhapDon = DateTime.Now;
        //            if (TiepDanInfo.CanBoTiepID == null) TiepDanInfo.CanBoTiepID = CanBoID;
        //            if (TiepDanInfo.CoQuanID == null) TiepDanInfo.CoQuanID = IdentityHelper.CoQuanID;
        //            //var Data = _GiaoXacMinhBUS.InsertGiaoXacMinh(IdentityHelper, TiepDanInfo, 0, false, false);
        //            //base.Data = Data;
        //            base.Status = 1;
        //            base.Message = "";
        //            return base.GetActionResult();
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        base.Status = -1;
        //        base.Message = ConstantLogMessage.API_Error_System;
        //        return base.GetActionResult();
        //        throw ex;
        //    }

        //}

        [Route("GetByID")]
        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetByID(int? XuLyDonID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                GiaoXacMinhModel Data;
                Data = _GiaoXacMinhBUS.GetByID(XuLyDonID ?? 0);
                if(Data != null && Data.DanhSachHoSoTaiLieu != null && Data.DanhSachHoSoTaiLieu.Count > 0)
                {
                    foreach (var item in Data.DanhSachHoSoTaiLieu)
                    {
                        if(item.FileDinhKem != null && item.FileDinhKem.Count > 0)
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
        [Route("CapNhatQuyetDinhGiaoXacMinh")]
        [CustomAuthAttribute(ChucNangEnum.CapNhatVanBanGiaoXacMinh, AccessLevel.Edit)]
        public IActionResult CapNhatQuyetDinhGiaoXacMinh(GiaoXacMinhModel GiaoXacMinhModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                {
                    IdentityHelper IdentityHelper = new IdentityHelper();
                    IdentityHelper.CanBoID = CanBoID;
                    IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    IdentityHelper.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    IdentityHelper.UserID = IdentityHelper.NguoiDungID;
                    IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    IdentityHelper.CapThanhTra = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapThanhTra").Value, 0);
                    IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                    IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                    IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                    IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);
                    IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    IdentityHelper.CapUBND = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "CapUBND").Value, false);
                    IdentityHelper.CapHanhChinh = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapHanhChinh").Value, 0);
                    IdentityHelper.ChuTichUBND = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "ChuTichUBND").Value, 0);


                    var Data = _GiaoXacMinhBUS.CapNhatQuyetDinhGiaoXacMinh(IdentityHelper, GiaoXacMinhModel);
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
        [Route("CapNhatQuyetDinhGiaoXacMinh_CTH")]
        public IActionResult CapNhatQuyetDinhGiaoXacMinh_CTH(GiaoXacMinhModel GiaoXacMinhModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                {
                    IdentityHelper IdentityHelper = new IdentityHelper();
                    IdentityHelper.CanBoID = CanBoID;
                    IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    IdentityHelper.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    IdentityHelper.UserID = IdentityHelper.NguoiDungID;
                    IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    IdentityHelper.CapThanhTra = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapThanhTra").Value, 0);
                    IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                    IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                    IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                    IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);
                    IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    IdentityHelper.CapUBND = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "CapUBND").Value, false);
                    IdentityHelper.CapHanhChinh = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapHanhChinh").Value, 0);
                    IdentityHelper.ChuTichUBND = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "ChuTichUBND").Value, 0);


                    var Data = _GiaoXacMinhBUS.CapNhatQuyetDinhGiaoXacMinh(IdentityHelper, GiaoXacMinhModel);
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


        [HttpGet]
        [Route("GetCoQuanGQ")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetAllCoQuan()
        {
            try
            {
                int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                int TotalRow = 0;
                IList<CoQuanInfo> Data;
                Data = _GiaoXacMinhBUS.GetCoQuanGQ(CoQuanID);
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
    }
}
