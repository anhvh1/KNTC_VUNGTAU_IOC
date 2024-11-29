using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.DanhMuc;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using GroupDocs.Viewer.Options;
using GroupDocs.Viewer.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;


//Create by NamNH - 24/10/2022
namespace Com.Gosol.KNTC.API.Controllers.DanhMuc
{
    [Route("api/v2/DanhMucBuocXacMinh")]
    [ApiController]
    public class DanhMucBuocXacMinhController : BaseApiController
    {
        private DanhMucBuocXacMinhBUS danhMucBuocXacMinhBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration config;
        private IOptions<AppSettings> appSettings;
        public DanhMucBuocXacMinhController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DanhMucBuocXacMinhController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this.danhMucBuocXacMinhBUS = new DanhMucBuocXacMinhBUS();
            this._host = hostingEnvironment;
            this.config = config;
            this.appSettings = Settings;
        }



        [HttpGet]
        [Route("DanhSachBuocXacMinh")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSach([FromQuery] ThamSoLocDanhMuc p, int? LoaiDon)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucBuocXacMinhBUS.DanhSach(p, LoaiDon);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("ChiTietBuocXacMinh")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult ChiTiet([FromQuery] int? BuocXacMinhID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucBuocXacMinhBUS.ChiTiet(BuocXacMinhID);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }


        [HttpPost]
        [Route("ThemMoiBuocXacMinh")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        public async Task<IActionResult> ThemMoi(IList<IFormFile> files, [FromForm] string DanhMucBuocXacMinhStr)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var DanhMucBuocXacMinh = JsonConvert.DeserializeObject<DanhMucBuocXacMinhThemMoiMOD>(DanhMucBuocXacMinhStr);
                var clsCommon = new Commons();
                //DanhMucBuocXacMinh.FileMau = new List<FileDinhKemModel>();

                if (DanhMucBuocXacMinh == null) return BadRequest();
                var Result = danhMucBuocXacMinhBUS.ThemMoi(DanhMucBuocXacMinh);
                if (Result is null)  return NotFound();
                var NghiepVuID = Utils.ConvertToInt32(Result.Data, 0);
                foreach (IFormFile source in files)
                {
                    FileDinhKemModel FileDinhKem = new FileDinhKemModel
                    {
                        FileType = 16,
                        NguoiCapNhat = 20,
                        NghiepVuID = NghiepVuID 
                    };
                    string TenFileGoc = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                    foreach (var item in DanhMucBuocXacMinh.FileMau)
                    {
                        if(item.TenFileGoc == TenFileGoc)
                        {
                            FileDinhKem.TenFile = item.TenFile;
                        }
                    }
                    var file = await clsCommon.InsertFileAsync(source, FileDinhKem, _host);
                    
                    //DanhMucBuocXacMinh.FileMau = file.FileUrl;
                    
                    if (file != null && file.FileID > 0)
                    {
                        DanhMucBuocXacMinh.FileMau.Add(file);
                    }
                }

                return Ok(Result);
            }
            catch (Exception)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }


