using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class SystemLog
    {
        #region Database query string

        private const string GET_ALL = @"SystemLog_GetAll";
        private const string GET_BY_ID = @"SystemLog_GetByID";
        private const string DELETE = @"SystemLog_Delete";
        private const string UPDATE = @"SystemLog_Update";
        private const string INSERT = @"SystemLog_Insert";
        private const string COUNT_ALL = @"SystemLog_CountAll";
        private const string COUNT_SEARCH = @"SystemLog_CountSearch";
        private const string GET_BY_PAGE = @"SystemLog_GetByPage";
        private const string GET_BY_SEARCH = @"SystemLog_GetBySearch";

        #endregion

        #region paramaters constant

        private const string PARM_SYSTEMLOGID = @"SystemLogID";
        private const string PARM_CANBOID = @"CanBoID";
        private const string PARM_LOGINFO = @"LogInfo";
        private const string PARM_LOGTIME = @"LogTime";
        private const string PARM_LOGTYPE = @"LogType";
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";
        #endregion

        private SystemLogInfo GetData(SqlDataReader rdr)
        {
            SystemLogInfo systemLogInfo = new SystemLogInfo();
            systemLogInfo.SystemLogID = Utils.GetInt32(rdr["SystemLogID"], 0);
            systemLogInfo.CanBoID = Utils.GetInt32(rdr["CanBoID"], 0);
            systemLogInfo.LogInfo = Utils.GetString(rdr["LogInfo"], String.Empty);
            systemLogInfo.LogTime = Utils.GetDateTime(rdr["LogTime"], DateTime.Now);
            systemLogInfo.LogType = Utils.GetInt32(rdr["LogType"], 0);
            systemLogInfo.TenCanBo = Utils.GetString(rdr["TenCanBo"], String.Empty);
            return systemLogInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_LOGINFO, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_LOGTIME, SqlDbType.DateTime),
                new SqlParameter(PARM_LOGTYPE, SqlDbType.Int)
            };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, SystemLogInfo systemLogInfo)
        {
            parms[0].Value = systemLogInfo.CanBoID;
            parms[1].Value = systemLogInfo.LogInfo;
            parms[2].Value = systemLogInfo.LogTime;
            parms[3].Value = systemLogInfo.LogType;
        }

        private SqlParameter[] GetUpdateParms()
        {
            List<SqlParameter> parms = GetInsertParms().ToList();
            parms.Insert(0, new SqlParameter(PARM_SYSTEMLOGID, SqlDbType.Int));
            return parms.ToArray();
        }

        private void SetUpdateParms(SqlParameter[] parms, SystemLogInfo systemLogInfo)
        {
            parms[0].Value = systemLogInfo.SystemLogID;
            parms[1].Value = systemLogInfo.CanBoID;
            parms[2].Value = systemLogInfo.LogInfo;
            parms[3].Value = systemLogInfo.LogTime;
            parms[4].Value = systemLogInfo.LogType;

        }

        public IList<SystemLogInfo> GetAll()
        {
            IList<SystemLogInfo> systemlogs = new List<SystemLogInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {
                    while (dr.Read())
                    {
                        SystemLogInfo systemLogInfo = GetData(dr);
                        systemlogs.Add(systemLogInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return systemlogs;
        }

        public SystemLogInfo GetByID(int systemlogID)
        {
            SystemLogInfo systemLogInfo = null;
            SqlParameter[] parameters = new SqlParameter[] {
new SqlParameter(PARM_SYSTEMLOGID, SqlDbType.Int) };
            parameters[0].Value = systemlogID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {
                    if (dr.Read())
                    {
                        systemLogInfo = GetData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return systemLogInfo;
        }

        public int Delete(int systemlogID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
new SqlParameter(PARM_SYSTEMLOGID, SqlDbType.Int) };
            parameters[0].Value = systemlogID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE, parameters);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }

        public int Update(SystemLogInfo systemLogInfo)
        {
            int val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, systemLogInfo);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE, parameters);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }


        public int Insert(SystemLogInfo systemLogInfo)
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
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT, parameters);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return Convert.ToInt32(val);
        }

        public IList<SystemLogInfo> GetBySearch(string keyword, int start, int end)
        {

            IList<SystemLogInfo> logs = new List<SystemLogInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,200),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int)
            };
            parameters[0].Value = keyword;
            parameters[1].Value = start;
            parameters[2].Value = end;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_SEARCH, parameters))
                {
                    while (dr.Read())
                    {
                        SystemLogInfo cInfo = GetData(dr);
                        logs.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return logs;
        }

        public IList<SystemLogInfo> GetByPage(int start, int end)
        {

            IList<SystemLogInfo> logs = new List<SystemLogInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int)
            };
            parameters[0].Value = start;
            parameters[1].Value = end;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_PAGE, parameters))
                {
                    while (dr.Read())
                    {
                        SystemLogInfo cInfo = GetData(dr);
                        logs.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }

            return logs;
        }

        public int CountAll()
        {
            int result = 0;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_ALL, null))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }

            return result;
        }

        public int CountSearch(string keyword)
        {

            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,200)
            };
            parameters[0].Value = keyword;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_SEARCH, parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return result;
        }

    }
}
