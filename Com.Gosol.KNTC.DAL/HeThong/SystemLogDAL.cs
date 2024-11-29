using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Ultilities;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Com.Gosol.KNTC.DAL.HeThong
{

    public class SystemLogDAL
    {
        #region Database query string

        private const string V1_SYSTEMLOG_INSERT = @"v1_SystemLog_Insert";
        private const string V1_SYSTEMLOG_GETPAGINGBYSEARCH = @"v1_SystemLog_GetPagingBySearch";
        private const string V1_SYSTEMLOG_GETPAGINGBYQUANTRIDULIEU = @"v1_SystemLog_GetPagingByQuanTriDuLieu";

        #endregion

        #region paramaters constant

        private const string PARM_SYSTEMLOGid = @"SystemLogid";
        private const string PARM_CanBoID = @"CanBoID";
        private const string PARM_LOGINFO = @"LogInfo";
        private const string PARM_LOGTIME = @"LogTime";
        private const string PARM_LOGTYPE = @"LogType";
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";
        #endregion

        #region Insert, update
        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_CanBoID, SqlDbType.Int),
                new SqlParameter(PARM_LOGINFO, SqlDbType.NVarChar),
                new SqlParameter(PARM_LOGTIME, SqlDbType.DateTime),
                new SqlParameter(PARM_LOGTYPE, SqlDbType.Int)
            };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, SystemLogModel systemLogInfo)
        {
            parms[0].Value = systemLogInfo.CanBoID;
            parms[1].Value = systemLogInfo.LogInfo;
            parms[2].Value = systemLogInfo.LogTime;
            parms[3].Value = systemLogInfo.LogType;
        }

        public int Insert(SystemLogModel systemLogInfo)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, systemLogInfo);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, V1_SYSTEMLOG_INSERT, parameters);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return Convert.ToInt32(val);
        }


        public List<SystemLogPartialModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {

            List<SystemLogPartialModel> list = new List<SystemLogPartialModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("Keyword",SqlDbType.NVarChar,200),
                new SqlParameter("OrderByName",SqlDbType.NVarChar,50),
                new SqlParameter("OrderByOption",SqlDbType.NVarChar,50),
                new SqlParameter("pLimit",SqlDbType.Int),
                new SqlParameter("pOffset",SqlDbType.Int),
                new SqlParameter("TotalRow",SqlDbType.Int),

            };
            parameters[0].Value = p.Keyword != null ? p.Keyword : "";
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, V1_SYSTEMLOG_GETPAGINGBYSEARCH, parameters))
                {
                    while (dr.Read())
                    {
                        SystemLogPartialModel item = new SystemLogPartialModel();
                        item.SystemLogid = Utils.ConvertToInt32(dr["SystemLogid"], 0);
                        item.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], "");
                        item.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], "");
                        item.LogInfo = Utils.ConvertToString(dr["LogInfo"], "");
                        item.LogTime = Utils.ConvertToDateTime(dr["LogTime"], DateTime.MinValue);

                        list.Add(item);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }


        public List<SystemLogPartialModel> GetPagingByQuanTriDuLieu(BasePagingParams p, ref int TotalRow)
        {

            List<SystemLogPartialModel> list = new List<SystemLogPartialModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("Keyword",SqlDbType.NVarChar,200),
                new SqlParameter("OrderByName",SqlDbType.NVarChar,50),
                new SqlParameter("OrderByOption",SqlDbType.NVarChar,50),
                new SqlParameter("pLimit",SqlDbType.Int),
                new SqlParameter("pOffset",SqlDbType.Int),
                new SqlParameter("TotalRow",SqlDbType.Int),

            };
            parameters[0].Value = p.Keyword != null ? p.Keyword : "";
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, V1_SYSTEMLOG_GETPAGINGBYQUANTRIDULIEU, parameters))
                {
                    while (dr.Read())
                    {
                        SystemLogPartialModel item = new SystemLogPartialModel();
                        item.SystemLogid = Utils.ConvertToInt32(dr["SystemLogid"], 0);
                        item.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], "");
                        item.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], "");
                        item.LogInfo = Utils.ConvertToString(dr["LogInfo"], "");
                        item.LogTime = Utils.ConvertToDateTime(dr["LogTime"], DateTime.MinValue);

                        list.Add(item);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        #endregion
        // Lấy danh sách systemlog từ ngày đến ngày
        public List<SystemLogModel> GetAllSystemLogFromTo(string TuNgay, string DenNgay)
        {
            List<SystemLogModel> ListSystemLogs = new List<SystemLogModel>();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@TuNgay",SqlDbType.VarChar),
                   new SqlParameter("@DenNgay",SqlDbType.VarChar)
              };
            parameters[0].Value = TuNgay ?? Convert.DBNull;
            parameters[1].Value = DenNgay ?? Convert.DBNull;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_GetListSystemLogFromTo", parameters))
                {
                    while (dr.Read())
                    {
                        SystemLogModel SystemLogModel = new SystemLogModel();
                        SystemLogModel.SystemLogid = Utils.ConvertToInt32(dr["SystemLogid"], 0);
                        SystemLogModel.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        SystemLogModel.LogInfo = Utils.ConvertToString(dr["LogInfo"], string.Empty);
                        SystemLogModel.LogTime = Utils.ConvertToDateTime(dr["LogTime"], DateTime.Now);
                        SystemLogModel.LogType = Utils.ConvertToInt32(dr["LogType"], 0);
                        ListSystemLogs.Add(SystemLogModel);
                    }
                    dr.Close();
                    //if (canBo != null && canBo.CanBoID > 0)
                    //{
                    //    canBo.DanhSachChucVuID = CanBoChucVu_GetBy_CanBoID(CanBoID.Value);
                    //    // lây danh sách chức vụ của cán bộ
                    //}
                }
            }
            catch
            {
                throw;
            }
            return ListSystemLogs;
        }

        // Tạo file log
        public XDocument CreateLogFile(string SystemLogPath, string TuNgay, string DenNgay)
        {
            XDocument doc = new XDocument();
            var ListSystemLog = GetAllSystemLogFromTo(TuNgay, DenNgay);
            doc =
                  new XDocument(
                  new XElement("LogConfig", ListSystemLog.Select(x =>
                  new XElement("SystemLog", new XAttribute("SystemLogID", x.SystemLogid), new XAttribute("CanBoID", x.CanBoID), new XAttribute("LogInfo", x.LogInfo), new XAttribute("LogTime", x.LogTime), new
                XAttribute("LogType", x.LogType)))));
            doc.Save(SystemLogPath);
            return doc;
        }

        // Get log by canboid and  system log info

        public List<SystemLogModel> GetListSystemLogByCanBoAndLogInfo(int? CanBoID, string LogInfo)
        {
            List<SystemLogModel> ListSystemLogs = new List<SystemLogModel>();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@CanBoID",SqlDbType.Int),
                   new SqlParameter("@LogInfo",SqlDbType.NVarChar)
              };
            parameters[0].Value = CanBoID ?? Convert.DBNull;
            parameters[1].Value = LogInfo ?? Convert.DBNull;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_GetListSystemLogByCanBoIDAndLogInfo", parameters))
                {
                    while (dr.Read())
                    {
                        SystemLogModel SystemLogModel = new SystemLogModel();
                        SystemLogModel.SystemLogid = Utils.ConvertToInt32(dr["SystemLogid"], 0);
                        SystemLogModel.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        SystemLogModel.LogInfo = Utils.ConvertToString(dr["LogInfo"], string.Empty);
                        SystemLogModel.LogTime = Utils.ConvertToDateTime(dr["LogTime"], DateTime.Now);
                        SystemLogModel.LogType = Utils.ConvertToInt32(dr["LogType"], 0);
                        ListSystemLogs.Add(SystemLogModel);
                    }
                    dr.Close();
                    //if (canBo != null && canBo.CanBoID > 0)
                    //{
                    //    canBo.DanhSachChucVuID = CanBoChucVu_GetBy_CanBoID(CanBoID.Value);
                    //    // lây danh sách chức vụ của cán bộ
                    //}
                }
            }
            catch
            {
                throw;
            }
            return ListSystemLogs;
        }
        
        #region Variable V2

        #region Param

        private readonly string pSystemLogID = "SystemLogID";
        private readonly string pLogInfo = "LogInfo";
        private readonly string pLogTime = "LogTime";
        private readonly string pLogType = "LogType";
        private readonly string pTenCanBo = "TenCanBo";
        private readonly string pCanBoID = "CanBoID";
        private readonly string pFrom = "From";
        private readonly string pTo = "To";
        private readonly string pLimit = "Limit";
        private readonly string pKeyWord = "KeyWord";
        private readonly string pOffset = "Offset";
        private readonly string pTotalRow = "TotalRow";

        #endregion

        #region StoreProcedure

        private readonly string sDanhSachNhatKy = "v2_SysTemLog_GetAll";

        #endregion

        #endregion 
        #region Function V2

        public BaseResultModel DanhSach(NhatKyHeThongThamSo thamSo)
        {
            BaseResultModel Result = new BaseResultModel();
            List<NhatKyHeThongModel> Data = new List<NhatKyHeThongModel>();

            SqlParameter[] parameters =
            {
                new SqlParameter(pKeyWord, SqlDbType.NVarChar),
                new SqlParameter(pLogType, SqlDbType.Int),
                new SqlParameter(pFrom, SqlDbType.Date),
                new SqlParameter(pTo, SqlDbType.Date),
                new SqlParameter(pOffset, SqlDbType.Int),
                new SqlParameter(pLimit, SqlDbType.Int),
                new SqlParameter(pTotalRow, SqlDbType.Int)
            };
            
            int offset = thamSo.PageNumber < 1 ? 0 : (thamSo.PageNumber - 1) * thamSo.PageSize;
            
            parameters[0].Value = thamSo.Keyword;
            parameters[1].Value = thamSo.LogType ?? Convert.DBNull;
            parameters[2].Value = thamSo.From ?? Convert.DBNull;
            parameters[3].Value = thamSo.To ?? Convert.DBNull;
            parameters[4].Value = offset;
            parameters[5].Value = thamSo.PageSize > 1 ? thamSo.PageSize : 1;
            parameters[6].Direction = ParameterDirection.Output;

            try
            {
                using SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sDanhSachNhatKy, parameters);
                while (dr.Read())
                {
                    Data.Add(new NhatKyHeThongModel
                    {
                        SystemLogID = Utils.ConvertToInt32(dr[pSystemLogID], 0),
                        CanBoID = Utils.ConvertToInt32(dr[pCanBoID], 0),
                        LogInfo = Utils.ConvertToString(dr[pLogInfo], ""),
                        LogTime = Utils.ConvertToDateTime_Hour(dr[pLogTime], DateTime.MinValue),
                        LogType = Utils.ConvertToInt32(dr[pLogType], 0),
                        TenCanBo = Utils.ConvertToString(dr[pTenCanBo], "")
                    });
                }

                dr.Close();
            }
            catch (Exception)
            {
                throw;
            }

            var totalCount = Utils.ConvertToInt32(parameters[6].Value, 0);

            Result.TotalRow = totalCount;
            if (totalCount == 0 || Data.Count == 0)
            {
                Result.Message = Constant.NO_DATA;
                Result.Status = 1;
            }
            else
            {
                Result.Message = "Thành công";
                Result.Status = 1;
            }

            Result.Data = Data;


            return Result;
        }

        #endregion
    }
}
