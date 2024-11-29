using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Controllers;
using Com.Gosol.KNTC.API.Controllers.DanhMuc;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Com.Gosol.KNTC.BUS.BaoCao;
using Com.Gosol.KNTC.BUS.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.API.Authorization;

namespace GO.API.Controllers.BacCao
{
    [Route("api/v2/KeKhaiDuLieuDauKy")]
    [ApiController]
    public class KeKhaiDuLieuDauKyController : BaseApiController
    {
        private KeKhaiDuLieuDauKyBUS _KeKhaiDuLieuDauKyBUS;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private IConfiguration _config;
        private IOptions<AppSettings> _AppSettings;
        public KeKhaiDuLieuDauKyController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config, ILogHelper _LogHelper, ILogger<KeKhaiDuLieuDauKyController> logger, IOptions<AppSettings> Settings) : base(_LogHelper, logger)
        {
            this._KeKhaiDuLieuDauKyBUS = new KeKhaiDuLieuDauKyBUS();
            this._host = hostingEnvironment;
            this._config = config;
            this._AppSettings = Settings;
        }

        [HttpGet]
        [CustomAuthAttribute(ChucNangEnum.KeKhaiDuLieuDauKyTCD01, AccessLevel.Read)]
        [Route("GetDuLieuDauKy")]
        public IActionResult GetDuLieuDauKy([FromQuery] KeKhaiDuLieuDauKyParams p)
        {
            try
            {
                return CreateActionResult(false, "", EnumLogType.GetList, () =>
                {
                    int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    if (!UserRole.CheckAdmin(NguoiDungID))
                    {
                        p.CoQuanID = CoQuanID;
                    }
                    else
                    {
                        if(p.CoQuanID == null || p.CoQuanID == 0)
                        {
                            base.Status = 0;
                            base.Message = "Đơn vị không được để trống";
                            return base.GetActionResult();
                        }
                    }
                    BaseResultModel Data = new BaseResultModel();
                    if (p.LoaiBaoCao == 1)
                    {
                        Data = _KeKhaiDuLieuDauKyBUS.TCD01(p);
                    }
                    else if (p.LoaiBaoCao == 2)
                    {
                        Data = _KeKhaiDuLieuDauKyBUS.TCD02(p);
                    }
                    else if (p.LoaiBaoCao == 3)
                    {
                        Data = _KeKhaiDuLieuDauKyBUS.XLD01(p);
                    }
                    else if (p.LoaiBaoCao == 4)
                    {
                        Data = _KeKhaiDuLieuDauKyBUS.XLD02(p);
                    }
                    else if (p.LoaiBaoCao == 5)
                    {
                        Data = _KeKhaiDuLieuDauKyBUS.XLD03(p);
                    }
                    else if (p.LoaiBaoCao == 6)
                    {
                        Data = _KeKhaiDuLieuDauKyBUS.XLD04(p);
                    }
                    else if (p.LoaiBaoCao == 7)
                    {
                        Data = _KeKhaiDuLieuDauKyBUS.KQGQ01(p);
                    }
                    else if (p.LoaiBaoCao == 8)
                    {
                        Data = _KeKhaiDuLieuDauKyBUS.KQGQ02(p);
                    }
                    else if (p.LoaiBaoCao == 9)
                    {
                        Data = _KeKhaiDuLieuDauKyBUS.KQGQ03(p);
                    }
                    else if (p.LoaiBaoCao == 10)
                    {
                        Data = _KeKhaiDuLieuDauKyBUS.KQGQ04(p);
                    }
                    base.Status = Data.Status;
                    base.Data = Data.Data;
                    base.Message = Data.Message;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                //base.GetActionResult();
                //throw;
                base.Message = ex.Message;
                return base.GetActionResult();
            }
        }

        [HttpPost]
        [Route("Save")]
        [CustomAuthAttribute(ChucNangEnum.KeKhaiDuLieuDauKyTCD01, AccessLevel.Edit)]
        public IActionResult Save(List<TableData> DuLieuDauKy)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_Them, EnumLogType.Insert, () =>
                {
                    //List<TableData> data = new List<TableData>();
                    //data.Add(DuLieuDauKy);
                    int NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    int CoQuanID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    if (!UserRole.CheckAdmin(NguoiDungID))
                    {
                        foreach (var item in DuLieuDauKy)
                        {
                            item.CoQuanID = CoQuanID;
                        }
                    }
                    var Data = _KeKhaiDuLieuDauKyBUS.Save(DuLieuDauKy);
                    base.Data = Data.Data;
                    base.Status = Data.Status;
                    base.Message = Data.Message;
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

    }
}
