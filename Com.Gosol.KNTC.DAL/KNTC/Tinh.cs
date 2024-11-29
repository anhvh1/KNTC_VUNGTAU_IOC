using Com.Gosol.KNTC.Ultilities;
using Spire.Pdf.Graphics;
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
    public class Tinh
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"DM_Tinh_GetAll";
        private const string GET_BY_ID = @"DM_Tinh_GetByID";
        private const string INSERT = @"DM_Tinh_Insert";
        private const string UPDATE = @"DM_Tinh_Update";
        private const string DELETE = @"DM_Tinh_Delete";
        private const string GET_BY_PAGE = @"DM_Tinh_GetByPage";
        private const string COUNT_ALL = @"DM_Tinh_CountAll";
        private const string SEARCH = @"DM_Tinh_GetBySearch";
        private const string COUNT_SEARCH = @"DM_Tinh_CountSearch";


        //Ten cac bien dau vao
        private const string PARAM_TINH_ID = "@TinhID";
        private const string PARAM_TEN_TINH = "@TenTinh";
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";


        private TinhInfo GetData(SqlDataReader dr)
        {
            TinhInfo cInfo = new TinhInfo();
            cInfo.TinhID = Utils.GetInt32(dr["TinhID"].ToString(), 0);
            cInfo.TenTinh = Utils.GetString(dr["TenTinh"].ToString(), string.Empty);
            return cInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_TINH, SqlDbType.NVarChar, 50)
                };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, TinhInfo cInfo)
        {

            parms[0].Value = cInfo.TenTinh;
        }

        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_TINH_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TEN_TINH,SqlDbType.NVarChar,50)
            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, TinhInfo cInfo)
        {

            parms[0].Value = cInfo.TinhID;
            parms[1].Value = cInfo.TenTinh;
        }

        public IList<TinhInfo> GetAll()
        {
            IList<TinhInfo> tinhs = new List<TinhInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        TinhInfo cInfo = GetData(dr);
                        tinhs.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return tinhs;
        }

        public TinhInfo GetByID(int cID)
        {

            TinhInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TINH_ID,SqlDbType.Int)
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
                new SqlParameter(PARAM_TINH_ID,SqlDbType.Int)
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

        public int Update(TinhInfo cInfo)
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

        public int Insert(TinhInfo cInfo)
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

        public IList<TinhInfo> GetBySearch(string keyword, int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<TinhInfo> tinhs = new List<TinhInfo>();
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
                        TinhInfo cInfo = GetData(dr);
                        tinhs.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return tinhs;
        }

        public IList<TinhInfo> GetByPage(int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<TinhInfo> tinhs = new List<TinhInfo>();
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
                        TinhInfo cInfo = GetData(dr);
                        tinhs.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return tinhs;
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
    }
}
