using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Models.KNTC;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class CapDAL
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"DM_Cap_GetAll";
        private const string GET_BY_ID = @"DM_Cap_GetByID";
        private const string INSERT = @"DM_Cap_Insert";
        private const string UPDATE = @"DM_Cap_Update";
        private const string DELETE = @"DM_Cap_Delete";
        private const string GET_BY_PAGE = @"DM_Cap_GetByPage";
        private const string COUNT_ALL = @"DM_Cap_CountAll";
        private const string SEARCH = @"DM_Cap_GetBySearch";
        private const string COUNT_SEARCH = @"DM_Cap_CountSearch";


        //Ten cac bien dau vao
        private const string PARAM_CAP_ID = "@CapID";
        private const string PARAM_TEN_CAP = "@TenCap";
        private const string PARAM_CAP_QUAN_LY = "@CapQuanLy";
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";
        private const string PARAM_THUTU = "@ThuTu";

        private CapInfo GetData(SqlDataReader dr)
        {
            CapInfo cInfo = new CapInfo();
            cInfo.CapID = Utils.GetInt32(dr["CapID"].ToString(), 0);
            cInfo.TenCap = Utils.GetString(dr["TenCap"].ToString(), string.Empty);
            cInfo.ThuTu = Utils.GetInt32(dr["ThuTu"], 0);
            if (cInfo.ThuTu != 0)
                cInfo.CapQuanLy = cInfo.ThuTu.ToString();

            return cInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_CAP, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_THUTU,SqlDbType.Int)
                };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, CapInfo cInfo)
        {

            parms[0].Value = cInfo.TenCap;
            parms[1].Value = cInfo.ThuTu;
        }

        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_CAP_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TEN_CAP,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_THUTU,SqlDbType.Int)
            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, CapInfo cInfo)
        {

            parms[0].Value = cInfo.CapID;
            parms[1].Value = cInfo.TenCap;
            parms[2].Value = cInfo.ThuTu;
        }

        public IList<CapInfo> GetAll()
        {
            IList<CapInfo> caps = new List<CapInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        CapInfo cInfo = GetData(dr);
                        caps.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return caps;
        }

        public CapInfo GetByID(int cID)
        {

            CapInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CAP_ID,SqlDbType.Int)
            };
            parameters[0].Value = cID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        cInfo = GetData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return cInfo;
        }

        public int Delete(int cID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CAP_ID,SqlDbType.Int)
            };
            parameters[0].Value = cID;
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

        public int Update(CapInfo cInfo)
        {

            int val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, cInfo);

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

        public int Insert(CapInfo cInfo)
        {

            int val = 0;

            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, cInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, INSERT, parameters);
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

        public IList<CapInfo> GetBySearch(string keyword, int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<CapInfo> caps = new List<CapInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int)
            };
            parameters[0].Value = keyword;
            parameters[1].Value = start;
            parameters[2].Value = end;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SEARCH, parameters))
                {
                    while (dr.Read())
                    {
                        CapInfo cInfo = GetData(dr);
                        caps.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return caps;
        }

        public IList<CapInfo> GetByPage(int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<CapInfo> caps = new List<CapInfo>();
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
                        CapInfo cInfo = GetData(dr);
                        caps.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return caps;
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
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50)
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

        #region -- get cap for bao cao th tinh hinh td, xld, gqd
        private BCTinhHinhTD_XLD_GQInfo GetDataCap(SqlDataReader dr)
        {
            BCTinhHinhTD_XLD_GQInfo cInfo = new BCTinhHinhTD_XLD_GQInfo();
            cInfo.CapID = Utils.GetInt32(dr["CapID"].ToString(), 0);
            cInfo.TenCap = Utils.GetString(dr["TenCap"].ToString(), string.Empty);

            return cInfo;
        }

        public IList<BCTinhHinhTD_XLD_GQInfo> GetAllCap()
        {
            IList<BCTinhHinhTD_XLD_GQInfo> caps = new List<BCTinhHinhTD_XLD_GQInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        BCTinhHinhTD_XLD_GQInfo cInfo = GetDataCap(dr);
                        caps.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return caps;
        }
        #endregion
    }
}
