using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestSharp;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Com.Gosol.KNTC.API.Controllers.DanhMuc
{
    [Route("api/v2/DanhMucCoQuanDonVi")]
    [ApiController]
    public class DanhMucCoQuanDonViController : BaseApiController
    {
        private DanhMucCoQuanDonViBUS _DanhMucCoQuanDonViBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public DanhMucCoQuanDonViController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DanhMucCoQuanDonViController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._DanhMucCoQuanDonViBUS = new DanhMucCoQuanDonViBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Create)]
        [Route("Insert")]
        public IActionResult Insert(DanhMucCoQuanDonViModel DanhMucCoQuanDonViModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_ThemCoQuanDonVi, EnumLogType.Insert, () =>
                 {

                     string Message = null;
                     int val = 0;
                     int CoQuanID = 0;
                     int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                     val = _DanhMucCoQuanDonViBUS.Insert(DanhMucCoQuanDonViModel, ref CoQuanID, NguoiDungID, ref Message, Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0));
                     base.Message = Message;
                     base.Status = val;
                     //base.Data = Data;
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
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Create)]
        [Route("Insert_New")]
        public IActionResult Insert_New(DanhMucCoQuanDonViModel DanhMucCoQuanDonViModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_ThemCoQuanDonVi, EnumLogType.Insert, () =>
                {

                    string Message = null;
                    int val = 0;
                    int valQuyMo = 1;
                    int valCaLamViec = 1;
                    int CoQuanID = 0;
                    int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    val = _DanhMucCoQuanDonViBUS.Insert_New(DanhMucCoQuanDonViModel, ref CoQuanID, NguoiDungID, ref Message, Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0));

                    base.Message = Message;
                    base.Status = val > 0 && valQuyMo > 0 && valCaLamViec > 0 ? 1 : 0;
                    //base.Data = Data;
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



        [ApiExplorerSettings(IgnoreApi = true)]
        private IRestResponse AddPersionToGroup(string GroupID, string Persion)
        {
            try
            {
                var client = new RestClient(_config.GetValue<string>("AppSettings:Api_AddPersonToGroup") + "/" + GroupID);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\r\n  \"exid\": \"" + Persion + "\"\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Edit)]
        [Route("Update")]
        public IActionResult Update(DanhMucCoQuanDonViModel DanhMucCoQuanDonViModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_SuaCoQuanDonVi, EnumLogType.Update, () =>
                 {
                     string Message = null;
                     int val = 0;
                     val = _DanhMucCoQuanDonViBUS.Update(DanhMucCoQuanDonViModel, ref Message);
                     base.Message = Message;
                     base.Status = val;
                     //base.Data = Data;
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
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Edit)]
        [Route("Update_New")]
        public IActionResult Update_New(DanhMucCoQuanDonViModel DanhMucCoQuanDonViModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_SuaCoQuanDonVi, EnumLogType.Update, () =>
                {
                    string Message = null;
                    int val = 0;
                    val = _DanhMucCoQuanDonViBUS.Update_New(DanhMucCoQuanDonViModel, ref Message);
                    base.Message = Message;
                    base.Status = val;
                    //base.Data = Data;
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

        // CHưa Sửa
        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Delete)]
        [Route("Delete")]
        public IActionResult Delete([FromBody] BaseDeleteParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_XoaCoQuanDonVi, EnumLogType.Delete, () =>
                {
                    Dictionary<int, string> dic = new Dictionary<int, string>();
                    dic = _DanhMucCoQuanDonViBUS.Delete(p.ListID);
                    base.Status = dic.FirstOrDefault().Key;
                    base.Message = dic.FirstOrDefault().Value;
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
        [CustomAuthAttribute(0, AccessLevel.Read)]
        [Route("FilterByName")]
        public IActionResult FilterByName(string TenCoQuan)
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_FilterByName, EnumLogType.GetList, () =>
                 {
                     var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                     int val = 0;
                     List<DanhMucCoQuanDonViModel> Data;
                     Data = _DanhMucCoQuanDonViBUS.FilterByName(TenCoQuan);
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


        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        [Route("GetByIDAndCap")]
        public IActionResult GetByIDAndCap(int CoQuanID)
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetByID, EnumLogType.GetList, () =>
                 {
                     var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                     DanhMucCoQuanDonViPartialNew Data;
                     Data = _DanhMucCoQuanDonViBUS.GetByID(CoQuanID);
                     base.Status = Data.CoQuanID > 0 ? 1 : 0;
                     base.Data = Data;
                     base.Message = Data.CoQuanID > 0 ? ConstantLogMessage.API_Success : ConstantLogMessage.API_Error;
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


        [Route("GetByID_New")]
        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetByID_New(int CoQuanID)
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetByID, EnumLogType.GetList, () =>
                {
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    DanhMucCoQuanDonViPartialNew Data;
                    Data = _DanhMucCoQuanDonViBUS.GetByID_New(CoQuanID);
                    base.Status = Data.CoQuanID > 0 ? 1 : 0;
                    base.Data = Data;
                    base.Message = Data.CoQuanID > 0 ? ConstantLogMessage.API_Success : ConstantLogMessage.API_Error;
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
        [CustomAuthAttribute(0, AccessLevel.Read)]
        [Route("GetListByidAndCap")]
        public IActionResult GetListByidAndCap()
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetByID, EnumLogType.GetList, () =>
                {
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    int val = 0;
                    List<DanhMucCoQuanDonViModel> Data;
                    Data = _DanhMucCoQuanDonViBUS.GetListByidAndCap();
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


        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        [Route("GetAllByCap")]
        public IActionResult GetAllByCap([FromQuery] int ID, int Cap, string Keyword)
        {
            try
            {

                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetAllByCap, EnumLogType.GetList, () =>
                {
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    IList<DanhMucCoQuanDonViModelPartial> Data;
                    Data = _DanhMucCoQuanDonViBUS.GetAllByCap(ID, Cap, Keyword);
                    if (Data.Count == 0)
                    {
                        base.Status = 1;
                        base.Message = ConstantLogMessage.API_NoData;
                        return base.GetActionResult();
                    }
                    base.Status = Data.Count > 0 ? 1 : 0;
                    base.Data = Data;
                    base.TotalRow = Data.Count;
                    return base.GetActionResult();
                });
            }
            catch
            {
                base.Status = -1;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        [Route("GetAll")]
        public IActionResult GetAll([FromQuery] int ID, string Keyword)
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetAllByCap, EnumLogType.GetList, () =>
                {
                    IList<DanhMucCoQuanDonViModelPartial> Data;
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    Data = _DanhMucCoQuanDonViBUS.GetALL(ID, CoQuanID, Keyword);
                    if (Data.Count == 0)
                    {
                        base.Status = 1;
                        base.Message = ConstantLogMessage.API_NoData;
                        return base.GetActionResult();
                    }
                    base.Status = Data.Count > 0 ? 1 : 0;
                    base.Data = Data;
                    base.TotalRow = Data.Count;
                    return base.GetActionResult();
                });
            }
            catch
            {
                base.Status = -1;
                return base.GetActionResult();
            }
        }


        [HttpGet]
        [Route("GetListByUser")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult PhanQuyen_GetDanhMuKNTCoQuan()
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetForPhanQuyen, EnumLogType.GetList, () =>
                {
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var result = _DanhMucCoQuanDonViBUS.GetListByUser(CoQuanID, NguoiDungID);
                    base.Status = 1;
                    base.Data = result;
                    base.Message = result.Count < 1 ? "Không có dữ liệu Cơ quan" : "";
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
        [Route("GetListCoQuanByUser")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetListCoQuanByUser()
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetForPhanQuyen, EnumLogType.GetList, () =>
                {
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var result = _DanhMucCoQuanDonViBUS.GetListCoQuanByUser(CoQuanID, NguoiDungID);
                    base.Status = 1;
                    base.Data = result;
                    base.Message = result.Count < 1 ? "Không có dữ liệu Cơ quan" : "";
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
        [Route("GetListByUser_FoPhanQuyen")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetListByUser_FoPhanQuyen(string? Keyword)
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetForPhanQuyen, EnumLogType.GetList, () =>
                {
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var result = _DanhMucCoQuanDonViBUS.GetByUser_FoPhanQuyen(CoQuanID, NguoiDungID, Keyword);
                    base.Status = 1;
                    base.Data = result;
                    base.Message = result.Count < 1 ? "Không có dữ liệu Cơ quan" : "";
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
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Read)]
        [Route("ImportCoQuan")]
        public async Task<IActionResult> ImportCoQuan([FromBody] Files file)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_ChucVu_ImportFile, EnumLogType.Other, () =>
                {
                    string Message = "";
                    string SavePath = _host.ContentRootPath + "\\Upload\\" + "Import_ChucVu.xlsx";
                    using (FileStream stream = System.IO.File.Create(SavePath))
                    {
                        byte[] byteArray = Convert.FromBase64String(file.files);
                        stream.Write(byteArray, 0, byteArray.Length);
                    }

                    var Result = _DanhMucCoQuanDonViBUS.ImportFile(SavePath, ref Message, Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0));
                    base.Status = Result;
                    base.Message = Message;
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
        [CustomAuthAttribute(0, AccessLevel.Read)]
        [Route("CheckMaCQ")]
        public IActionResult CheckMaCQ([FromQuery] int? CoQuanID, string MaCQ)
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetByID, EnumLogType.GetList, () =>
                {

                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var Data = _DanhMucCoQuanDonViBUS.CheckMaCQ(CoQuanID, MaCQ);
                    base.Status = Data.Status;
                    base.Message = Data.Message;
                    //base.Data = Data;
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
        [CustomAuthAttribute(0, AccessLevel.Read)]
        [Route("GetAllByCoQuanDangNhap")]
        public IActionResult GetAllByCoQuanDangNhap()
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetAllByCap, EnumLogType.GetList, () =>
                {
                    IList<DanhMucCoQuanDonViModel> Data;
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    Data = _DanhMucCoQuanDonViBUS.GetAll(CoQuanID, NguoiDungID);
                    if (Data.Count == 0)
                    {
                        base.Status = 1;
                        base.Message = ConstantLogMessage.API_NoData;
                        return base.GetActionResult();
                    }
                    base.Status = Data.Count > 0 ? 1 : 0;
                    base.Data = Data;
                    base.TotalRow = Data.Count;
                    return base.GetActionResult();
                });
            }
            catch
            {
                base.Status = -1;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        [Route("GenerateTicKet")]
        public IActionResult GenerateTicKet()
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_GetAllByCap, EnumLogType.GetList, () =>
                {
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    //string info = CoQuanID + "_" + NguoiDungID + "_" + DateTime.Now.ToString("ddMMyyyy_hhmmss");
                    //var key = _config.GetValue<string>("AppSettings:Key_TicKet");
                    //if (key == null && key == "") key = "jh1f23bnhr3dt6h#1e%13463d&&@G%^&";
                    //string ticket = _DanhMuKNTCoQuanDonViBUS.GenerateTicKet(key, info);
                    //int sum = 0;
                    //foreach (char c in ticket)
                    //{
                    //    sum += System.Convert.ToInt32(c);
                    //}
                    base.Status = 1;
                    //base.Data = sum + "_" + NguoiDungID + "_" + DateTime.Now.ToString("ddMMyy_hhmmss");
                    //base.Data = Guid.NewGuid();
                    base.Data = CoQuanID + "_" + DateTime.Now.ToString("ddMMyy_hhmmss");
                    return base.GetActionResult();
                });
            }
            catch
            {
                base.Status = -1;
                return base.GetActionResult();
            }
        }




        [HttpGet]
        [Route("UpdateNgayReset")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult UpdateNgayReset([FromQuery] DateTime NgayResetPhep)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_SuaCoQuanDonVi, EnumLogType.Update, () =>
                {
                    string Message = null;
                    int val = 0;
                    DanhMucCoQuanDonViModel CoQuanInfo = new DanhMucCoQuanDonViModel();
                    CoQuanInfo.CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    CoQuanInfo.NgayResetPhep = NgayResetPhep;
                    val = _DanhMucCoQuanDonViBUS.UpdateNgayReset(CoQuanInfo, ref Message);
                    base.Message = Message;
                    base.Status = val;
                    //base.Data = Data;
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
        [Route("GetNgayReset")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetNgayReset()
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetByID, EnumLogType.GetList, () =>
                {
                    int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var Data = _DanhMucCoQuanDonViBUS.GetNgayReset(CoQuanID);
                    base.Status = Data != null ? 1 : 0;
                    base.Data = Data;
                    base.Message = Data != null ? ConstantLogMessage.API_Success : ConstantLogMessage.API_Error;
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
        [CustomAuthAttribute(0, AccessLevel.Read)]
        [Route("UpdateQuyTrinh")]
        public IActionResult UpdateQuyTrinh(DanhMucCoQuanDonViModel DanhMucCoQuanDonViModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CoQuanDonVi_SuaCoQuanDonVi, EnumLogType.Update, () =>
                {
                    string Message = "";
                    int val = 0;
                    int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    DanhMucCoQuanDonViModel.CoQuanID = CoQuanID;
                    if (DanhMucCoQuanDonViModel.QuyTrinhID == 1)
                    {
                        DanhMucCoQuanDonViModel.SuDungQuyTrinh = true;
                        DanhMucCoQuanDonViModel.SuDungQuyTrinhGQ = true;
                    }
                    else
                    {
                        DanhMucCoQuanDonViModel.SuDungQuyTrinh = false;
                        DanhMucCoQuanDonViModel.SuDungQuyTrinhGQ = false;
                    }
                    val = _DanhMucCoQuanDonViBUS.UpdateQuyTrinh(DanhMucCoQuanDonViModel, ref Message);
                    base.Message = Message;
                    base.Status = val;
                    //base.Data = Data;
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

        [Route("GetByID")]
        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetByID(int? CoQuanID)
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.DanhMuc_CoQuanDonVi_GetByID, EnumLogType.GetList, () =>
                {
                    DanhMucCoQuanDonViPartialNew Data;
                    int CoQuanDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    Data = _DanhMucCoQuanDonViBUS.GetByID_New(CoQuanID ?? CoQuanDangNhapID);
                    if(Data != null)
                    {
                        if(Data.SuDungQuyTrinh == true)
                        {
                            Data.QuyTrinhID = 1;
                        }
                        else Data.QuyTrinhID = 2;
                    }
                    base.Status = Data.CoQuanID > 0 ? 1 : 0;
                    base.Data = Data;
                    base.Message = Data.CoQuanID > 0 ? ConstantLogMessage.API_Success : ConstantLogMessage.API_Error;
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