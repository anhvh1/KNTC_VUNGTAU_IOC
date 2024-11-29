using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Controllers.DanhMuc;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GO.API.Controllers.DanhMuc
{

  
    public class DanhMucCoQuanDonViController_v2 : BaseApiController
    {
        private DanhMucCoQuanDonViBUS_v2 _DanhMucCoQuanDonViBUS_v2;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public DanhMucCoQuanDonViController_v2(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DanhMucCoQuanDonViController_v2> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._DanhMucCoQuanDonViBUS_v2 = new DanhMucCoQuanDonViBUS_v2();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }
       
        // get all
        [HttpGet]
        [Route("api/v2/DanhMucCoQuanDonVi/DanhSachCacCap")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSach(ThamSoLocDanhMuc_v2 p)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var obj = new DanhMucCoQuanDonViMODPartial();

                var Result = _DanhMucCoQuanDonViBUS_v2.DanhSach(p);
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
        // THEM 

        [HttpPost]
        [Route("api/v2/DanhMucCoQuanDonVi/ThemMoiCoQuanDonVi")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Create)]
        public IActionResult ThemMoi([FromBody] AddDanhMucCoQuanMOD item)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                if (item == null) return BadRequest();
                var Result = _DanhMucCoQuanDonViBUS_v2.ThemMoi(item);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }
        // xoa
        [HttpGet]
        [Route("api/v2/DanhMucCoQuanDonVi/XoaCoQuanDonVi")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Delete)]
        public IActionResult Xoa( int? CoQuanID)
        {
            var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
            if (CoQuanID == null) return BadRequest();
            var Result = _DanhMucCoQuanDonViBUS_v2.Xoa(CoQuanID);
            if (Result != null) return Ok(new
            {
               
                Result
            });
            else return NotFound();
        }
        [HttpGet]
        [Route("api/v2/DanhMucCoQuanDonVi/SearchCoQuan")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult SearchName([FromBody] string Name)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _DanhMucCoQuanDonViBUS_v2.SearchName(Name);
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
        // update
        [HttpPost]
        [Route("api/v2/DanhMucCoQuanDonVi/CapNhatCoQuan")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Edit)]
        public IActionResult CapNhat([FromBody] UpdateDanhMucCoQuanMOD item)
        {
            try
            {
                if (item == null) return BadRequest();
                var Result = _DanhMucCoQuanDonViBUS_v2.CapNhat(item);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
        }

        //--------
        [HttpGet]
        [Route("api/v2/DanhMucCoQuanDonVi/ChiTietCoQuanID")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult ChiTiet( int? ID)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _DanhMucCoQuanDonViBUS_v2.ChiTiet(ID);
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

        //-------------
        [HttpGet]
        [Route("/api/v2/DanhMucCoQuanDonVi/DanhSachCacCapDonVi")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSachCap( ThamSoLocDanhMuc_v2 p)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var obj = new NameCapCoQuanID();

                var Result = _DanhMucCoQuanDonViBUS_v2.DanhSachCap(p);
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
        [Route("/api/v2/DanhMucCoQuanDonVi/DanhSachCacCapDonVi_HDSD")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSachCap_HDSD(ThamSoLocDanhMuc_v2 p)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var obj = new NameCapCoQuanID();

                var Result = _DanhMucCoQuanDonViBUS_v2.DanhSachCap_HDSD(p);
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

        //-------------
        [HttpGet]
        [Route("/api/v2/DanhMucCoQuanDonVi/DanhSachThamQuyen")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSachThamQuyen( ThamSoLocDanhMuc p)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var obj = new NameCapCoQuanID();

                var Result = _DanhMucCoQuanDonViBUS_v2.CacCapCoQuan(p);
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
        [Route("api/v2/DanhMucCoQuanDonVi/DanhSachCoQuanCha")]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        public IActionResult DanhSachCoQuanCha( ThamSoLocDanhMuc p)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var obj = new NameCapCoQuanID();

                var Result = _DanhMucCoQuanDonViBUS_v2.DanhSachCoQuanCha(p);
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
