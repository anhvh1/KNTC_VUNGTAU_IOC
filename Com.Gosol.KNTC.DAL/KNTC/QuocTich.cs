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
    public class QuocTich
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"DM_QuocTich_GetAll";
        private const string GET_BY_ID = @"DM_QuocTich_GetByID";
        private const string INSERT = @"DM_QuocTich_Insert";
        private const string UPDATE = @"DM_QuocTich_Update";
        private const string DELETE = @"DM_QuocTich_Delete";

        private const string GET_BY_PAGE = @"DM_QuocTich_GetByPage";
        private const string COUNT_ALL = @"DM_QuocTich_CountAll";
        private const string COUNT_SEARCH = @"DM_QuocTich_CountSearch";
        private const string SEARCH = @"DM_QuocTich_GetBySearch";


        //Ten cac bien dau vao
        private const string PARAM_QUOC_TICH_ID = "@QuocTichID";
        private const string PARAM_TEN_QUOC_TICH = "@TenQuocTich";

        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";


        private QuocTichInfo GetData(SqlDataReader dr)
        {
            QuocTichInfo QTInfo = new QuocTichInfo();
            QTInfo.QuocTichID = Utils.GetInt32(dr["QuocTichID"].ToString(), 0);
            QTInfo.TenQuocTich = Utils.GetString(dr["TenQuocTich"].ToString(), string.Empty);

            return QTInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_QUOC_TICH, SqlDbType.NVarChar, 20),

                };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, QuocTichInfo QTInfo)
        {

            parms[0].Value = QTInfo.TenQuocTich;

        }

        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_QUOC_TICH_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TEN_QUOC_TICH,SqlDbType.NVarChar,50)

            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, QuocTichInfo QTInfo)
        {

            parms[0].Value = QTInfo.QuocTichID;
            parms[1].Value = QTInfo.TenQuocTich;

        }

        public IList<QuocTichInfo> GetAll()
        {
            IList<QuocTichInfo> LsQT = new List<QuocTichInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        QuocTichInfo QTInfo = GetData(dr);
                        LsQT.Add(QTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return LsQT;
        }

        public QuocTichInfo GetByID(int QTID)
        {

            QuocTichInfo QTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_QUOC_TICH_ID,SqlDbType.Int)
            };
            parameters[0].Value = QTID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        QTInfo = GetData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return QTInfo;
        }

        public int Delete(int QT_ID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_QUOC_TICH_ID,SqlDbType.Int)
            };
            parameters[0].Value = QT_ID;
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

        public int Update(QuocTichInfo QTInfo)
        {

            int val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, QTInfo);

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

        public int Insert(QuocTichInfo QTInfo)
        {

            int val = 0;

            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, QTInfo);

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
        public IList<QuocTichInfo> Search(string key)
        {
            IList<QuocTichInfo> quoctichs = new List<QuocTichInfo>();
            QuocTichInfo QTInfo = null;
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
                        QTInfo = GetData(dr);
                        quoctichs.Add(QTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }

            return quoctichs;
        }

        //search co phan trang
        public IList<QuocTichInfo> GetBySearch(string keyword, int start, int end)
        {

            IList<QuocTichInfo> quoctichs = new List<QuocTichInfo>();
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
                        QuocTichInfo qtInfo = GetData(dr);
                        quoctichs.Add(qtInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return quoctichs;
        }

        public IList<QuocTichInfo> GetByPage(int start, int end)
        {

            IList<QuocTichInfo> ls_dantoc = new List<QuocTichInfo>();
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
                        QuocTichInfo QTInfo = GetData(dr);
                        ls_dantoc.Add(QTInfo);
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

        //dem ket qua tim kiem duoc
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
