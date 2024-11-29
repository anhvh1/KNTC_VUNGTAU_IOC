using System.Data;
using System.Data.SqlClient;
using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Com.Gosol.KNTC.API.Controllers.DanhMuc
{
    [Route("api/v2/DanhMucFile")]
    [ApiController]
    public class DanhMucFileController : BaseApiController
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment host;
        private IConfiguration _config;
        private IOptions<AppSettings> _appSettings;
        private DanhMucFileBUS _danhMucFileBUS;

        public DanhMucFileController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
            IConfiguration config, ILogHelper _LogHelper, ILogger<DanhMucDanTocController> logger,
            IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this.host = hostingEnvironment;
            this._config = config;
            this._appSettings = Settings;
            _danhMucFileBUS = new DanhMucFileBUS();
        }

        [HttpGet]
        [Route("DanhSachNhomFile")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSach([FromQuery] ThamSoLocDanhMuc thamSo)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _danhMucFileBUS.DanhSachNhomFile(thamSo);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("DanhSachFile")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSachFile([FromQuery] ThamSoFileModel thamSo)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _danhMucFileBUS.DanhSachFile(thamSo);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("ChiTietNhomFile")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult ChiTietNhomFile(int nhomFileID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _danhMucFileBUS.ChiTietNhomFile(nhomFileID);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("ChiTietFile")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult ChiTietFile(int fileID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _danhMucFileBUS.ChiTietFile(fileID);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        [HttpGet]
        [Route("DanhSachChucNang")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSachChucNang()
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _danhMucFileBUS.DanhSachChucNang();
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
        [Route("ThemNhomFile")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucFileDinhKem, AccessLevel.Create)]
        public IActionResult ThemNhomFile(ThemNhomFileModel nhomFile)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _danhMucFileBUS.ThemNhomFile(nhomFile);
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
        [Route("CapNhatFile")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucFileDinhKem, AccessLevel.Edit)]
        public IActionResult CapNhatFile(UpdateFileModel file)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _danhMucFileBUS.UpdateFile(file);
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
        [Route("XoaNhomFile")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucFileDinhKem, AccessLevel.Delete)]
        public IActionResult XoaNhomFile(DeleteFileModel fileModel)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0); 
                var Result = _danhMucFileBUS.XoaNhomFile(fileModel.NhomFileID);
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
        [Route("CapNhatNhomFile")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucFileDinhKem, AccessLevel.Edit)]
        public IActionResult UpdateNhomFile(DanhMucNhomFileModel nhomFile)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _danhMucFileBUS.UpdateNhomFile(nhomFile);
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
        [Route("ThemFile")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucFileDinhKem, AccessLevel.Create)]
        public IActionResult ThemFile(ThemFileModel files)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _danhMucFileBUS.ThemFile(files);
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
        [Route("XoaFile")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucFileDinhKem, AccessLevel.Delete)]
        public IActionResult XoaFile(XoaFileModel file)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _danhMucFileBUS.XoaFile(file.FileID);
                return Ok(Result);
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