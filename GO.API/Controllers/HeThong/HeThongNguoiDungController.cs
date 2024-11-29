using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Mvc;
using static Com.Gosol.KNTC.Models.HeThong.HeThongNguoiDungModelPartial;
//using LogHelper = Com.Gosol.KNTC.API.Formats.LogHelper;

namespace Com.Gosol.KNTC.API.Controllers.HeThong
{
    [Route("api/v2/HeThongNguoiDung")]
    [ApiController]
    public class HeThongNguoidungController : BaseApiController
    {
        private HeThongNguoidungBUS _HeThongNguoidungBUS;
        private HeThongCanBoBUS _HeThongCanBoBUS;
        private PhanQuyenBUS _PhanQuyenBUS;
        public HeThongNguoidungController(ILogHelper _LogHelper, ILogger<HeThongNguoidungController> logger) : base(_LogHelper, logger)
        {
            this._HeThongNguoidungBUS = new HeThongNguoidungBUS();
            this._HeThongCanBoBUS = new HeThongCanBoBUS();
            _PhanQuyenBUS = new PhanQuyenBUS();
        }

        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.QuanLyNguoiDung, AccessLevel.Create)]
        [Route("Insert")]
        public IActionResult Insert(HeThongNguoiDungModel HeThongNguoiDungModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_Nguoidung_ThemNguoidung, EnumLogType.Insert, () =>
                {
                    string Message = null;
                    int val = 0;
                    val = _HeThongNguoidungBUS.Insert(HeThongNguoiDungModel, ref Message);
                    base.Message = Message;
                    base.Status = val > 0 ? 1 : 0;
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


        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.QuanLyNguoiDung, AccessLevel.Edit)]
        [Route("Update")]
        public IActionResult Update(HeThongNguoiDungModel HeThongNguoiDungModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_Nguoidung_SuaNguoidung, EnumLogType.Update, () =>
                {
                    string Message = null;
                    int val = 0;
                    val = _HeThongNguoidungBUS.Update(HeThongNguoiDungModel, ref Message);
                    base.Message = Message;
                    base.Status = val > 0 ? 1 : 0;
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


        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.QuanLyNguoiDung, AccessLevel.Delete)]
        [Route("Delete")]
        public IActionResult Delete([FromBody] BaseDeleteParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_Nguoidung_XoaNguoidung, EnumLogType.Delete, () =>
                 {
                     int Status = 0;
                     var Result = _HeThongNguoidungBUS.Delete(p.ListID, ref Status);
                     //if(Result.Count <= 0)
                     //{
                     //    base.Status = 1;
                     //    base.Message = "Xóa thành công!";
                     //    return base.GetActionResult();
                     //}
                     //else
                     //{
                     base.Status = Status;
                     base.Data = Result;
                     return base.GetActionResult();
                     //}

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
        [CustomAuthAttribute(ChucNangEnum.QuanLyNguoiDung, AccessLevel.Read)]
        [Route("GetByID")]
        public IActionResult GetByID(int NguoiDungID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_Nguoidung_GetByID, EnumLogType.GetByID, () =>
                 {
                     HeThongNguoiDungModel Data;
                     Data = _HeThongNguoidungBUS.GetByID(NguoiDungID);
                     base.Status = Data.CanBoID > 0 ? 1 : 0;
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


        [HttpGet]
        [CustomAuthAttribute(ChucNangEnum.QuanLyNguoiDung, AccessLevel.Read)]
        [Route("GetListPaging1")]
        public IActionResult GetPagingBySearch1([FromQuery] BasePagingParams p, int? CoQuanID, int? TrangThai)
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.HT_Nguoidung_GetListPaging, EnumLogType.GetList, () =>
                 {
                     IList<object> Data;
                     int TotalRow = 0;
                     Data = _HeThongNguoidungBUS.GetPagingBySearch(p, ref TotalRow, CoQuanID, TrangThai);
                     int totalRow = Data.Count();
                     if (totalRow == 0)
                     {
                         base.Message = ConstantLogMessage.API_NoData;
                         base.Status = 1;
                         return base.GetActionResult();
                     }

                     base.Status = TotalRow > 0 ? 1 : 0;
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
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetListPaging")]
        public IActionResult GetPagingBySearch([FromQuery] BasePagingParamsForFilter p)
        {
            try
            {
                var crCoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                var crNguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                //var crCanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                return CreateActionResult(false, ConstantLogMessage.HT_Nguoidung_GetListPaging, EnumLogType.GetList, () =>
                {
                    IList<object> Data;
                    int TotalRow = 0;
                    Data = _HeThongNguoidungBUS.GetPagingBySearch_New(p, ref TotalRow, Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0), crNguoiDungID, crCoQuanID);
                    int totalRow = Data.Count();
                    if (totalRow == 0)
                    {
                        base.Message = ConstantLogMessage.API_NoData;
                        base.Status = 1;
                        base.GetActionResult();
                    }
                    base.Status = totalRow > 0 ? 1 : 0;
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
        [CustomAuthAttribute(ChucNangEnum.QuanLyNguoiDung, AccessLevel.Read)]
        [Route("ResetPassword")]
        public IActionResult ResetPassword([FromQuery] int NguoiDungID)
        {
            try
            {
                return CreateActionResult("Reset lại mật khẩu", EnumLogType.Other, () =>
                {
                    var Result = _HeThongNguoidungBUS.ResetPassword(NguoiDungID);
                    try
                    {
                        if (Utils.ConvertToInt32(Result.FirstOrDefault().Key, 0) > 0)
                        {
                            var clsCommon = new Commons();
                            var NguoiDung = _HeThongNguoidungBUS.GetByID(NguoiDungID);
                            HeThongNguoiDungModelPartial p = new HeThongNguoiDungModelPartial();
                            p.TenNguoiDung = NguoiDung.TenNguoiDung;
                            p.CoQuanID = NguoiDung.CoQuanID;
                            p.Url = clsCommon.GetServerPath(HttpContext);
                            SendMail(p);
                        }
                    }
                    catch (Exception)
                    {
                        //throw;
                    }
                    base.Message = Result.FirstOrDefault().Value.ToString();
                    base.Status = Utils.ConvertToInt32(Result.FirstOrDefault().Key, 0);
                    base.Data = Data;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }


        [Route("GetByIDForPhanQuyen")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [HttpGet]
        public IActionResult GetByIDForPhanQuyen(int? NguoiDungID)
        {
            try
            {

                return CreateActionResult("Đăng nhập lại (f5)", EnumLogType.DangNhap, () =>
                {
                    if (NguoiDungID == null)
                    {
                        return Ok(new
                        {
                            Status = -1,
                            Message = "Param NguoiDungID is NULL",
                        });
                    }
                    else
                    {
                        var NguoiDungIDToken = Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == "NguoiDungID").Value, 0);
                        if (NguoiDungIDToken != NguoiDungID)
                        {
                            return Ok(new
                            {
                                Status = -1,
                                Message = "Sai token",
                            });
                        }
                    }
                    NguoiDungModel NguoiDung = null;
                    NguoiDung = _HeThongNguoidungBUS.GetByIDForPhanQuyen(NguoiDungID.Value);

                    if (NguoiDung != null && NguoiDung.NguoiDungID > 0)
                    {
                        var clsCommon = new Commons();
                        var CanBoInfo = _HeThongCanBoBUS.GetCanBoByID(NguoiDung.CanBoID);
                        NguoiDung.AnhHoSo = CanBoInfo.AnhHoSo != string.Empty ? clsCommon.GetServerPath(HttpContext) + CanBoInfo.AnhHoSo : string.Empty;
                        //   Task.Run(() => _ILogHelper.Log(NguoiDung.CanBoID, "Đăng nhập hệ thống", (int)LogType.Action));
                        var ListChucNang = _PhanQuyenBUS.GetListChucNangByNguoiDungID(NguoiDungID.Value);
                        NguoiDung.ExpiresAt = Utils.ConvertToDateTime(User.Claims.FirstOrDefault(c => c.Type == "expires_at").Value, DateTime.Now.Date);
                        NguoiDung.expires_at = NguoiDung.ExpiresAt;
                        NguoiDung.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                        NguoiDung.PhongBanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "PhongBanID").Value, 0);
                        NguoiDung.CapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                        NguoiDung.RoleID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value, 0);
                        NguoiDung.TinhID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "TinhID").Value, 0);
                        NguoiDung.HuyenID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "HuyenID").Value, 0);
                        NguoiDung.XaID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "XaID").Value, 0);
                        NguoiDung.TenHuyen = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "TenHuyen").Value, string.Empty);
                        NguoiDung.TenXa = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "TenXa").Value, string.Empty);
                        NguoiDung.CapCoQuan = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapCoQuan").Value, 0);
                        NguoiDung.VaiTro = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "VaiTro").Value, 0);
                        NguoiDung.MaCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "MaCoQuan").Value, string.Empty);
                        //NguoiDung.TenCoQuan = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "TenCoQuan").Value, string.Empty);
                        NguoiDung.SuDungQuyTrinhPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhPhucTap").Value, false);
                        NguoiDung.SuDungQuyTrinhGQPhucTap = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQuyTrinhGQPhucTap").Value, false);
                        NguoiDung.SuDungQTVanThuTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepDan").Value, false);
                        NguoiDung.SuDungQTVanThuTiepNhanDon = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "SuDungQTVanThuTiepNhanDon").Value, false);
                        NguoiDung.CapUBND = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapUBND").Value, 0);
                        NguoiDung.QuyTrinhGianTiep = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "QuyTrinhGianTiep").Value, 0);
                        NguoiDung.BanTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "BanTiepDan").Value, false);
                        NguoiDung.CapThanhTra = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "CapThanhTra").Value, false);
                        NguoiDung.ChuTichUBND = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "ChuTichUBND").Value, 0);
                        NguoiDung.ChanhThanhTra = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "ChanhThanhTra").Value, 0);
                        NguoiDung.CapHanhChinh = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapHanhChinh").Value, 0);

                        if (UserRole.CheckAdmin(NguoiDung.NguoiDungID))
                        {
                            NguoiDung.isAdmin = true;
                        }
                        else NguoiDung.isAdmin = false;
                    
                        int TrangThaiGiaoXacMinh = 0;
                        if (NguoiDung?.CapID == CapQuanLy.CapUBNDTinh.GetHashCode() || NguoiDung?.CapID == CapQuanLy.CapSoNganh.GetHashCode())
                        {
                            if (NguoiDung?.SuDungQuyTrinhGQPhucTap == true)
                            {
                                if (NguoiDung?.RoleID == RoleEnum.LanhDao.GetHashCode())
                                {
                                    TrangThaiGiaoXacMinh = 1;
                                }
                                else if (NguoiDung?.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                                {
                                    TrangThaiGiaoXacMinh = 2;
                                }
                                else TrangThaiGiaoXacMinh = 0;
                            }
                            else
                            {
                                if (NguoiDung?.RoleID == RoleEnum.LanhDao.GetHashCode())
                                {
                                    TrangThaiGiaoXacMinh = 2;
                                }
                                else TrangThaiGiaoXacMinh = 0;
                            }


                        }
                        else if (NguoiDung?.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode())
                        {
                            if (NguoiDung?.RoleID == RoleEnum.LanhDao.GetHashCode())
                            {
                                TrangThaiGiaoXacMinh = 2;
                            }
                            else TrangThaiGiaoXacMinh = 0;
                        }
                        else if (NguoiDung?.CapID == CapQuanLy.CapPhong.GetHashCode() && NguoiDung?.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode())
                        {
                            if (NguoiDung?.SuDungQuyTrinhGQPhucTap == true)
                            {
                                if (NguoiDung?.RoleID == RoleEnum.LanhDao.GetHashCode())
                                {
                                    TrangThaiGiaoXacMinh = 2;
                                }
                                else TrangThaiGiaoXacMinh = 0;
                            }
                            else
                            {
                                if (NguoiDung?.RoleID == RoleEnum.LanhDao.GetHashCode())
                                {
                                    TrangThaiGiaoXacMinh = 2;
                                }
                                else TrangThaiGiaoXacMinh = 0;
                            }
                        }
                        else if (NguoiDung?.CapID == CapQuanLy.CapPhong.GetHashCode() && NguoiDung?.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocSo.GetHashCode())
                        {
                            if (NguoiDung?.RoleID == RoleEnum.LanhDao.GetHashCode() || NguoiDung?.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                            {
                                TrangThaiGiaoXacMinh = 2;
                            }
                            else TrangThaiGiaoXacMinh = 0;
                        }
                        else
                        {
                            if (NguoiDung?.RoleID == RoleEnum.LanhDao.GetHashCode())
                            {
                                TrangThaiGiaoXacMinh = 2;
                            }
                            else TrangThaiGiaoXacMinh = 0;
                        }
                        //quy trinh don gian
                        if (NguoiDung?.SuDungQuyTrinhGQPhucTap == false)
                        {
                            TrangThaiGiaoXacMinh = 2;
                        }

                        NguoiDung.TrangThaiGiaoXacMinh = TrangThaiGiaoXacMinh;
                        NguoiDung.ListCap = GetListCap(NguoiDung?.CapID, NguoiDung?.CoQuanID ?? 0);

                        return Ok(new
                        {
                            Status = 1,
                            User = NguoiDung,
                            ListRole = ListChucNang
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            Status = -98,
                            Message = Constant.NOT_ACCOUNT,
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.GetActionResult();
                throw ex;
            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<CapInfo> GetListCap(int? CapID, int CoQuanID)
        {
            var listCap = new List<CapInfo>();
            if (CapID == CapQuanLy.CapUBNDTinh.GetHashCode())
            {
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
            }
            else if (CapID == CapQuanLy.CapSoNganh.GetHashCode())
            {
                try
                {
                    var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
                    if (listThanhTraTinh.Contains(CoQuanID))
                    {
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
                    }
                    else
                    {
                        CapInfo capSo = new CapInfo();
                        capSo.CapID = 1;
                        capSo.TenCap = "Cấp Sở, Ngành";
                        listCap.Add(capSo);
                    }
                }
                catch (Exception)
                {
                    CapInfo capSo = new CapInfo();
                    capSo.CapID = 1;
                    capSo.TenCap = "Cấp Sở, Ngành";
                    listCap.Add(capSo);
                }
            }
            else if (CapID == CapQuanLy.CapUBNDHuyen.GetHashCode())
            {
                CapInfo capHuyen = new CapInfo();
                capHuyen.CapID = 2;
                capHuyen.TenCap = "UBND Cấp Huyện";
                listCap.Add(capHuyen);

                CapInfo capXa = new CapInfo();
                capXa.CapID = 3;
                capXa.TenCap = "UBND Cấp Xã";
                listCap.Add(capXa);
            }
            else if (CapID == CapQuanLy.CapUBNDXa.GetHashCode())
            {
                CapInfo capXa = new CapInfo();
                capXa.CapID = 3;
                capXa.TenCap = "UBND Cấp Xã";
                listCap.Add(capXa);
            }
            return listCap;
        }

        [Route("SendMail")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [HttpPost]
        public IActionResult SendMail([FromBody] HeThongNguoiDungModelPartial p)
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.API_SendMail, EnumLogType.Other, () =>
                {

                    var Result = _HeThongNguoidungBUS.SendMail(p);
                    base.Status = Result.Status;
                    base.Message = Result.Message;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }


        [Route("SetNewPassword")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [HttpPost]
        public IActionResult UpdateNguoiDung([FromBody] QuenMatKhauModelPar p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.API_SendMail, EnumLogType.Other, () =>
                {
                    var Result = _HeThongNguoidungBUS.UpdateNguoiDung(p.TenDangNhap, p.MatKhauMoi);
                    base.Status = Result.Status;
                    base.Message = Result.Message;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }



        }


        [Route("CheckMaMail")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [HttpGet]
        public IActionResult CheckMaMail([FromQuery] string Token)
        {
            try
            {
                return CreateActionResult(false, "Check mã mail", EnumLogType.Other, () =>
                {
                    var Result = _HeThongNguoidungBUS.CheckMaMail(Token);
                    base.Status = Result.Status;
                    base.Message = Result.Message;
                    base.Data = Result.Data;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }



        }


        [HttpPost]
        [CustomAuthAttribute(0, AccessLevel.Edit)]
        [Route("ChangePassword")]
        public IActionResult ChangePassword([FromBody] DoiMatKhauModel p)
        {
            try
            {
                //var expires_at = Utils.ConvertToDateTime(User.Claims.FirstOrDefault(c => c.Type == "expires_at").Value, DateTime.Now.Date);
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                return CreateActionResult("Đổi mật khẩu", EnumLogType.Other, () =>
                {
                    var Result = _HeThongNguoidungBUS.ChangePassword(NguoiDungID, p.OldPassword, p.NewPassword);
                    base.Status = Result.Status;
                    base.Message = Result.Message;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }


        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("HeThong_NguoiDung_GetListBy_NhomNguoiDungID")]
        public IActionResult HeThong_NguoiDung_GetListBy_NhomNguoiDungID(int NhomNguoiDungID, int? CoQuanID)
        {
            try
            {
                return CreateActionResult(false, "Lấy danh sách người dùng theo nhóm người dùng", EnumLogType.GetList, () =>
                {
                    IList<HeThongNguoiDungModel> Data;
                    int TotalRow = 0;
                    //Data = _HeThongNguoidungBUS.HeThong_NguoiDung_GetListBy_NhomNguoiDungID(NhomNguoiDungID);
                    Data = _PhanQuyenBUS.HeThong_NguoiDung_GetListBy_NhomNguoiDungID(NhomNguoiDungID, Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0));
                    if(CoQuanID > 0)
                    {
                        Data = Data.Where(x => x.CoQuanID == CoQuanID).ToList();
                    }
                    int totalRow = Data.Count();
                    if (totalRow == 0)
                    {
                        base.Message = ConstantLogMessage.API_NoData;
                        base.Status = 1;
                        base.GetActionResult();
                    }
                    base.Status = totalRow >= 0 ? 1 : 0;
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