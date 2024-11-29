using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.BUS.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Ultilities;
using GO.API.Controllers.KNTC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;

namespace Com.Gosol.KNTC.API.Controllers.HeThong
{
    [Route("api/v2/LoginSSO")]
    [ApiController]
    public class LoginSSOController : BaseApiController
    {
        private IOptions<AppSettings> _AppSettings;
        private NguoiDungBUS _NguoiDungBUS;
        private HeThongCanBoBUS _CanBoBUS;
        private PhanQuyenBUS _PhanQuyenBUS;
        private ILogger logger;
        private SystemConfigBUS _systemConfigBUS;
        private ILogHelper _LogHelper;


        public LoginSSOController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<TiepDanController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            _AppSettings = Settings;
            _NguoiDungBUS = new NguoiDungBUS();
            _CanBoBUS = new HeThongCanBoBUS();
            _PhanQuyenBUS = new PhanQuyenBUS();
            this.logger = logger;
            this._systemConfigBUS = new SystemConfigBUS();
            this._LogHelper = _LogHelper;
        }


        [Route("Login")]
        [HttpPost]
        public IActionResult LoginSSO(LoginSSOModel login_sso)
        {
            try
            {
                // lấy accesstoken
                var access_token_info = Get_AccessToken(login_sso);
                var user_info = new UserInfo();
                if (access_token_info != null && access_token_info.access_token != null && access_token_info.access_token.Length > 0)
                {
                    // lấy thông tin người dùng đăng nhập trên sso
                    user_info = Get_UserInfo(access_token_info.access_token);
                }
                if (user_info != null && user_info.sub != null && user_info.sub.Length > 0)
                {
                    //thực hiện đăng nhập trên hệ thống KNTC
                    NguoiDungModel NguoiDung = null;
                    if (_NguoiDungBUS.VerifyUserSSO(user_info.sub, ref NguoiDung))
                    {
                        Task.Run(() => _LogHelper.Log(NguoiDung.CanBoID, "Đăng nhập hệ thống", (int)EnumLogType.DangNhap));
                        var claims = new List<Claim>();
                        var ListChucNang = _PhanQuyenBUS.GetListChucNangByNguoiDungID(NguoiDung.NguoiDungID);

                        string ClaimRead = "," + string.Join(",", ListChucNang.Where(t => t.Xem == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                        string ClaimCreate = "," + string.Join(",", ListChucNang.Where(t => t.Them == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                        string ClaimEdit = "," + string.Join(",", ListChucNang.Where(t => t.Sua == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                        string ClaimDelete = "," + string.Join(",", ListChucNang.Where(t => t.Xoa == 1).Select(t => t.ChucNangID).ToArray()) + ",";
                        string ClaimFullAccess = "," + string.Join(",", ListChucNang.Where(t => t.Xem == 1 && t.Them == 1 && t.Sua == 1 && t.Xoa == 1).Select(t => t.ChucNangID).ToArray()) + ",";

                        //claims.Add(new Claim(PermissionLevel.FULLACCESS, ClaimFull));
                        claims.Add(new Claim(PermissionLevel.READ, ClaimRead));
                        claims.Add(new Claim(PermissionLevel.CREATE, ClaimCreate));
                        claims.Add(new Claim(PermissionLevel.EDIT, ClaimEdit));
                        claims.Add(new Claim(PermissionLevel.DELETE, ClaimDelete));
                        claims.Add(new Claim(PermissionLevel.FULLACCESS, ClaimFullAccess));

                        claims.Add(new Claim("CanBoID", NguoiDung?.CanBoID.ToString()));
                        claims.Add(new Claim("NguoiDungID", NguoiDung?.NguoiDungID.ToString()));
                        claims.Add(new Claim("CoQuanID", NguoiDung?.CoQuanID.ToString()));
                        claims.Add(new Claim("CapID", NguoiDung?.CapID.ToString()));
                        claims.Add(new Claim("RoleID", NguoiDung?.RoleID.ToString()));
                        claims.Add(new Claim("TinhID", NguoiDung?.TinhID.ToString()));
                        claims.Add(new Claim("HuyenID", NguoiDung?.HuyenID.ToString()));
                        claims.Add(new Claim("CapCoQuan", NguoiDung?.CapCoQuan.ToString()));
                        claims.Add(new Claim("VaiTro", NguoiDung?.VaiTro.ToString()));
                        claims.Add(new Claim("expires_at", Utils.ConvertToDateTime(DateTime.Now.AddSeconds(access_token_info.expires_in).ToString(), DateTime.Now.Date).ToString()));
                        claims.Add(new Claim("TenCanBo", NguoiDung?.TenCanBo.ToString()));
                        claims.Add(new Claim("MaCoQuan", NguoiDung?.MaCoQuan.ToString()));
                        claims.Add(new Claim("SuDungQuyTrinhPhucTap", NguoiDung?.SuDungQuyTrinhPhucTap.ToString()));
                        claims.Add(new Claim("SuDungQuyTrinhGQPhucTap", NguoiDung?.SuDungQuyTrinhGQPhucTap.ToString()));
                        claims.Add(new Claim("SuDungQTVanThuTiepDan", NguoiDung?.SuDungQTVanThuTiepDan.ToString()));

                        // add claims các thông tin trong sso
                        claims.Add(new Claim("code", login_sso.code.ToString()));
                        claims.Add(new Claim("session_state", login_sso.code.ToString()));
                        claims.Add(new Claim("access_token", access_token_info.access_token.ToString()));
                        claims.Add(new Claim("refresh_token", access_token_info.refresh_token.ToString()));
                        claims.Add(new Claim("id_token", access_token_info.id_token.ToString()));
                        claims.Add(new Claim("expires_in", access_token_info.expires_in.ToString()));
                        claims.Add(new Claim("sub", user_info.sub.ToString()));
                        /////



                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_AppSettings.Value.AudienceSecret);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(claims),
                            Expires = DateTime.Now.AddSeconds(access_token_info.expires_in),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

                        };

                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        NguoiDung.Token = tokenHandler.WriteToken(token);
                        NguoiDung.expires_at = DateTime.Now.AddSeconds(access_token_info.expires_in);
                        NguoiDung.ExpiresAt = DateTime.Now.AddSeconds(access_token_info.expires_in);
                        NguoiDung.id_token = access_token_info.id_token;
                        return Ok(new
                        {
                            Status = 1,
                            User = NguoiDung,
                            ListRole = ListChucNang
                        });
                    }
                    else
                    {
                        string message = Constant.NOT_ACCOUNT;
                        if (user_info != null && user_info.sub != null && user_info.sub.Length > 0)
                        {
                            message = Constant.NOT_ACCOUNT_CAS;
                        }
                        return Ok(new
                        {
                            Status = -1,
                            Message = message
                        });
                    }
                }
                else
                {
                    string message = "Đăng nhập SSO không thành công, vui lòng kiểm tra lại thông tin tài khoản!";

                    return Ok(new
                    {
                        Status = -1,
                        Message = message
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message, "Đăng nhập hệ thống thông qua SSO");
                throw;
            }


        }


        [Route("Logout")]
        [HttpGet]
        public IActionResult Logout(string id_token_hint)
        {
            try
            {
                //var id_token_hint = Utils.ConvertToString(User.Claims.FirstOrDefault(c => c.Type == "id_token").Value, "");
                var Result = LogoutSSO(id_token_hint);
                base.Status = Result.Status;
                base.Message = Result.Message;
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message, "Đăng nhập hệ thống thông qua SSO");
                throw;
            }
            return base.GetActionResult();
        }






        [ApiExplorerSettings(IgnoreApi = true)]
        public Oauth Get_AccessToken(LoginSSOModel login_sso)
        {
            var Result = new Oauth();
            var client_id = "n7Jhej6TZnTPsPf_2yRlyjJeIPQa";
            var client_secret = "kqG8FQu20cck5RFyFcTmEKiNQD0a";
            var grant_type = "authorization_code";
            var redirect_uri = "https://localhost:9443/console/login";
            var api_oauth = "https://lgsp-sso.vinhphuc.gov.vn/oauth2/token";
            try
            {
                var client_config = _systemConfigBUS.GetByKey("SSO_LOGIN");
                if (client_config != null && client_config.ConfigValue != null && client_config.ConfigValue.Length > 0)
                {
                    var config_value = client_config.ConfigValue.Split(';');
                    if (config_value != null && config_value.Count() == 3)
                    {
                        client_id = config_value[0];
                        client_secret = config_value[1];
                        redirect_uri = config_value[2];
                    }
                }
                // API lấy token 
                api_oauth = _systemConfigBUS.GetByKey("SSO_OAUTH").ConfigValue;
                var client = new RestClient(api_oauth);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("client_id", client_id);
                request.AddParameter("client_secret", client_secret);
                request.AddParameter("grant_type", grant_type);
                request.AddParameter("code", login_sso.code);
                request.AddParameter("redirect_uri", "https://unittest.gosol.com.vn/");
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return Result;
                if (response.Content != null)
                {
                    Result = JsonConvert.DeserializeObject<Oauth>(response.Content);

                }
                //Console.WriteLine(response.Content);
            }
            catch (Exception)
            {
                return Result;
            }
            return Result;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public UserInfo Get_UserInfo(string access_token)
        {
            var Result = new UserInfo();
            var api_userinfo = "https://lgsp-sso.vinhphuc.gov.vn/oauth2/userinfo?schema=openid";
            try
            {

                // API lấy thông tin người dùng 
                api_userinfo = _systemConfigBUS.GetByKey("SSO_USERINFO").ConfigValue;
                var client = new RestClient(api_userinfo);

                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Bearer " + access_token);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return Result;
                if (response.Content != null)
                {
                    Result = JsonConvert.DeserializeObject<UserInfo>(response.Content);
                }
                //Console.WriteLine(response.Content);
            }
            catch (Exception)
            {
                return Result;
            }
            return Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public BaseResultModel LogoutSSO(string id_token_hint)
        {
            var Result = new BaseResultModel();

            try
            {
                var api_logout = "https://lgsp-sso.vinhphuc.gov.vn/oidc/logout";

                api_logout = _systemConfigBUS.GetByKey("SSO_LOGOUT_URL").ConfigValue ?? "https://lgsp-sso.vinhphuc.gov.vn/oidc/logout";
                var post_logout_redirect_uri = _systemConfigBUS.GetByKey("SSO_LOGOUT_POST").ConfigValue ?? "https://lgsp-sso.vinhphuc.gov.vn/oauth2/authorize";
                var client = new RestClient(api_logout + "?id_token_hint=" + id_token_hint + "&post_logout_redirect_uri=" + post_logout_redirect_uri);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Result.Status = 1;
                    Result.Message = "Đăng xuất thành công!";
                }
                else
                {
                    Result.Status = 0;
                    Result.Message = response.ErrorMessage;
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                return Result;
            }
            return Result;
        }
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LoginSSOModel
    {
        public string code { get; set; }
        public string session_state { get; set; }
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    public class OauthError
    {
        public string error_description { get; set; }
        public string error { get; set; }
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    public class Oauth
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string id_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserInfo
    {
        public string sub { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class SessionInfo
    {
        public string code { get; set; }
        public string session_state { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string id_token { get; set; }
        public double expires_in { get; set; }
        public string sub { get; set; }


    }
}
