using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Controllers;
using Com.Gosol.KNTC.API.Controllers.DanhMuc;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Com.Gosol.KNTC.BUS.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.API.Authorization;

namespace GO.API.Controllers.BacCao
{
    [Route("api/v2/TraCuu")]
    [ApiController]
    public class TraCuuController : BaseApiController
    {
        private TraCuuBUS _TraCuuBUS;
        private TiepDanBUS _TiepDanBUS;
        private BanHanhQuyetDinhGQBUS _BanHanhQuyetDinhGQBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public TraCuuController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<TraCuuController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._TraCuuBUS = new TraCuuBUS();
            this._TiepDanBUS = new TiepDanBUS();
            this._BanHanhQuyetDinhGQBUS = new BanHanhQuyetDinhGQBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.TraCuu, AccessLevel.Read)]
        [Route("TraCuuQuyetDinhGiaiQuyet")]
        public IActionResult TraCuuQuyetDinhGiaiQuyet([FromQuery] TraCuuParams p)
        {
            try
            {   
                int TotalRow = 0;
                string ContentRootPath = _host.ContentRootPath;
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                var Data = _TraCuuBUS.TraCuu(p, ref TotalRow);
                if(Data.Count > 0)
                {
                    foreach (var item in Data)
                    {
                        if (item != null && item.DanhSachHoSoTaiLieu != null && item.DanhSachHoSoTaiLieu.Count > 0)
                        {
                            foreach (var hs in item.DanhSachHoSoTaiLieu)
                            {
                                if (hs.FileDinhKem != null && hs.FileDinhKem.Count > 0)
                                {
                                    foreach (var f in hs.FileDinhKem)
                                    {
                                        f.FileUrl = serverPath + f.FileUrl;
                                    }
                                }
                            }
                        }
                    }
                }
                base.Status = 1;
                base.Data = Data;
                base.TotalRow = TotalRow;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                //base.GetActionResult();
                //throw;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("TraCuuTrangThaiXuLyHoSo")]
        public IActionResult TraCuuTrangThaiXuLyHoSo([FromQuery] TraCuuParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    //var Data = _TraCuuBUS.GetDonThuByCoQuanTiepNhan(p);
                    //base.Status = 1;
                    //base.Data = Data;
                    int TotalRow = 0;
                    string ContentRootPath = _host.ContentRootPath;
                    var Data = _TraCuuBUS.TraCuu(p, ref TotalRow);
                    base.Status = 1;
                    base.Data = Data;
                    base.TotalRow = TotalRow;
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        [Route("GetAllCoQuan")]
        public IActionResult GetAllCoQuan()
        {
            try
            {
                int TotalRow = 0;
                IList<CoQuanInfo> Data;
                Data = _TiepDanBUS.GetAllCoQuan();
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

        [HttpGet]
        [Route("GetDanhSachCacQuyetDinhGiaiQuyet")]
        [CustomAuthAttribute(ChucNangEnum.TraCuuQuyetDinhGiaiQuyet, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] XuLyDonParamsForFilter p)
        {
            try
            {
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
                IdentityHelper.ChuTichUBND = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "ChuTichUBND").Value, 0);
                //int Loai 
                if (p.LoaiKhieuToID == 2)
                {
                    p.LoaiKhieuToID = 8;//khieu nai
                }
                else if (p.LoaiKhieuToID == 3)
                {
                    p.LoaiKhieuToID = 9;//kien nghi, phan anh
                }
                int TotalRow = 0;
                IList<ChuyenXuLyInfo> Data;
            
                if (p.LoaiKhieuToID == 2)
                {
                    p.LoaiKhieuToID = 8;//khieu nai
                }
                else if (p.LoaiKhieuToID == 3)
                {
                    p.LoaiKhieuToID = 9;//kien nghi, phan anh
                }
                Data = _BanHanhQuyetDinhGQBUS.GetBySearch(IdentityHelper, p, ref TotalRow);
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

        [HttpPost]
        [Route("UpdateTrangThaiKhoa")]
        [CustomAuthAttribute(ChucNangEnum.TraCuuQuyetDinhGiaiQuyet, AccessLevel.Edit)]
        public IActionResult UpdateTrangThaiKhoa(ChuyenXuLyInfo Info)
        {
            try
            {
                int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                var Data = _TraCuuBUS.UpdateTrangThaiKhoa(Info.XuLyDonID, Info.TrangThaiKhoa);
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

        [HttpGet]
        [Route("DanhSachHoSoDuocTraCuu")]
        [CustomAuthAttribute(ChucNangEnum.TraCuuTrangThaiHoSo, AccessLevel.Read)]
        public IActionResult DanhSachHoSoDuocTraCuu([FromQuery] TraCuuParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    string ContentRootPath = _host.ContentRootPath;
                    var Data = _TraCuuBUS.DanhSachHoSoDuocTraCuu(p, ref TotalRow);
                    base.Status = 1;
                    base.Data = Data;
                    base.TotalRow = TotalRow;
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        [Route("LichSuTraCuu")]
        [CustomAuthAttribute(ChucNangEnum.TraCuuTrangThaiHoSo, AccessLevel.Read)]
        public IActionResult LichSuTraCuu([FromQuery] TraCuuParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    string ContentRootPath = _host.ContentRootPath;
                    var Data = _TraCuuBUS.LichSuTraCuu(p, ref TotalRow);
                    base.Status = 1;
                    base.Data = Data;
                    base.TotalRow = TotalRow;
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }
    }
}
