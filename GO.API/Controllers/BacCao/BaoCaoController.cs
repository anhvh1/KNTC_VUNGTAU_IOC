using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Controllers;
using Com.Gosol.KNTC.API.Controllers.DanhMuc;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.BaoCao;
using Com.Gosol.KNTC.BUS.DanhMuc;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace GO.API.Controllers.BacCao
{
    [Route("api/v2/BaoCao")]
    [ApiController]
    public class BaoCaoController : BaseApiController
    {
        private BaoCaoBUS _BaoCaoBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public BaoCaoController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<BaoCaoController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._BaoCaoBUS = new BaoCaoBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [Route("DanhMucLoaiKhieuTo")]
        public IActionResult DanhMucLoaiKhieuTo(int? LoaiKhieuToID)
        {
            try
            {
                int TotalRow = 0;
                IList<LoaiKhieuToInfo> Data;
                Data = _BaoCaoBUS.DanhMucLoaiKhieuTo(LoaiKhieuToID ?? 0);
               
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
        [Route("GetListCap")]
        public IActionResult GetListCap()
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    List<CapInfo> listCap = new List<CapInfo>();
                    CapInfo capTinh = new CapInfo();
                    capTinh.CapID = 4;
                    capTinh.TenCap = "UBND Cấp Tỉnh";
                    listCap.Add(capTinh);

                    CapInfo capSo = new CapInfo();
                    capSo.CapID = 1;
                    capSo.TenCap = "Cấp Sở, Ngành";
                    listCap.Add(capSo);

                    CapInfo capHuyen = new CapInfo();
                    capHuyen.CapID = 2;
                    capHuyen.TenCap = "UBND Cấp Huyện";
                    listCap.Add(capHuyen);

                    CapInfo capXa = new CapInfo();
                    capXa.CapID = 3;
                    capXa.TenCap = "UBND Cấp Xã";
                    listCap.Add(capXa);

                    base.Status = 1;
                    base.Data = listCap;
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
        [Route("GetPhamVi")]
        public IActionResult GetPhamVi([FromQuery] int? Type)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    List<PhamViModel> list = new List<PhamViModel>();
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
                    if (Type == 2)
                    {
                        if ((listThanhTraTinh.Contains(CoQuanID) && RoleID == EnumChucVu.LanhDao.GetHashCode()) 
                        || (RoleID == (int)CapQuanLy.CapUBNDTinh && RoleID == EnumChucVu.LanhDao.GetHashCode()) 
                        || CapID == (int)CapQuanLy.CapTrungUong)
                        {
                            list.Add(new PhamViModel("Toàn tỉnh", 2));
                            list.Add(new PhamViModel("Trong đơn vị", 5));
                        }
                        else {
                            list.Add(new PhamViModel("Trong đơn vị", 5));
                        }

                    }
                    else if (Type == 3)
                    {
                        list.Add(new PhamViModel("Toàn tỉnh", 2));
                        list.Add(new PhamViModel("Cấp Sở", 3));
                        list.Add(new PhamViModel("Cấp Huyện", 4));
                    }
                    else if (Type == 4 || Type == 5 || Type == 6)
                    {
                        list.Add(new PhamViModel("Toàn tỉnh", 2));
                        list.Add(new PhamViModel("Cấp Huyện", 4));
                        list.Add(new PhamViModel("Trong đơn vị", 5));
                    }


                    base.Status = 1;
                    base.Data = list;
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
        [Route("GetCoQuanByPhamVi")]
        public IActionResult GetCoQuanByPhamVi([FromQuery] int? PhamViID)
        {
            try
            {
                List<CoQuanInfo> resultList = new List<CoQuanInfo>();
                if (PhamViID == 3)
                {
                    List<CoQuanInfo> cqList = new CoQuan().GetCoQuanByCap((int)CapQuanLy.CapSoNganh).ToList();         
                    foreach (CoQuanInfo cqInfo in cqList)
                    {
                        resultList.Add(cqInfo);
                    }
                
                }
                else if (PhamViID == 4)
                {
                    int TinhID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    List<HuyenInfo> huyenList = new Com.Gosol.KNTC.DAL.KNTC.Huyen().GetByTinh(TinhID).ToList();
                    foreach (var item in huyenList)
                    {
                        CoQuanInfo cq = new CoQuanInfo();
                        cq.CoQuanID = item.HuyenID;
                        cq.TenCoQuan = item.TenHuyen;
                        resultList.Add(cq);
                    }
                }

                base.Status = 1;
                base.Data = resultList;
                return base.GetActionResult();
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        [Route("GetDanhSachCoQuan")]
        public IActionResult GetDanhSachCoQuan()
        {
            try
            {
                IdentityHelper IdentityHelper = new IdentityHelper();
                IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                IdentityHelper.CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                IdentityHelper.TinhID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                IdentityHelper.HuyenID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                IdentityHelper.RoleID = EnumChucVu.LanhDao.GetHashCode();
                List<CoQuanInfo> Data;
                Data = _BaoCaoBUS.DanhSachCoQuan(IdentityHelper);
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

        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("TCD01")]
        public IActionResult TongHopKetQuaTiepCongDanThuongXuyenDinhKyVaDotXuat([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if(p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if(arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.TCD01(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
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
        [Route("TCD01_GetDSChiTietDonThu")]
        public IActionResult TCD01_GetDSChiTietDonThu([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                { 
                    if(p.CapID == null || p.CoQuanID == null) return base.GetActionResult();

                    string ContentRootPath = _host.ContentRootPath;
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                  
                    if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.TCD01_GetDSChiTietDonThu(ContentRootPath, p);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("TCD01_Excel")]
        public IActionResult TCD01_Excel([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.TCD01_XuatExcel(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
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

        //[HttpGet]
        ////[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        //[Route("TTR01")]
        //public IActionResult TongHopKetQuaThanhTraHanhChinh([FromQuery] BaseReportParams p)
        //{
        //    try
        //    {
        //        return CreateActionResult(false, "", EnumLogType.GetList, () =>
        //        {
        //            var Data = _BaoCaoBUS.TTR01(p);
        //            base.Status = Data.Status;
        //            base.Data = Data.Data;
        //            return base.GetActionResult();
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        base.Status = -1;
        //        base.GetActionResult();
        //        throw;
        //    }
        //}
 
        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("TCD02")]
        public IActionResult TongHopKetQuaPhanLoaiXuLyDonQuaTiepCongDan([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.TCD02(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
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
        [Route("TCD02_GetDSChiTietDonThu")]
        public IActionResult TCD02_GetDSChiTietDonThu([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.CapID == null || p.CoQuanID == null) return base.GetActionResult();
                    string ContentRootPath = _host.ContentRootPath;
                    //var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    //var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    //var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    //var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    //var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    //var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                    var Data = _BaoCaoBUS.TCD02_GetDSChiTietDonThu(ContentRootPath, p);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("TCD02_Excel")]
        public IActionResult TCD02_Excel([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.TCD02_XuatExcel(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("XLD01")]
        public IActionResult TongHopKetQuaXuLyDon([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.XLD01(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
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
        [Route("XLD01_GetDSChiTietDonThu")]
        public IActionResult XLD01_GetDSChiTietDonThu([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.CapID == null || p.CoQuanID == null) return base.GetActionResult();
                    string ContentRootPath = _host.ContentRootPath;      
                    if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                    var Data = _BaoCaoBUS.XLD01_GetDSChiTietDonThu(ContentRootPath, p);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("XLD01_Excel")]
        public IActionResult XLD01_Excel([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.XLD01_XuatExcel(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("XLD02")]
        public IActionResult TongHopKetQuaXuLyDonKhieuNai([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.XLD02(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
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
        [Route("XLD02_GetDSChiTietDonThu")]
        public IActionResult XLD02_GetDSChiTietDonThu([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.CapID == null || p.CoQuanID == null) return base.GetActionResult();
                    string ContentRootPath = _host.ContentRootPath;
                    //var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    //var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    //var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    //var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    //var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    //var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                    var Data = _BaoCaoBUS.XLD02_GetDSChiTietDonThu(ContentRootPath, p);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("XLD02_Excel")]
        public IActionResult XLD02_Excel([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.XLD02_XuatExcel(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("XLD03")]
        public IActionResult TongHopKetQuaXuLyDonToCao([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.XLD03(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
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
        [Route("XLD03_GetDSChiTietDonThu")]
        public IActionResult XLD03_GetDSChiTietDonThu([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.CapID == null || p.CoQuanID == null) return base.GetActionResult();
                    string ContentRootPath = _host.ContentRootPath;
                    //var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    //var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    //var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    //var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    //var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    //var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                    var Data = _BaoCaoBUS.XLD03_GetDSChiTietDonThu(ContentRootPath, p);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("XLD03_Excel")]
        public IActionResult XLD03_Excel([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.XLD03_XuatExcel(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("XLD04")]
        public IActionResult TongHopKetQuaXuLyDonKienNghiPhanAnh([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.XLD04(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
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
        [Route("XLD04_GetDSChiTietDonThu")]
        public IActionResult XLD04_GetDSChiTietDonThu([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.CapID == null || p.CoQuanID == null) return base.GetActionResult();
                    string ContentRootPath = _host.ContentRootPath;
                    //var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    //var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    //var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    //var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    //var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    //var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                    var Data = _BaoCaoBUS.XLD04_GetDSChiTietDonThu(ContentRootPath, p);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("XLD04_Excel")]
        public IActionResult XLD04_Excel([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.XLD04_XuatExcel(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("KQGQ01")]
        public IActionResult TongHopKetQuaGiaiQuyetThuocThamQuyen([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.KQGQ01(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.GetActionResult();
                base.Message = ex.Message;
                throw;
            }
        }

        [HttpGet]
        [Route("KQGQ01_GetDSChiTietDonThu")]
        public IActionResult KQGQ01_GetDSChiTietDonThu([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.CapID == null || p.CoQuanID == null) return base.GetActionResult();
                    string ContentRootPath = _host.ContentRootPath;
                    //var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    //var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    //var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    //var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    //var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    //var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                    var Data = _BaoCaoBUS.KQGQ01_GetDSChiTietDonThu(ContentRootPath, p);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("KQGQ01_Excel")]
        public IActionResult KQGQ01_Excel([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.KQGQ01_XuatExcel(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("KQGQ02")]
        public IActionResult TongHopKetQuaThiHanhQuyetDinhGiaiQuyetKhieuNai([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.KQGQ02(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
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
        [Route("KQGQ02_GetDSChiTietDonThu")]
        public IActionResult KQGQ02_GetDSChiTietDonThu([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.CapID == null || p.CoQuanID == null) return base.GetActionResult();
                    string ContentRootPath = _host.ContentRootPath;
                    //var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    //var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    //var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    //var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    //var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    //var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                    var Data = _BaoCaoBUS.KQGQ02_GetDSChiTietDonThu(ContentRootPath, p);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("KQGQ02_Excel")]
        public IActionResult KQGQ02_Excel([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.KQGQ02_XuatExcel(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("KQGQ03")]
        public IActionResult TongHopKetQuaGiaiQuyetToCaoThuocThamQuyen([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.KQGQ03(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
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
        [Route("KQGQ03_GetDSChiTietDonThu")]
        public IActionResult KQGQ03_GetDSChiTietDonThu([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.CapID == null || p.CoQuanID == null) return base.GetActionResult();
                    string ContentRootPath = _host.ContentRootPath;
                    //var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    //var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    //var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    //var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    //var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    //var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                    var Data = _BaoCaoBUS.KQGQ03_GetDSChiTietDonThu(ContentRootPath, p);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("KQGQ03_Excel")]
        public IActionResult KQGQ03_Excel([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.KQGQ03_XuatExcel(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("KQGQ04")]
        public IActionResult TongHopKetQuaThucHienKetLuanNoiDungToCao([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.KQGQ04(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
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
        [Route("KQGQ04_GetDSChiTietDonThu")]
        public IActionResult KQGQ04_GetDSChiTietDonThu([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.CapID == null || p.CoQuanID == null) return base.GetActionResult();
                    string ContentRootPath = _host.ContentRootPath;
                    //var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    //var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    //var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    //var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    //var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    //var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                    var Data = _BaoCaoBUS.KQGQ04_GetDSChiTietDonThu(ContentRootPath, p);
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("KQGQ04_Excel")]
        public IActionResult KQGQ04_Excel([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var TinhDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    var HuyenDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = _BaoCaoBUS.KQGQ04_XuatExcel(p, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID);
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("BaoCaoThongKe")]
        public IActionResult BaoCaoThongKe([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.TuNgay == null) p.TuNgay = DateTime.Now.Date;
                    if (p.DenNgay == null) p.DenNgay = DateTime.Now;

                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }

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
                    BaseResultModel Data = new BaseResultModel();

                    // BÁO CÁO TỔNG HỢP KẾT QUẢ GIẢI QUYẾT ĐƠN KIẾN NGHỊ, PHẢN ÁNH
                    if (p.Type == 1)
                    {
                        Data = _BaoCaoBUS.THKQGiaiQuyetDonKienNghiPhanAnh(p, ContentRootPath, IdentityHelper);
                    }
                    // BÁO CÁO THỐNG KÊ THEO LOẠI KHIẾU TỐ
                    else if (p.Type == 2)
                    {
                        Data = _BaoCaoBUS.BaoCaoThongKeTheoLoaiKhieuTo(p, ContentRootPath, IdentityHelper);
                    }
                    // BÁO CÁO THỐNG KÊ THEO CƠ QUAN CHUYỂN ĐƠN
                    else if (p.Type == 3)
                    {
                        if (p.CapBaoCao == 2)
                        {
                            Data = _BaoCaoBUS.ThongKeTheoCOQuanChuyenDon_GetDSCoQuanNhanDon(p, ContentRootPath, IdentityHelper);
                        }
                        else Data = _BaoCaoBUS.ThongKeTheoCOQuanChuyenDon(p, ContentRootPath, IdentityHelper);
                    }
                    // BÁO CÁO THỐNG KÊ THEO ĐỊA CHỈ CHỦ ĐƠN
                    else if (p.Type == 4)
                    {
                        Data = _BaoCaoBUS.ThongKeTheoDiaChiChuDon(p, ContentRootPath, IdentityHelper);
                    }
                    // BÁO CÁO THỐNG KÊ THEO NƠI PHÁT SINH
                    else if (p.Type == 5)
                    {
                        Data = _BaoCaoBUS.ThongKeTheoNoiPhatSinh(p, ContentRootPath, IdentityHelper);
                    }
                    // BÁO CÁO THỐNG KÊ VỤ VIỆC ĐÔNG NGƯỜI
                    else if (p.Type == 6)
                    {
                        Data = _BaoCaoBUS.ThongKeTheoVuViecDongNguoi(p, ContentRootPath, IdentityHelper);
                    }
                    // BÁO CÁO THỐNG KÊ RÚT ĐƠN
                    else if (p.Type == 7)
                    {
                        Data = _BaoCaoBUS.ThongKeTheoRutDon(p, ContentRootPath, IdentityHelper);
                    }
                    // BÁO CÁO TỔNG HỢP TÌNH HÌNH TIẾP DÂN, XỬ LÝ ĐƠN VÀ GIẢI QUYẾT ĐƠN THEO ĐƠN VỊ
                    else if (p.Type == 8)
                    {
                        Data = _BaoCaoBUS.TongHopTinhHinhTCD_XL_GQD(p, ContentRootPath, IdentityHelper);
                    }
                    // BÁO CÁO XỬ LÝ CÔNG VIỆC
                    else if (p.Type == 9)
                    {
                        Data = _BaoCaoBUS.BaoCaoXuLyCongViec(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 10)
                    {
                        Data = _BaoCaoBUS.ThongKeDonChuyenGiaiQuyet(p, ContentRootPath, IdentityHelper);
                    }
                    // THKQ TIẾP CÔNG DÂN THƯỜNG XUYÊN, ĐỊNH KỲ VÀ ĐỘT XUẤT
                    else if (p.Type == 11)
                    {
                        Data = _BaoCaoBUS.TCD01(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    // THKQ PHÂN LOẠI, XỬ LÝ ĐƠN QUA TIẾP CÔNG DÂN
                    else if (p.Type == 12)
                    {
                        Data = _BaoCaoBUS.TCD02(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    // THKQ XỬ LÝ ĐƠN
                    else if (p.Type == 13)
                    {
                        Data = _BaoCaoBUS.XLD01(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    // THKQ XỬ LÝ ĐƠN KHIẾU NẠI
                    else if (p.Type == 14)
                    {
                        Data = _BaoCaoBUS.XLD02(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    // THKQ XỬ LÝ ĐƠN TỐ CÁO
                    else if (p.Type == 15)
                    {
                        Data = _BaoCaoBUS.XLD03(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    // THKQ XỬ LÝ ĐƠN KIẾN NGHỊ, PHẢN ÁNH
                    else if (p.Type == 16)
                    {
                        Data = _BaoCaoBUS.XLD04(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    // THKQ GIẢI QUYẾT THUỘC THẨM QUYỀN
                    else if (p.Type == 17)
                    {
                        Data = Data = _BaoCaoBUS.KQGQ01(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    // THKQ THI HÀNH QUYẾT ĐỊNH GIẢI QUYẾT KHIẾU NẠI
                    else if (p.Type == 18)
                    {
                        Data = Data = _BaoCaoBUS.KQGQ02(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    // THKQ GIẢI QUYẾT TỐ CÁO THUỘC THẨM QUYỀN
                    else if (p.Type == 19)
                    {
                        Data = Data = _BaoCaoBUS.KQGQ03(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    // THKQ THỰC HIỆN KẾT LUẬN NỘI DUNG TỐ CÁO
                    else if (p.Type == 20)
                    {
                        Data = Data = _BaoCaoBUS.KQGQ04(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }

                    base.Status = Data.Status;
                    base.Data = Data.Data;
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("BaoCaoThongKe_DSChiTietDonThu")]
        public IActionResult BaoCaoThongKe_DSChiTietDonThu([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
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
                    BaseResultModel Data = new BaseResultModel();
                    if (p.TuNgay == null) p.TuNgay = DateTime.Now.Date;
                    if (p.DenNgay == null) p.DenNgay = DateTime.Now;
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    if (p.Type == 1)
                    {
                        Data = _BaoCaoBUS.THKQGiaiQuyetDonKienNghiPhanAnh_GetDSChiTietDonThu(ContentRootPath, p, IdentityHelper);
                    }
                    else if (p.Type == 2)
                    {
                        Data = _BaoCaoBUS.BaoCaoThongKeTheoLoaiKhieuTo_GetDSChiTietDonThu(ContentRootPath, p, IdentityHelper);
                    }
                    else if (p.Type == 3)
                    {
                        Data = _BaoCaoBUS.ThongKeTheoCOQuanChuyenDon_GetDSChiTietDonThu(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 4)
                    {
                        Data = _BaoCaoBUS.ThongKeTheoDiaChiChuDon_GetDSChiTietDonThu(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 5)
                    {
                        Data = _BaoCaoBUS.ThongKeTheoNoiPhatSinh_GetDSChiTietDonThu(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 6)
                    {
                        Data = _BaoCaoBUS.ThongKeTheoVuViecDongNguoi_GetDSChiTietDonThu(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 7)
                    {
                      
                    }
                    else if (p.Type == 8)
                    {
                        Data = _BaoCaoBUS.TongHopTinhHinhTCD_XL_GQD_GetDSChiTietDonThu(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 9)
                    {
                        Data = _BaoCaoBUS.BaoCaoXuLyCongViec_GetDSChiTietDonThu(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 10)
                    {
                        Data = _BaoCaoBUS.ThongKeDonChuyenGiaiQuyet_GetDSChiTietDonThu(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 11)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.TCD01_GetDSChiTietDonThu(ContentRootPath, p);
                    }
                    else if (p.Type == 12)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.TCD02_GetDSChiTietDonThu(ContentRootPath, p);
                    }
                    else if (p.Type == 13)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.XLD01_GetDSChiTietDonThu(ContentRootPath, p);
                    }
                    else if (p.Type == 14)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.XLD02_GetDSChiTietDonThu(ContentRootPath, p);
                    }
                    else if (p.Type == 15)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.XLD03_GetDSChiTietDonThu(ContentRootPath, p);
                    }
                    else if (p.Type == 16)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.XLD04_GetDSChiTietDonThu(ContentRootPath, p);
                    }
                    else if (p.Type == 17)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.KQGQ01_GetDSChiTietDonThu(ContentRootPath, p);
                    }
                    else if (p.Type == 18)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.KQGQ02_GetDSChiTietDonThu(ContentRootPath, p);
                    }
                    else if (p.Type == 19)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.KQGQ03_GetDSChiTietDonThu(ContentRootPath, p);
                    }
                    else if (p.Type == 20)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.KQGQ04_GetDSChiTietDonThu(ContentRootPath, p);
                    }

                    base.Status = Data.Status;
                    base.Data = Data.Data;
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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("BaoCaoThongKe_Excel")]
        public IActionResult BaoCaoThongKe_Excel([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    if (p.ListCapIDStr != null && p.ListCapIDStr.Length > 0)
                    {
                        var arr = p.ListCapIDStr.Split(",");
                        if (arr != null && arr.Length > 0)
                        {
                            p.ListCapID = new List<int>();
                            foreach (var item in arr)
                            {
                                p.ListCapID.Add(Utils.ConvertToInt32(item, 0));
                            }
                            p.ListCapID = p.ListCapID.Distinct().ToList();
                        }
                    }
                    if (p.TuNgay == null) p.TuNgay = DateTime.Now.Date;
                    if (p.DenNgay == null) p.DenNgay = DateTime.Now;

                    IdentityHelper IdentityHelper = new IdentityHelper();
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    IdentityHelper.CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    IdentityHelper.TinhID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    IdentityHelper.HuyenID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    IdentityHelper.XaID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "XaID").Value, 0);
                    IdentityHelper.RoleID = EnumChucVu.LanhDao.GetHashCode();
                    BaseResultModel Data = new BaseResultModel();
                    if (p.Type == 1)
                    {
                        Data = _BaoCaoBUS.THKQGiaiQuyetDonKienNghiPhanAnh_XuatExcel(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 2)
                    {
                        Data = _BaoCaoBUS.BaoCaoThongKeTheoLoaiKhieuTo_XuatExcel(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 3)
                    {
                        Data = _BaoCaoBUS.ThongKeTheoCOQuanChuyenDon_XuatExcel(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 4)
                    {
                        Data = _BaoCaoBUS.ThongKeTheoDiaChiChuDon_XuatExcel(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 5)
                    {
                        Data = _BaoCaoBUS.ThongKeTheoNoiPhatSinh_XuatExcel(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 6)
                    {
                        
                    }
                    else if (p.Type == 7)
                    {
                        Data = _BaoCaoBUS.ThongKeTheoRutDon_XuatExcel(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 8)
                    {
                        Data = _BaoCaoBUS.TongHopTinhHinhTCD_XL_GQD_XuatExcel(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 9)
                    {
                        Data = _BaoCaoBUS.BaoCaoXuLyCongViec_XuatExcel(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 10)
                    {
                        Data = _BaoCaoBUS.ThongKeDonChuyenGiaiQuyet_XuatExcel(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 11)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.TCD01_XuatExcel(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    else if (p.Type == 12)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.TCD02_XuatExcel(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    else if (p.Type == 13)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.XLD01_XuatExcel(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    else if (p.Type == 14)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.XLD02_XuatExcel(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    else if (p.Type == 15)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.XLD03_XuatExcel(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    else if (p.Type == 16)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.XLD04_XuatExcel(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    else if (p.Type == 17)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.KQGQ01_XuatExcel(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    else if (p.Type == 18)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.KQGQ02_XuatExcel(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    else if (p.Type == 19)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.KQGQ03_XuatExcel(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    else if (p.Type == 20)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.KQGQ04_XuatExcel(p, ContentRootPath, IdentityHelper.RoleID ?? 0, IdentityHelper.CapID ?? 0, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID ?? 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0);
                    }
                    
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
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
        [Route("DSChiTietDonThu_Excel")]
        public IActionResult DSChiTietDonThu_Excel([FromQuery] BaseReportParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    string ContentRootPath = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    var CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);

                    IdentityHelper IdentityHelper = new IdentityHelper(); 
                    IdentityHelper.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                    IdentityHelper.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    IdentityHelper.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    IdentityHelper.CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    IdentityHelper.TinhID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                    IdentityHelper.HuyenID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                    IdentityHelper.XaID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "XaID").Value, 0);
                    IdentityHelper.RoleID = EnumChucVu.LanhDao.GetHashCode();
                    if (p.TuNgay == null) p.TuNgay = DateTime.Now.Date;
                    if (p.DenNgay == null) p.DenNgay = DateTime.Now;
                    p.PageSize = 999999;
                    RoleID = EnumChucVu.LanhDao.GetHashCode();
                    var Data = new BaseResultModel();
                    if (p.Type == 1)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.THKQGiaiQuyetDonKienNghiPhanAnh_GetDSChiTietDonThu_XuatExcel(IdentityHelper, p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 2)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.BaoCaoThongKeTheoLoaiKhieuTo_GetDSChiTietDonThu_XuatExcel(IdentityHelper, p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 3)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.ThongKeTheoCOQuanChuyenDon_GetDSChiTietDonThu_XuatExcel(IdentityHelper, p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 4)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.ThongKeTheoDiaChiChuDon_GetDSChiTietDonThu_XuatExcel(IdentityHelper, p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 5)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.ThongKeTheoNoiPhatSinh_GetDSChiTietDonThu_XuatExcel(IdentityHelper, p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 6)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.ThongKeTheoVuViecDongNguoi_GetDSChiTietDonThu_XuatExcel(IdentityHelper, p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 7)
                    {

                    }
                    else if (p.Type == 8)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.TongHopTinhHinhTCD_XL_GQD_GetDSChiTietDonThu_XuatExcel(IdentityHelper, p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 9)
                    {
                        Data = _BaoCaoBUS.BaoCaoXuLyCongViec_GetDSChiTietDonThu_XuatExcel(p, ContentRootPath, IdentityHelper);
                    }
                    else if (p.Type == 10)
                    {
                        Data = _BaoCaoBUS.ThongKeDonChuyenGiaiQuyet_GetDSChiTietDonThu_XuatExcel(p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 11)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.TCD01_GetDSChiTietDonThu_XuatExcel(p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 12)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.TCD02_GetDSChiTietDonThu_XuatExcel(p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 13)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.XLD01_GetDSChiTietDonThu_XuatExcel(p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 14)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.XLD02_GetDSChiTietDonThu_XuatExcel(p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 15)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.XLD03_GetDSChiTietDonThu_XuatExcel(p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 16)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.XLD04_GetDSChiTietDonThu_XuatExcel(p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 17)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.KQGQ01_GetDSChiTietDonThu_XuatExcel(p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 18)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.KQGQ02_GetDSChiTietDonThu_XuatExcel(p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 19)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.KQGQ03_GetDSChiTietDonThu_XuatExcel(p, ContentRootPath, CoQuanDangNhapID);
                    }
                    else if (p.Type == 20)
                    {
                        if (p.CanBoID == null || p.CanBoID == 0) p.CanBoID = CanBoID;
                        Data = _BaoCaoBUS.KQGQ04_GetDSChiTietDonThu_XuatExcel(p, ContentRootPath, CoQuanDangNhapID);
                    }
                    base.Status = Data.Status;
                    base.Data = serverPath + Data.Data;
                    base.Message = Data.Message;

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
        //[CustomAuthAttribute(ChucNangEnum.BaoCao, AccessLevel.Read)]
        [Route("GetCapBaoCao")]
        public IActionResult GetCapBaoCao([FromQuery] int? Type)
        {
            try
            {
                if (Type == 3)
                {
                    base.Data = 3;
                }
                else
                {
                    base.Data = 2;
                }
                base.Status = 1;
                return base.GetActionResult();
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
