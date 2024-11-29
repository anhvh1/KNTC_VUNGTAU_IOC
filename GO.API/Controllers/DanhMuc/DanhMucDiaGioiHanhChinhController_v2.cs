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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GO.API.Controllers.DanhMuc
{
    [Route("api/v2/DanhMucDiaGioiHanhChinh_V2")]
    [ApiController]
    public class DanhMucDiaGioiHanhChinhController_v2 : BaseApiController
    {
        private DanhMucDiaGioiHanhChinhBUS_v2 DanhMucDiaGioiHanhChinhBUS_v2;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public DanhMucDiaGioiHanhChinhController_v2(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<DanhMucDiaGioiHanhChinhController_v2> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this.DanhMucDiaGioiHanhChinhBUS_v2 = new DanhMucDiaGioiHanhChinhBUS_v2();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        // get all theo cap
        [HttpGet]
        [Route("DanhSachCacCap/Tinh/Huyen/Xa")]
        public IActionResult GetAllByCap([FromQuery] ThamSoLocDanhMuc1 thamSoLocDanhMuc1)
        {

            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Data = DanhMucDiaGioiHanhChinhBUS_v2.GetAllByCap(thamSoLocDanhMuc1);

                base.Message = "Thanh cong";
                base.Status = 1;
                base.Data = Data;

                return base.GetActionResult();
            }
            catch (Exception)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }

        }
        // get by id ang cap
        [HttpGet]
        [CustomAuthAttribute(0, AccessLevel.Read)]
        [Route("GetByIDAndCap/Tinh/Huyen/Xa")]
        public IActionResult GetByIDAnfCap([FromQuery] ThamSoLocDanhMuc1 thamSoLocDanhMuc1)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = DanhMucDiaGioiHanhChinhBUS_v2.GetDGHCByIDAndCap(thamSoLocDanhMuc1);
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
        // insert dia gioi hanh chinh
        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Create)]
        [Route("InsertDiaGioiHanhChinhNew")]
        public IActionResult InsertDiaGioiHanhChinh([FromBody] DanhMucDiaGioiHanhChinhMODPartial_v2 DanhMucDiaGioiHanhChinhMODPartial_v2)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_DiaGioiHanhChinh_ThemDGHC, EnumLogType.Insert, () =>
                {
                    if (string.IsNullOrEmpty(DanhMucDiaGioiHanhChinhMODPartial_v2.TenDayDu) || DanhMucDiaGioiHanhChinhMODPartial_v2.TenDayDu.Trim().Length <= 0)
                    {
                        base.Message = ConstantLogMessage.API_Error_NotFill;
                        base.Status = 0;
                        base.Data = Data;
                        return base.GetActionResult();
                    }
                    if (string.IsNullOrEmpty(DanhMucDiaGioiHanhChinhMODPartial_v2.Ten) || DanhMucDiaGioiHanhChinhMODPartial_v2.Ten.Trim().Length <= 0)
                    {
                        base.Message = ConstantLogMessage.API_Error_NotFill;
                        base.Status = 0;
                        base.Data = Data;
                        return base.GetActionResult();
                    }
                    if (!Utils.CheckSpecialCharacter(DanhMucDiaGioiHanhChinhMODPartial_v2.Ten))
                    {
                        base.Message = ConstantLogMessage.API_Error_NotSpecialCharacter;
                        base.Status = 0;
                        base.Data = Data;
                        return base.GetActionResult();
                    }
                    Dictionary<int, int> dic = new Dictionary<int, int>();
                    int ID = 0;
                    var crInsert = new DanhMucDiaGioiHanhChinhMOD_v2();
                    crInsert.TenTinh = DanhMucDiaGioiHanhChinhMODPartial_v2.Ten;
                    crInsert.TenHuyen = DanhMucDiaGioiHanhChinhMODPartial_v2.Ten;
                    crInsert.TenXa = DanhMucDiaGioiHanhChinhMODPartial_v2.Ten;
                    crInsert.TenDayDu = DanhMucDiaGioiHanhChinhMODPartial_v2.TenDayDu;

                    if (DanhMucDiaGioiHanhChinhMODPartial_v2.Cap == 1)
                    {
                        dic = DanhMucDiaGioiHanhChinhBUS_v2.InsertTinh(crInsert, ref ID);
                        var id = dic.FirstOrDefault().Value; // luu y
                    }
                    else if (DanhMucDiaGioiHanhChinhMODPartial_v2.Cap == 2)
                    {
                        crInsert.TinhID = DanhMucDiaGioiHanhChinhMODPartial_v2.ParentID.Value;
                        dic = DanhMucDiaGioiHanhChinhBUS_v2.InsertHuyen(crInsert, ref ID);
                        
                    }
                    else if (DanhMucDiaGioiHanhChinhMODPartial_v2.Cap == 3)
                    {
                        crInsert.HuyenID = DanhMucDiaGioiHanhChinhMODPartial_v2.ParentID.Value;
                        dic = DanhMucDiaGioiHanhChinhBUS_v2.InsertXa(crInsert, ref ID);
                        
                    }
                    if (dic.FirstOrDefault().Key == -1) { base.Message = ConstantLogMessage.API_Error_System; }
                    else if (dic.FirstOrDefault().Key == 0) { base.Message = ConstantLogMessage.Alert_Error_NotExist("Địa giới hành chính"); }
                    else if (dic.FirstOrDefault().Key == 2) { base.Message = ConstantLogMessage.Alert_Error_NotExist("Parent ID"); }
                    else if (dic.FirstOrDefault().Key == 1) { base.Message = ConstantLogMessage.Alert_Insert_Success("Địa giới hành chính"); }
                    base.Data = dic.FirstOrDefault().Value;
                    base.Status = dic.FirstOrDefault().Key;
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
        [Route("XoaDanhMucTheoCap/Tinh/Huyen/Xa")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Delete)]
        public IActionResult Xoa( int ID, int Cap)
        {

            if (ID == null || Cap == null) return BadRequest();
            var crDelete = new DanhMucDiaGioiHanhChinhMOD_v2();
            crDelete.TinhID = ID;
            crDelete.HuyenID = ID;
            crDelete.XaID = ID;
            var Result1 = DanhMucDiaGioiHanhChinhBUS_v2.XoaTinh(ID);
            var Result2 = DanhMucDiaGioiHanhChinhBUS_v2.XoaHuyen(ID);
            var Result3 = DanhMucDiaGioiHanhChinhBUS_v2.XoaXa(ID);
            
            if(Cap == 1)
            {
                return Ok(Result1);
            }else if(Cap == 2)
            {
                return Ok(Result2);
            }
            else if(Cap == 3)
            {
                return Ok(Result3);
            }else
            {
                return BadRequest();
            }

        }

        //-- cap nhat 

        [HttpPost]
        [Route("CapNhatDanhMucTheoCap / 1 / 2/ 3")]
        [CustomAuthAttribute(ChucNangEnum.DanhMucCoQuan, AccessLevel.Create)]
        public IActionResult CapNhatCapBac213([FromBody] DanhMucDiaGioiHanhChinhMODPartial_v2 item )
        {
            try
            {
                if (item == null) return BadRequest();
                var Result1 = DanhMucDiaGioiHanhChinhBUS_v2.CapNhatTinh(item);
                //return Ok(Result1);
                var Result2 = DanhMucDiaGioiHanhChinhBUS_v2.CapNhatHuyen(item);
                var Result3 = DanhMucDiaGioiHanhChinhBUS_v2.CapNhatXa(item);

                if (item.Cap == 1)
                {
                    return Ok(Result1);
                }
                if (item.Cap == 2)
                {
                    return Ok(Result2);
                }
                if (item.Cap == 3)
                {
                    return Ok(Result3);
                }
                else return NotFound();
            }
            catch (Exception)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
            }
            
        }


    }
}
