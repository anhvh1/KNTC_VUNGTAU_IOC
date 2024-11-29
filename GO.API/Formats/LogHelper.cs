using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.Models.HeThong;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;


namespace Com.Gosol.KNTC.API.Formats
{
    public interface ILogHelper
    {
        public void Log(int CanBoID, String logIngo, int logType);
        public void Log(int CanBoID, String logInfo, int logType, DateTime logTime);
        public ClaimsPrincipal getCurrentUser();
    }
    public class LogHelper : ILogHelper
    {
        private ISystemLogBUS _SystemLogBUS;
        private readonly IHttpContextAccessor _HttpContextAKNTCessor;
        public LogHelper(ISystemLogBUS SystemLogBUS, IHttpContextAccessor HttpContextAcess)
        {
            _SystemLogBUS = SystemLogBUS;
            _HttpContextAKNTCessor = HttpContextAcess;
        }
        public void Log(int CanBoID, String logInfo, int logType)
        {
            SystemLogModel systemLogInfo = new SystemLogModel();
            systemLogInfo.CanBoID = CanBoID;
            systemLogInfo.LogInfo = logInfo;
            systemLogInfo.LogTime = DateTime.Now;
            systemLogInfo.LogType = logType;

            try
            {
                _SystemLogBUS.Insert(systemLogInfo);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }
        public void Log(int CanBoID, String logInfo, int logType, DateTime logTime)
        {
            SystemLogModel systemLogInfo = new SystemLogModel();
            systemLogInfo.CanBoID = CanBoID;
            systemLogInfo.LogInfo = logInfo;
            systemLogInfo.LogTime = logTime;
            systemLogInfo.LogType = logType;

            try
            {
                _SystemLogBUS.Insert(systemLogInfo);
            }
            catch
            {
            }
        }

        public ClaimsPrincipal getCurrentUser()
        {
            return _HttpContextAKNTCessor.HttpContext.User;
        }
    }
}