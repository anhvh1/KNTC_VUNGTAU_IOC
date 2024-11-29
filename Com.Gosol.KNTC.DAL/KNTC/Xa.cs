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
    public class Xa
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"DM_Xa_GetAll";
        private const string GET_BY_ID = @"DM_Xa_GetByID";
        private const string INSERT = @"DM_Xa_Insert";
        private const string UPDATE = @"DM_Xa_Update";
        private const string DELETE = @"DM_Xa_Delete";
        private const string GET_BY_PAGE = @"DM_Xa_GetByPage";
        private const string COUNT_ALL = @"DM_Xa_CountAll";
        private const string SEARCH = @"DM_Xa_GetBySearch";
        private const string COUNT_SEARCH = @"DM_Xa_CountSearch";

        private const string GET_XA_BYHUYEN = @"DM_Xa_GetXaByHuyen";


        //Ten cac bien dau vao
        private const string PARAM_XA_ID = "@XaID";
        private const string PARAM_TEN_XA = "@TenXa";
        private const string PARAM_HUYEN_ID = @"HuyenID";
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";


        private XaInfo GetData(SqlDataReader dr)
        {
            XaInfo cInfo = new XaInfo();
            cInfo.XaID = Utils.GetInt32(dr["XaID"].ToString(), 0);
            cInfo.TenXa = Utils.GetString(dr["TenXa"].ToString(), string.Empty);
            cInfo.HuyenID = Utils.GetInt32(dr["HuyenID"].ToString(), 0);
            //cInfo.TenHuyen = Utils.GetString(dr["TenHuyen"].ToString(), string.Empty);
            //cInfo.TinhID = Utils.GetInt32(dr["TinhID"].ToString(), 0);
            //cInfo.TenTinh = Utils.GetString(dr["TenTinh"].ToString(), string.Empty);
            return cInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_XA, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_HUYEN_ID,SqlDbType.Int)
                };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, XaInfo cInfo)
        {

            parms[0].Value = cInfo.TenXa;
            parms[1].Value = cInfo.HuyenID;
        }

        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_XA_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TEN_XA,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_HUYEN_ID,SqlDbType.Int)
            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, XaInfo cInfo)
        {

            parms[0].Value = cInfo.XaID;
            parms[1].Value = cInfo.TenXa;
            parms[2].Value = cInfo.HuyenID;
        }

        public IList<XaInfo> GetAll()
        {
            IList<XaInfo> xas = new List<XaInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        XaInfo cInfo = GetData(dr);
                        cInfo.TenDayDu = Utils.GetString(dr["TenDayDu"].ToString(), string.Empty);
                        xas.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return xas;
        }

        public IList<XaInfo> GetXaByHuyen(int huyenID)
        {

            IList<XaInfo> xas = new List<XaInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_HUYEN_ID,SqlDbType.Int)
            };
            parameters[0].Value = huyenID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_XA_BYHUYEN, parameters))
                {
                    while (dr.Read())
                    {
                        XaInfo cInfo = GetData(dr);
                        cInfo.TenDayDu = Utils.GetString(dr["TenDayDu"].ToString(), string.Empty);
                        cInfo.TenHuyen = Utils.GetString(dr["TenHuyen"].ToString(), string.Empty);
                        xas.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return xas;
        }

        public XaInfo GetByID(int cID)
        {

            XaInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XA_ID,SqlDbType.Int)
            };
            parameters[0].Value = cID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        cInfo = GetData(dr);
                        cInfo.TinhID = Utils.ConvertToInt32(dr["TinhID"], 0);
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
                new SqlParameter(PARAM_XA_ID,SqlDbType.Int)
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

        public int Update(XaInfo cInfo)
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

        public int Insert(XaInfo cInfo)
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

        public IList<XaInfo> GetBySearch(string keyword, int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<XaInfo> xas = new List<XaInfo>();
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
                        XaInfo cInfo = GetData(dr);
                        xas.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return xas;
        }

        public IList<XaInfo> GetByPage(int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<XaInfo> xas = new List<XaInfo>();
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
                        XaInfo cInfo = GetData(dr);
                        xas.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return xas;
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

        private const string GET_BY_HUYEN = @"DM_Xa_GetByHuyen";
        public IList<XaInfo> GetByHuyen(int id)
        {
            IList<XaInfo> xa = new List<XaInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_HUYEN_ID,SqlDbType.Int)
            };
            parameters[0].Value = id;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_HUYEN, parameters))
                {
                    while (dr.Read())
                    {
                        XaInfo cInfo = GetData(dr);
                        xa.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return xa;
        }
    }
}
