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
    [Route("api/v2/TiepDan")]
    [ApiController]
    public class TiepDanController : BaseApiController
    {
        private TiepDanBUS _TiepDanBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public TiepDanController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<TiepDanController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._TiepDanBUS = new TiepDanBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetListPaging")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery] TiepDanParamsForFilter p)
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    IList<TiepDanKhongDonInfo> Data;
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    //int Loai 
                    if (p.LoaiKhieuToID == 2)
                    {
                        p.LoaiKhieuToID = 8;//khieu nai
                    }
                    else if (p.LoaiKhieuToID == 3)
                    {
                        p.LoaiKhieuToID = 9;//kien nghi, phan anh
                    }
                    Data = _TiepDanBUS.GetBySearch(ref TotalRow, CanBoID, CoQuanID, p.PageSize, p.PageNumber, p.LoaiKhieuToID ?? 0, p.Keyword, p.TuNgay ?? DateTime.MinValue, p.DenNgay ?? DateTime.MinValue, 0, 0, 0, p.HuongXuLyID ?? 0, p.LoaiTiepDanID ?? 0);
                    base.Status = 1;
                    base.TotalRow = TotalRow;
                    base.Data = Data;
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

        [Route("GetByID")]
        [HttpGet]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Read)]
        public IActionResult GetByID(int TiepDanID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                //return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetByID, EnumLogType.GetList, () =>
                //{
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                TiepDanInfo Data;
                Data = _TiepDanBUS.GetByID(TiepDanID);
                if (Data == null)
                {
                    base.Status = 1;
                    base.Message = "Tiếp dân không tồn tại";
                }
                else
                {
                    if (Data.DanhSachHoSoTaiLieu != null && Data.DanhSachHoSoTaiLieu.Count > 0)
                    {
                        foreach (var item in Data.DanhSachHoSoTaiLieu)
                        {
                            if (item.FileDinhKem != null)
                            {
                                foreach (var f in item.FileDinhKem)
                                {
                                    f.FileUrl = serverPath + f.FileUrl;
                                }
                            }
                        }
                    }
                    base.Status = Data.TiepDanKhongDonID > 0 ? 1 : 0;
                    base.Data = Data;
                    base.Message = Data.CoQuanID > 0 ? ConstantLogMessage.API_Success : ConstantLogMessage.API_Error;
                }
                return base.GetActionResult();
                //});
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [Route("TiepDan_DanKhongDen_GetByID")]
        [HttpGet]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Read)]
        public IActionResult TiepDan_DanKhongDen_GetByID(int DanKhongDenID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                TiepCongDan_DanKhongDenInfo Data;
                Data = _TiepDanBUS.TiepDan_DanKhongDen_GetByID(DanKhongDenID);
                if (Data == null)
                {
                    base.Status = 1;
                    base.Message = "Tiếp dân không tồn tại";
                }
                else
                {
                    base.Status = Data.DanKhongDenID > 0 ? 1 : 0;
                    base.Data = Data;
                    base.Message = Data.CoQuanID > 0 ? ConstantLogMessage.API_Success : ConstantLogMessage.API_Error;
                }
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
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Create)]
        public IActionResult Insert(TiepDanInfo TiepDanInfo)
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

                    if (TiepDanInfo.NguonDonDen == null) TiepDanInfo.NguonDonDen = EnumNguonDonDen.TrucTiep.GetHashCode();
                    if (TiepDanInfo.NgayNhapDon == null) TiepDanInfo.NgayNhapDon = DateTime.Now;
                    if (TiepDanInfo.CanBoTiepID == null) TiepDanInfo.CanBoTiepID = CanBoID;
                    if (TiepDanInfo.CoQuanID == null) TiepDanInfo.CoQuanID = IdentityHelper.CoQuanID;
                    TiepDanInfo.CoQuanDangNhapID = IdentityHelper.CoQuanID;
                    var Data = _TiepDanBUS.Save(TiepDanInfo, IdentityHelper);
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
        [Route("SaveTiepCongDan_DanKhongDen")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Create)]
        public IActionResult SaveTiepCongDan_DanKhongDen(TiepCongDan_DanKhongDenInfo TiepDanInfo)
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

                    if (TiepDanInfo.NgayTruc == null) TiepDanInfo.NgayTruc = DateTime.Now;
                    if (TiepDanInfo.NguoiTaoID == null) TiepDanInfo.NguoiTaoID = CanBoID;
                    var Data = _TiepDanBUS.SaveTiepCongDan_DanKhongDen(TiepDanInfo);
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
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Delete)]
        public IActionResult Delete(List<TiepDanKhongDonInfo> p)
        {
            try
            {
                IdentityHelper IdentityHelper = new IdentityHelper();
                IdentityHelper.CanBoID = CanBoID;
                IdentityHelper.CoQuanID = CoQuanID;
                IdentityHelper.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                IdentityHelper.UserID = IdentityHelper.NguoiDungID;
                IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);

                if (p != null && p.Count > 0)
                {
                    foreach (var item in p)
                    {
                        if (item.DanKhongDenID > 0)
                        {
                            var Data = _TiepDanBUS.DeleteDanKhongDen(item.DanKhongDenID ?? 0);
                            base.Data = Data.Data;
                            base.Status = Data.Status;
                            base.Message = Data.Message;
                        }
                        else if (item.TiepDanKhongDonID > 0)
                        {
                            var Data = _TiepDanBUS.Delete(item.TiepDanKhongDonID);
                            base.Data = Data.Data;
                            base.Status = Data.Status;
                            base.Message = Data.Message;
                        }

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

        [HttpPost]
        [Route("DeleteDanKhongDen")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Delete)]
        public IActionResult DeleteDanKhongDen(BaseDeleteParams p)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                //{
                IdentityHelper IdentityHelper = new IdentityHelper();
                IdentityHelper.CanBoID = CanBoID;
                IdentityHelper.CoQuanID = CoQuanID;
                IdentityHelper.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                IdentityHelper.UserID = IdentityHelper.NguoiDungID;
                IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                IdentityHelper.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, String.Empty);
                IdentityHelper.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                IdentityHelper.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                IdentityHelper.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);

                if (p.ListID != null && p.ListID.Count > 0)
                {
                    foreach (var item in p.ListID)
                    {
                        var Data = _TiepDanBUS.DeleteDanKhongDen(item);
                        base.Data = Data.Data;
                        base.Status = Data.Status;
                        base.Message = Data.Message;
                    }
                }

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
        [Route("GetAllHuongXuLy")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetAllHuongXuLy()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<HuongGiaiQuyetInfo> Data;
                Data = _TiepDanBUS.GetAllHuongXuLy();
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
        [Route("DanhMucLoaiKhieuTo")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhMucLoaiKhieuTo(int? LoaiKhieuToID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<LoaiKhieuToInfo> Data;
                Data = _TiepDanBUS.DanhMucLoaiKhieuTo(LoaiKhieuToID ?? 0);
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
        [Route("DanhMucQuocTich")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhMucQuocTich()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<QuocTichInfo> Data;
                Data = _TiepDanBUS.DanhMucQuocTich();
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
        [Route("DanhMucDanToc")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhMucDanToc()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<DanTocInfo> Data;
                Data = _TiepDanBUS.DanhMucDanToc();
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
        [Route("DanhMucTinh")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhMucTinh()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<TinhInfo> Data;
                Data = _TiepDanBUS.DanhMucTinh();
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
        [Route("DanhMucHuyen")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhMucHuyen(int? TinhID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<HuyenInfo> Data;
                Data = _TiepDanBUS.DanhMucHuyen(TinhID ?? 0);
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
        [Route("DanhMucXa")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhMucXa(int? HuyenID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<XaInfo> Data;
                Data = _TiepDanBUS.DanhMucXa(HuyenID ?? 0);
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
        [Route("DanhMucChucVu")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhMucChucVu()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<ChucVuInfo> Data;
                Data = _TiepDanBUS.DanhMucChucVu();
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
        [Route("HinhThucDaGiaiQuyet")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult HinhThucDaGiaiQuyet()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                List<DanhMucChungModel> Data = new List<DanhMucChungModel>();
                Data.Add(new DanhMucChungModel("Chưa giải quyết", EnumKetQuaTiepDan.ChuaGiaiQuyet.GetHashCode()));
                Data.Add(new DanhMucChungModel("Chưa có QĐ giải quyết", EnumKetQuaTiepDan.ChuaCoQDGiaiQuyet.GetHashCode()));
                Data.Add(new DanhMucChungModel("Đã có QĐ giải quyết", EnumKetQuaTiepDan.DaCoQDGiaiQuyet.GetHashCode()));
                Data.Add(new DanhMucChungModel("Đã có bản án của tòa", EnumKetQuaTiepDan.DaCoBanAnCuaToa.GetHashCode()));
                base.Status = 1;
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
        [Route("GetCanBoXuLy")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetCanBoXuLy()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<CanBoInfo> Data;
                int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                Data = _TiepDanBUS.GetCanBoXuLy(CoQuanID);
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
        [Route("GetDanhSachLanhDao")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetDanhSachLanhDao(int? CoQuanID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<CanBoInfo> Data;
                Data = _TiepDanBUS.GetDanhSachLanhDao();
                if (CoQuanID > 0 && Data.Count > 0)
                {
                    Data = Data.Where(x => x.CoQuanID == CoQuanID).ToList();
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
            }
        }

        [HttpGet]
        [Route("GetPhongXuLy")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetPhongXuLy()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<PhongBanInfo> Data;
                Data = _TiepDanBUS.GetPhongXuLy(CoQuanID);
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
        [Route("DanhMucTenFile")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhMucTenFile()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<DanhMucFileInfo> Data;
                Data = _TiepDanBUS.DanhMucTenFile();
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
        [Route("GetAllCanBo")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetAllCanBo()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<CanBoInfo> Data;
                Data = _TiepDanBUS.GetAllCanBo();
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
        [Route("GetAllCoQuan")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetAllCoQuan()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
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
        [Route("GetAllCap")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetAllCap()
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<CapInfo> Data;
                Data = _TiepDanBUS.GetAllCap();
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
        [Route("CheckSoDonTrung")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Read)]
        public IActionResult CheckSoDonTrung([FromQuery] string? hoTen, string? cmnd, string? diaChi, string? noiDung)
        {
            try
            {
                int Data = 0;
                int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                Data = _TiepDanBUS.CheckSoDonTrung(hoTen, cmnd, diaChi, noiDung, CoQuanID);
                base.Status = 1;
                base.Data = Data;
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
        [Route("GetDonTrung")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Read)]
        public IActionResult GetDonTrung([FromQuery] string? hoTen, string? cmnd, string? diaChi, string? noiDung)
        {
            try
            {
                IList<TiepDanInfo> Data;
                int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                Data = _TiepDanBUS.GetDonTrung(hoTen, cmnd, diaChi, noiDung, CoQuanID);
                base.Status = 1;
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                //base.Message = ConstantLogMessage.API_Error_System;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("CTDonTrung")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Read)]
        public IActionResult CTDonTrung([FromQuery] int? DonThuID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                IList<TiepDanInfo> Data;
                //int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                Data = _TiepDanBUS.CTDonTrung(DonThuID ?? 0);
                base.Status = 1;
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                //base.Message = ConstantLogMessage.API_Error_System;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }
        [HttpGet]
        [Route("CTDonKhieuToLan2")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Read)]
        public IActionResult CTDonKhieuToLan2([FromQuery] int? DonThuID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                IList<TiepDanInfo> Data;
                //int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                Data = _TiepDanBUS.CTDonKhieuToLan2(DonThuID ?? 0);
                base.Status = 1;
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                //base.Message = ConstantLogMessage.API_Error_System;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("KiemTraKhieuToLan2")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Read)]
        public IActionResult KiemTraKhieuToLan2([FromQuery] string? hoTen, string? cmnd, string? diaChi, string? noiDung)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                IList<TiepDanInfo> Data;
                Data = _TiepDanBUS.KiemTraKhieuToLan2(hoTen, cmnd, diaChi, noiDung, CoQuanID);
                base.Status = 1;
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                //base.Message = ConstantLogMessage.API_Error_System;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("GetSTTHoSo")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetSTT(int? namTiepNhan)
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
                var result = _TiepDanBUS.GetSoDonThuByNamTiepNhan(IdentityHelper.CoQuanID ?? 0, IdentityHelper, namTiepNhan);
                base.Status = 1;
                base.Data = result;
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
        [Route("TiepDanDinhKy_GetListPaging")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Read)]
        public IActionResult TiepDanDinhKy_GetListPaging([FromQuery] TiepDanParamsForFilter p)
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    IList<TiepDanDinhKyModel> Data;
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    p.CoQuanID = CoQuanID;
                    Data = _TiepDanBUS.GetBySearch_TiepDanDinhKy(ref TotalRow, p);
                    base.Status = 1;
                    base.TotalRow = TotalRow;
                    base.Data = Data;
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

        [Route("TiepDanDinhKy_GetByID")]
        [HttpGet]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Read)]
        public IActionResult TiepDanDinhKy_GetByID(int TiepDinhKyID)
        {
            try
            {
                int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);
                TiepDanDinhKyModel Data;
                Data = _TiepDanBUS.GetByID_TiepDanDinhKy_New(TiepDinhKyID);
                if (Data == null)
                {
                    base.Status = 1;
                    base.Message = "Tiếp dân không tồn tại";
                }
                else
                {
                    if (Data.DanhSachHoSoTaiLieu != null && Data.DanhSachHoSoTaiLieu.Count > 0)
                    {
                        foreach (var item in Data.DanhSachHoSoTaiLieu)
                        {
                            if (item.FileDinhKem != null)
                            {
                                foreach (var f in item.FileDinhKem)
                                {
                                    f.FileUrl = serverPath + f.FileUrl;
                                }
                            }
                        }
                    }
                    base.Status = Data.TiepDinhKyID > 0 ? 1 : 0;
                    base.Data = Data;
                    base.Message = Data.CoQuanID > 0 ? ConstantLogMessage.API_Success : ConstantLogMessage.API_Error;
                }
                return base.GetActionResult();
                //});
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        //[HttpPost]
        //[Route("SaveTiepDanDinhKy")]
        ////[CustomAuthAttribute(ChucNangEnum.QuanLyPhanQuyen, AccessLevel.Create)]
        //public IActionResult SaveTiepDanDinhKy(TiepDanDinhKyModel TiepDanInfo)
        //{
        //    try
        //    {
        //        return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
        //        {
        //            int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);

        //            TiepDanInfo.CoQuanID = CoQuanID;
        //            var Data = _TiepDanBUS.SaveTiepDanDinhKy(TiepDanInfo);
        //            base.Data = Data.Data;
        //            base.Status = Data.Status;
        //            base.Message = Data.Message;
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

        [HttpPost]
        [Route("SaveTiepDanDinhKy")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Edit)]
        public IActionResult SaveTiepDanDinhKy_New(TiepDanDinhKyModel TiepDanInfo)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                {
                    int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);

                    TiepDanInfo.CoQuanID = CoQuanID;
                    var Data = _TiepDanBUS.SaveTiepDanDinhKy_New(TiepDanInfo);
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
        [Route("DeleteTiepDanDinhKy")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Delete)]
        public IActionResult DeleteTiepDanDinhKy(BaseDeleteParams p)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                //{
                IdentityHelper IdentityHelper = new IdentityHelper();
                IdentityHelper.CanBoID = CanBoID;
                IdentityHelper.CoQuanID = CoQuanID;
                IdentityHelper.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);

                if (p.ListID != null && p.ListID.Count > 0)
                {
                    foreach (var item in p.ListID)
                    {
                        var Data = _TiepDanBUS.DeleteTiepDanDinhKy(item);
                        base.Data = Data.Data;
                        base.Status = Data.Status;
                        base.Message = Data.Message;
                    }
                }

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
        [Route("DeleteVuViec")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Delete)]
        public IActionResult DeleteVuViec(List<TiepDanDinhKyModel> p)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                //{
                IdentityHelper IdentityHelper = new IdentityHelper();
                IdentityHelper.CanBoID = CanBoID;
                IdentityHelper.CoQuanID = CoQuanID;
                IdentityHelper.NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);

                if (p != null && p.Count > 0)
                {
                    foreach (var item in p)
                    {
                        if (item.DanKhongDenID > 0)
                        {
                            var Data = _TiepDanBUS.DeleteDanKhongDen(item.DanKhongDenID ?? 0);
                            base.Data = Data.Data;
                            base.Status = Data.Status;
                            base.Message = Data.Message;
                        }
                        else
                        {
                            var Data = _TiepDanBUS.DeleteVuViec(item.TiepDinhKyID);
                            base.Data = Data.Data;
                            base.Status = Data.Status;
                            base.Message = Data.Message;
                        }


                    }
                }

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
        [Route("DanhSachBieuMau")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSachBieuMau()
        {
            try
            {
                IList<BieuMauInfo> Data;
                int CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                Data = _TiepDanBUS.DanhSachBieuMau(CapID);
                base.Status = 1;
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                //base.Message = ConstantLogMessage.API_Error_System;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("InPhieu")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult InPhieu([FromQuery] string? MaPhieuIn, int? XuLyDonID, int? DonThuID, int? TiepDanKhongDonID)
        {
            try
            {
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);

                int CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                int CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                var Data = _TiepDanBUS.InPhieu(MaPhieuIn, XuLyDonID ?? 0, DonThuID ?? 0, TiepDanKhongDonID ?? 0, CapID, _host.ContentRootPath, CoQuanID, CanBoID);
                base.Status = Data.Status;
                base.Data = serverPath + Data.Data;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                //base.Message = ConstantLogMessage.API_Error_System;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }
        [HttpGet]
        [Route("InPhieuPDF")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult InPhieuPDF([FromQuery] string? MaPhieuIn, int? XuLyDonID, int? DonThuID, int? TiepDanKhongDonID)
        {
            try
            {
                var clsCommon = new Commons();
                string serverPath = clsCommon.GetServerPath(HttpContext);

                int CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                int CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                var Data = _TiepDanBUS.InPhieuPDF(MaPhieuIn, XuLyDonID ?? 0, DonThuID ?? 0, TiepDanKhongDonID ?? 0, CapID, _host.ContentRootPath, CoQuanID, CanBoID, _config);
                base.Status = Data.Status;
                base.Data = serverPath + Data.Data;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                //base.Message = ConstantLogMessage.API_Error_System;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("DanhSachDonThuDaTiepNhan")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSachDonThuDaTiepNhan([FromQuery] TiepDanParamsForFilter p)
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    IList<DTXuLyInfo> Data;
                    IdentityHelper IdentityHelper = new IdentityHelper();
                    string ContentRootPath = _host.ContentRootPath;
                    IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    IdentityHelper.CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    IdentityHelper.TinhID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    IdentityHelper.HuyenID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    IdentityHelper.XaID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "XaID").Value, 0);
                    IdentityHelper.RoleID = EnumChucVu.LanhDao.GetHashCode();

                    //int Loai 
                    if (p.LoaiKhieuToID == 2)
                    {
                        p.LoaiKhieuToID = 8;//khieu nai
                    }
                    else if (p.LoaiKhieuToID == 3)
                    {
                        p.LoaiKhieuToID = 9;//kien nghi, phan anh
                    }
                    IdentityHelper.PageSize = p.PageSize;
                    Data = _TiepDanBUS.GetBySearchDonThuDaTiepNhan(ref TotalRow, IdentityHelper, p.LoaiKhieuToID, p.Keyword, p.TuNgay, p.DenNgay, p.CQChuyenDonDenID, p.LoaiRutDon, p.PageNumber);
                    base.Status = 1;
                    base.TotalRow = TotalRow;
                    base.Data = Data;
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
        [Route("CapNhapSoDonThuTheoNam")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Create)]
        public IActionResult CapNhapSoDonThuTheoNam([FromBody] int namTiepNhan)
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


                    var Data = _TiepDanBUS.CapNhapSoDonThuTheoNam(IdentityHelper, namTiepNhan);
                    base.Data = Data.Data;
                    base.Status = Data.Status;
                    base.Message = Data.Message;
                    base.MessageDetail = Data.MessageDetail;
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
        [Route("ChuyenDonSangTiepNhanDon")]
        [CustomAuthAttribute(ChucNangEnum.TiepDanThuongXuyen, AccessLevel.Create)]
        public IActionResult ChuyenDonSangTiepNhanDon([FromQuery] string xuLyDonIDIdS , string donThuIDIDs)
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


                    var Data = _TiepDanBUS.ChuyenDonTDTTSangTND(xuLyDonIDIdS, donThuIDIDs);
                    base.Data = Data.Data;
                    base.Status = Data.Status;
                    base.Message = Data.Message;
                    base.MessageDetail = Data.MessageDetail;
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
