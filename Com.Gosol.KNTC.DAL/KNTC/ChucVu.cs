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
    public class ChucVu
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"DM_ChucVu_GetAll";
        private const string GET_BY_ID = @"DM_ChucVu_GetByID";
        private const string INSERT = @"DM_ChucVu_Insert";
        private const string UPDATE = @"DM_ChucVu_Update";
        private const string DELETE = @"DM_ChucVu_Delete";
        private const string GET_BY_PAGE = @"DM_ChucVu_GetByPage";
        private const string COUNT_ALL = @"DM_ChucVu_CountAll";
        private const string SEARCH = @"DM_ChucVu_GetBySearch";
        private const string COUNT_SEARCH = @"DM_ChucVu_CountSearch";



        //Ten cac bien dau vao
        private const string PARAM_CHUC_VU_ID = "@ChucVuID";
        private const string PARAM_TEN_CHUC_VU = "@TenChucVu";
        //private const string PARAM_CAP_QUAN_LY = "@CapQuanLy";
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";


        private ChucVuInfo GetData(SqlDataReader dr)
        {
            ChucVuInfo cInfo = new ChucVuInfo();
            cInfo.ChucVuID = Utils.GetInt32(dr["ChucVuID"].ToString(), 0);
            cInfo.TenChucVu = Utils.GetString(dr["TenChucVu"].ToString(), string.Empty);
            //cInfo.CapQuanLy= Utils.GetString(dr["CapQuanLy"].ToString(), string.Empty);
            return cInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_CHUC_VU, SqlDbType.NVarChar, 50),
                //new SqlParameter(PARAM_CAP_QUAN_LY,SqlDbType.NVarChar,50)
                };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, ChucVuInfo cInfo)
        {

            parms[0].Value = cInfo.TenChucVu;
            //parms[1].Value = cInfo.CapQuanLy;
        }

        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_CHUC_VU_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TEN_CHUC_VU,SqlDbType.NVarChar,50),
                //new SqlParameter(PARAM_CAP_QUAN_LY,SqlDbType.NVarChar,50)
            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, ChucVuInfo cInfo)
        {

            parms[0].Value = cInfo.ChucVuID;
            parms[1].Value = cInfo.TenChucVu;
            //parms[2].Value = cInfo.CapQuanLy;
        }

        public IList<ChucVuInfo> GetAll()
        {
            IList<ChucVuInfo> chucvus = new List<ChucVuInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        ChucVuInfo cInfo = GetData(dr);
                        chucvus.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return chucvus;
        }

        public ChucVuInfo GetByID(int cID)
        {

            ChucVuInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CHUC_VU_ID,SqlDbType.Int)
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
                new SqlParameter(PARAM_CHUC_VU_ID,SqlDbType.Int)
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
                        val = 1;
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

        public int Update(ChucVuInfo cInfo)
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

        public int Insert(ChucVuInfo cInfo)
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

        public IList<ChucVuInfo> GetBySearch(string keyword, int start, int end)
        {

            IList<ChucVuInfo> chucvus = new List<ChucVuInfo>();
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
                        ChucVuInfo cInfo = GetData(dr);
                        chucvus.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return chucvus;
        }


        public IList<ChucVuInfo> GetByPage(int start, int end)
        {

            IList<ChucVuInfo> caps = new List<ChucVuInfo>();
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
                        ChucVuInfo cInfo = GetData(dr);
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
    }
}
