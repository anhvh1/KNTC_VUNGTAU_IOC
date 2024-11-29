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
    [Route("api/v2/GiaiQuyetDon")]
    [ApiController]
    public class GiaiQuyetDonController : BaseApiController
    {
        private GiaiQuyetDonBUS _GiaiQuyetDonBUS;
        private RutDonBUS _RutDonBUS;
        private ChucNangBUS _ChucNangBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public GiaiQuyetDonController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<GiaiQuyetDonController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._GiaiQuyetDonBUS = new GiaiQuyetDonBUS();
            this._ChucNangBUS = new ChucNangBUS();
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
                IdentityHelper.CapHanhChinh = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapHanhChinh").Value, 0);
                IdentityHelper.BanTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "BanTiepDan").Value, false);

                //check role theo ma chuc nang 
                //var listChucNang = _ChucNangBUS.GetListMenuByNguoiDungID(IdentityHelper.NguoiDungID ?? 0);
                //if(listChucNang != null && listChucNang.Count > 0)
                //{
                //    foreach (var item in listChucNang)
                //    {
                //        if (item.Children != null && item.Children.Count > 0)
                //        {
                //            foreach (var chucNang in item.Children)
                //            {
                //                if(chucNang.MaMenu == "giao-va-duyet-bao-cao-xm")
                //                {
                //                    if (IdentityHelper.RoleID != RoleEnum.LanhDaoPhong.GetHashCode())
                //                    {
                //                        IdentityHelper.RoleID = RoleEnum.LanhDao.GetHashCode();
                //                    }
                //                }
                //                break;
                //            }
                //        }
                //    }
                //}

                if (IdentityHelper.SuDungQuyTrinhGQPhucTap == false)
                {
                    IdentityHelper.RoleID = RoleEnum.LanhDao.GetHashCode();
                    IdentityHelper.CanBoID = 0;
                }

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
                IList<DonThuGiaiQuyetInfo> Data;
                Data = _GiaiQuyetDonBUS.GetBySearch(ref TotalRow, p, IdentityHelper);
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

        [HttpGet]
        [Route("GetListPaging_QuyTrinhDonGian")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetListPaging_QuyTrinhDonGian([FromQuery] XuLyDonParamsForFilter p)
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
                IList<DonThuGiaiQuyetInfo> Data;
                Data = _GiaiQuyetDonBUS.GetBySearch_QuyTrinhDonGian(ref TotalRow, p, IdentityHelper);
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
                KetQuaXacMinhModel Data = new KetQuaXacMinhModel();
                Data = _GiaiQuyetDonBUS.GetByID(XuLyDonID ?? 0);
                if(Data != null && Data.BuocXacMinh != null && Data.BuocXacMinh.Count > 0)
                {
                    foreach (var item in Data.BuocXacMinh)
                    {
                        if(item.DanhSachHoSoTaiLieu != null && item.DanhSachHoSoTaiLieu.Count > 0)
                        {
                            foreach (var hs in item.DanhSachHoSoTaiLieu)
                            {
                                if(hs.FileDinhKem != null && hs.FileDinhKem.Count > 0)
                                {
                                    foreach (var file in hs.FileDinhKem)
                                    {
                                        if(file.FileUrl != null && file.FileUrl.Length > 0)
                                        {
                                            file.FileUrl = serverPath + file.FileUrl;
                                        }
                                    }
                                }
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

        [Route("GetTruongPhong")]
        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetTruongPhong()
        {
            try
            {
                IdentityHelper IdentityHelper = new IdentityHelper();
                IdentityHelper.CanBoID = CanBoID;
                IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                  
                IList<CanBoInfo> Data;
                Data = _GiaiQuyetDonBUS.GetTruongPhong(IdentityHelper);
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

        [Route("GetCanBoGQ")]
        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetCanBoGQ(int? XuLyDonID)
        {
            try
            {
                IdentityHelper IdentityHelper = new IdentityHelper();
                IdentityHelper.CanBoID = CanBoID;
                IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);

                IList<CanBoInfo> Data;
                Data = _GiaiQuyetDonBUS.GetCanBoGQ(IdentityHelper, XuLyDonID ?? 0);
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

        [Route("DanhSachBuocXacMinhByXuLyDonID")]
        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSachBuocXacMinhByXuLyDonID(int? XuLyDonID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                //IdentityHelper IdentityHelper = new IdentityHelper();
                //IdentityHelper.CanBoID = CanBoID;
                //IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                //IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);

                IList<BuocXacMinhInfo> Data;
                Data = _GiaiQuyetDonBUS.DanhSachBuocXacMinhByXuLyDonID(XuLyDonID ?? 0);
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
        [Route("GiaoXacMinh")]
        [CustomAuthAttribute(ChucNangEnum.GiaiQuyetDonThu, AccessLevel.Create)]
        public IActionResult GiaoXacMinh(GiaoXacMinhModel GiaoXacMinhModel)
        {
            try
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

                var Data = _GiaiQuyetDonBUS.GiaoXacMinh(IdentityHelper, GiaoXacMinhModel);
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
        [Route("CapNhapDoanToXacMinh")]
        [CustomAuthAttribute(ChucNangEnum.GiaiQuyetDonThu, AccessLevel.Create)]
        public IActionResult CapNhapDoanToXacMinh(GiaoXacMinhModel GiaoXacMinhModel)
        {
            try
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

                var Data = _GiaiQuyetDonBUS.CapNhapDoanToXacMinh(IdentityHelper, GiaoXacMinhModel);
                base.Data = Data.Data;
                base.Status = Data.Status;
                base.Message = Data.Message;
                base.MessageDetail = Data.MessageDetail;
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
        [Route("GiaoXacMinh_QuyTrinhDonGian")]
        [CustomAuthAttribute(ChucNangEnum.GiaiQuyetDonThu, AccessLevel.Create)]
        public IActionResult GiaoXacMinh_QuyTrinhDonGian(GiaoXacMinhModel GiaoXacMinhModel)
        {
            try
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

                IdentityHelper.RoleID = RoleEnum.LanhDao.GetHashCode();
                var Data = _GiaiQuyetDonBUS.GiaoXacMinh(IdentityHelper, GiaoXacMinhModel);
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
        [Route("CapNhatQuyetDinhGiaoXacMinh")]
        [CustomAuthAttribute(ChucNangEnum.GiaiQuyetDonThu, AccessLevel.Edit)]
        public IActionResult CapNhatQuyetDinhGiaoXacMinh(BuocXacMinhInfo BuocXacMinhInfo)
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
                    IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                    IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                    IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                    IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);
                    IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    IdentityHelper.CapThanhTra = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapThanhTra").Value, 0);
                    IdentityHelper.CapUBND = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "CapUBND").Value, false);

                    var Data = _GiaiQuyetDonBUS.CapNhatBuocXacMinh(IdentityHelper, BuocXacMinhInfo);
                    //if (Data.Status > 0 && IdentityHelper.SuDungQuyTrinhGQPhucTap == false)
                    //{
                    //    try
                    //    {
                    //        var ds = _GiaiQuyetDonBUS.GetByID(BuocXacMinhInfo.XuLyDonID ?? 0);
                    //        var check = false;
                    //        foreach (var item in ds.BuocXacMinh)
                    //        {
                    //            if (item.OrderBy == 8)
                    //            {
                    //                check = true;
                    //            }
                    //        }
                    //        BaoCaoXacMinhModel BaoCaoXacMinhModel = new BaoCaoXacMinhModel();
                    //        BaoCaoXacMinhModel.XuLyDonID = BuocXacMinhInfo.XuLyDonID;
                    //        BaoCaoXacMinhModel.NoiDung = "";
                    //        if (BuocXacMinhInfo.OrderBy == 8 && check) //Bao cao xac minh
                    //        {
                    //            _GiaiQuyetDonBUS.TrinhKy(IdentityHelper, BaoCaoXacMinhModel);
                    //        }
                    //        else
                    //        {
                    //            if (check == false)
                    //            {
                    //                _GiaiQuyetDonBUS.TrinhKy(IdentityHelper, BaoCaoXacMinhModel);
                    //            }
                    //        }
                    //    }
                    //    catch (Exception)
                    //    {
                    //    }      
                    //}
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
        [Route("CapNhatBuocXacMinh")]
        [CustomAuthAttribute(ChucNangEnum.GiaiQuyetDonThu, AccessLevel.Edit)]
        public IActionResult CapNhatBuocXacMinh(BuocXacMinhInfo BuocXacMinhInfo)
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
                    IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                    IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                    IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                    IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);
                    IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    IdentityHelper.CapThanhTra = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapThanhTra").Value, 0);
                    IdentityHelper.CapUBND = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "CapUBND").Value, false);

                    var Data = _GiaiQuyetDonBUS.CapNhatBuocXacMinh(IdentityHelper, BuocXacMinhInfo);
                    //if(Data.Status > 0 && IdentityHelper.SuDungQuyTrinhGQPhucTap == false)
                    //{
                    //    var ds = _GiaiQuyetDonBUS.GetByID(BuocXacMinhInfo.XuLyDonID ?? 0);
                    //    var check = false;
                    //    foreach (var item in ds.BuocXacMinh)
                    //    {
                    //        if(item.OrderBy == 8)
                    //        {
                    //            check = true;
                    //        }
                    //    }
                    //    BaoCaoXacMinhModel BaoCaoXacMinhModel = new BaoCaoXacMinhModel();
                    //    BaoCaoXacMinhModel.XuLyDonID = BuocXacMinhInfo.XuLyDonID;
                    //    BaoCaoXacMinhModel.NoiDung = "";
                    //    if (BuocXacMinhInfo.OrderBy == 8 && check) //Bao cao xac minh
                    //    {                       
                    //        _GiaiQuyetDonBUS.TrinhKy(IdentityHelper, BaoCaoXacMinhModel);
                    //    }
                    //    else
                    //    {
                    //        if(check == false)
                    //        {
                    //            _GiaiQuyetDonBUS.TrinhKy(IdentityHelper, BaoCaoXacMinhModel);
                    //        }
                    //    }
                    //}
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
        [Route("TrinhBaoCaoXacMinh")]
        [CustomAuthAttribute(ChucNangEnum.GiaiQuyetDonThu, AccessLevel.Edit)]
        public IActionResult TrinhBaoCaoXacMinh(BaoCaoXacMinhModel BaoCaoXacMinhModel)
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
                    IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                    IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                    IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                    IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);
                    IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    IdentityHelper.CapThanhTra = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapThanhTra").Value, 0);
                    IdentityHelper.CapUBND = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "CapUBND").Value, false);
                    IdentityHelper.CapHanhChinh = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapHanhChinh").Value, 0);
                    if (IdentityHelper.SuDungQuyTrinhGQPhucTap == false)
                    {
                        IdentityHelper.RoleID = RoleEnum.ChuyenVien.GetHashCode();
                    }

                    var Data = _GiaiQuyetDonBUS.TrinhKy(IdentityHelper, BaoCaoXacMinhModel);
                    base.Data = Data.Data;
                    base.Status = Data.Status;
                    //base.Message = Data.Message;
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
        [Route("DuyetBaoCaoXacMinh")]
        [CustomAuthAttribute(ChucNangEnum.GiaiQuyetDonThu, AccessLevel.Edit)]
        public IActionResult DuyetBaoCaoXacMinh(BaoCaoXacMinhModel BaoCaoXacMinhModel)
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
                    IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                    IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                    IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                    IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);
                    IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    IdentityHelper.CapThanhTra = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapThanhTra").Value, 0);
                    IdentityHelper.CapUBND = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "CapUBND").Value, false);
                    IdentityHelper.CapHanhChinh = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapHanhChinh").Value, 0);

                    var Data = _GiaiQuyetDonBUS.DuyetBaoCaoXacMinh(IdentityHelper, BaoCaoXacMinhModel);
                    // bổ sung BaoCaoXacMinhModel.TrangThaiPheDuyet == 1 để khi không phê duyệt thì chỉ cần xác minh 1 lần
                    if (IdentityHelper.CapID == CapQuanLy.CapUBNDXa.GetHashCode() && IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode() && IdentityHelper.SuDungQuyTrinhGQPhucTap == true&&BaoCaoXacMinhModel.TrangThaiPheDuyet == 1)
                    {                       
                        Data = _GiaiQuyetDonBUS.DuyetBaoCaoXacMinh(IdentityHelper, BaoCaoXacMinhModel);
                    }
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
        [Route("RutDon")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult SaveRutDon(RutDonInfo RutDonInfo)
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
                    IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                    IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                    IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                    IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);
                    IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    IdentityHelper.CapThanhTra = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapThanhTra").Value, 0);
                    IdentityHelper.CapUBND = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "CapUBND").Value, false);

                    var Data = _RutDonBUS.SaveRutDon(IdentityHelper, RutDonInfo);
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
        [Route("GiaHanThoiGianXacMinh")]
        [CustomAuthAttribute(ChucNangEnum.GiaiQuyetDonThu, AccessLevel.Edit)]
        public IActionResult GiaHanThoiGianXacMinh(KetQuaInfo Info)
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
                    IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                    IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                    IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                    IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);
                    IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    IdentityHelper.CapThanhTra = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapThanhTra").Value, 0);
                    IdentityHelper.CapUBND = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "CapUBND").Value, false);

                    var Data = _GiaiQuyetDonBUS.GiaHanThoiGianXacMinh(Info, IdentityHelper);
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
    }
}
