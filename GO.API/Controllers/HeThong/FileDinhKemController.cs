using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Controllers;
using Com.Gosol.KNTC.API.Controllers.HeThong;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using GroupDocs.Viewer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace GO.API.Controllers.HeThong
{
    [Route("api/v2/FileDinhKem")]
    [ApiController]
    public class FileDinhKemController : BaseApiController
    {
        private FileDinhKemBUS _FileDinhKemBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IOptions<AppSettings> _AppSettings;
        public FileDinhKemController(IHostingEnvironment HostingEnvironment, IOptions<AppSettings> Settings, ILogHelper _LogHelper, ILogger<FileDinhKemController> logger) : base(_LogHelper, logger)
        {
            this._FileDinhKemBUS = new FileDinhKemBUS();
            this._host = HostingEnvironment;
            this._AppSettings = Settings;
        }

        //[HttpPost]
        //[Route("Insert")]
        //[CustomAuthAttribute(0, AccessLevel.Create)]
        //public async Task<IActionResult> Insert(IList<IFormFile> files, [FromForm] string FileDinhKemStr)
        //{
        //    try
        //    {
        //        int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
        //        var FileDinhKem = JsonConvert.DeserializeObject<FileDinhKemModel>(FileDinhKemStr);
        //        var clsCommon = new Commons();
        //        if (files != null && files.Count > 0 && FileDinhKem != null)
        //        {
        //            FileDinhKem.NguoiCapNhat = CanBoID;          
        //            List<FileDinhKemModel> ListFileUrl = new List<FileDinhKemModel>();
        //            foreach (IFormFile source in files)
        //            {
        //                var file = await clsCommon.InsertFileAsync(source, FileDinhKem, _host);
        //                if (file != null && file.FileID > 0) 
        //                {
        //                    ListFileUrl.Add(file);
        //                }
        //            }
        //            if(ListFileUrl.Count == 0)
        //            {
        //                base.Status = 0;
        //                base.Message = "Thêm mới file đính kèm không thành công";
        //            }
        //            else
        //            {
        //                base.Data = ListFileUrl;
        //                base.Status = 1;
        //                base.Message = "Thêm mới file đính kèm thành công";
        //            }        
        //        }
        //        else
        //        {
        //            base.Status = 0;
        //            base.Message = "Vui lòng chọn file đính kèm";
        //        }
        //        return base.GetActionResult();
        //    }
        //    catch (Exception ex)
        //    {
        //        base.Status = -1;
        //        base.Message = ConstantLogMessage.API_Error_System;
        //        base.GetActionResult();
        //        throw ex;
        //    }
        //}

        [HttpPost]
        [Route("Insert")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        public IActionResult Insert(IList<IFormFile> files, [FromForm] string FileDinhKemStr)
        {
            try
            {
                int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var FileDinhKem = JsonConvert.DeserializeObject<FileDinhKemModel>(FileDinhKemStr);
                var clsCommon = new Commons();
                if (files != null && files.Count > 0 && FileDinhKem != null)
                {
                    FileDinhKem.NguoiCapNhat = CanBoID;
                    List<FileDinhKemModel> ListFileUrl = new List<FileDinhKemModel>();
                    foreach (IFormFile source in files)
                    {
                        var file = clsCommon.InsertFile(source, FileDinhKem, _host);
                        if (file != null && file.FileID > 0)
                        {
                            ListFileUrl.Add(file);
                        }
                    }
                    if (ListFileUrl.Count == 0)
                    {
                        base.Status = 0;
                        base.Message = "Thêm mới file đính kèm không thành công";
                    }
                    else
                    {
                        base.Data = ListFileUrl;
                        base.Status = 1;
                        base.Message = "Thêm mới file đính kèm thành công";
                    }
                }
                else
                {
                    base.Status = 0;
                    base.Message = "Vui lòng chọn file đính kèm";
                }
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                base.GetActionResult();
                throw ex;
            }
        }

        [HttpPost]
        [Route("Update")]
        [CustomAuthAttribute(0, AccessLevel.Edit)]
        public async Task<IActionResult> Update(List<int> ListFileDinhKemID, int? XuLyDonID, int? DonThuID)
        {
            try
            {
                if (ListFileDinhKemID != null && ListFileDinhKemID.Count > 0)
                {
                    int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    Data = _FileDinhKemBUS.UpdateFileHoSo(ListFileDinhKemID, XuLyDonID ?? 0, DonThuID ?? 0);
                    base.Data = ListFileDinhKemID;
                    base.Status = 1;
                    base.Message = "Thêm mới file đính kèm thành công";
                }
                else
                {
                    base.Status = 0;
                    base.Message = "Vui lòng chọn file đính kèm";
                }
                return base.GetActionResult();
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                base.GetActionResult();
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetByID")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetByID([FromQuery] int FileDinhKemID, int FileType)
        {
            try
            {
                int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                FileDinhKemModel Data = new FileDinhKemModel();
                Data = _FileDinhKemBUS.GetByID(FileDinhKemID, FileType);
                if (Data != null && Data.FileID > 0)
                {
                    base.Status = 1;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    Data.FileUrl = serverPath + Data.FileUrl;
                }
                else
                {
                    base.Status = 0;
                }
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        [Route("GetByNghiepVuID")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult GetByNghiepVuID([FromQuery] int NghiepVuID, int FileType)
        {
            try
            {
                int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                List<FileDinhKemModel> Data = new List<FileDinhKemModel>();
                Data = _FileDinhKemBUS.GetByNgiepVuID(NghiepVuID, FileType);
                if (Data != null && Data.Count > 0)
                {
                    base.Status = 1;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    foreach (var item in Data)
                    {
                        item.FileUrl = serverPath + item.FileUrl;
                    }
                }
                else
                {
                    base.Status = 0;
                }
                base.Data = Data;
                return base.GetActionResult();
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }
        }

        [HttpPost]
        [Route("Delete")]
        [CustomAuthAttribute(0, AccessLevel.Delete)]
        public IActionResult Delete(List<FileDinhKemModel> p)
        {
            try
            {
                int nguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _FileDinhKemBUS.Delete(p);
                base.Status = Result.Status;
                base.Message = Result.Message;
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

    }
}
