//using Com.Gosol.KKTS.BUS.KeKhai;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Controllers;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Com.Gosol.KKTS.API.Controllers
{
    [Route("api/v2/HuongDanSuDung")]
    [ApiController]
    public class HuongDanSuDungController : BaseApiController
    {
        private SystemConfigBUS _SystemConfigBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private HuongDanSuDungBUS _HuongDanSuDungBUS;
        private FileDinhKemBUS _FileDinhKemBUS;
        public HuongDanSuDungController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, ILogHelper _LogHelper, ILogger<HuongDanSuDungController> logger) :
            base(_LogHelper, logger)
        {
            _HuongDanSuDungBUS = new HuongDanSuDungBUS();
            this._FileDinhKemBUS = new FileDinhKemBUS();
            this._host = hostingEnvironment;
        }

        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetAll")]
        public IActionResult GetAll([FromQuery] BasePagingParamsForFilter p)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_HuongDanSuDung_GetListPaging, EnumLogType.GetList, () =>
                //{
                int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<HuongDanSuDungModel> Data;
                Data = _HuongDanSuDungBUS.GetPagingBySearch(p, ref TotalRow);

                //var pathSaveFile = _SystemConfigBUS.GetByKey("UploadFile_Path").ConfigValue;
                //var folderName = Path.Combine(pathSaveFile);
                //for (int i = 0; i < Data.Count; i++)
                //{
                //    Data[i].UrlFile = Path.Combine(_host.WebRootPath + folderName, Data[i].TenFileHeThong);

                //}

                /*var cmClass = new Commons();
                var pathSaveFile = _SystemConfigBUS.GetByKey("UploadFile_Path").ConfigValue;
                var folderName = Path.Combine(pathSaveFile);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);*/

                //for (int i = 0; i < Data.Count; i++)
                //{
                //var base64 = string.Empty;
                //var fullPath = Path.Combine(pathToSave, Data[i].TenFileHeThong);
                //base64 = "data:application/pdf;base64" + "," + cmClass.ConvertFileToBase64(fullPath);
                //if (base64 != string.Empty && base64.Length > 0)
                //{
                //    Data[i].Base64String = base64;
                //    base.Status = 1;
                //    base.Message = string.Empty;
                //}
                //Data[i].UrlFile = Path.Combine(_host.WebRootPath + folderName, Data[i].TenFileHeThong);
                //Data[i].UrlFile = "Upload/FileDinhKemDuyetKeKhai/" + Data[i].TenFileHeThong;
                //Data[i].UrlFile = "HuongDanSuDung/taifile?HuongDanSuDungID=" + Data[i].HuongDanSuDungID;
                // }


                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
                return base.GetActionResult();
                //});
            }
            catch (Exception e)
            {
                base.Status = -1;
                base.Message = e.ToString();
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetAllChucNang")]
        public IActionResult GetAllChucNang()
        {
            try
            {
                int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _HuongDanSuDungBUS.DanhSach_ChucNang();

                return Ok(Result);
                //});
            }
            catch (Exception e)
            {
                base.Status = -1;
                base.Message = e.ToString();
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("GetListMenuByCap")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        public IActionResult GetListMenu([FromQuery] int? CapID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_ChucNang_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    int capID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var Data = _HuongDanSuDungBUS.GetListMenuByNguoiDungID(nguoiDungID, CapID ?? capID);
                    base.Status = 1;
                    base.TotalRow = Data.Count;
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

        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetListPaging")]
        public IActionResult GetListPaging([FromQuery] BasePagingParamsForFilter p)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_HuongDanSuDung_GetListPaging, EnumLogType.GetList, () =>
                //{
                int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                int TotalRow = 0;
                IList<HuongDanSuDungModel> Data;
                Data = _HuongDanSuDungBUS.GetPagingBySearch(p, ref TotalRow);
                if (Data != null && Data.Count > 0)
                {
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    foreach (var item in Data)
                    {
                        var listFile = _FileDinhKemBUS.GetByNgiepVuID(item.HuongDanSuDungID, EnumLoaiFile.FileHuongDanSuDung.GetHashCode());
                        if (listFile.Count > 0)
                        {
                            item.UrlFile = clsCommon.GetServerPath(HttpContext) + listFile[0].FileUrl;
                        }
                    }
                }

                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
                return base.GetActionResult();
                //});
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }
        }

        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.HuongDanSuDung, AccessLevel.Create)]
        [Route("Insert")]
        public async Task<IActionResult> InsertAsync(IFormFile files, [FromForm] string HuongDanSuDungModelStr)
        {
            try
            {
                var HuongDanSuDungModel = JsonConvert.DeserializeObject<HuongDanSuDungModel>(HuongDanSuDungModelStr);

                var checkFile = _HuongDanSuDungBUS.CheckFile(files);
                if (checkFile.Status != 1)
                {
                    base.Status = checkFile.Status;
                    base.Message = checkFile.Message;
                    return base.GetActionResult();
                }

                FileDinhKemModel FileDinhKem = new FileDinhKemModel
                {
                    FileType = EnumLoaiFile.FileHuongDanSuDung.GetHashCode(),
                    NguoiCapNhat = CanBoID
                };

                if (HuongDanSuDungModel != null)
                {
                    BaseResultModel Result = new BaseResultModel();

                    Result = _HuongDanSuDungBUS.ValidateInsert(HuongDanSuDungModel);

                    if (Result.Status != 1)
                    {
                        base.Status = Result.Status;
                        base.Message = Result.Message;
                        return base.GetActionResult();
                    }


                    //HuongDanSuDungModel.TenFileGoc = files.FileName;
                    //HuongDanSuDungModel.TenFileHeThong = file.TenFile;

                    Result = _HuongDanSuDungBUS.Insert(HuongDanSuDungModel, CanBoID);
                    FileDinhKem.NghiepVuID = Utils.ConvertToInt32(Result.Data, 0);

                    var clsCommon = new Commons();
                    var file = await clsCommon.InsertFileAsync(files, FileDinhKem, _host);
                    base.Status = Result.Status;
                    base.Message = Result.Message;
                    return base.GetActionResult();
                }

                base.Status = 0;
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }

        [HttpPost]
        [CustomAuth(ChucNangEnum.HuongDanSuDung, AccessLevel.Edit)]
        [Route("Update")]
        public async Task<IActionResult> UpdateAsync(IFormFile? files, [FromForm] string HuongDanSuDungModelStr)
        {
            try
            {
                var HuongDanSuDungModel = JsonConvert.DeserializeObject<HuongDanSuDungModel>(HuongDanSuDungModelStr);
                if (HuongDanSuDungModel == null)
                    return BadRequest();
                BaseResultModel Result = _HuongDanSuDungBUS.ValidateUpdate(HuongDanSuDungModel);
                if (Result.Status != 1)
                {
                    return Ok(Result);
                }

                FileDinhKemModel FileDinhKem = new FileDinhKemModel
                {
                    FileType = EnumLoaiFile.FileHuongDanSuDung.GetHashCode(),
                    NguoiCapNhat = CanBoID
                };

                // Cập nhật dữ liệu
                Result = _HuongDanSuDungBUS.Update(HuongDanSuDungModel, CanBoID);
                FileDinhKem.NghiepVuID = Utils.ConvertToInt32(HuongDanSuDungModel.HuongDanSuDungID, 0);
                if (files != null)
                {
                    //xoa file cu
                    var listFile = _FileDinhKemBUS.GetByNgiepVuID(HuongDanSuDungModel.HuongDanSuDungID, EnumLoaiFile.FileHuongDanSuDung.GetHashCode());
                    if (listFile.Count > 0)
                    {
                        _FileDinhKemBUS.Delete(listFile);
                    }

                    var clsCommon = new Commons();
                    var file = await clsCommon.InsertFileAsync(files, FileDinhKem, _host);

                }

                base.Status = Result.Status;
                base.Message = Result.Message;

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


        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.HuongDanSuDung, AccessLevel.Delete)]
        [Route("Delete")]
        public IActionResult Delete([FromBody] BaseDeleteParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_HuongDanSuDung_Xoa, EnumLogType.Delete, () =>
                {
                    // var crCanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value , 0);
                    var Result = _HuongDanSuDungBUS.Delete(p.ListID, 20);
                    base.Status = Result.Status;
                    base.Message = Result.Message;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ex.ToString();
                base.Data = "";
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetByID_Old")]
        public IActionResult GetByID_Old(int HuongDanSuDungID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_HuongDanSuDung_GetByID, EnumLogType.GetByID, () =>
                {
                    int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    var Data = _HuongDanSuDungBUS.GetByID(HuongDanSuDungID);
                    if (Data == null || Data.HuongDanSuDungID < 1)
                    {
                        base.Message = ConstantLogMessage.API_NoData;
                        base.Status = 0;
                    }
                    else
                    {
                        //var cmClass = new Commons();
                        base.Message = " ";
                        base.Status = 1;
                        var base64 = string.Empty;
                        //var pathSaveFile = _SystemConfigBUS.GetByKey("UploadFile_Path").ConfigValue;
                        //var folderName = Path.Combine(pathSaveFile);
                        //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        //var fullPath = Path.Combine(pathToSave, Data.TenFileHeThong);
                        //base64 = "data:application/pdf;base64" + "," + cmClass.ConvertFileToBase64(fullPath);

                        var clsCommon = new Commons();
                        /*var listFile = _FileDinhKemBUS.GetAllField_FileDinhKem_ByNghiepVuID_AndType(HuongDanSuDungID, EnumLoaiFileDinhKem.HuongDanSuDung.GetHashCode());
                        if (listFile.Count > 0)
                        {
                            Data.UrlFile = clsCommon.GetServerPath(HttpContext) + listFile[0].FileUrl;
                            base64 = "data:application/pdf;base64" + "," + clsCommon.ConvertFileToBase64(listFile[0].FileUrl);
                        }*/

                        if (base64 != string.Empty && base64.Length > 0)
                        {
                            Data.Base64String = base64;
                            base.Status = 1;
                            base.Message = string.Empty;
                        }
                        else
                        {
                            base.Status = 0;
                            base.Message = "Bản cứng không tồn tại";
                        }
                    }

                    base.Data = Data;
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetByID")]
        public IActionResult GetByID(int HuongDanSuDungID)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_HuongDanSuDung_GetByID, EnumLogType.GetByID, () =>
                //{
                int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Data = _HuongDanSuDungBUS.GetByID(HuongDanSuDungID);
                if (Data == null || Data.HuongDanSuDungID < 1)
                {
                    base.Message = ConstantLogMessage.API_NoData;
                    base.Status = 0;
                    base.Data = new List<HuongDanSuDungModel>();
                }
                else
                {
                    //var cmClass = new Commons();
                    base.Message = " ";
                    base.Status = 1;
                    var base64 = string.Empty;
                    var clsCommon = new Commons();
                    var listFile = _FileDinhKemBUS.GetByNgiepVuID(Data.HuongDanSuDungID, EnumLoaiFile.FileHuongDanSuDung.GetHashCode());
                    if (listFile.Count > 0)
                    {
                        Data.UrlFile = clsCommon.GetServerPath(HttpContext) + listFile[0].FileUrl;
                    }
                    base.Data = Data;
                }

                return base.GetActionResult();
                //});
            }
            catch (Exception e)
            {
                base.Status = -1;
                base.Message = e.ToString();
                return base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetByMaChucNang")]
        public IActionResult GetByMaChucNang(string? MaChucNang, int? CapID)
        {
            try
            {
                int capID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                int capHanhChinh = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapHanhChinh").Value, 0);
                int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var banTiepDan = Utils.ConvertToBoolean(User.Claims.FirstOrDefault(c => c.Type == "BanTiepDan").Value, false);
                if (capID == CapQuanLy.CapUBNDTinh.GetHashCode())
                {
                    if (banTiepDan)
                    {
                        capHanhChinh = EnumCapHanhChinhHDSD.BTDTinh.GetHashCode();
                    }
                }
                else if (capHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode())
                {
                    if (banTiepDan)
                    {
                        capHanhChinh = EnumCapHanhChinhHDSD.BTDHuyen.GetHashCode();
                    }
                }
                var id = capHanhChinh;

                var Data = _HuongDanSuDungBUS.GetByMaChucNang(MaChucNang, CapID ?? id);
                if (Data == null || Data.HuongDanSuDungID < 1)
                {
                    base.Message = ConstantLogMessage.API_NoData;
                    base.Status = 0;
                    base.Data = new List<HuongDanSuDungModel>();
                }
                else
                {
                    base.Message = "";
                    base.Status = 1;
                    var base64 = string.Empty;
                    var clsCommon = new Commons();
                    var listFile = _FileDinhKemBUS.GetByNgiepVuID(Data.HuongDanSuDungID, EnumLoaiFile.FileHuongDanSuDung.GetHashCode());
                    if (listFile.Count > 0)
                    {
                        Data.UrlFile = clsCommon.GetServerPath(HttpContext) + listFile[0].FileUrl;
                        //base64 = "data:application/pdf;base64" + "," + clsCommon.ConvertFileToBase64(listFile[0].FileUrl);
                    }
                    base.Data = Data;
                }

                return base.GetActionResult();
                //});
            }
            catch (Exception e)
            {
                base.Status = -1;
                base.Message = e.ToString();
                return base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetByMaChucNang_Old")]
        public IActionResult GetByMaChucNang_Old(string MaChucNang)
        {
            try
            {
                return CreateActionResult(true, ConstantLogMessage.HT_HuongDanSuDung_GetByMaChucNang, EnumLogType.GetByID, () =>
                {
                    int capID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CapID").Value, 0);
                    var Data = _HuongDanSuDungBUS.GetByMaChucNang(MaChucNang, capID);
                    if (Data == null || Data.HuongDanSuDungID < 1)
                    {
                        base.Message = ConstantLogMessage.API_NoData;
                        base.Status = 0;
                    }
                    else
                    {
                        var cmClass = new Commons();
                        base.Message = " ";
                        base.Status = 1;
                        var stringBase64 = string.Empty;
                        var pathSaveFile = _SystemConfigBUS.GetByKey("UploadFile_Path").ConfigValue;
                        var folderName = Path.Combine(pathSaveFile);
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        var fullPath = Path.Combine(pathToSave, Data.TenFileHeThong);
                        stringBase64 = cmClass.ConvertFileToBase64(fullPath);

                        if (stringBase64 != string.Empty && stringBase64.Length > 0)
                        {
                            Data.Base64String = "data:application/pdf;base64" + "," + stringBase64;
                            base.Status = 1;
                            base.Message = string.Empty;
                        }
                        else
                        {
                            base.Status = 0;
                            base.Message = "Bản cứng không tồn tại";
                        }
                    }

                    base.Data = Data;
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }
        }




        //[HttpGet]
        ////[CustomAuthAttribute(ChucNangEnum.HeThong_HuongDanSuDung, AccessLevel.Read)]
        //[Route("TaiFile")]
        //public async Task<IActionResult> TaiFile(int HuongDanSuDungID)
        //{
        //    try
        //    {
        //        //return CreateActionResult(ConstantLogMessage.HT_HuongDanSuDung_GetByID, EnumLogType.GetByID, () =>
        //        //{
        //        var Data = _HuongDanSuDungBUS.GetByID(HuongDanSuDungID);
        //        if (Data == null || Data.HuongDanSuDungID < 1)
        //        { base.Message = ConstantLogMessage.API_NoData; base.Status = 0; return null; }
        //        else
        //        {
        //            var cmClass = new Commons();
        //            base.Message = " "; base.Status = 1;
        //            var base64 = string.Empty;
        //            var pathSaveFile = _SystemConfigBUS.GetByKey("UploadFile_Path").ConfigValue;
        //            var folderName = Path.Combine(pathSaveFile);
        //            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //            var fullPath = Path.Combine(pathToSave, Data.TenFileHeThong);
        //            var memory = new MemoryStream();
        //            using (var stream = new FileStream(fullPath, FileMode.Open))
        //            {
        //                await stream.CopyToAsync(memory);
        //            }
        //            memory.Position = 0;
        //            return File(memory, GetContentType(fullPath));
        //        }
        //        //base.Data = Data;
        //        //return base.GetActionResult();
        //        //});
        //    }
        //    catch (Exception)
        //    {
        //        base.Status = -1;
        //        return base.GetActionResult();
        //        throw;
        //    }
        //}

        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("TaiFile")]
        public IActionResult TaiFile(int HuongDanSuDungID)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_HuongDanSuDung_GetByID, EnumLogType.GetByID, () =>
                //{
                int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Data = _HuongDanSuDungBUS.GetByID(HuongDanSuDungID);
                if (Data == null || Data.HuongDanSuDungID < 1)
                {
                    base.Message = ConstantLogMessage.API_NoData;
                    base.Status = 0;
                    return null;
                }
                else
                {
                    var cmClass = new Commons();
                    base.Message = " ";
                    base.Status = 1;
                    var base64 = string.Empty;
                    var pathSaveFile = _SystemConfigBUS.GetByKey("UploadFile_Path").ConfigValue;
                    var folderName = Path.Combine(pathSaveFile);
                    //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    //var fullPath = Path.Combine(pathToSave, Data.TenFileHeThong);
                    var fullPath = Path.Combine(folderName, Data.TenFileHeThong);
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(fullPath, FileMode.Open))
                    {
                        stream.CopyTo(memory);
                    }

                    memory.Position = 0;
                    return File(memory, GetContentType(fullPath));
                }
                //base.Data = Data;
                //return base.GetActionResult();
                //});
            }
            catch (Exception ex)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw ex;
            }
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".doc", "application/vnd.ms-word" },
                { ".docx", "application/vnd.ms-word" },
                { ".xls", "application/vnd.ms-excel" },
                //{".xlsx", "application/vnd.openxmlformats
                //           officedocument.spreadsheetml.sheet"},
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".gif", "image/gif" },
                { ".csv", "text/csv" }
            };
        }


        //[HttpPost]
        ////[CustomAuthAttribute(ChucNangEnum.KeKhai_TaiSan, AccessLevel.Create)]
        //[Route("UploadBanCung")]
        //public IActionResult UploadBanCung([FromBody] PostFile HuongDanSuDung)
        //{
        //    try
        //    {
        //        var crCanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
        //        return CreateActionResult(ConstantLogMessage.KK_HuongDanSuDung_Them, EnumLogType.Insert, () =>
        //         {
        //             if (HuongDanSuDung.KeKhaiID != null && HuongDanSuDung.ListBase64.Count > 0)
        //             {
        //                 var DanhSachFileDaCo = _HuongDanSuDungBUS.GetListHuongDanSuDungByKeKhaiID(HuongDanSuDung.KeKhaiID).Where(x => x.TrangThai == 1).ToList();
        //                 for (int i = 0; i < DanhSachFileDaCo.Count; i++)
        //                 {
        //                     if (HuongDanSuDung.ListBase64.Any(x => x.TenFile == DanhSachFileDaCo[i].TenFileGoc))
        //                     {
        //                         base.Status = 0;
        //                         base.Message = "Tên file đã tồn tại";
        //                         return base.GetActionResult();
        //                     }

        //                 }
        //                 for (int i = 0; i < HuongDanSuDung.ListBase64.Count; i++)
        //                 {
        //                     var TenFileGoc = HuongDanSuDung.ListBase64[i].TenFile;
        //                     var FileType = HuongDanSuDung.ListBase64[i].Base64.Split(',')[0];
        //                     var crObj = new HuongDanSuDungModel();
        //                     crObj.NgayCapNhat = DateTime.Now;
        //                     crObj.NguoiCapNhat = crCanBoID;
        //                     crObj.KeKhaiID = HuongDanSuDung.KeKhaiID;
        //                     crObj.TenFileHeThong = crCanBoID.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + "_" + TenFileGoc;
        //                     crObj.TenFileGoc = TenFileGoc;
        //                     crObj.FileType = FileType;
        //                     crObj.Base64String = HuongDanSuDung.ListBase64[i].Base64;
        //                     var Result = _HuongDanSuDungBUS.Insert(crObj);
        //                     if (Result.Status < 1)
        //                     {
        //                         base.Status = 0;
        //                         base.Message = "Không thể đính kèm file";
        //                     }
        //                     var crFile = new FileModel();
        //                     crFile.TenFile = crObj.TenFileGoc;
        //                     crFile.Base64 = HuongDanSuDung.ListBase64[i].Base64;
        //                     SaveBase64ToFile(crFile, crObj.TenFileHeThong);
        //                 }
        //                 base.Status = 1;
        //                 base.Message = "Thêm mới file đính kèm thành công";
        //             }
        //             else
        //             {
        //                 base.Status = 0;
        //                 base.Message = "Vui lòng chọn file đính kèm";
        //             }
        //             return base.GetActionResult();
        //         });
        //    }
        //    catch (Exception ex)
        //    {
        //        base.Status = -1;
        //        base.Message = ConstantLogMessage.API_Error_System;
        //        base.GetActionResult();
        //        throw ex;
        //    }
        //}

        //[HttpGet]
        ////[CustomAuthAttribute(ChucNangEnum.DanhMuc_TrangThai, AccessLevel.Read)]
        //[Route("GetByID")]
        //public IActionResult GetByID(int HuongDanSuDungID)
        //{
        //    try
        //    {
        //        return CreateActionResult(ConstantLogMessage.KK_HuongDanSuDung_GetByID, EnumLogType.GetList, () =>
        //        {
        //            HuongDanSuDungModel Data = new HuongDanSuDungModel();
        //            Data = _HuongDanSuDungBUS.GetByID(HuongDanSuDungID);
        //            if (Data == null || Data.KeKhaiID == null || Data.KeKhaiID < 1)
        //            {
        //                base.Status = 0;
        //                base.Message = "Bản cứng không tồn tại";
        //            }
        //            //if (Data != null && Data.FileID > 0 && Data.TenFileHeThong != null && Data.TenFileHeThong.Length > 0)
        //            //{
        //            //    var folderName = Path.Combine("Upload", "HuongDanSuDungDuyetKeKhai");
        //            //    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //            //    var pathFile = Path.Combine(pathToSave, Data.TenFileHeThong);
        //            //    if (!System.IO.File.Exists(pathFile))
        //            //    {
        //            //        base.Status = 0;
        //            //        base.Message = "File không tồn tại";
        //            //    }
        //            //    var Base64String = ConvertFileToBase64(pathFile);
        //            //    if (Base64String != string.Empty && Base64String.Length > 0)
        //            //    {
        //            //        Data.Base64String = string.Concat(Data.FileType + "," + Base64String);
        //            //    }
        //            //}
        //            else
        //            {
        //                var base64 = string.Empty;
        //                var pathSaveFile = _SystemConfigBUS.GetByKey("UploadFile_Path").ConfigValue;
        //                var folderName = Path.Combine(pathSaveFile);
        //                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //                var fullPath = Path.Combine(pathToSave, Data.TenFileHeThong);
        //                base64 = Data.FileType + "," + ConvertFileToBase64(fullPath);
        //                if (base64 != string.Empty && base64.Length > 0)
        //                {
        //                    Data.Base64String = base64;
        //                    base.Status = 1;
        //                    base.Message = string.Empty;
        //                }
        //                else
        //                {
        //                    base.Status = 0;
        //                    base.Message = "Bản cứng không tồn tại";
        //                }

        //            }
        //            base.Data = Data;
        //            return base.GetActionResult();
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        base.Status = -1;
        //        return base.GetActionResult();
        //        throw;
        //    }
        //}


        //[HttpPost]
        ////[CustomAuthAttribute(ChucNangEnum.DanhMuc_TrangThai, AccessLevel.Delete)]
        //[Route("Delete")]
        //public IActionResult Delete([FromBody] BaseDeleteParams p)
        //{
        //    try
        //    {
        //        return CreateActionResult(ConstantLogMessage.KK_HuongDanSuDung_Xoa, EnumLogType.Delete, () =>
        //        {
        //            var Result = _HuongDanSuDungBUS.Delete(p.ListID);
        //            base.Status = Result.Status;
        //            base.Message = Result.Message;
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


        //[HttpGet]
        ////[CustomAuthAttribute(ChucNangEnum.DanhMuc_TrangThai, AccessLevel.Read)]
        //[Route("GetAllField_HuongDanSuDung_ByKeKhaiID")]
        //public IActionResult GetAllField_HuongDanSuDung_ByKeKhaiID(int KeKhaiID)
        //{
        //    try
        //    {
        //        return CreateActionResult(ConstantLogMessage.KK_HuongDanSuDung_GetListPaging, EnumLogType.GetList, () =>
        //        {
        //            List<HuongDanSuDungModel> Data = new List<HuongDanSuDungModel>();
        //            Data = _HuongDanSuDungBUS.GetAllField_HuongDanSuDung_ByKeKhaiID(KeKhaiID);
        //            base.Status = 1;
        //            base.Message = string.Empty;
        //            base.Data = Data;
        //            return base.GetActionResult();
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        base.Status = -1;
        //        return base.GetActionResult();
        //        throw;
        //    }
        //}


        /// <summary>
        /// chưa chùng
        /// </summary>
        /// <param name="HuongDanSuDungID"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("DownloadFile")]
        //public async Task<IActionResult> DownloadFile([FromQuery] int HuongDanSuDungID)
        //{
        //    try
        //    {
        //        HuongDanSuDungModel Data = new HuongDanSuDungModel();
        //        Data = _HuongDanSuDungBUS.GetByID(HuongDanSuDungID);
        //        if (Data != null && Data.FileID > 0 && Data.TenFileHeThong != null && Data.TenFileHeThong.Length > 0)
        //        {
        //            var folderName = Path.Combine("Upload", "HuongDanSuDungDuyetKeKhai");
        //            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //            var pathFile = Path.Combine(pathToSave, Data.TenFileHeThong);
        //            var memory = new MemoryStream();
        //            using (var stream = new FileStream(pathFile, FileMode.Open))
        //            {
        //                await stream.CopyToAsync(memory);
        //            }
        //            memory.Position = 0;
        //            return File(memory, GetContentType(pathFile), Data.TenFileHeThong);
        //        }
        //        else return null;
        //    }
        //    catch (Exception)
        //    {
        //        base.Status = -1;
        //        return base.GetActionResult();
        //        throw;
        //    }
        //}


        //[HttpPost]
        ////[CustomAuthAttribute(ChucNangEnum.KeKhai_TaiSan, AccessLevel.Create)]
        //[Route("UploadBanCung_UploadFile")]
        //public IActionResult UploadBanCung_UploadFile([FromBody] PostFile HuongDanSuDung)
        //{
        //    try
        //    {
        //        //var b64 = HuongDanSuDung.ListBase64[0];
        //        //b64 = b64.Split(',')[1];
        //        //byte[] bytes = Convert.FromBase64String(b64);
        //        //var folderName = Path.Combine("Upload", "HuongDanSuDungDuyetKeKhai");
        //        //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        //var fullPath = Path.Combine(pathToSave, "xxx.png");
        //        //System.IO.File.WriteAllBytes(fullPath, bytes);
        //        //  int KeKhaiID = 0;
        //        var crCanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
        //        return CreateActionResult(ConstantLogMessage.KK_HuongDanSuDung_Them, EnumLogType.Insert, () =>
        //        {
        //            // gọi hàm upload file
        //            if (HuongDanSuDung.KeKhaiID != null && HuongDanSuDung.ListBase64.Count > 0)
        //            {
        //                for (int i = 0; i < HuongDanSuDung.ListBase64.Count; i++)
        //                {
        //                    var TenFileGoc = HuongDanSuDung.ListBase64[i].TenFile;
        //                    var FileType = HuongDanSuDung.ListBase64[i].Base64.Split(',')[0];
        //                    var crObj = new HuongDanSuDungModel();
        //                    crObj.NgayCapNhat = DateTime.Now;
        //                    crObj.NguoiCapNhat = crCanBoID;
        //                    crObj.KeKhaiID = HuongDanSuDung.KeKhaiID;
        //                    crObj.TenFileHeThong = crCanBoID.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + "_" + TenFileGoc;
        //                    crObj.TenFileGoc = TenFileGoc;
        //                    crObj.FileType = FileType;
        //                    var isHuongDanSuDung = _HuongDanSuDungBUS.Insert(crObj);
        //                    if (isHuongDanSuDung.Status > 0)
        //                    {
        //                        var upload = UploadFileFromBase64(HuongDanSuDung.ListBase64[i], crObj.TenFileHeThong);
        //                        if (!upload)
        //                        {
        //                            base.Status = 0;
        //                            base.Message = "Không thể upload file";
        //                            break;
        //                        }
        //                    }
        //                }
        //                base.Status = 1;
        //                base.Message = "Thêm mới file đính kèm thành công";
        //            }
        //            else
        //            {
        //                base.Status = 0;
        //                base.Message = "Vui lòng chọn file đính kèm";
        //            }
        //            return base.GetActionResult();
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        base.Status = -1;
        //        base.GetActionResult();
        //        throw ex;
        //    }
        //}


        /// <summary>
        /// save base64 to file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="TenFileHeThong"></param>
        /// <returns></returns>
        //public bool SaveBase64ToFile(FileModel file, string TenFileHeThong)
        //{
        //    try
        //    {
        //        if (file.Base64.Length > 0)
        //        {
        //            var pathSaveFile = _SystemConfigBUS.GetByKey("UploadFile_Path").ConfigValue;
        //            var b64 = file.Base64;
        //            b64 = b64.Split(',')[1];
        //            byte[] bytes = Convert.FromBase64String(b64);
        //            //var folderName = Path.Combine("Upload", "HuongDanSuDungDuyetKeKhai");
        //            var folderName = Path.Combine(pathSaveFile);
        //            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //            var fullPath = Path.Combine(pathToSave, TenFileHeThong);
        //            System.IO.File.WriteAllBytes(fullPath, bytes);
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //        throw ex;
        //    }
        //}

        //public string ConvertFileToBase64(string pathFile)
        //{
        //    try
        //    {
        //        var at = System.IO.File.GetAttributes(pathFile);

        //        byte[] fileBit = System.IO.File.ReadAllBytes(pathFile);
        //        var file = System.IO.Path.Combine(pathFile);

        //        string AsBase64String = Convert.ToBase64String(fileBit);
        //        return AsBase64String;
        //    }
        //    catch (Exception ex)
        //    {
        //        return string.Empty;
        //        throw ex;
        //    }
        //}

        #region  V2

        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("GetAll_v2")]
        public IActionResult GetAll_V2([FromQuery] HuongDanSuDungParams p)
        {
            try
            {
                var root = this.HttpContext.Request.Scheme + "://" + this.HttpContext.Request.Host.Value + "/api/v1/HuongDanSuDung/TaiFile_v2?filename=";
                var data = _HuongDanSuDungBUS.DanhSach_v2(p, root);

                return Ok(data);
            }
            catch (Exception e)
            {
                base.Status = -1;
                base.Message = e.ToString();
                return base.GetActionResult();
            }
        }
        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        [Route("ChiTiet")]
        public IActionResult ChiTiet([Required] int HuongDanSuDungID)
        {
            try
            {
                var root = this.HttpContext.Request.Scheme + "://" + this.HttpContext.Request.Host.Value + "/api/v1/HuongDanSuDung/TaiFile_v2?filename=";
                var data = _HuongDanSuDungBUS.ChiTiet(HuongDanSuDungID, root);

                return Ok(data);
            }
            catch (Exception e)
            {
                base.Status = -1;
                base.Message = e.ToString();
                return base.GetActionResult();
            }
        }
        [HttpGet("TaiFile_v2")]
        public IActionResult TaiFile_v2([Required] string filename)
        {
            string path = $"/UploadFiles/FileHuongDanSuDung/{filename}";
            try
            {
                var filePath = _host.ContentRootPath + path;
                //GO.API/UploadFiles/FileBieuMau/0_2022-11-04-094159_1.docx
                /*Console.WriteLine(Directory.GetCurrentDirectory());

                new FileExtensionContentTypeProvider().TryGetContentType(path2, out var contentType);

                Console.WriteLine(contentType);*/

                var bytes = System.IO.File.ReadAllBytes(filePath);
                return File(bytes, "application/pdf", filename);
            }
            catch (Exception)
            {
                return Ok("url error or file not exists");
            }
        }
        [HttpPost]
        [CustomAuth(ChucNangEnum.HuongDanSuDung, AccessLevel.Delete)]
        [Route("Xoa")]
        public IActionResult Xoa_v2(HuongDanSuDungXoa thamso)
        {
            try
            {
                var Result = _HuongDanSuDungBUS.Xoa_v2(thamso.HuongDanSuDungID, _host);

                return Ok(Result);
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }

        }

        #endregion
    }
}