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
    public class Huyen
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"DM_Huyen_GetAll";
        private const string GET_BY_ID = @"DM_Huyen_GetByID";
        private const string INSERT = @"DM_Huyen_Insert";
        private const string UPDATE = @"DM_Huyen_Update";
        private const string DELETE = @"DM_Huyen_Delete";
        private const string GET_BY_PAGE = @"DM_Huyen_GetByPage";
        private const string COUNT_ALL = @"DM_Huyen_CountAll";
        private const string SEARCH = @"DM_Huyen_GetBySearch";
        private const string COUNT_SEARCH = @"DM_Huyen_CountSearch";
        private const string GET_BY_TINH = @"DM_Huyen_GetByTinh";
        private const string CHECK_UPDATE = @"DM_Huyen_CheckUpdate";


        //Ten cac bien dau vao
        private const string PARAM_HUYEN_ID = "@HuyenID";
        private const string PARAM_TEN_HUYEN = "@TenHuyen";
        private const string PARAM_TINH_ID = @"TinhID";
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";



        private HuyenInfo GetData(SqlDataReader dr)
        {
            HuyenInfo cInfo = new HuyenInfo();
            cInfo.HuyenID = Utils.GetInt32(dr["HuyenID"].ToString(), 0);
            cInfo.TenHuyen = Utils.GetString(dr["TenHuyen"].ToString(), string.Empty);
            cInfo.TinhID = Utils.GetInt32(dr["TinhID"].ToString(), 0);
            cInfo.TenTinh = Utils.GetString(dr["TenTinh"].ToString(), string.Empty);
            return cInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_HUYEN, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_TINH_ID,SqlDbType.Int)
                };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, HuyenInfo cInfo)
        {

            parms[0].Value = cInfo.TenHuyen;
            parms[1].Value = cInfo.TinhID;
        }

        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_HUYEN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TEN_HUYEN,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_TINH_ID,SqlDbType.Int)
            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, HuyenInfo cInfo)
        {

            parms[0].Value = cInfo.HuyenID;
            parms[1].Value = cInfo.TenHuyen;
            parms[2].Value = cInfo.TinhID;
        }

        public IList<HuyenInfo> GetAll()
        {
            IList<HuyenInfo> huyens = new List<HuyenInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        HuyenInfo cInfo = GetData(dr);
                        huyens.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return huyens;
        }

        public HuyenInfo GetByID(int cID)
        {

            HuyenInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_HUYEN_ID,SqlDbType.Int)
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
                new SqlParameter(PARAM_HUYEN_ID,SqlDbType.Int)
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

        public int Update(HuyenInfo cInfo)
        {

            object val = null;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, cInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE, parameters);
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
            return Utils.ConvertToInt32(val, 0);
        }

        public int Insert(HuyenInfo cInfo)
        {

            object val = null;

            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, cInfo);

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
            return Utils.ConvertToInt32(val, 0);
        }

        public IList<HuyenInfo> GetBySearch(string keyword, int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<HuyenInfo> huyens = new List<HuyenInfo>();
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
                        HuyenInfo cInfo = GetData(dr);
                        huyens.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return huyens;
        }

        public IList<HuyenInfo> GetByPage(int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<HuyenInfo> huyens = new List<HuyenInfo>();
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
                        HuyenInfo cInfo = GetData(dr);
                        huyens.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return huyens;
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
        public IList<HuyenInfo> GetByTinh(int id)
        {
            IList<HuyenInfo> huyens = new List<HuyenInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TINH_ID,SqlDbType.Int)
            };
            parameters[0].Value = id;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_TINH, parameters))
                {
                    while (dr.Read())
                    {
                        HuyenInfo cInfo = GetData(dr);
                        cInfo.TenDayDu = Utils.GetString(dr["TenDayDu"].ToString(), string.Empty);
                        huyens.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return huyens;
        }
    }
}
