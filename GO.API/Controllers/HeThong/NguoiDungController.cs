using AutoMapper.Configuration.Annotations;
using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;

namespace Com.Gosol.KNTC.API.Controllers.HeThong
{
    [Route("api/v2/Nguoidung")]
    [ApiController]
    public class NguoidungController : ControllerBase
    {
        private IOptions<AppSettings> _AppSettings;
        private NguoiDungBUS _NguoiDungBUS;
        private HeThongCanBoBUS _CanBoBUS;
        private PhanQuyenBUS _PhanQuyenBUS;
        private ILogger logger;
        private SystemConfigBUS _systemConfigBUS;
        private ILogHelper _LogHelper;
        public NguoidungController(IOptions<AppSettings> Settings, ILogHelper LogHelper)
        {
            _AppSettings = Settings;
            _NguoiDungBUS = new NguoiDungBUS();
            _CanBoBUS = new HeThongCanBoBUS();
            _PhanQuyenBUS = new PhanQuyenBUS();
            this.logger = logger;
            this._systemConfigBUS = new SystemConfigBUS();
            this._LogHelper = LogHelper;
        }


        [Route("DangNhap")]
        [HttpPost]
        public IActionResult Login(LoginModel User)
        {
            try
            {
                //string Password = Cryptor.EncryptPasswordUser(User.UserName.Trim().ToLower(), User.Password);
                string Password = "";
                if (User.Ticket != null && User.Ticket.Length > 0)
                {
                    Password = User.Ticket;
                }
                else Password = Utils.HashFile(Encoding.ASCII.GetBytes(User.Password)).ToUpper();
                NguoiDungModel NguoiDung = null;
                if (_NguoiDungBUS.VerifyUser(User.UserName.Trim(), Password, User.Email, ref NguoiDung))
                {
                    Task.Run(() => _LogHelper.Log(NguoiDung.CanBoID, "Đăng nhập hệ thống", (int)EnumLogType.DangNhap));
                    var claims = new List<Claim>();
                    //var ListChucNang = _PhanQuyenBUS.GetListChucNangByNguoiDungID(NguoiDung.NguoiDungID);

                    //string ClaimRead = "," + string.Join(",", ListChucNang.Where(t => t.Xem == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    //string ClaimCreate = "," + string.Join(",", ListChucNang.Where(t => t.Them == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    //string ClaimEdit = "," + string.Join(",", ListChucNang.Where(t => t.Sua == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    //string ClaimDelete = "," + string.Join(",", ListChucNang.Where(t => t.Xoa == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                    //string ClaimFullAccess = "," + string.Join(",", ListChucNang.Where(t => t.Xem == 1 && t.Them == 1 && t.Sua == 1 && t.Xoa == 1).Select(t => t.ChucNangID).ToArray()) + ",";

                    ////claims.Add(new Claim(PermissionLevel.FULLACCESS, ClaimFull));
                    //claims.Add(new Claim(PermissionLevel.READ, ClaimRead));
                    //claims.Add(new Claim(PermissionLevel.CREATE, ClaimCreate));
                    //claims.Add(new Claim(PermissionLevel.EDIT, ClaimEdit));
                    //claims.Add(new Claim(PermissionLevel.DELETE, ClaimDelete));
                    //claims.Add(new Claim(PermissionLevel.FULLACCESS, ClaimFullAccess));

                    //claims.Add(new Claim("CanBoID", NguoiDung?.CanBoID.ToString()));
                    //claims.Add(new Claim("NguoiDungID", NguoiDung?.NguoiDungID.ToString()));
                    //claims.Add(new Claim("CoQuanID", NguoiDung?.CoQuanID.ToString()));
                    //claims.Add(new Claim("CoQuanChaID", NguoiDung?.CoQuanChaID.ToString()));
                    //claims.Add(new Claim("PhongBanID", NguoiDung?.PhongBanID.ToString()));
                    //claims.Add(new Claim("CapID", NguoiDung?.CapID.ToString()));
                    //claims.Add(new Claim("RoleID", NguoiDung?.RoleID.ToString()));
                    //claims.Add(new Claim("TinhID", NguoiDung?.TinhID.ToString()));
                    //claims.Add(new Claim("HuyenID", NguoiDung?.HuyenID.ToString()));
                    //claims.Add(new Claim("XaID", NguoiDung?.XaID.ToString()));
                    //claims.Add(new Claim("TenHuyen", NguoiDung?.TenHuyen.ToString()));
                    //claims.Add(new Claim("TenXa", NguoiDung?.TenXa.ToString()));
                    //claims.Add(new Claim("CapCoQuan", NguoiDung?.CapCoQuan.ToString()));
                    //claims.Add(new Claim("VaiTro", NguoiDung?.VaiTro.ToString()));
                    //claims.Add(new Claim("expires_at", Utils.ConvertToDateTime(DateTime.UtcNow.AddDays(_AppSettings.Value.NumberDateExpire).ToString(), DateTime.Now.Date).ToString()));
                    //claims.Add(new Claim("TenCanBo", NguoiDung?.TenCanBo.ToString()));
                    //claims.Add(new Claim("QuanLyThanNhan", NguoiDung?.QuanLyThanNhan.ToString()));
                    //claims.Add(new Claim("MaCoQuan", NguoiDung?.MaCoQuan.ToString()));
                    //claims.Add(new Claim("SuDungQuyTrinhPhucTap", NguoiDung?.SuDungQuyTrinhPhucTap.ToString()));
                    //claims.Add(new Claim("SuDungQuyTrinhGQPhucTap", NguoiDung?.SuDungQuyTrinhGQPhucTap.ToString()));
                    //claims.Add(new Claim("SuDungQTVanThuTiepDan", NguoiDung?.SuDungQTVanThuTiepDan.ToString()));
                    //claims.Add(new Claim("SuDungQTVanThuTiepNhanDon", NguoiDung?.SuDungQTVanThuTiepNhanDon.ToString()));
                    //claims.Add(new Claim("CapUBND", NguoiDung?.CapUBND.ToString()));
                    //claims.Add(new Claim("QuyTrinhGianTiep", NguoiDung?.QuyTrinhGianTiep.ToString()));
                    //claims.Add(new Claim("BanTiepDan", NguoiDung?.BanTiepDan.ToString()));
                    //claims.Add(new Claim("CapThanhTra", NguoiDung?.CapThanhTra.ToString()));
                    //claims.Add(new Claim("ChuTichUBND", NguoiDung?.ChuTichUBND.ToString()));
                    //claims.Add(new Claim("ChanhThanhTra", NguoiDung?.ChanhThanhTra.ToString()));
                    //claims.Add(new Claim("TenCoQuan", NguoiDung?.TenCoQuan.ToString()));
                    //claims.Add(new Claim("CapHanhChinh", NguoiDung?.CapHanhChinh.ToString()));
                    //if (UserRole.CheckAdmin(NguoiDung.NguoiDungID))
                    //{
                    //    NguoiDung.isAdmin = true;
                    //}
                    //else NguoiDung.isAdmin = false;
                    //claims.Add(new Claim("isAdmin", NguoiDung?.isAdmin.ToString()));

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
                    claims.Add(new Claim("TrangThaiGiaoXacMinh", TrangThaiGiaoXacMinh.ToString()));
                    //claims.Add(new Claim("expires_at", new DateTime(2020,01,07,13,45,00).ToString()));
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_AppSettings.Value.AudienceSecret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddDays(_AppSettings.Value.NumberDateExpire),
                        //new DateTime(2020, 01, 07, 13, 45, 00),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        //,Issuer = _AppSettings.Value.ApiUrl
                        //, Audience = _AppSettings.Value.AudienceSecret

                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    NguoiDung.Token = tokenHandler.WriteToken(token);
                    NguoiDung.expires_at = DateTime.UtcNow.AddDays(_AppSettings.Value.NumberDateExpire);
                    //tokenDescriptor.Expires;
                    //var clsCommon = new Commons();
                    //var listFile = _FileDinhKemBUS.GetAllField_FileDinhKem_ByNghiepVuID_AndType(NguoiDung.CanBoID, EnumLoaiFileDinhKem.AnhHoSo.GetHashCode());
                    //if (listFile.Count > 0)
                    //    NguoiDung.AnhHoSo = clsCommon.GetServerPath(HttpContext) + listFile[0].FileUrl;
                    NguoiDung.ListCap = GetListCap(NguoiDung?.CapID, NguoiDung?.CoQuanID ?? 0);
                    if (NguoiDung != null && NguoiDung.NguoiDungID > 0)
                    {
                        string messErr = "";
                        NguoiDung.SoLanLogin = 0;
                        NguoiDung.ThoiGianLogin = DateTime.Now;
                        NguoiDung.TrangThai = 1;
                        //var temp = _NguoiDungBUS.UpdateThoiGianlogin(NguoiDung, ref messErr);
                    }

                    return Ok(new
                    {
                        Status = 1,
                        User = NguoiDung,
                        //ListRole = ListChucNang
                    });
                }
                else
                {
                    //string message = Constant.NOT_ACCOUNT;
                    //if (User.Email != null && User.Email != "")
                    //{
                    //    message = Constant.NOT_ACCOUNT_CAS;
                    //}
                    //return Ok(new
                    //{
                    //    Status = -1,
                    //    Message = message
                    //});
                   bool Captcha = false;
                    //var _nguoiDung = _NguoiDungBUS.GetByTenNguoiDung(User.UserName.Trim());
                    string mess = "";
                    string messErr = "";
                    var thamSoGioiHan = new string[] { };
                    //var _systemConfig = _systemConfigBUS.GetByKey("Gioi_Han_So_Lan_Login_Sai");
                    //if (_systemConfig != null && (_systemConfig.ConfigValue == null || _systemConfig.ConfigValue == ""))
                    //{
                    //    _systemConfig.ConfigValue = "5/5";
                    //}

                    //if (_systemConfig != null && _systemConfig.ConfigValue != null && _systemConfig.ConfigValue.Length > 0)
                    //{
                    //    thamSoGioiHan = _systemConfig.ConfigValue.Split("/");

                    //    int SoLanGioiHanLogin = 0;
                    //    int KhoangPhutGioiHan = 0;
                    //    if (thamSoGioiHan.Length > 0)
                    //    {
                    //        SoLanGioiHanLogin = Utils.ConvertToInt32(thamSoGioiHan[0], 0);
                    //        KhoangPhutGioiHan = Utils.ConvertToInt32(thamSoGioiHan[1], 0);
                    //    }
                    //    if (_nguoiDung.NguoiDungID > 0) /*&& _nguoiDung.SoLanLogin < SoLanGioiHanLogin)*/
                    //    {
                    //        if (_nguoiDung.TrangThai == 0)
                    //        {
                    //            mess = "Tài khoản đã bị khoá!";
                    //        }
                    //        else
                    //        {
                    //            if (_nguoiDung.ThoiGianLogin == null)
                    //            {
                    //                //trường hợp đăng nhập lần đầu sai
                    //                //update trườg ThoiGianLogin và số lần login
                    //                _nguoiDung.ThoiGianLogin = DateTime.Now;
                    //                _nguoiDung.SoLanLogin = 1;
                    //                mess = "Mật khẩu không đúng! Vui lòng thử lại!";
                    //            }
                    //            else
                    //            {
                    //                if (DateTime.Now >= _nguoiDung.ThoiGianLogin.Value && DateTime.Now <= _nguoiDung.ThoiGianLogin.Value.AddMinutes(KhoangPhutGioiHan))
                    //                {
                    //                    _nguoiDung.SoLanLogin++;
                    //                    mess = "Mật khẩu không đúng! Vui lòng thử lại!";
                    //                }
                    //                else
                    //                {
                    //                    _nguoiDung.ThoiGianLogin = DateTime.Now;
                    //                    _nguoiDung.SoLanLogin = 1;
                    //                    mess = "Mật khẩu không đúng! Vui lòng thử lại!";
                    //                }
                    //            }
                    //            if (_nguoiDung.SoLanLogin > SoLanGioiHanLogin /*&& User.Captcha != null*/)
                    //            {
                    //                Captcha = true;
                    //                mess = "Tài khoản nhập sai mật khẩu quá " + SoLanGioiHanLogin + " lần trong " + KhoangPhutGioiHan + " phút!";
                    //                //_nguoiDung.TrangThai = 0;
                    //                //mess = "Tài khoản đã bị khoá do nhập sai mật khẩu quá " + SoLanGioiHanLogin + " lần trong " + KhoangPhutGioiHan + " phút!";
                    //                //if (_nguoiDung.TenNguoiDung.Contains("@") && _nguoiDung.TenNguoiDung.Contains(".com"))
                    //                //{
                    //                //    string TieuDe = "Thông báo bảo mật!";
                    //                //    string NoiDung = ("<p>" + "Bạn đã nhập sai mật khẩu " + SoLanGioiHanLogin + " liên tiếp trong " + KhoangPhutGioiHan + " phút!" + "</p>" +
                    //                //                     "<p>" + "Để đảm bảo tài khoản của bạn dược an toàn, chúng tôi tạm khoá tài khoản của bạn!" + "</p>" +
                    //                //                     "<p>" + "Vui lòng liên hệ với quản trị viên để được hỗ trợ mở lại tài khoản!" + "</p>" +
                    //                //                     "<p>" + "Cảm ơn!" + "</p>"
                    //                //                    ).ToString();
                    //                //    List<string> MailTo = new List<string>();
                    //                //    MailTo.Add(_nguoiDung.TenNguoiDung);
                    //                //    _nhacViecBUS.SendMail(MailTo, TieuDe, NoiDung);
                    //                //}
                    //            }
                    //            //var temp = _NguoiDungBUS.UpdateThoiGianlogin(_nguoiDung, ref messErr);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        mess = "Tài khoản không tồn tại!";
                    //    }
                    //}
                    //else
                    //{
                    //    if (_nguoiDung.NguoiDungID > 0)
                    //    {
                    //        mess = "Mật khẩu không đúng! Vui lòng thử lại!";
                    //    }
                    //    else
                    //    {
                    //        mess = "Tài khoản không tồn tại!";
                    //    }
                    //}
                    return Ok(new
                    {
                        Status = -1,
                        Message = string.IsNullOrEmpty(messErr) ? mess : messErr,
                        //SoLanLogin = _nguoiDung.SoLanLogin ?? 0,
                        Captcha
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message, "Đăng nhập hệ thống");
                throw;
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
    }
}