        [HttpPost]
        [Route("CapNhatBuocXacMinh")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        public async Task<IActionResult> CapNhat(IList<IFormFile>? uploadFiles, [FromForm] string DanhMucBuocXacMinhCapNhatStr)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var BuocXacMinhCapNhat = JsonConvert.DeserializeObject<DanhMucBuocXacMinhCapNhatMOD>(DanhMucBuocXacMinhCapNhatStr);
                
                //DanhMucBuocXacMinh.FileMau = new List<FileDinhKemModel>();

                if (BuocXacMinhCapNhat == null) return BadRequest();
                var Result = danhMucBuocXacMinhBUS.CapNhat(BuocXacMinhCapNhat);
                if (Result is null) return NotFound();
                var NghiepVuID = Utils.ConvertToInt32(Result.Data, 0);

                int index = 0;
                foreach (IFormFile source in uploadFiles)
                {
                    int fileID = BuocXacMinhCapNhat.FileMau[index].FileID;
                    // Xóa file cũ
                    var model = new DanhMucBuocXacMinhDAL().ChiTietFile(fileID, "");
                    var fileName = ((CapNhatFileMOD)model.Data).FileURL;
                    var path = _host.ContentRootPath + fileName;
                    System.IO.File.Delete(path);
                    // Thêm file mới
                    FileDinhKemModel FileDinhKem = new FileDinhKemModel
                    {
                        FileID = fileID,
                        FileType = 16,
                        NguoiCapNhat = 20,
                        NghiepVuID = BuocXacMinhCapNhat.BuocXacMinhID
                    };
                    string TenFileGoc = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                    foreach (var item in BuocXacMinhCapNhat.FileMau)
                    {
                        if (item.TenFileGoc == TenFileGoc)
                        {
                            FileDinhKem.TenFile = item.TenFile;
                        }
                    }

                    var clsCommon = new Commons();
                    var file = await clsCommon.UpdateFileAsync(source, FileDinhKem, _host);
                    //BuocXacMinhCapNhat.FileMau = file.FileUrl;

                    if (file != null && file.FileID > 0)
                    {
                        BuocXacMinhCapNhat.FileMau.Add(file);
                    }

                    index++;
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }


        [HttpPost]
        [Route("XoaBuocXacMinh")]
        [CustomAuthAttribute(0, AccessLevel.Delete)]
        public IActionResult Xoa([FromBody] int? BuocXacMinhID)
        {
            var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
            if (BuocXacMinhID == null) return BadRequest();
            var Result = danhMucBuocXacMinhBUS.Xoa(BuocXacMinhID);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        /*[HttpPost]
        [Route("BuocXacMinhUploadFile")]
        //[CustomAuthAttribute(ChucNangEnum.GoManager, AccessLevel.Read)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                var fileStream = await danhMucBuocXacMinhBUS.SaveFile(file, "Upload/DMBuocXacMinh");
                //var fsResult = new FileStreamResult(fileStream, "application/pdf");

                return Ok(fileStream);
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ex.ToString();
                return base.GetActionResult();
            }
        }*/

        [HttpGet]
        [Route("Download")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DownloadFile(string FileName)
        {
            string path = $"/UploadFiles/DMBuocXacMinh/{FileName}";
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var filePath = _host.ContentRootPath + path;
                var bytes = System.IO.File.ReadAllBytes(filePath);

                new FileExtensionContentTypeProvider().TryGetContentType(FileName, out var contentType);

                return File(bytes, contentType ?? "application/octet-stream", FileName);
            }
            catch (Exception)
            {
                base.Status = -1;
                base.Message = "File không tồn tại!";
                return base.GetActionResult();
            }
        }

        //===========================================================//
        [HttpGet]
        [Route("DanhSachFileMau")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSachFile([FromQuery] int? DMBuocXacMinhID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucBuocXacMinhBUS.DanhSachFile(DMBuocXacMinhID);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("DanhSachFileID")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSachFileID([FromQuery] ThamSoLocDanhMuc p)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucBuocXacMinhBUS.DanhSachFileID(p);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("ChiTietFileMau")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult ChiTietFile([FromQuery] int? FileDanhMucBuocXacMinhID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucBuocXacMinhBUS.ChiTietFile(FileDanhMucBuocXacMinhID, "");
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        [HttpPost]
        [Route("ThemMoiFileMau")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        public async Task<IActionResult> ThemMoiFile(IList<IFormFile> files, [FromForm] string FileDMBuocXacMinhMODStr)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var FileDMBuocXacMinh = JsonConvert.DeserializeObject<ThemMoiFileMauMOD>(FileDMBuocXacMinhMODStr);
                var clsCommon = new Commons();
                if (files != null && files.Count > 0 && FileDMBuocXacMinh != null)
                {
                    //FileDMBuocXacMinh.NguoiCapNhat = CanBoID;
                    List<FileDinhKemModel> ListFileUrl = new List<FileDinhKemModel>();

                    int i = 0;
                    foreach (IFormFile source in files)
                    {
                        FileDinhKemModel FileDinhKem = new FileDinhKemModel
                        {
                            FileType = 16,
                            NguoiCapNhat = 20,
                            NghiepVuID = FileDMBuocXacMinh.themMoiFileMOD[i].DMBuocXacMinhID
                        };
                        string TenFileGoc = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                        foreach (var item in FileDMBuocXacMinh.themMoiFileMOD)
                        {
                            if (item.TenFileGoc == TenFileGoc)
                            {
                                FileDinhKem.TenFile = item.TenFile;
                            }
                        }
                        var file = await clsCommon.InsertFileAsync(source, FileDinhKem, _host);
                        if (file != null && file.FileID > 0)
                        {
                            ListFileUrl.Add(file);
                        }

                        i++;
                    }

                    base.Data = ListFileUrl;
                    base.Status = 1;
                    base.Message = "Thêm mới file mẫu thành công";
                }
                else
                {
                    base.Status = 0;
                    base.Message = "Vui lòng chọn file mẫu";
                }
                return base.GetActionResult();
                /*return BadRequest();
                var Result = danhMucBuocXacMinhBUS.ThemMoiFile(item);
                if (Result != null) return Ok(Result);
                else return NotFound();*/
            }
            catch (Exception ex)
            {
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        /*[HttpPost]
        [Route("CapNhapFileMau")]
        //[CustomAuthAttribute(ChucNangEnum.GoManager, AccessLevel.Read)]
        public IActionResult CapNhatFile([FromBody] CapNhatFileMOD item)
        {
            try
            {
                var Result = danhMucBuocXacMinhBUS.CapNhatFile(item);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }*/
        [HttpPost]
        [Route("CapNhatFileMau")]
        [CustomAuthAttribute(0, AccessLevel.Edit)]
        public async Task<IActionResult> CapNhatFile(IFormFile? UploadFile, [FromForm] string CapNhatFileStr)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                /*var CapNhatFile = JsonConvert.DeserializeObject<CapNhatFileMOD>(CapNhatFileStr);
                var clsCommon = new Commons();
                if (UploadFile != null)
                {
                    // xoa file cu
                    var model = danhMucBuocXacMinhBUS.ChiTietFile(CapNhatFile.FileDanhMucBuocXacMinhID, "");
                    var fileName = ((CapNhatFileMOD)model.Data).FileURL;
                    var path = _host.ContentRootPath + "\\UploadFiles\\DMBuocXacMinh" + fileName;
                    System.IO.File.Delete(path);
                    //
                }*/
                //return CreateActionResult(ConstantLogMessage.HT_HuongDanSuDung_Sua, EnumLogType.Update, () =>
                //{
                var CapNhatFile = JsonConvert.DeserializeObject<CapNhatFileMOD>(CapNhatFileStr);



                //var crCanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                if (UploadFile != null)
                {

                    //Xóa file cũ
                    var model = new DanhMucBuocXacMinhDAL().ChiTietFile(CapNhatFile.FileDanhMucBuocXacMinhID, "");
                    var fileName = ((CapNhatFileMOD)model.Data).FileURL;
                    var path = _host.ContentRootPath + fileName;
                    System.IO.File.Delete(path);
                    //Insert file mới
                    FileDinhKemModel FileDinhKem = new FileDinhKemModel
                    {
                        FileType = 16,
                        NguoiCapNhat = 20
                    };

                    string TenFileGoc = ContentDispositionHeaderValue.Parse(UploadFile.ContentDisposition).FileName.Trim('"');
                    if (CapNhatFile.TenFileGoc == TenFileGoc)
                    {
                        FileDinhKem.TenFile = CapNhatFile.TenFile;
                    }
                    var clsCommon = new Commons();
                    var file = await clsCommon.UpdateFileAsync(UploadFile, FileDinhKem, _host);
                    CapNhatFile.FileURL = file.FileUrl;
                }   
                var Result = danhMucBuocXacMinhBUS.CapNhatFile(CapNhatFile);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        [HttpPost]
        [Route("XoaFileMau")]
        [CustomAuthAttribute(0, AccessLevel.Delete)]
        public IActionResult XoaFile([FromBody] int FileDanhMucBuocXacMinhID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = danhMucBuocXacMinhBUS.XoaFile(FileDanhMucBuocXacMinhID);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }


    }
}