using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.API.Authorization;
using Com.Gosol.KNTC.Security;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Com.Gosol.KNTC.BUS.DanhMuc;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Com.Gosol.KNTC.API.Controllers.HeThong
{
    [Route("api/v2/QuanTriDuLieu")]
    [ApiController]
    public class QuanTriDuLieuController : BaseApiController
    {
        private QuanTriDuLieuBUS _QuanTriDuLieuBUS;
        private ILogHelper LogHelper;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        public QuanTriDuLieuController(Microsoft.AspNetCore.Hosting.IHostingEnvironment HostingEnvironment, ILogHelper logHelper) : base(logHelper)
        {
            this._QuanTriDuLieuBUS = new QuanTriDuLieuBUS();
            this.LogHelper = logHelper;
            this._host = HostingEnvironment;
        }

        [HttpGet]
        [Route("BackupDatabase")]
        [CustomAuthAttribute(ChucNangEnum.SaoLuuDuLieu, AccessLevel.FullAKNTCess)]
        public IActionResult BackupDatabase(string fileName)
        {
            try
            {
                //var tenCanBo = User.Claims.FirstOrDefault(t => t.Type == "TenCanBo").Value;
                int val = 0;
                val = _QuanTriDuLieuBUS.BackupData(fileName, _host.ContentRootPath);
                if (val == -1)
                {
                    base.Message = ConstantLogMessage.API_Error_System;
                    return base.GetActionResult();
                }
                if (val == 0)
                {
                    base.Message = ConstantLogMessage.Alert_Error_Exist("Tên file sao lưu - " + "\"" + fileName + "\"");
                    return base.GetActionResult();
                }
                else
                {
                    base.Message = "Sao lưu dữ liệu thành công";
                    try
                    {
                        string ThaoTac = string.Format("Sao lưu dữ liệu {0}.bak", fileName);
                        string ThoiGian = Utils.ConvertToString(DateTime.UtcNow.ToLocalTime(), string.Empty);
                        string NguoiThucHien = "Admin";
                        _QuanTriDuLieuBUS.WirteFile(ThaoTac, ThoiGian, NguoiThucHien);
                        return CreateActionResult("Sao lưu dữ liệu - " + fileName, EnumLogType.BackupDatabase, () =>
                        {
                            // _logger.LogInformation(User.Claims.FirstOrDefault(c => c.Type == "TenCanBo").Value.ToString() + " - Sao lưu dữ liệu ", fileName);
                            base.Status = val;
                            base.Data = Data;
                            return base.GetActionResult();
                        });
                    }catch(Exception ex)
                    {
                        return CreateActionResult("Sao lưu dữ liệu - " + fileName, EnumLogType.BackupDatabase, () =>
                        {
                            // _logger.LogInformation(User.Claims.FirstOrDefault(c => c.Type == "TenCanBo").Value.ToString() + " - Sao lưu dữ liệu ", fileName);
                            base.Status = val;
                            base.Data = Data;
                            return base.GetActionResult();
                        });
                    }
                }

                //return CreateActionResult(ConstantLogMessage.HT_QuanTriDuLieu_BackupDatabase, LogType.BackupDatabase, () =>
                //{
                //    int val = 0;
                //    val = _QuanTriDuLieuBUS.BackupData(fileName);
                //    if (val == -1)
                //    {
                //        base.Message = ConstantLogMessage.API_Error_System;
                //    }
                //    if (val == 0)
                //    {
                //        base.Message = ConstantLogMessage.Alert_Error_Exist(fileName);
                //    }
                //    else
                //    {
                //        base.Message = "Sao lưu dữ liệu thành công";
                //    }
                //    base.Status = val;
                //    base.Data = Data;
                //    return base.GetActionResult();
                //});
            }
            catch (Exception)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw;
            }

        }


        [HttpGet]
        [Route("RestoreDatabase")]
        [CustomAuthAttribute(ChucNangEnum.SaoLuuDuLieu, AccessLevel.FullAKNTCess)]
        public IActionResult RestoreDatabase(string fileName)
        {
            try
            {
                int val = 0;
                val = _QuanTriDuLieuBUS.RestoreDatabase(fileName);
                if (val == -1)
                {
                    base.Message = ConstantLogMessage.API_Error_System;
                    return base.GetActionResult();
                }
                if (val == 0)
                {
                    base.Message = ConstantLogMessage.Alert_Error_NotExist("File sao lưu - " + "\"" + fileName + "\"");
                    return base.GetActionResult();
                }
                else
                {
                    base.Message = "Phục hồi dữ liệu thành công";
                    base.Status = val;
                    base.Data = Data;
                    try
                    {
                        string ThaoTac = string.Format("Phục hồi dữ liệu {0}", fileName);
                        string ThoiGian = Utils.ConvertToString(DateTime.UtcNow.ToLocalTime(), string.Empty);
                        string NguoiThucHien = "Admin";
                        _QuanTriDuLieuBUS.WirteFile(ThaoTac, ThoiGian, NguoiThucHien);
                        return CreateActionResult("Phục hồi dữ liệu - " + fileName, EnumLogType.RestoreDatabase, () =>
                        {
                            // _logger.LogInformation(User.Claims.FirstOrDefault(c => c.Type == "TenCanBo").Value.ToString() + " - Phục hồi dữ liệu ", fileName);
                            return base.GetActionResult();
                        });
                    }
                    catch (Exception)
                    {
                        return CreateActionResult("Phục hồi dữ liệu - " + fileName, EnumLogType.RestoreDatabase, () =>
                        {
                            // _logger.LogInformation(User.Claims.FirstOrDefault(c => c.Type == "TenCanBo").Value.ToString() + " - Phục hồi dữ liệu ", fileName);
                            return base.GetActionResult();
                        });
                    }
                    

                }

                //return CreateActionResult(ConstantLogMessage.HT_QuanTriDuLieu_RestoreDatabase, LogType.RestoreDatabase, () =>
                //{
                //    if (val == -1)
                //    {
                //        base.Message = ConstantLogMessage.API_Error_System;
                //    }
                //    if (val == 0)
                //    {
                //        base.Message = ConstantLogMessage.Alert_Error_NotExist(fileName);
                //    }
                //    else
                //    {
                //        base.Message = "Phục hồi dữ liệu thành công";
                //    }
                //    base.Status = val;
                //    base.Data = Data;
                //    return base.GetActionResult();
                //});
            }
            catch (Exception)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw;
            }

        }


        [HttpGet]
        [Route("GetFileInDerectory")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        public IActionResult GetFileInDerectory()
        {
            try
            {
                return CreateActionResult(false, ConstantLogMessage.HT_QuanTriDuLieu_GetListFileBackup, EnumLogType.GetList, () =>
                {
                    var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                    int val = 0;
                    var Data = _QuanTriDuLieuBUS.GetFileInDerectory();
                    if (Data.Count < 1)
                    {
                        base.Message = ConstantLogMessage.API_NoData;
                        val = 0;
                    }
                    else if (Data == null)
                    {
                        base.Message = ConstantLogMessage.API_Error_System;
                        val = -1;
                    }
                    else
                    {
                        base.Message = " ";
                        val = 1;
                    }
                    base.Status = val;
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
        /*[Route("GhiFile")]
        //[CustomAuthAttribute(ChucNangEnum.GoManager, AccessLevel.Read)]
        public IActionResult WirteFile()
        {
            try
            {
                var Result = _QuanTriDuLieuBUS.WirteFile("");
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                Status = -1;
                Message = ConstantLogMessage.API_Error_System;
                return GetActionResult();
            }
        }*/

        /*[HttpGet]
        [Route("DocFile")]
        //[CustomAuthAttribute(ChucNangEnum.GoManager, AccessLevel.Read)]
        public IActionResult ReadFile()
        {
            try
            {
                var Result = _QuanTriDuLieuBUS.ReadFile();
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                Status = -1;
                Message = ConstantLogMessage.API_Error_System;
                return GetActionResult();
            }
        }*/

        [HttpGet]
        [Route("ReadFileTXT")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        public IActionResult ReadFileTXT()
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _QuanTriDuLieuBUS.ReadFileTXT();
                if (Result.Status < 1)
                {
                    Status = 0;
                    Message = ConstantLogMessage.API_NoData;
                }
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            catch (Exception ex)
            {
                Status = -1;
                Message = ConstantLogMessage.API_Error_System;
                return GetActionResult();
            }
        }

        [HttpGet]
        [Route("TimKiem")]
        [CustomAuthAttribute(0, AccessLevel.Create)]
        public IActionResult TimKIem(string? Keyword)
        {
            try
            {
                var NguoiDungID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "NguoiDungID").Value, 0);
                var Result = _QuanTriDuLieuBUS.TimKiem(Keyword);
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