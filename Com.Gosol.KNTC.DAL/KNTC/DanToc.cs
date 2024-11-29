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
    public class DanToc
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"DM_DanToc_GetAll";
        private const string GET_BY_ID = @"DM_DanToc_GetByID";
        private const string INSERT = @"DM_DanToc_Insert";
        private const string UPDATE = @"DM_DanToc_Update";
        private const string DELETE = @"DM_DanToc_Delete";

        private const string GET_BY_PAGE = @"DM_DanToc_GetByPage";
        private const string COUNT_ALL = @"DM_DanToc_CountAll";
        private const string COUNT_SEARCH = @"DM_DanToc_CountSearch";
        private const string SEARCH = @"DM_DanToc_GetBySearch";


        //Ten cac bien dau vao
        private const string PARAM_DAN_TOC_ID = "@DanTocID";
        private const string PARAM_TEN_DAN_TOC = "@TenDanToc";

        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";


        private DanTocInfo GetData(SqlDataReader dr)
        {
            DanTocInfo DTInfo = new DanTocInfo();
            DTInfo.DanTocID = Utils.GetInt32(dr["DanTocID"].ToString(), 0);
            DTInfo.TenDanToc = Utils.GetString(dr["TenDanToc"].ToString(), string.Empty);

            return DTInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_DAN_TOC, SqlDbType.NVarChar, 50),

                };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, DanTocInfo DTInfo)
        {

            parms[0].Value = DTInfo.TenDanToc;

        }

        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_DAN_TOC_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TEN_DAN_TOC,SqlDbType.NVarChar,50)

            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, DanTocInfo DTInfo)
        {

            parms[0].Value = DTInfo.DanTocID;
            parms[1].Value = DTInfo.TenDanToc;

        }

        public IList<DanTocInfo> GetAll()
        {
            IList<DanTocInfo> LsDT = new List<DanTocInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        DanTocInfo DTInfo = GetData(dr);
                        LsDT.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return LsDT;
        }

        public DanTocInfo GetByID(int cID)
        {

            DanTocInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DAN_TOC_ID,SqlDbType.Int)
            };
            parameters[0].Value = cID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }

        public int Delete(int DT_ID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DAN_TOC_ID,SqlDbType.Int)
            };
            parameters[0].Value = DT_ID;
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

        public int Update(DanTocInfo DTInfo)
        {

            int val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, DTInfo);

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

        public int Insert(DanTocInfo DTInfo)
        {

            int val = 0;

            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, DTInfo);

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

        //search chua co phan trang
        public IList<DanTocInfo> Search(string key)
        {
            IList<DanTocInfo> dantocs = new List<DanTocInfo>();
            DanTocInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50)
            };
            parameters[0].Value = key;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SEARCH, parameters))
                {

                    while (dr.Read())
                    {
                        cInfo = GetData(dr);
                        dantocs.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }

            return dantocs;
        }

        //search da co phan trang
        public IList<DanTocInfo> GetBySearch(string keyword, int start, int end)
        {

            IList<DanTocInfo> dantocs = new List<DanTocInfo>();
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
                        DanTocInfo dtInfo = GetData(dr);
                        dantocs.Add(dtInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dantocs;
        }


        public IList<DanTocInfo> GetByPage(int start, int end)
        {

            IList<DanTocInfo> ls_dantoc = new List<DanTocInfo>();
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
                        DanTocInfo DTInfo = GetData(dr);
                        ls_dantoc.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ls_dantoc;
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

        //dem ket qua search duoc
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
