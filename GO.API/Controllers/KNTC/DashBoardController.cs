using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.BaoCao;
using Com.Gosol.KNTC.BUS.KNTC;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using GO.API.Controllers.BacCao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel;

namespace GO.API.Controllers.KNTC
{
    [Route("api/v2/DashBoard")]
    [ApiController]
    public class DashBoardController : BaseApiController
    {
        private DashBoardBUS _DashBoardBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public DashBoardController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DashBoardController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._DashBoardBUS = new DashBoardBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("GetDanhSachData")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetDuLieuDashBoard([FromQuery] DashBoardParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    string ContentRootPath = _host.ContentRootPath;
                    p.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    p.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    p.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    p.CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    p.TinhID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    p.HuyenID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    p.ChuTichUBND = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "ChuTichUBND").Value, 0);
                    //string tuNgay = "01/01/" + DateTime.Now.Year;
                    //string denNgay = "31/12/" + DateTime.Now.Year; 
                    //string tuNgayCungKy = "01/01/" + (DateTime.Now.Year - 1);
                    //string denNgayCungKy = "31/12/" + (DateTime.Now.Year - 1);
                    string tuNgay = "01/01/2021";
                    string denNgay = "31/12/2021";
                    string tuNgayCungKy = "01/01/2020";
                    string denNgayCungKy = "31/12/2020";
                    p.TuNgayCungKy = tuNgayCungKy;
                    p.DenNgayCungKy = denNgayCungKy;
                    if (p.TuNgay == null)
                    {
                        p.TuNgay = tuNgay;
                    }
                    else
                    {
                        if (p.TuNgay.Length > 8)
                        {
                            int Nam = Utils.ConvertToInt32(p.TuNgay.Substring(p.TuNgay.Length - 4, 4), 0);
                            p.TuNgayCungKy = p.TuNgay.Substring(0, 6) + (Nam - 1);
                        }
                    }

