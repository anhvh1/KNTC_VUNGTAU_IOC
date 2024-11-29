using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Com.Gosol.KNTC.API.Controllers.HeThong
{
    [Route("api/v2/HeThongCanBo")]
    [ApiController]
    public class HeThongCanBoController : BaseApiController
    {
        private HeThongCanBoBUS _HeThongCanBoBUS;
        private PhanQuyenBUS _PhanQuyenBUS;
        private HeThongNguoidungBUS _HeThongNguoidungBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IOptions<AppSettings> _AppSettings;
        public HeThongCanBoController(Microsoft.AspNetCore.Hosting.IHostingEnvironment HostingEnvironment, IOptions<AppSettings> Settings, ILogHelper _LogHelper, ILogger<HeThongCanBoController> logger) : base(_LogHelper, logger)
        {
            this._HeThongNguoidungBUS = new HeThongNguoidungBUS();
            this._HeThongCanBoBUS = new HeThongCanBoBUS();
            this._PhanQuyenBUS = new PhanQuyenBUS();
            this._host = HostingEnvironment;
            this._AppSettings = Settings;
        }


        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.QuanLyNguoiDung, AccessLevel.Create)]
        [Route("Insert")]
        public IActionResult Insert(HeThongCanBoModel HeThongCanBoModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_ThemCanBo, EnumLogType.Insert, () =>
                {
                    string Message = null;
                    int val = 0;
                    int CanBoID = 0;
                    var crCoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    val = _HeThongCanBoBUS.Insert(HeThongCanBoModel, ref CanBoID, ref Message, crCoQuanID, NguoiDungID, CanBoDangNhapID);
                    if (val > 0) base.Status = 1;
                    base.Message = Message;
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
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("DangKy")]
        public IActionResult DangKy(HeThongCanBoModel HeThongCanBoModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_ThemCanBo, EnumLogType.Insert, () =>
                {
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    string Message = null;
                    int val = 0;
                    val = _HeThongCanBoBUS.InsertNguoiDangKy(HeThongCanBoModel, ref Message);
                    if (val > 0)
                    {
                        base.Status = 1;
                        base.Data = val;
                    }
                    else
                    {
                        base.Status = 0;
                    }
                    base.Message = Message;
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
        public IActionResult Update(HeThongCanBoModel HeThongCanBoModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_SuaCanBo, EnumLogType.Update, () =>
                {
                    string Message = null;
                    int val = 0;
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    val = _HeThongCanBoBUS.Update(HeThongCanBoModel, ref Message, NguoiDungID);
                    if (val > 0)
                    {
                        base.Status = 1;
                        base.Message = Message;
                    }
                    else
                    {
                        base.Status = 0;
                        base.Message = Message;
                    }
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
        [Route("UpdateCu")]
        public async Task<IActionResult> UpdateAsync(IList<IFormFile> files, [FromForm] string HeThongCanBoModelStr)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_CanBo_SuaCanBo, EnumLogType.Update, () =>
                //{
                var HeThongCanBoModel = JsonConvert.DeserializeObject<HeThongCanBoModel>(HeThongCanBoModelStr);

                string Message = null;
                int val = 0;
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                val = _HeThongCanBoBUS.Update(HeThongCanBoModel, ref Message, NguoiDungID);
                if (val > 0 && files != null && files.Count > 0)
                {
                    var crCanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                }
                base.Message = Message;
                base.Status = val > 0 ? 1 : 0;
                base.Data = Data;
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


        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.QuanLyNguoiDung, AccessLevel.Delete)]
        [Route("Delete")]
        public IActionResult Delete([FromBody] BaseDeleteParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_XoaCanBo, EnumLogType.Delete, () =>
                 {
                     var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                     var Result = _HeThongCanBoBUS.Delete(p.ListID, NguoiDungID);
                     if (Result.Count > 0)
                     {
                         //base.Message = "Lỗi!";
                         var mess = "";
                         foreach (var item in Result)
                         {
                             mess = mess + item + "\n";
                         }
                         base.Message = mess;
                         base.Data = Result;
                         base.Status = 0;
                         return base.GetActionResult();
                     }
                     else
                     {
                         base.Message = "Xóa thành công!";
                         base.Data = Result;
                         base.Status = 1;
                         return base.GetActionResult();
                     }
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
        public IActionResult GetCanBoByID([FromQuery] int CanBoID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_GetByID, EnumLogType.GetByID, () =>
                 {
                     var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                     HeThongCanBoModel Data;
                     Data = _HeThongCanBoBUS.GetCanBoByID(CanBoID);
                     if (Data != null && Data.CanBoID > 0)
                     {
                         base.Status = Data.CanBoID > 0 ? 1 : 0;
                         base.Data = Data;
                     }
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
        [Route("GetListPaging")]
        public IActionResult GetPagingBySearch([FromQuery] BasePagingParams p, int? CoQuanID, int? TrangThaiID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_GetListPaging, EnumLogType.Insert, () =>
                 {
                     var clsCommon = new Commons();
                     var host = clsCommon.GetServerPath(HttpContext);
                     IList<HeThongCanBoModel> Data;
                     int TotalRow = 0;
                     var CoQuan_ID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == "CoQuanID").Value, 0);
                     var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == "CanBoID").Value, 0);
                     Data = _HeThongCanBoBUS.GetPagingBySearch(p, ref TotalRow, CoQuanID, TrangThaiID, CoQuan_ID, NguoiDungID, host);
                     if (Data.Count == 0)
                     {
                         base.Message = ConstantLogMessage.API_NoData;
                         base.Status = 1;
                         base.TotalRow = 0;
                         base.Data = Data;
                         return base.GetActionResult();
                     }
                     //List<CanBoResult> canBoResults = new List<CanBoResult>();
                     //foreach (var item in Data)
                     //{
                     //    canBoResults.Add(new CanBoResult(item));
                     //}
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


        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.QuanLyNguoiDung, AccessLevel.Create)]
        [Route("ReadExcelFile")]
        public async Task<IActionResult> ReadExcelFile(IList<IFormFile> files)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_CanBo_ImportFile, EnumLogType.Other, async () =>
                //{
                string SavePath = _host.ContentRootPath + "\\Upload\\" + "CanBo.xlsx";
                if (System.IO.File.Exists(SavePath))
                {
                    System.IO.File.Delete(SavePath);
                }
                foreach (IFormFile source in files)
                {
                    using (FileStream output = System.IO.File.Create(SavePath))
                        await source.CopyToAsync(output);
                }
                var coQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                var CanBoDangNhapID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                return base.GetActionResult();
                //});

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("DowloadExel")]
        public async Task<IActionResult> DowloadExel()
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_ExportFile, EnumLogType.Other, () =>
                {
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    //var host = _host.ContentRootPath;
                    var clsCommon = new Commons();
                    var host = clsCommon.GetServerPath(HttpContext);
                    //var expath = host + "\\Upload\\CanBo_Template.xlsm";
                    var expath = "Upload\\NhanVien_Template.xlsm";
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetThanNhanByCanBoID")]
        public IActionResult GetThanNhanByCanBoID() // lấy thân nhân của cán bộ đang đăng nhập
        {
            try
            {
                return CreateActionResult("Lấy thân nhân theo cán bộ", EnumLogType.Insert, () =>
                {
                    List<HeThongCanBoShortModel> list = new List<HeThongCanBoShortModel>();
                    list = _HeThongCanBoBUS.GetThanNhanByCanBoID(Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == "CanBoID").Value, 0));
                    base.Status = 1;
                    Data = list;
                    base.Message = "Danh sách thân nhân";
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
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetAllCanBoByCoQuanID")]
        public IActionResult GetAllCanBoByCoQuanID(int? CoQuanID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_CanBo_GetListPaging, EnumLogType.Other, () =>
                {
                    var Result = _HeThongCanBoBUS.GetAllCanBoByCoQuanID(CoQuanID ?? 0, Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("CoQuanID")).Value, 0));
                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetAllCanBoTrongCoQuan")]
        public IActionResult GetAllCanBoTrongCoQuan()
        {
            try
            {
                return CreateActionResult("Lấy tất cả cán bộ trong cơ quan", EnumLogType.Other, () =>
                {
                    var Result = _HeThongCanBoBUS.GetAllByCoQuanID(Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("CoQuanID")).Value, 0));
                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GenerationMaCanBo")]
        public IActionResult GenerationMaCanBo([FromQuery] int CoQuanID)
        {
            try
            {
                return CreateActionResult("Tạo mã bởi cơ quan", EnumLogType.GetByID, () =>
                {
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var Data = _HeThongCanBoBUS.GenerationMaCanBo(CoQuanID);
                    if (string.IsNullOrEmpty(Data))
                    {
                        base.Status = 0;
                        base.Data = Data;
                        return base.GetActionResult();

                    }
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
        [HttpGet]
        [CustomAuthAttribute(ChucNangEnum.QuanLyNguoiDung, AccessLevel.FullAKNTCess)]
        [Route("GetAllInCoQuanCha")]
        public IActionResult GetAllInCoQuanCha([FromQuery] int? CoQuanID)
        {
            try
            {
                return CreateActionResult("Lấy tất cả cán bộ trong cơ quan", EnumLogType.Other, () =>
                {
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var Result = _HeThongCanBoBUS.GetAllInCoQuanCha(CoQuanID.Value);
                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("HeThongCanBo_GetThongTinCoQuan")]
        public IActionResult HeThongCanBo_GetThongTinCoQuan()
        {
            try
            {
                return CreateActionResult("Lấy thông tin cơ quan của cán bộ", EnumLogType.Other, () =>
                {
                    var CanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("CanBoID")).Value, 0);
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("NguoiDungID")).Value, 0);
                    var Result = _HeThongCanBoBUS.HeThongCanBo_GetThongTinCoQuan(CanBoID, NguoiDungID);
                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy thông tin cán bộ và thân nhân cán bộ theo CanBoID
        /// </summary>
        /// <param name="CanBoID"></param>
        /// <returns></returns>


        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("ThongTinCanBo_GetThongTinCanBo")]
        public IActionResult HeThongCanBo_GetThongTinCoQuan(int CanBoID)
        {
            try
            {
                return CreateActionResult("Lấy thông tin của cán bộ", EnumLogType.GetByID, () =>
                {
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    ThongTinCanBoModel Result = new ThongTinCanBoModel();
                    Result.ThongTinCanBo = _HeThongCanBoBUS.GetCanBoByID(CanBoID);
                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>


        [HttpPost]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("ThongTinCanBo_UpdateThongTinCanBo")]
        public async Task<IActionResult> ThongTinCanBo_UpdateThongTinCanBo(IFormFile AnhHoSo, [FromForm] string NghiepVuID, [FromForm] string ThongTinCanBo, [FromForm] string ListThongTinThanNhan, [FromForm] string ListDeleteConCai)
        {
            int resCanBo = 1;
            var resThanNhan = new BaseResultModel();
            var resDeleteConCai = new BaseResultModel();
            var resAnhHoSo = "";
            string Message = string.Empty;
            if (ThongTinCanBo != null)
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var HeThongCanBoModel = JsonConvert.DeserializeObject<HeThongCanBoModel>(ThongTinCanBo);
                resCanBo = _HeThongCanBoBUS.Update(HeThongCanBoModel, ref Message, NguoiDungID);
            }
            if (ListThongTinThanNhan != null)
            {
                //var ListThanNhanCanBoModel = JsonConvert.DeserializeObject<List<KeKhaiThanNhanModel>>(ListThongTinThanNhan);
                //var listInsert = ListThanNhanCanBoModel.Where(x => x.ThanNhanID == null || x.ThanNhanID < 1).ToList();
                //var listUpdate = ListThanNhanCanBoModel.Where(x => x.ThanNhanID != null && x.ThanNhanID > 0).ToList();
                //if (listInsert.Count > 0) { resThanNhan = _KeKhaiThanNhanBUS.InsertAll(listInsert); }
                //if (listUpdate.Count > 0) { resThanNhan = _KeKhaiThanNhanBUS.UpdateAll(listUpdate); }

                if (resThanNhan.Status < 1)
                {
                    Message = resThanNhan.Message;
                }
            }
            else
            {
                resThanNhan.Status = 1;
            }
            if (ListDeleteConCai != null)
            {
                List<int> listDeleteConCai = JsonConvert.DeserializeObject<List<int>>(ListDeleteConCai);
                //resDeleteConCai = _KeKhaiThanNhanBUS.Delete(listDeleteConCai);
            }
            else
            {
                resDeleteConCai.Status = 1;
            }
            base.Message = Message;
            base.Status = (resCanBo > 0 && resThanNhan.Status > 0 && resDeleteConCai.Status > 0 && resAnhHoSo != "") ? 1 : 0;
            base.Data = Data;
            return base.GetActionResult();
        }


        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetListCanBoPhuHopVoiThoiGianCaLamviec")]
        public IActionResult GetListCanBoPhuHopVoiThoiGianCaLamviec(TimeSpan? ThoiGianBatDau, TimeSpan? ThoiGianKetThuc, int? CoQuanID_Filter)
        {
            try
            {
                return CreateActionResult("Lấy danh sách cán bộ phù hợp với thời gian ca làm việc", EnumLogType.Other, () =>
                {
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("CoQuanID")).Value, 0);
                    var Result = _HeThongCanBoBUS.GetListCanBoPhuHopVoiThoiGianCaLamviec(ThoiGianBatDau, ThoiGianKetThuc, CoQuanID, CoQuanID_Filter);
                    base.Data = Result;
                    base.Status = 1;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("CheckTaiKhoan")]
        public IActionResult CheckTaiKhoan(HeThongCanBoModel HeThongCanBoModel)
        {
            try
            {
                return CreateActionResult("CheckTaiKhoan", EnumLogType.Other, () =>
                {
                    var CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == ("CoQuanID")).Value, 0);

                    var NguoiDung = _HeThongNguoidungBUS.GetByName(HeThongCanBoModel.TenNguoiDung, 0);
                    if (NguoiDung.NguoiDungID > 0)
                    {
                        List<string> TaiKhoanHopLe = new List<string>();
                        if (HeThongCanBoModel.TenNguoiDung.Contains("@") && HeThongCanBoModel.TenNguoiDung.Contains(".com"))
                        {
                            var tenArray = HeThongCanBoModel.TenNguoiDung.Split('@');
                            var tenGoiY = tenArray[0];
                            Random rd = new Random();
                            while (TaiKhoanHopLe.Count < 3)
                            {
                                var tenHopLe = tenGoiY.ToLower() + rd.Next(100, 999);
                                var nd = _HeThongNguoidungBUS.GetByName(tenHopLe, 0);
                                if (nd.NguoiDungID == 0) TaiKhoanHopLe.Add(tenHopLe);
                            }
                        }
                        else
                        {
                            if (HeThongCanBoModel.TenCanBo != null && HeThongCanBoModel.TenCanBo.Length > 0)
                            {
                                var ten = Utils.NonUnicode(HeThongCanBoModel.TenCanBo.Trim());
                                var tenArray = ten.Split(' ');
                                var tenGoiY = tenArray[tenArray.Length - 1];
                                for (int i = 0; i < tenArray.Length; i++)
                                {
                                    if (i != tenArray.Length - 1)
                                    {
                                        tenGoiY += tenArray[i][0];
                                    }
                                }
                                Random rd = new Random();
                                while (TaiKhoanHopLe.Count < 3)
                                {
                                    var tenHopLe = tenGoiY.ToLower() + rd.Next(100, 999);
                                    var nd = _HeThongNguoidungBUS.GetByName(tenHopLe, 0);
                                    if (nd.NguoiDungID == 0) TaiKhoanHopLe.Add(tenHopLe);
                                }

                            }
                        }
                        base.Data = TaiKhoanHopLe;
                        base.Status = 0;
                        base.Message = "Tài khoản đã tồn tại";
                    }
                    else
                    {
                        base.Status = 1;
                    }
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetAllForLichSuChamCong")]
        public IActionResult GetAllForLichSuChamCong()
        {
            try
            {
                return CreateActionResult("Lấy danh sách nhân viên cho màn hình lịch sử chấm công", EnumLogType.GetList, () =>
                {
                    var CoQuan_ID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == "CoQuanID").Value, 0);
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == "NguoiDungID").Value, 0);
                    var Data = _HeThongCanBoBUS.GetAllForLichSuChamCong(CoQuanID, NguoiDungID);
                    base.Message = ConstantLogMessage.API_NoData;
                    base.Status = 1;
                    base.Data = Data;
                    base.TotalRow = Data.Count;
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
