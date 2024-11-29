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
    public class HuongGiaiQuyet
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"DM_HuongGiaiQuyet_GetAll";
        private const string GET_BY_ID = @"DM_HuongGiaiQuyet_GetByID";
        private const string INSERT = @"DM_HuongGiaiQuyet_Insert";
        private const string UPDATE = @"DM_HuongGiaiQuyet_Update";
        private const string DELETE = @"DM_HuongGiaiQuyet_Delete";
        private const string GET_BY_PAGE = @"DM_HuongGiaiQuyet_GetByPage";
        private const string COUNT_ALL = @"DM_HuongGiaiQuyet_CountAll";
        private const string SEARCH = @"DM_HuongGiaiQuyet_GetBySearch";
        private const string COUNT_SEARCH = @"DM_HuongGiaiQuyet_CountSearch";


        //Ten cac bien dau vao
        private const string PARAM_HUONG_GIAI_QUYET_ID = "@HuongGiaiQuyetID";
        private const string PARAM_TEN_HUONG_GIAI_QUYET = "@TenHuongGiaiQuyet";
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";
        private const string PARAM_SUDUNG = "@SuDung";


        private HuongGiaiQuyetInfo GetData(SqlDataReader dr)
        {
            HuongGiaiQuyetInfo cInfo = new HuongGiaiQuyetInfo();
            cInfo.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"].ToString(), 0);
            cInfo.TenHuongGiaiQuyet = Utils.GetString(dr["TenHuongGiaiQuyet"].ToString(), string.Empty);
            cInfo.SuDung = Utils.GetBoolean(dr["SuDung"], false);
            return cInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_HUONG_GIAI_QUYET, SqlDbType.NVarChar, 20),
                new SqlParameter(PARAM_SUDUNG, SqlDbType.Bit)
                };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, HuongGiaiQuyetInfo cInfo)
        {

            parms[0].Value = cInfo.TenHuongGiaiQuyet;
            parms[1].Value = cInfo.SuDung;
        }

        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_HUONG_GIAI_QUYET_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TEN_HUONG_GIAI_QUYET,SqlDbType.NVarChar,20),
                new SqlParameter(PARAM_SUDUNG, SqlDbType.Bit)
            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, HuongGiaiQuyetInfo cInfo)
        {

            parms[0].Value = cInfo.HuongGiaiQuyetID;
            parms[1].Value = cInfo.TenHuongGiaiQuyet;
            parms[2].Value = cInfo.SuDung;
        }

        public IList<HuongGiaiQuyetInfo> GetAll()
        {
            IList<HuongGiaiQuyetInfo> caps = new List<HuongGiaiQuyetInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        HuongGiaiQuyetInfo cInfo = GetData(dr);
                        if (cInfo.SuDung)
                        {
                            caps.Add(cInfo);
                        }
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return caps;
        }

        public HuongGiaiQuyetInfo GetByID(int cID)
        {

            HuongGiaiQuyetInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_HUONG_GIAI_QUYET_ID,SqlDbType.Int)
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
                new SqlParameter(PARAM_HUONG_GIAI_QUYET_ID,SqlDbType.Int)
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
                        if (val > 0)
                            trans.Commit();
                        else
                            trans.Rollback();
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

        public int Update(HuongGiaiQuyetInfo cInfo)
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

        public int Insert(HuongGiaiQuyetInfo cInfo)
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

        public IList<HuongGiaiQuyetInfo> GetBySearch(string keyword, int start, int end)
        {

            IList<HuongGiaiQuyetInfo> huongs = new List<HuongGiaiQuyetInfo>();
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
                        HuongGiaiQuyetInfo cInfo = GetData(dr);
                        huongs.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return huongs;
        }

        public IList<HuongGiaiQuyetInfo> GetByPage(int start, int end)
        {

            IList<HuongGiaiQuyetInfo> huongs = new List<HuongGiaiQuyetInfo>();
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
                        HuongGiaiQuyetInfo cInfo = GetData(dr);
                        huongs.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }

            return huongs;
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