                    if (p.DenNgay == null)
                    {
                        p.DenNgay = denNgay;
                    }
                    else
                    {
                        if (p.DenNgay.Length > 8)
                        {
                            int Nam = Utils.ConvertToInt32(p.DenNgay.Substring(p.DenNgay.Length - 4, 4), 0);
                            p.DenNgayCungKy = p.DenNgay.Substring(0, 6) + (Nam - 1);
                        }
                    }
                    var Data = _DashBoardBUS.GetDuLieuDashBoard(p);
                    base.Status = 1;
                    base.Data = Data;
                    //base.Data = new DashBoardModel();
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
        [Route("LayDanhSachCanhBao")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult LayDanhSachCanhBao([FromQuery] CanhBaoParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    string ContentRootPath = _host.ContentRootPath;
                    p.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    p.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    p.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    p.CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    p.TinhID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    p.HuyenID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    p.ChuTichUBND = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "ChuTichUBND").Value, 0);
                    p.BanTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "BanTiepDan").Value, false);
                    var SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                    var SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);

                    var capHanhChinh = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapHanhChinh").Value, 0);

                    var laThanhTraTinh = false;
                    try
                    {
                        var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
                        if (listThanhTraTinh.Contains(p.CoQuanID ?? 0))
                        {
                            laThanhTraTinh = true;
                        }
                    }
                    catch (Exception)
                    {

                    }

                    DashBoardModel Data = new DashBoardModel();
                    Data.SoLieuCanhBao = new List<SoLieuCanhBao>();
                    var soLieuChung = _DashBoardBUS.GetDataDashBoard_By_User(p);

                    if (capHanhChinh == EnumCapHanhChinh.CapUBNDTinh.GetHashCode())
                    {
                        if (p.BanTiepDan == true) // BTD tỉnh
                        {
                            if (p.RoleID == RoleEnum.LanhDao.GetHashCode())
                            {
                                SoLieuCanhBao sl2 = new SoLieuCanhBao();
                                sl2.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                                sl2.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                                sl2.Data = new List<Data>();
                                sl2.Data.Add(new Data("Cần phê duyệt", soLieuChung.CanPheDuyet));
                                sl2.Data.Add(new Data("Đã phê duyệt", soLieuChung.DaPheDuyet));
                                sl2.Data.Add(new Data("Tổng số", soLieuChung.CanPheDuyet + soLieuChung.DaPheDuyet));
                                Data.SoLieuCanhBao.Add(sl2);

                                SoLieuCanhBao sl3 = new SoLieuCanhBao();
                                sl3.TenCanhBao = "Cảnh báo cần trình dự thảo quyết định giao xác minh";
                                sl3.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                                sl3.Data = new List<Data>();
                                sl3.Data.Add(new Data("Cần trình dự thảo", soLieuChung.CanTrinhDuThao));
                                sl3.Data.Add(new Data("Đã trình dự thảo", soLieuChung.DaTrinhDuThao));
                                sl3.Data.Add(new Data("Tổng số", soLieuChung.CanTrinhDuThao + soLieuChung.DaTrinhDuThao));
                                Data.SoLieuCanhBao.Add(sl3);

                                SoLieuCanhBao sl4 = new SoLieuCanhBao();
                                sl4.TenCanhBao = "Cảnh báo cập nhật nội dung quyết định giao xác minh";
                                sl4.MaChucNang = "giao-xac-minh";
                                sl4.Data = new List<Data>();
                                sl4.Data.Add(new Data("Cần cập nhật", soLieuChung.CanCapNhatNDQDGXM));
                                sl4.Data.Add(new Data("Đã cập nhật:", soLieuChung.DaCapNhatNDQDGXM));
                                sl4.Data.Add(new Data("Tổng số", soLieuChung.CanCapNhatNDQDGXM + soLieuChung.DaCapNhatNDQDGXM));
                                Data.SoLieuCanhBao.Add(sl4);

                                SoLieuCanhBao sl5 = new SoLieuCanhBao();
                                sl5.TenCanhBao = "Cảnh báo cập nhật nội dung báo cáo, kết luận, quyết định giải quyết";
                                sl5.MaChucNang = "ban-hanh-qd";
                                sl5.Data = new List<Data>();
                                sl5.Data.Add(new Data("Cần cập nhật", soLieuChung.CanCapNhatBCQDKL));
                                sl5.Data.Add(new Data("Đã cập nhật", soLieuChung.DaCapNhatBCQDKL));
                                sl5.Data.Add(new Data("Tổng số", soLieuChung.CanCapNhatBCQDKL + soLieuChung.DaCapNhatBCQDKL));
                                Data.SoLieuCanhBao.Add(sl5);
                            }
                            else
                            {
                                SoLieuCanhBao sl2 = new SoLieuCanhBao();
                                sl2.TenCanhBao = "Cảnh báo đơn thư cần xử lý";
                                sl2.MaChucNang = "xu-ly-don-thu";
                                sl2.Data = new List<Data>();
                                sl2.Data.Add(new Data("Cần xử lý đơn", soLieuChung.CanXuLy));
                                sl2.Data.Add(new Data("Đã xử lý đơn", soLieuChung.DaXuLy));
                                sl2.Data.Add(new Data("Tổng số", soLieuChung.CanXuLy + soLieuChung.DaXuLy));
                                Data.SoLieuCanhBao.Add(sl2);

                                SoLieuCanhBao sl3 = new SoLieuCanhBao();
                                sl3.TenCanhBao = "Cảnh báo cần trình duyệt xử lý đơn";
                                sl3.MaChucNang = "xu-ly-don-thu";
                                sl3.Data = new List<Data>();
                                sl3.Data.Add(new Data("Cần trình kết quả xử lý đơn", soLieuChung.CanTrinhKetQua));
                                sl3.Data.Add(new Data("Đã trình kết quả xử lý", soLieuChung.DaTrinhKetQua));
                                sl3.Data.Add(new Data("Tổng số", soLieuChung.CanTrinhKetQua + soLieuChung.DaTrinhKetQua));
                                Data.SoLieuCanhBao.Add(sl3);

                                SoLieuCanhBao sl5 = new SoLieuCanhBao();
                                sl5.TenCanhBao = "Cảnh báo cập nhật nội dung báo cáo, kết luận, quyết định giải quyết";
                                sl5.MaChucNang = "ban-hanh-qd";
                                sl5.Data = new List<Data>();
                                sl5.Data.Add(new Data("Cần cập nhật", soLieuChung.CanCapNhatBCQDKL));
                                sl5.Data.Add(new Data("Đã cập nhật", soLieuChung.DaCapNhatBCQDKL));
                                sl5.Data.Add(new Data("Tổng số", soLieuChung.CanCapNhatBCQDKL + soLieuChung.DaCapNhatBCQDKL));
                                Data.SoLieuCanhBao.Add(sl5);
                            }
                        }
                        else
                        {
                            if (p.ChuTichUBND == 1)  // chủ tịch tỉnh
                            {
                                SoLieuCanhBao sl2 = new SoLieuCanhBao();
                                sl2.TenCanhBao = "Cảnh báo cần ban hành quyết định giao xác minh";
                                sl2.MaChucNang = "giai-quyet-don-thu";
                                sl2.Data = new List<Data>();
                                sl2.Data.Add(new Data("Cần ban hành", soLieuChung.CanBanHanhGXM));
                                sl2.Data.Add(new Data("Đã ban hành", soLieuChung.DaBanHanhGXM));
                                sl2.Data.Add(new Data("Tổng số", soLieuChung.CanBanHanhGXM + soLieuChung.DaBanHanhGXM));
                                Data.SoLieuCanhBao.Add(sl2);

                                SoLieuCanhBao sl3 = new SoLieuCanhBao();
                                sl3.TenCanhBao = "Cảnh báo cần ban hành quyết định giải quyết";
                                sl3.MaChucNang = "giai-quyet-don-thu";
                                sl3.Data = new List<Data>();
                                sl3.Data.Add(new Data("Cần ban hành", soLieuChung.CanBanHanhGQ));
                                sl3.Data.Add(new Data("Đã ban hành", soLieuChung.DaBanHanhGQ));
                                sl3.Data.Add(new Data("Tổng số", soLieuChung.CanBanHanhGQ + soLieuChung.DaBanHanhGQ));
                                Data.SoLieuCanhBao.Add(sl3);
                            }
                        }
                    }
                    else if (capHanhChinh == EnumCapHanhChinh.CapSoNganh.GetHashCode())
                    {
                        if (p.RoleID == 1)
                        {
                            if (SuDungQuyTrinhPhucTap)
                            {
                                SoLieuCanhBao sl2 = new SoLieuCanhBao();
                                sl2.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                                sl2.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                                sl2.Data = new List<Data>();
                                sl2.Data.Add(new Data("Cần phê duyệt", soLieuChung.CanPheDuyet));
                                sl2.Data.Add(new Data("Đã phê duyệt", soLieuChung.DaPheDuyet));
                                sl2.Data.Add(new Data("Tổng số", soLieuChung.CanPheDuyet + soLieuChung.DaPheDuyet));
                                Data.SoLieuCanhBao.Add(sl2);
                            }

                            SoLieuCanhBao sl3 = new SoLieuCanhBao();
                            sl3.TenCanhBao = "Cảnh báo cần giao xác minh";
                            sl3.MaChucNang = "giai-quyet-don-thu";
                            sl3.Data = new List<Data>();
                            sl3.Data.Add(new Data("Cần giao xác minh", soLieuChung.CanGiaoXacMinh));
                            sl3.Data.Add(new Data("Đã giao xác minh", soLieuChung.DaGiaoXacMinh));
                            sl3.Data.Add(new Data("Tổng số", soLieuChung.CanGiaoXacMinh + soLieuChung.DaGiaoXacMinh));
                            Data.SoLieuCanhBao.Add(sl3);

                            SoLieuCanhBao sl4 = new SoLieuCanhBao();
                            sl4.TenCanhBao = "Cảnh báo duyệt báo cáo xác minh";
                            sl4.MaChucNang = "giai-quyet-don-thu";
                            sl4.Data = new List<Data>();
                            sl4.Data.Add(new Data("Cần duyệt báo cáo xác minh", soLieuChung.CanDuyetBCXacMinh));
                            sl4.Data.Add(new Data("Đã duyệt", soLieuChung.DaDuyetBCXacMinh));
                            sl4.Data.Add(new Data("Tổng số", soLieuChung.CanDuyetBCXacMinh + soLieuChung.DaDuyetBCXacMinh));
                            Data.SoLieuCanhBao.Add(sl4);

                            SoLieuCanhBao sl5 = new SoLieuCanhBao();
                            sl5.TenCanhBao = "Cảnh báo cập nhật nội dung báo cáo, kết luận, quyết định giải quyết";
                            sl5.MaChucNang = "ban-hanh-qd";
                            sl5.Data = new List<Data>();
                            sl5.Data.Add(new Data("Cần cập nhật", soLieuChung.CanCapNhatBCQDKL));
                            sl5.Data.Add(new Data("Đã cập nhật", soLieuChung.DaCapNhatBCQDKL));
                            sl5.Data.Add(new Data("Tổng số", soLieuChung.CanCapNhatBCQDKL + soLieuChung.DaCapNhatBCQDKL));
                            Data.SoLieuCanhBao.Add(sl5);
                        }
                        else if (p.RoleID == 2)
                        {
                            if (SuDungQuyTrinhPhucTap)
                            {
                                SoLieuCanhBao sl2 = new SoLieuCanhBao();
                                sl2.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                                sl2.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                                sl2.Data = new List<Data>();
                                sl2.Data.Add(new Data("Cần phê duyệt", soLieuChung.CanPheDuyet));
                                sl2.Data.Add(new Data("Đã phê duyệt", soLieuChung.DaPheDuyet));
                                sl2.Data.Add(new Data("Tổng số", soLieuChung.CanPheDuyet + soLieuChung.DaPheDuyet));
                                Data.SoLieuCanhBao.Add(sl2);
                            }

                            SoLieuCanhBao sl3 = new SoLieuCanhBao();
                            sl3.TenCanhBao = "Cảnh báo cần giao xác minh";
                            sl3.MaChucNang = "giai-quyet-don-thu";
                            sl3.Data = new List<Data>();
                            sl3.Data.Add(new Data("Cần giao xác minh", soLieuChung.CanGiaoXacMinh));
                            sl3.Data.Add(new Data("Đã giao xác minh", soLieuChung.DaGiaoXacMinh));
                            sl3.Data.Add(new Data("Tổng số", soLieuChung.CanGiaoXacMinh + soLieuChung.DaGiaoXacMinh));
                            Data.SoLieuCanhBao.Add(sl3);

                            SoLieuCanhBao sl4 = new SoLieuCanhBao();
                            sl4.TenCanhBao = "Cảnh báo duyệt báo cáo xác minh";
                            sl4.MaChucNang = "giai-quyet-don-thu";
                            sl4.Data = new List<Data>();
                            sl4.Data.Add(new Data("Cần duyệt báo cáo xác minh", soLieuChung.CanDuyetBCXacMinh));
                            sl4.Data.Add(new Data("Đã duyệt", soLieuChung.DaDuyetBCXacMinh));
                            sl4.Data.Add(new Data("Tổng số", soLieuChung.CanDuyetBCXacMinh + soLieuChung.DaDuyetBCXacMinh));
                            Data.SoLieuCanhBao.Add(sl4);
                        }
                        else
                        {
                            SoLieuCanhBao sl2 = new SoLieuCanhBao();
                            sl2.TenCanhBao = "Cảnh báo đơn thư cần xử lý";
                            sl2.MaChucNang = "xu-ly-don-thu";
                            sl2.Data = new List<Data>();
                            sl2.Data.Add(new Data("Cần xử lý đơn", soLieuChung.CanXuLy));
                            sl2.Data.Add(new Data("Đã xử lý đơn", soLieuChung.DaXuLy));
                            sl2.Data.Add(new Data("Tổng số", soLieuChung.CanXuLy + soLieuChung.DaXuLy));
                            Data.SoLieuCanhBao.Add(sl2);

                            SoLieuCanhBao sl3 = new SoLieuCanhBao();
                            sl3.TenCanhBao = "Cảnh báo cần trình duyệt xử lý đơn";
                            sl3.MaChucNang = "xu-ly-don-thu";
                            sl3.Data = new List<Data>();
                            sl3.Data.Add(new Data("Cần trình kết quả xử lý đơn", soLieuChung.CanTrinhKetQua));
                            sl3.Data.Add(new Data("Đã trình kết quả xử lý", soLieuChung.DaTrinhKetQua));
                            sl3.Data.Add(new Data("Tổng số", soLieuChung.CanTrinhKetQua + soLieuChung.DaTrinhKetQua));
                            Data.SoLieuCanhBao.Add(sl3);

                            SoLieuCanhBao sl4 = new SoLieuCanhBao();
                            sl4.TenCanhBao = "Cảnh báo xác minh đơn thư";
                            sl4.MaChucNang = "giai-quyet-don-thu";
                            sl4.Data = new List<Data>();
                            sl4.Data.Add(new Data("Cần xác minh", soLieuChung.CanXMDonThu));
                            sl4.Data.Add(new Data("Đã xác minh:", soLieuChung.DaXMDonThu));
                            sl4.Data.Add(new Data("Tổng số", soLieuChung.CanXMDonThu + soLieuChung.DaXMDonThu));
                            Data.SoLieuCanhBao.Add(sl4);

                            SoLieuCanhBao sl6 = new SoLieuCanhBao();
                            sl6.TenCanhBao = "Cảnh báo cần cập nhật kết quản thi hành";
                            sl6.MaChucNang = "thi-hanh";
                            sl6.Data = new List<Data>();
                            sl6.Data.Add(new Data("Cần cập nhật kết quả thi hành:", soLieuChung.CanThiHanh));
                            sl6.Data.Add(new Data("Đã thi hành", soLieuChung.DaThiHanh));
                            sl6.Data.Add(new Data("Tổng số", soLieuChung.CanThiHanh + soLieuChung.DaThiHanh));
                            Data.SoLieuCanhBao.Add(sl6);
                        }
                    }
                    else if (capHanhChinh == EnumCapHanhChinh.CapUBNDHuyen.GetHashCode())
                    {
                        if (p.ChuTichUBND == 1)  // chủ tịch tỉnh
                        {
                            SoLieuCanhBao slcpd = new SoLieuCanhBao();
                            slcpd.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                            slcpd.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                            slcpd.Data = new List<Data>();
                            slcpd.Data.Add(new Data("Cần phê duyệt", soLieuChung.CanPheDuyet));
                            slcpd.Data.Add(new Data("Đã phê duyệt", soLieuChung.DaPheDuyet));
                            slcpd.Data.Add(new Data("Tổng số", soLieuChung.CanPheDuyet + soLieuChung.DaPheDuyet));
                            Data.SoLieuCanhBao.Add(slcpd);

                            SoLieuCanhBao sl2 = new SoLieuCanhBao();
                            sl2.TenCanhBao = "Cảnh báo cần ban hành quyết định giao xác minh";
                            sl2.MaChucNang = "giai-quyet-don-thu";
                            sl2.Data = new List<Data>();
                            sl2.Data.Add(new Data("Cần ban hành", soLieuChung.CanBanHanhGXM));
                            sl2.Data.Add(new Data("Đã ban hành", soLieuChung.DaBanHanhGXM));
                            sl2.Data.Add(new Data("Tổng số", soLieuChung.CanBanHanhGXM + soLieuChung.DaBanHanhGXM));
                            Data.SoLieuCanhBao.Add(sl2);

                            SoLieuCanhBao sl3 = new SoLieuCanhBao();
                            sl3.TenCanhBao = "Cảnh báo cần ban hành quyết định giải quyết";
                            sl3.MaChucNang = "giai-quyet-don-thu";
                            sl3.Data = new List<Data>();
                            sl3.Data.Add(new Data("Cần ban hành", soLieuChung.CanBanHanhGQ));
                            sl3.Data.Add(new Data("Đã ban hành", soLieuChung.DaBanHanhGQ));
                            sl3.Data.Add(new Data("Tổng số", soLieuChung.CanBanHanhGQ + soLieuChung.DaBanHanhGQ));
                            Data.SoLieuCanhBao.Add(sl3);
                        }
                    }
                    else if (capHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode())
                    {
                        if (p.BanTiepDan ?? false)
                        {
                            if (p.RoleID == RoleEnum.LanhDao.GetHashCode())
                            {
                                SoLieuCanhBao sl2 = new SoLieuCanhBao();
                                sl2.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                                sl2.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                                sl2.Data = new List<Data>();
                                sl2.Data.Add(new Data("Cần phê duyệt", soLieuChung.CanPheDuyet));
                                sl2.Data.Add(new Data("Đã phê duyệt", soLieuChung.DaPheDuyet));
                                sl2.Data.Add(new Data("Tổng số", soLieuChung.CanPheDuyet + soLieuChung.DaPheDuyet));
                                Data.SoLieuCanhBao.Add(sl2);

                                SoLieuCanhBao sl3 = new SoLieuCanhBao();
                                sl3.TenCanhBao = "Cảnh báo cần trình dự thảo quyết định giao xác minh";
                                sl3.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                                sl3.Data = new List<Data>();
                                sl3.Data.Add(new Data("Cần trình dự thảo", soLieuChung.CanTrinhDuThao));
                                sl3.Data.Add(new Data("Đã trình dự thảo", soLieuChung.DaTrinhDuThao));
                                sl3.Data.Add(new Data("Tổng số", soLieuChung.CanTrinhDuThao + soLieuChung.DaTrinhDuThao));
                                Data.SoLieuCanhBao.Add(sl3);
                            }
                            else
                            {
                                SoLieuCanhBao sl2 = new SoLieuCanhBao();
                                sl2.TenCanhBao = "Cảnh báo đơn thư cần xử lý";
                                sl2.MaChucNang = "xu-ly-don-thu";
                                sl2.Data = new List<Data>();
                                sl2.Data.Add(new Data("Cần xử lý đơn", soLieuChung.CanXuLy));
                                sl2.Data.Add(new Data("Đã xử lý đơn", soLieuChung.DaXuLy));
                                sl2.Data.Add(new Data("Tổng số", soLieuChung.CanXuLy + soLieuChung.DaXuLy));
                                Data.SoLieuCanhBao.Add(sl2);

                                SoLieuCanhBao sl3 = new SoLieuCanhBao();
                                sl3.TenCanhBao = "Cảnh báo cần trình duyệt xử lý đơn";
                                sl3.MaChucNang = "xu-ly-don-thu";
                                sl3.Data = new List<Data>();
                                sl3.Data.Add(new Data("Cần trình kết quả xử lý đơn", soLieuChung.CanTrinhKetQua));
                                sl3.Data.Add(new Data("Đã trình kết quả xử lý", soLieuChung.DaTrinhKetQua));
                                sl3.Data.Add(new Data("Tổng số", soLieuChung.CanTrinhKetQua + soLieuChung.DaTrinhKetQua));
                                Data.SoLieuCanhBao.Add(sl3);
                            }
                        }
                        else
                        {
                            if (p.RoleID == 1)
                            {
                                if (SuDungQuyTrinhPhucTap)
                                {
                                    SoLieuCanhBao sl2 = new SoLieuCanhBao();
                                    sl2.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                                    sl2.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                                    sl2.Data = new List<Data>();
                                    sl2.Data.Add(new Data("Cần phê duyệt", soLieuChung.CanPheDuyet));
                                    sl2.Data.Add(new Data("Đã phê duyệt", soLieuChung.DaPheDuyet));
                                    sl2.Data.Add(new Data("Tổng số", soLieuChung.CanPheDuyet + soLieuChung.DaPheDuyet));
                                    Data.SoLieuCanhBao.Add(sl2);
                                }

                                SoLieuCanhBao sl3 = new SoLieuCanhBao();
                                sl3.TenCanhBao = "Cảnh báo cần giao xác minh";
                                sl3.MaChucNang = "giai-quyet-don-thu";
                                sl3.Data = new List<Data>();
                                sl3.Data.Add(new Data("Cần giao xác minh", soLieuChung.CanGiaoXacMinh));
                                sl3.Data.Add(new Data("Đã giao xác minh", soLieuChung.DaGiaoXacMinh));
                                sl3.Data.Add(new Data("Tổng số", soLieuChung.CanGiaoXacMinh + soLieuChung.DaGiaoXacMinh));
                                Data.SoLieuCanhBao.Add(sl3);

                                SoLieuCanhBao sl4 = new SoLieuCanhBao();
                                sl4.TenCanhBao = "Cảnh báo duyệt báo cáo xác minh";
                                sl4.MaChucNang = "giai-quyet-don-thu";
                                sl4.Data = new List<Data>();
                                sl4.Data.Add(new Data("Cần duyệt báo cáo xác minh", soLieuChung.CanDuyetBCXacMinh));
                                sl4.Data.Add(new Data("Đã duyệt", soLieuChung.DaDuyetBCXacMinh));
                                sl4.Data.Add(new Data("Tổng số", soLieuChung.CanDuyetBCXacMinh + soLieuChung.DaDuyetBCXacMinh));
                                Data.SoLieuCanhBao.Add(sl4);

                                SoLieuCanhBao sl5 = new SoLieuCanhBao();
                                sl5.TenCanhBao = "Cảnh báo cập nhật nội dung báo cáo, kết luận, quyết định giải quyết";
                                sl5.MaChucNang = "ban-hanh-qd";
                                sl5.Data = new List<Data>();
                                sl5.Data.Add(new Data("Cần cập nhật", soLieuChung.CanCapNhatBCQDKL));
                                sl5.Data.Add(new Data("Đã cập nhật", soLieuChung.DaCapNhatBCQDKL));
                                sl5.Data.Add(new Data("Tổng số", soLieuChung.CanCapNhatBCQDKL + soLieuChung.DaCapNhatBCQDKL));
                                Data.SoLieuCanhBao.Add(sl5);
                            }
                            else
                            {
                                SoLieuCanhBao sl2 = new SoLieuCanhBao();
                                sl2.TenCanhBao = "Cảnh báo đơn thư cần xử lý";
                                sl2.MaChucNang = "xu-ly-don-thu";
                                sl2.Data = new List<Data>();
                                sl2.Data.Add(new Data("Cần xử lý đơn", soLieuChung.CanXuLy));
                                sl2.Data.Add(new Data("Đã xử lý đơn", soLieuChung.DaXuLy));
                                sl2.Data.Add(new Data("Tổng số", soLieuChung.CanXuLy + soLieuChung.DaXuLy));
                                Data.SoLieuCanhBao.Add(sl2);

                                SoLieuCanhBao sl3 = new SoLieuCanhBao();
                                sl3.TenCanhBao = "Cảnh báo cần trình duyệt xử lý đơn";
                                sl3.MaChucNang = "xu-ly-don-thu";
                                sl3.Data = new List<Data>();
                                sl3.Data.Add(new Data("Cần trình kết quả xử lý đơn", soLieuChung.CanTrinhKetQua));
                                sl3.Data.Add(new Data("Đã trình kết quả xử lý", soLieuChung.DaTrinhKetQua));
                                sl3.Data.Add(new Data("Tổng số", soLieuChung.CanTrinhKetQua + soLieuChung.DaTrinhKetQua));
                                Data.SoLieuCanhBao.Add(sl3);

                                SoLieuCanhBao sl4 = new SoLieuCanhBao();
                                sl4.TenCanhBao = "Cảnh báo xác minh đơn thư";
                                sl4.MaChucNang = "giai-quyet-don-thu";
                                sl4.Data = new List<Data>();
                                sl4.Data.Add(new Data("Cần xác minh", soLieuChung.CanXMDonThu));
                                sl4.Data.Add(new Data("Đã xác minh:", soLieuChung.DaXMDonThu));
                                sl4.Data.Add(new Data("Tổng số", soLieuChung.CanXMDonThu + soLieuChung.DaXMDonThu));
                                Data.SoLieuCanhBao.Add(sl4);

                                SoLieuCanhBao sl6 = new SoLieuCanhBao();
                                sl6.TenCanhBao = "Cảnh báo cần cập nhật kết quản thi hành";
                                sl6.MaChucNang = "thi-hanh";
                                sl6.Data = new List<Data>();
                                sl6.Data.Add(new Data("Cần cập nhật kết quả thi hành:", soLieuChung.CanThiHanh));
                                sl6.Data.Add(new Data("Đã thi hành", soLieuChung.DaThiHanh));
                                sl6.Data.Add(new Data("Tổng số", soLieuChung.CanThiHanh + soLieuChung.DaThiHanh));
                                Data.SoLieuCanhBao.Add(sl6);
                            }
                        }

                    }
                    else if (capHanhChinh == EnumCapHanhChinh.CapUBNDXa.GetHashCode())
                    {
                        if (p.RoleID == 1)
                        {
                            //var soLieu = _DashBoardBUS.DashBoard_CanhBaoPheDuyetKetQuaXuLy(p);
                            //var soLieuGQD = _DashBoardBUS.DashBoard_CanhBaoGiaiQuyetDon(p);

                            //SoLieuCanhBao sl1 = new SoLieuCanhBao();
                            //sl1.TenCanhBao = "Cảnh báo vụ việc đến hạn giải quyết";
                            //sl1.MaChucNang = "giai-quyet-don-thu";
                            ////sl1.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                            //sl1.Data = new List<Data>();
                            //sl1.Data.Add(new Data("Quá hạn", soLieu.QuaHan));
                            //sl1.Data.Add(new Data("Đến hạn", soLieu.DenHan));
                            //sl1.Data.Add(new Data("Chưa đến hạn", soLieu.ChuaDenHan));
                            //sl1.Data.Add(new Data("Tổng số", soLieu.QuaHan + soLieu.DenHan + soLieu.ChuaDenHan));
                            //Data.SoLieuCanhBao.Add(sl1);

                            if (SuDungQuyTrinhPhucTap)
                            {
                                SoLieuCanhBao sl2 = new SoLieuCanhBao();
                                sl2.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                                sl2.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                                sl2.Data = new List<Data>();
                                sl2.Data.Add(new Data("Cần phê duyệt", soLieuChung.CanPheDuyet));
                                sl2.Data.Add(new Data("Đã phê duyệt", soLieuChung.DaPheDuyet));
                                sl2.Data.Add(new Data("Tổng số", soLieuChung.CanPheDuyet + soLieuChung.DaPheDuyet));
                                Data.SoLieuCanhBao.Add(sl2);
                            }

                            SoLieuCanhBao sl3 = new SoLieuCanhBao();
                            sl3.TenCanhBao = "Cảnh báo cần giao xác minh";
                            sl3.MaChucNang = "giai-quyet-don-thu";
                            sl3.Data = new List<Data>();
                            sl3.Data.Add(new Data("Cần giao xác minh", soLieuChung.CanGiaoXacMinh));
                            sl3.Data.Add(new Data("Đã giao xác minh", soLieuChung.DaGiaoXacMinh));
                            sl3.Data.Add(new Data("Tổng số", soLieuChung.CanGiaoXacMinh + soLieuChung.DaGiaoXacMinh));
                            Data.SoLieuCanhBao.Add(sl3);

                            SoLieuCanhBao sl4 = new SoLieuCanhBao();
                            sl4.TenCanhBao = "Cảnh báo duyệt báo cáo xác minh";
                            sl4.MaChucNang = "giai-quyet-don-thu";
                            sl4.Data = new List<Data>();
                            sl4.Data.Add(new Data("Cần duyệt báo cáo xác minh", soLieuChung.CanDuyetBCXacMinh));
                            sl4.Data.Add(new Data("Đã duyệt", soLieuChung.DaDuyetBCXacMinh));
                            sl4.Data.Add(new Data("Tổng số", soLieuChung.CanDuyetBCXacMinh + soLieuChung.DaDuyetBCXacMinh));
                            Data.SoLieuCanhBao.Add(sl4);

                        }

                        else
                        {
                            SoLieuCanhBao sl2 = new SoLieuCanhBao();
                            sl2.TenCanhBao = "Cảnh báo đơn thư cần xử lý";
                            sl2.MaChucNang = "xu-ly-don-thu";
                            sl2.Data = new List<Data>();
                            sl2.Data.Add(new Data("Cần xử lý đơn", soLieuChung.CanXuLy));
                            sl2.Data.Add(new Data("Đã xử lý đơn", soLieuChung.DaXuLy));
                            sl2.Data.Add(new Data("Tổng số", soLieuChung.CanXuLy + soLieuChung.DaXuLy));
                            Data.SoLieuCanhBao.Add(sl2);

                            SoLieuCanhBao sl3 = new SoLieuCanhBao();
                            sl3.TenCanhBao = "Cảnh báo cần trình duyệt xử lý đơn";
                            sl3.MaChucNang = "xu-ly-don-thu";
                            sl3.Data = new List<Data>();
                            sl3.Data.Add(new Data("Cần trình kết quả xử lý đơn", soLieuChung.CanTrinhKetQua));
                            sl3.Data.Add(new Data("Đã trình kết quả xử lý", soLieuChung.DaTrinhKetQua));
                            sl3.Data.Add(new Data("Tổng số", soLieuChung.CanTrinhKetQua + soLieuChung.DaTrinhKetQua));
                            Data.SoLieuCanhBao.Add(sl3);

                            SoLieuCanhBao sl5 = new SoLieuCanhBao();
                            sl5.TenCanhBao = "Cảnh báo cập nhật nội dung báo cáo, kết luận, quyết định giải quyết";
                            sl5.MaChucNang = "ban-hanh-qd";
                            sl5.Data = new List<Data>();
                            sl5.Data.Add(new Data("Cần cập nhật", soLieuChung.CanCapNhatBCQDKL));
                            sl5.Data.Add(new Data("Đã cập nhật", soLieuChung.DaCapNhatBCQDKL));
                            sl5.Data.Add(new Data("Tổng số", soLieuChung.CanCapNhatBCQDKL + soLieuChung.DaCapNhatBCQDKL));
                            Data.SoLieuCanhBao.Add(sl5);

                            SoLieuCanhBao sl6 = new SoLieuCanhBao();
                            sl6.TenCanhBao = "Cảnh báo cần cập nhật kết quản thi hành";
                            sl6.MaChucNang = "thi-hanh";
                            sl6.Data = new List<Data>();
                            sl6.Data.Add(new Data("Cần cập nhật kết quả thi hành:", soLieuChung.CanThiHanh));
                            sl6.Data.Add(new Data("Đã thi hành", soLieuChung.DaThiHanh));
                            sl6.Data.Add(new Data("Tổng số", soLieuChung.CanThiHanh + soLieuChung.DaThiHanh));
                            Data.SoLieuCanhBao.Add(sl6);
                        }
                    }
                    else
                    {
                        if (p.RoleID == 1)
                        {
                            var soLieu = _DashBoardBUS.DashBoard_CanhBaoPheDuyetKetQuaXuLy(p);

                            SoLieuCanhBao sl1 = new SoLieuCanhBao();
                            sl1.TenCanhBao = "Cảnh báo vụ việc đến hạn giải quyết";
                            sl1.MaChucNang = "giai-quyet-don-thu";
                            sl1.Data = new List<Data>();
                            sl1.Data.Add(new Data("Quá hạn", soLieu.QuaHan));
                            sl1.Data.Add(new Data("Đến hạn", soLieu.DenHan));
                            sl1.Data.Add(new Data("Chưa đến hạn", soLieu.ChuaDenHan));
                            sl1.Data.Add(new Data("Tổng số", soLieu.QuaHan + soLieu.DenHan + soLieu.ChuaDenHan));
                            Data.SoLieuCanhBao.Add(sl1);

                            SoLieuCanhBao sl2 = new SoLieuCanhBao();
                            sl2.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                            sl2.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                            sl2.Data = new List<Data>();
                            sl2.Data.Add(new Data("Cần phê duyệt", soLieu.CanPheDuyet));
                            sl2.Data.Add(new Data("Đã phê duyệt", soLieu.DaPheDuyet));
                            sl2.Data.Add(new Data("Tổng số", soLieu.CanPheDuyet + soLieu.DaPheDuyet));
                            Data.SoLieuCanhBao.Add(sl2);

                            SoLieuCanhBao sl3 = new SoLieuCanhBao();
                            sl3.TenCanhBao = "Cảnh báo cần trình dự thảo quyết định giao xác minh";
                            sl3.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                            sl3.Data = new List<Data>();
                            sl3.Data.Add(new Data("Cần trình dự thảo", soLieu.CanTrinhDuThao));
                            sl3.Data.Add(new Data("Đã trình dự thảo", soLieu.DaTrinhDuThao));
                            sl3.Data.Add(new Data("Tổng số", soLieu.CanTrinhDuThao + soLieu.DaTrinhDuThao));
                            Data.SoLieuCanhBao.Add(sl3);
                        }
                        else
                        {
                            var soLieu = _DashBoardBUS.DashBoard_CanhBaoXuLyDon(p);
                            var soLieuGXM = _DashBoardBUS.DashBoard_CanhBaoCapNhatGiaoXacMinh(p);
                            var soLieuGQD = _DashBoardBUS.DashBoard_CanhBaoCapNhatQuyetDinhGiaiQuyet(p);

                            SoLieuCanhBao sl1 = new SoLieuCanhBao();
                            sl1.TenCanhBao = "Cảnh báo vụ việc đến hạn giải quyết";
                            sl1.MaChucNang = "giai-quyet-don-thu";
                            sl1.Data = new List<Data>();
                            sl1.Data.Add(new Data("Quá hạn", soLieu.QuaHan));
                            sl1.Data.Add(new Data("Đến hạn", soLieu.DenHan));
                            sl1.Data.Add(new Data("Chưa đến hạn", soLieu.ChuaDenHan));
                            sl1.Data.Add(new Data("Tổng số", soLieu.QuaHan + soLieu.DenHan + soLieu.ChuaDenHan));
                            Data.SoLieuCanhBao.Add(sl1);

                            SoLieuCanhBao sl2 = new SoLieuCanhBao();
                            sl2.TenCanhBao = "Cảnh báo đơn thư cần xử lý";
                            sl2.MaChucNang = "xu-ly-don-thu";
                            sl2.Data = new List<Data>();
                            sl2.Data.Add(new Data("Cần xử lý đơn", soLieu.CanXuLy));
                            sl2.Data.Add(new Data("Đã xử lý đơn", soLieu.DaXuLy));
                            sl2.Data.Add(new Data("Tổng số", soLieu.CanXuLy + soLieu.DaXuLy));
                            Data.SoLieuCanhBao.Add(sl2);

                            SoLieuCanhBao sl3 = new SoLieuCanhBao();
                            sl3.TenCanhBao = "Cảnh báo cần trình duyệt xử lý đơn";
                            sl3.MaChucNang = "xu-ly-don-thu";
                            sl3.Data = new List<Data>();
                            sl3.Data.Add(new Data("Cần trình kết quả xử lý đơn", soLieu.CanTrinhKetQua));
                            sl3.Data.Add(new Data("Đã trình kết quả xử lý", soLieu.DaTrinhKetQua));
                            sl3.Data.Add(new Data("Tổng số", soLieu.CanTrinhKetQua + soLieu.DaTrinhKetQua));
                            Data.SoLieuCanhBao.Add(sl3);

                            SoLieuCanhBao sl4 = new SoLieuCanhBao();
                            sl4.TenCanhBao = "Cảnh báo cập nhật nội dung quyết định giao xác minh";
                            sl4.MaChucNang = "giao-xac-minh";
                            sl4.Data = new List<Data>();
                            sl4.Data.Add(new Data("Cần cập nhật", soLieuGXM.CanCapNhat));
                            sl4.Data.Add(new Data("Đã cập nhật:", soLieuGXM.DaCapNhat));
                            sl4.Data.Add(new Data("Tổng số", soLieuGXM.CanCapNhat + soLieuGXM.DaCapNhat));
                            Data.SoLieuCanhBao.Add(sl4);

                            SoLieuCanhBao sl5 = new SoLieuCanhBao();
                            sl5.TenCanhBao = "Cảnh báo cập nhật nội dung báo cáo, kết luận, quyết định giải quyết";
                            sl5.MaChucNang = "ban-hanh-qd";
                            sl5.Data = new List<Data>();
                            sl5.Data.Add(new Data("Cần cấp nhật", soLieuGQD.CanCapNhat));
                            sl5.Data.Add(new Data("Đã cập nhật", soLieuGQD.DaCapNhat));
                            sl5.Data.Add(new Data("Tổng số", soLieuGQD.CanCapNhat + soLieuGQD.DaCapNhat));
                            Data.SoLieuCanhBao.Add(sl5);
                        }
                    }


                    //if ((p.CapID == CapQuanLy.CapUBNDTinh.GetHashCode() || p.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()) && p.ChuTichUBND == 1)
                    //{
                    //    var soLieu = _DashBoardBUS.DashBoard_ChuTichUBND(p);

                    //    SoLieuCanhBao sl1 = new SoLieuCanhBao();
                    //    sl1.TenCanhBao = "Cảnh báo vụ việc đến hạn giải quyết";
                    //    sl1.MaChucNang = "giai-quyet-don-thu";
                    //    sl1.Data = new List<Data>();
                    //    sl1.Data.Add(new Data("Quá hạn", soLieu.QuaHanBanHanh));
                    //    sl1.Data.Add(new Data("Đến hạn", soLieu.DenHanBanHanh));
                    //    sl1.Data.Add(new Data("Chưa đến hạn", soLieu.ChuaDenHanBanHanh));
                    //    sl1.Data.Add(new Data("Tổng số", soLieu.QuaHanBanHanh + soLieu.DenHanBanHanh + soLieu.ChuaDenHanBanHanh));
                    //    Data.SoLieuCanhBao.Add(sl1);

                    //    SoLieuCanhBao sl2 = new SoLieuCanhBao();
                    //    sl2.TenCanhBao = "Cảnh báo cần ban hành quyết định giao xác minh";
                    //    sl2.MaChucNang = "giai-quyet-don-thu";
                    //    sl2.Data = new List<Data>();
                    //    sl2.Data.Add(new Data("Cần ban hành", soLieu.CanBanHanhGXM));
                    //    sl2.Data.Add(new Data("Đã ban hành", soLieu.DaBanHanhGXM));
                    //    sl2.Data.Add(new Data("Tổng số", soLieu.CanBanHanhGXM + soLieu.DaBanHanhGXM));
                    //    Data.SoLieuCanhBao.Add(sl2);

                    //    SoLieuCanhBao sl3 = new SoLieuCanhBao();
                    //    sl3.TenCanhBao = "Cảnh báo cần ban hành quyết định giải quyết";
                    //    sl3.MaChucNang = "giai-quyet-don-thu";
                    //    sl3.Data = new List<Data>();
                    //    sl3.Data.Add(new Data("Cần ban hành", soLieu.CanBanHanhGQ));
                    //    sl3.Data.Add(new Data("Đã ban hành", soLieu.DaBanHanhGQ));
                    //    sl3.Data.Add(new Data("Tổng số", soLieu.CanBanHanhGQ + soLieu.DaBanHanhGQ));
                    //    Data.SoLieuCanhBao.Add(sl3);
                    //}
                    //else if (p.CapID == CapQuanLy.CapUBNDTinh.GetHashCode() && p.BanTiepDan == true)
                    //{
                    //    if (p.RoleID == 1)
                    //    {
                    //        var soLieu = _DashBoardBUS.DashBoard_CanhBaoPheDuyetKetQuaXuLy(p);

                    //        SoLieuCanhBao sl1 = new SoLieuCanhBao();
                    //        sl1.TenCanhBao = "Cảnh báo vụ việc đến hạn giải quyết";
                    //        sl1.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                    //        sl1.Data = new List<Data>();
                    //        sl1.Data.Add(new Data("Quá hạn", soLieu.QuaHan));
                    //        sl1.Data.Add(new Data("Đến hạn", soLieu.DenHan));
                    //        sl1.Data.Add(new Data("Chưa đến hạn", soLieu.ChuaDenHan));
                    //        sl1.Data.Add(new Data("Tổng số", soLieu.QuaHan + soLieu.DenHan + soLieu.ChuaDenHan));
                    //        Data.SoLieuCanhBao.Add(sl1);

                    //        SoLieuCanhBao sl2 = new SoLieuCanhBao();
                    //        sl2.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                    //        sl2.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                    //        sl2.Data = new List<Data>();
                    //        sl2.Data.Add(new Data("Cần phê duyệt", soLieu.CanPheDuyet));
                    //        sl2.Data.Add(new Data("Đã phê duyệt", soLieu.DaPheDuyet));
                    //        sl2.Data.Add(new Data("Tổng số", soLieu.CanPheDuyet + soLieu.DaPheDuyet));
                    //        Data.SoLieuCanhBao.Add(sl2);

                    //        SoLieuCanhBao sl3 = new SoLieuCanhBao();
                    //        sl3.TenCanhBao = "Cảnh báo cần trình dự thảo quyết định giao xác minh";
                    //        sl3.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                    //        sl3.Data = new List<Data>();
                    //        sl3.Data.Add(new Data("Cần trình dự thảo", soLieu.CanTrinhDuThao));
                    //        sl3.Data.Add(new Data("Đã trình dự thảo", soLieu.DaTrinhDuThao));
                    //        sl3.Data.Add(new Data("Tổng số", soLieu.CanTrinhDuThao + soLieu.DaTrinhDuThao));
                    //        Data.SoLieuCanhBao.Add(sl3);
                    //    }
                    //    else
                    //    {
                    //        var soLieu = _DashBoardBUS.DashBoard_CanhBaoXuLyDon(p);
                    //        var soLieuGXM = _DashBoardBUS.DashBoard_CanhBaoCapNhatGiaoXacMinh(p);
                    //        var soLieuGQD = _DashBoardBUS.DashBoard_CanhBaoCapNhatQuyetDinhGiaiQuyet(p);

                    //        SoLieuCanhBao sl1 = new SoLieuCanhBao();
                    //        sl1.TenCanhBao = "Cảnh báo vụ việc đến hạn giải quyết";
                    //        sl1.MaChucNang = "xu-ly-don-thu";
                    //        sl1.Data = new List<Data>();
                    //        sl1.Data.Add(new Data("Quá hạn", soLieu.QuaHan));
                    //        sl1.Data.Add(new Data("Đến hạn", soLieu.DenHan));
                    //        sl1.Data.Add(new Data("Chưa đến hạn", soLieu.ChuaDenHan));
                    //        sl1.Data.Add(new Data("Tổng số", soLieu.QuaHan + soLieu.DenHan + soLieu.ChuaDenHan));
                    //        Data.SoLieuCanhBao.Add(sl1);

                    //        SoLieuCanhBao sl2 = new SoLieuCanhBao();
                    //        sl2.TenCanhBao = "Cảnh báo đơn thư cần xử lý";
                    //        sl2.MaChucNang = "xu-ly-don-thu";
                    //        sl2.Data = new List<Data>();
                    //        sl2.Data.Add(new Data("Cần xử lý đơn", soLieu.CanXuLy));
                    //        sl2.Data.Add(new Data("Đã xử lý đơn", soLieu.DaXuLy));
                    //        sl2.Data.Add(new Data("Tổng số", soLieu.CanXuLy + soLieu.DaXuLy));
                    //        Data.SoLieuCanhBao.Add(sl2);

                    //        SoLieuCanhBao sl3 = new SoLieuCanhBao();
                    //        sl3.TenCanhBao = "Cảnh báo cần trình duyệt xử lý đơn";
                    //        sl3.MaChucNang = "xu-ly-don-thu";
                    //        sl3.Data = new List<Data>();
                    //        sl3.Data.Add(new Data("Cần trình kết quả xử lý đơn", soLieu.CanTrinhKetQua));
                    //        sl3.Data.Add(new Data("Đã trình kết quả xử lý", soLieu.DaTrinhKetQua));
                    //        sl3.Data.Add(new Data("Tổng số", soLieu.CanTrinhKetQua + soLieu.DaTrinhKetQua));
                    //        Data.SoLieuCanhBao.Add(sl3);

                    //        SoLieuCanhBao sl4 = new SoLieuCanhBao();
                    //        sl4.TenCanhBao = "Cảnh báo cập nhật nội dung quyết định giao xác minh";
                    //        sl4.MaChucNang = "giao-xac-minh";
                    //        sl4.Data = new List<Data>();
                    //        sl4.Data.Add(new Data("Cần cập nhật", soLieuGXM.CanCapNhat));
                    //        sl4.Data.Add(new Data("Đã cập nhật:", soLieuGXM.DaCapNhat));
                    //        sl4.Data.Add(new Data("Tổng số", soLieuGXM.CanCapNhat + soLieuGXM.DaCapNhat));
                    //        Data.SoLieuCanhBao.Add(sl4);

                    //        SoLieuCanhBao sl5 = new SoLieuCanhBao();
                    //        sl5.TenCanhBao = "Cảnh báo cập nhật nội dung báo cáo, kết luận, quyết định giải quyết";
                    //        sl5.MaChucNang = "ban-hanh-qd";
                    //        sl5.Data = new List<Data>();
                    //        sl5.Data.Add(new Data("Cần cấp nhật", soLieuGQD.CanCapNhat));
                    //        sl5.Data.Add(new Data("Đã cập nhật", soLieuGQD.DaCapNhat));
                    //        sl5.Data.Add(new Data("Tổng số", soLieuGQD.CanCapNhat + soLieuGQD.DaCapNhat));
                    //        Data.SoLieuCanhBao.Add(sl5);
                    //    }
                    //}
                    //else if (p.CapID == CapQuanLy.CapSoNganh.GetHashCode() || laThanhTraTinh)
                    //{
                    //    if (p.RoleID == 1)
                    //    {
                    //        var soLieu = _DashBoardBUS.DashBoard_CanhBaoPheDuyetKetQuaXuLy(p);
                    //        var soLieuGQD = _DashBoardBUS.DashBoard_CanhBaoGiaiQuyetDon(p);

                    //        SoLieuCanhBao sl1 = new SoLieuCanhBao();
                    //        sl1.TenCanhBao = "Cảnh báo vụ việc đến hạn giải quyết";
                    //        sl1.MaChucNang = "giai-quyet-don-thu";
                    //        //sl1.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                    //        sl1.Data = new List<Data>();
                    //        sl1.Data.Add(new Data("Quá hạn", soLieu.QuaHan));
                    //        sl1.Data.Add(new Data("Đến hạn", soLieu.DenHan));
                    //        sl1.Data.Add(new Data("Chưa đến hạn", soLieu.ChuaDenHan));
                    //        sl1.Data.Add(new Data("Tổng số", soLieu.QuaHan + soLieu.DenHan + soLieu.ChuaDenHan));
                    //        Data.SoLieuCanhBao.Add(sl1);

                    //        if (SuDungQuyTrinhPhucTap)
                    //        {
                    //            SoLieuCanhBao sl2 = new SoLieuCanhBao();
                    //            sl2.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                    //            sl2.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                    //            sl2.Data = new List<Data>();
                    //            sl2.Data.Add(new Data("Cần phê duyệt", soLieu.CanPheDuyet));
                    //            sl2.Data.Add(new Data("Đã phê duyệt", soLieu.DaPheDuyet));
                    //            sl2.Data.Add(new Data("Tổng số", soLieu.CanPheDuyet + soLieu.DaPheDuyet));
                    //            Data.SoLieuCanhBao.Add(sl2);
                    //        }

                    //        SoLieuCanhBao sl3 = new SoLieuCanhBao();
                    //        sl3.TenCanhBao = "Cảnh báo cần giao xác minh";
                    //        sl3.MaChucNang = "giai-quyet-don-thu";
                    //        sl3.Data = new List<Data>();
                    //        sl3.Data.Add(new Data("Cần giao xác minh", soLieuGQD.CanGiaoXacMinh));
                    //        sl3.Data.Add(new Data("Đã giao xác minh", soLieuGQD.DaGiaoXacMinh));
                    //        sl3.Data.Add(new Data("Tổng số", soLieuGQD.CanGiaoXacMinh + soLieuGQD.DaGiaoXacMinh));
                    //        Data.SoLieuCanhBao.Add(sl3);

                    //        SoLieuCanhBao sl4 = new SoLieuCanhBao();
                    //        sl4.TenCanhBao = "Cảnh báo duyệt báo cáo xác minh";
                    //        sl4.MaChucNang = "giai-quyet-don-thu";
                    //        sl4.Data = new List<Data>();
                    //        sl4.Data.Add(new Data("Cần duyệt báo cáo xác minh", soLieuGQD.CanDuyetBCXacMinh));
                    //        sl4.Data.Add(new Data("Đã duyệt", soLieuGQD.DaDuyetBCXacMinh));
                    //        sl4.Data.Add(new Data("Tổng số", soLieuGQD.CanDuyetBCXacMinh + soLieuGQD.DaDuyetBCXacMinh));
                    //        Data.SoLieuCanhBao.Add(sl4);

                    //        SoLieuCanhBao sl5 = new SoLieuCanhBao();
                    //        sl5.TenCanhBao = "Cảnh báo cần trình báo cáo xác minh";
                    //        sl5.MaChucNang = "giai-quyet-don-thu";
                    //        sl5.Data = new List<Data>();
                    //        sl5.Data.Add(new Data("Cần trình báo cáo xác minh", soLieuGQD.CanDuyetBCXacMinh));
                    //        sl5.Data.Add(new Data("Đã trình", soLieuGQD.DaDuyetBCXacMinh));
                    //        sl5.Data.Add(new Data("Tổng số", soLieuGQD.CanDuyetBCXacMinh + soLieuGQD.DaDuyetBCXacMinh));
                    //        Data.SoLieuCanhBao.Add(sl5);

                    //        SoLieuCanhBao sl6 = new SoLieuCanhBao();
                    //        sl6.TenCanhBao = "Cảnh báo cần ban hành quyết định giải quyết";
                    //        sl6.MaChucNang = "giai-quyet-don-thu";
                    //        sl6.Data = new List<Data>();
                    //        sl6.Data.Add(new Data("Cần ban hành quyết định giải quyết", soLieuGQD.CanBanHanhQDGQ));
                    //        sl6.Data.Add(new Data("Đã trình", soLieuGQD.DaBanHanhQDGQ));
                    //        sl6.Data.Add(new Data("Tổng số", soLieuGQD.CanBanHanhQDGQ + soLieuGQD.DaBanHanhQDGQ));
                    //        Data.SoLieuCanhBao.Add(sl6);
                    //    }
                    //    else if (p.RoleID == 2)
                    //    {
                    //        var soLieu = _DashBoardBUS.DashBoard_CanhBaoPheDuyetKetQuaXuLy(p);
                    //        var soLieuGQD = _DashBoardBUS.DashBoard_CanhBaoGiaiQuyetDon(p);

                    //        SoLieuCanhBao sl1 = new SoLieuCanhBao();
                    //        sl1.TenCanhBao = "Cảnh báo vụ việc đến hạn giải quyết";
                    //        sl1.MaChucNang = "giai-quyet-don-thu";
                    //        //sl1.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                    //        sl1.Data = new List<Data>();
                    //        sl1.Data.Add(new Data("Quá hạn", soLieu.QuaHan));
                    //        sl1.Data.Add(new Data("Đến hạn", soLieu.DenHan));
                    //        sl1.Data.Add(new Data("Chưa đến hạn", soLieu.ChuaDenHan));
                    //        sl1.Data.Add(new Data("Tổng số", soLieu.QuaHan + soLieu.DenHan + soLieu.ChuaDenHan));
                    //        Data.SoLieuCanhBao.Add(sl1);

                    //        //SoLieuCanhBao sl2 = new SoLieuCanhBao();
                    //        //sl2.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                    //        //sl2.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                    //        //sl2.Data = new List<Data>();
                    //        //sl2.Data.Add(new Data("Cần phê duyệt", soLieu.CanPheDuyet));
                    //        //sl2.Data.Add(new Data("Đã phê duyệt", soLieu.DaPheDuyet));
                    //        //sl2.Data.Add(new Data("Tổng số", soLieu.CanPheDuyet + soLieu.DaPheDuyet));
                    //        //Data.SoLieuCanhBao.Add(sl2);

                    //        if (SuDungQuyTrinhPhucTap)
                    //        {
                    //            SoLieuCanhBao sl2 = new SoLieuCanhBao();
                    //            sl2.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                    //            sl2.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                    //            sl2.Data = new List<Data>();
                    //            sl2.Data.Add(new Data("Cần phê duyệt", soLieu.CanPheDuyet));
                    //            sl2.Data.Add(new Data("Đã phê duyệt", soLieu.DaPheDuyet));
                    //            sl2.Data.Add(new Data("Tổng số", soLieu.CanPheDuyet + soLieu.DaPheDuyet));
                    //            Data.SoLieuCanhBao.Add(sl2);
                    //        }

                    //        SoLieuCanhBao sl3 = new SoLieuCanhBao();
                    //        sl3.TenCanhBao = "Cảnh báo cần giao xác minh";
                    //        sl3.MaChucNang = "giai-quyet-don-thu";
                    //        sl3.Data = new List<Data>();
                    //        sl3.Data.Add(new Data("Cần giao xác minh", soLieuGQD.CanGiaoXacMinh));
                    //        sl3.Data.Add(new Data("Đã giao xác minh", soLieuGQD.DaGiaoXacMinh));
                    //        sl3.Data.Add(new Data("Tổng số", soLieuGQD.CanGiaoXacMinh + soLieuGQD.DaGiaoXacMinh));
                    //        Data.SoLieuCanhBao.Add(sl3);

                    //        SoLieuCanhBao sl4 = new SoLieuCanhBao();
                    //        sl4.TenCanhBao = "Cảnh báo duyệt báo cáo xác minh";
                    //        sl4.MaChucNang = "giai-quyet-don-thu";
                    //        sl4.Data = new List<Data>();
                    //        sl4.Data.Add(new Data("Cần duyệt báo cáo xác minh", soLieuGQD.CanDuyetBCXacMinh));
                    //        sl4.Data.Add(new Data("Đã duyệt", soLieuGQD.DaDuyetBCXacMinh));
                    //        sl4.Data.Add(new Data("Tổng số", soLieuGQD.CanDuyetBCXacMinh + soLieuGQD.DaDuyetBCXacMinh));
                    //        Data.SoLieuCanhBao.Add(sl4);

                    //        SoLieuCanhBao sl5 = new SoLieuCanhBao();
                    //        sl5.TenCanhBao = "Cảnh báo cần trình báo cáo xác minh";
                    //        sl5.MaChucNang = "giai-quyet-don-thu";
                    //        sl5.Data = new List<Data>();
                    //        sl5.Data.Add(new Data("Cần trình báo cáo xác minh", soLieuGQD.CanDuyetBCXacMinh));
                    //        sl5.Data.Add(new Data("Đã trình", soLieuGQD.DaDuyetBCXacMinh));
                    //        sl5.Data.Add(new Data("Tổng số", soLieuGQD.CanDuyetBCXacMinh + soLieuGQD.DaDuyetBCXacMinh));
                    //        Data.SoLieuCanhBao.Add(sl5);
                    //    }
                    //    else
                    //    {
                    //        var soLieu = _DashBoardBUS.DashBoard_CanhBaoPheDuyetKetQuaXuLy(p);

                    //        SoLieuCanhBao sl1 = new SoLieuCanhBao();
                    //        sl1.TenCanhBao = "Cảnh báo vụ việc đến hạn giải quyết";
                    //        sl1.MaChucNang = "giai-quyet-don-thu";
                    //        sl1.Data = new List<Data>();
                    //        sl1.Data.Add(new Data("Quá hạn", soLieu.QuaHan));
                    //        sl1.Data.Add(new Data("Đến hạn", soLieu.DenHan));
                    //        sl1.Data.Add(new Data("Chưa đến hạn", soLieu.ChuaDenHan));
                    //        sl1.Data.Add(new Data("Tổng số", soLieu.QuaHan + soLieu.DenHan + soLieu.ChuaDenHan));
                    //        Data.SoLieuCanhBao.Add(sl1);

                    //        SoLieuCanhBao sl5 = new SoLieuCanhBao();
                    //        sl5.TenCanhBao = "Cảnh báo cần trình báo cáo xác minh";
                    //        sl5.MaChucNang = "giai-quyet-don-thu";
                    //        sl5.Data = new List<Data>();
                    //        sl5.Data.Add(new Data("Cần trình báo cáo xác minh", soLieu.CanPheDuyet));
                    //        sl5.Data.Add(new Data("Đã trình", soLieu.DaPheDuyet));
                    //        sl5.Data.Add(new Data("Tổng số", soLieu.CanPheDuyet + soLieu.DaPheDuyet));
                    //        Data.SoLieuCanhBao.Add(sl5);

                    //        SoLieuCanhBao sl6 = new SoLieuCanhBao();
                    //        sl6.TenCanhBao = "Cảnh báo cần cập nhật kết quản thi hành";
                    //        sl6.MaChucNang = "thi-hanh";
                    //        sl6.Data = new List<Data>();
                    //        sl6.Data.Add(new Data("Cần cập nhật kết quả thi hành:", soLieu.CanPheDuyet));
                    //        sl6.Data.Add(new Data("Đã trình", soLieu.DaPheDuyet));
                    //        sl6.Data.Add(new Data("Tổng số", soLieu.CanPheDuyet + soLieu.DaPheDuyet));
                    //        Data.SoLieuCanhBao.Add(sl6);
                    //    }
                    //}
                    //else
                    //{
                    //    if (p.RoleID == 1)
                    //    {
                    //        var soLieu = _DashBoardBUS.DashBoard_CanhBaoPheDuyetKetQuaXuLy(p);

                    //        SoLieuCanhBao sl1 = new SoLieuCanhBao();
                    //        sl1.TenCanhBao = "Cảnh báo vụ việc đến hạn giải quyết";
                    //        sl1.MaChucNang = "giai-quyet-don-thu";
                    //        sl1.Data = new List<Data>();
                    //        sl1.Data.Add(new Data("Quá hạn", soLieu.QuaHan));
                    //        sl1.Data.Add(new Data("Đến hạn", soLieu.DenHan));
                    //        sl1.Data.Add(new Data("Chưa đến hạn", soLieu.ChuaDenHan));
                    //        sl1.Data.Add(new Data("Tổng số", soLieu.QuaHan + soLieu.DenHan + soLieu.ChuaDenHan));
                    //        Data.SoLieuCanhBao.Add(sl1);

                    //        SoLieuCanhBao sl2 = new SoLieuCanhBao();
                    //        sl2.TenCanhBao = "Cảnh báo cần phê duyệt kết quả xử lý";
                    //        sl2.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                    //        sl2.Data = new List<Data>();
                    //        sl2.Data.Add(new Data("Cần phê duyệt", soLieu.CanPheDuyet));
                    //        sl2.Data.Add(new Data("Đã phê duyệt", soLieu.DaPheDuyet));
                    //        sl2.Data.Add(new Data("Tổng số", soLieu.CanPheDuyet + soLieu.DaPheDuyet));
                    //        Data.SoLieuCanhBao.Add(sl2);

                    //        SoLieuCanhBao sl3 = new SoLieuCanhBao();
                    //        sl3.TenCanhBao = "Cảnh báo cần trình dự thảo quyết định giao xác minh";
                    //        sl3.MaChucNang = "phe-duyet-ket-qua-xu-ly";
                    //        sl3.Data = new List<Data>();
                    //        sl3.Data.Add(new Data("Cần trình dự thảo", soLieu.CanTrinhDuThao));
                    //        sl3.Data.Add(new Data("Đã trình dự thảo", soLieu.DaTrinhDuThao));
                    //        sl3.Data.Add(new Data("Tổng số", soLieu.CanTrinhDuThao + soLieu.DaTrinhDuThao));
                    //        Data.SoLieuCanhBao.Add(sl3);
                    //    }
                    //    else
                    //    {
                    //        var soLieu = _DashBoardBUS.DashBoard_CanhBaoXuLyDon(p);
                    //        var soLieuGXM = _DashBoardBUS.DashBoard_CanhBaoCapNhatGiaoXacMinh(p);
                    //        var soLieuGQD = _DashBoardBUS.DashBoard_CanhBaoCapNhatQuyetDinhGiaiQuyet(p);

                    //        SoLieuCanhBao sl1 = new SoLieuCanhBao();
                    //        sl1.TenCanhBao = "Cảnh báo vụ việc đến hạn giải quyết";
                    //        sl1.MaChucNang = "giai-quyet-don-thu";
                    //        sl1.Data = new List<Data>();
                    //        sl1.Data.Add(new Data("Quá hạn", soLieu.QuaHan));
                    //        sl1.Data.Add(new Data("Đến hạn", soLieu.DenHan));
                    //        sl1.Data.Add(new Data("Chưa đến hạn", soLieu.ChuaDenHan));
                    //        sl1.Data.Add(new Data("Tổng số", soLieu.QuaHan + soLieu.DenHan + soLieu.ChuaDenHan));
                    //        Data.SoLieuCanhBao.Add(sl1);

                    //        SoLieuCanhBao sl2 = new SoLieuCanhBao();
                    //        sl2.TenCanhBao = "Cảnh báo đơn thư cần xử lý";
                    //        sl2.MaChucNang = "xu-ly-don-thu";
                    //        sl2.Data = new List<Data>();
                    //        sl2.Data.Add(new Data("Cần xử lý đơn", soLieu.CanXuLy));
                    //        sl2.Data.Add(new Data("Đã xử lý đơn", soLieu.DaXuLy));
                    //        sl2.Data.Add(new Data("Tổng số", soLieu.CanXuLy + soLieu.DaXuLy));
                    //        Data.SoLieuCanhBao.Add(sl2);

                    //        SoLieuCanhBao sl3 = new SoLieuCanhBao();
                    //        sl3.TenCanhBao = "Cảnh báo cần trình duyệt xử lý đơn";
                    //        sl3.MaChucNang = "xu-ly-don-thu";
                    //        sl3.Data = new List<Data>();
                    //        sl3.Data.Add(new Data("Cần trình kết quả xử lý đơn", soLieu.CanTrinhKetQua));
                    //        sl3.Data.Add(new Data("Đã trình kết quả xử lý", soLieu.DaTrinhKetQua));
                    //        sl3.Data.Add(new Data("Tổng số", soLieu.CanTrinhKetQua + soLieu.DaTrinhKetQua));
                    //        Data.SoLieuCanhBao.Add(sl3);

                    //        SoLieuCanhBao sl4 = new SoLieuCanhBao();
                    //        sl4.TenCanhBao = "Cảnh báo cập nhật nội dung quyết định giao xác minh";
                    //        sl4.MaChucNang = "giao-xac-minh";
                    //        sl4.Data = new List<Data>();
                    //        sl4.Data.Add(new Data("Cần cập nhật", soLieuGXM.CanCapNhat));
                    //        sl4.Data.Add(new Data("Đã cập nhật:", soLieuGXM.DaCapNhat));
                    //        sl4.Data.Add(new Data("Tổng số", soLieuGXM.CanCapNhat + soLieuGXM.DaCapNhat));
                    //        Data.SoLieuCanhBao.Add(sl4);

                    //        SoLieuCanhBao sl5 = new SoLieuCanhBao();
                    //        sl5.TenCanhBao = "Cảnh báo cập nhật nội dung báo cáo, kết luận, quyết định giải quyết";
                    //        sl5.MaChucNang = "ban-hanh-qd";
                    //        sl5.Data = new List<Data>();
                    //        sl5.Data.Add(new Data("Cần cấp nhật", soLieuGQD.CanCapNhat));
                    //        sl5.Data.Add(new Data("Đã cập nhật", soLieuGQD.DaCapNhat));
                    //        sl5.Data.Add(new Data("Tổng số", soLieuGQD.CanCapNhat + soLieuGQD.DaCapNhat));
                    //        Data.SoLieuCanhBao.Add(sl5);
                    //    }
                    //}

                    //var Data = _DashBoardBUS.GetDuLieuDashBoard(p);
                    base.Status = 1;
                    base.Data = Data;
                    //base.Data = new DashBoardModel();
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
        [Route("GetCoQuanByPhamViID")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetCoQuanByPhamViID([FromQuery] string PhamViID)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {

                    //var optionToanTinh = $('<option value = "2">Toàn tỉnh</option>');
                    //var optionSo = $('<option value = "3">Cấp sở</option>');
                    //var optionHuyen = $('<option value = "4">Cấp huyện</option>');
                    //var optionToanHuyen = $('<option value = "4">Toàn huyện</option>');
                    //var optionXa = $('<option value = "5">Cấp xã</option>');
                    //mapping du lieu cũ
                    if (PhamViID == "12")
                    {
                        PhamViID = "2";
                    }
                    else if (PhamViID == "1")
                    {
                        PhamViID = "3";
                    }
                    else if (PhamViID == "2")
                    {
                        PhamViID = "4";
                    }
                    else if (PhamViID == "3")
                    {
                        PhamViID = "5";
                    }

                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var TinhID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    var Data = _DashBoardBUS.GetCoQuanByPhamViID(PhamViID, CapID, TinhID, HuyenID);
                    base.Status = 1;
                    base.Data = Data;
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
