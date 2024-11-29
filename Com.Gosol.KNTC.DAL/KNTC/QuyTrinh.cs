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
    public class QuyTrinh
    {
        #region Database query string
        private const string SELECT_BY_CAPID = @"DM_QuyTrinh_GetByCapID";
        private const string SELECT_ALL_TENCAPBYCHAID = @"DM_CapQuyTrinh_GetTenCapByCapChaID";
        private const string SELECT_BY_URL = @"DM_QuyTrinh_GetByUrl";
        private const string UPDATE_TEN_BY_URL = @"DM_QuyTrinh_UpdateTenByUrl";
        #endregion

        #region paramaters constant
        private const string PARM_CAPID = @"CapID";
        private const string PARM_CAPCHAID = @"CapChaID";
        private const string PARM_URL = @"ImgUrl";
        private const string PARM_TENQUYTRINH = @"TenQuyTrinh";
        private const string PARM_NEWURL = @"NewUrl";
        #endregion

        private QuyTrinhInfo GetData(SqlDataReader rdr)
        {
            QuyTrinhInfo info = new QuyTrinhInfo();
            info.QuyTrinhID = Utils.GetInt32(rdr["QuyTrinhID"], 0);
            info.TenQuyTrinh = Utils.GetString(rdr["TenQuyTrinh"], String.Empty);
            info.ImgUrl = Utils.GetString(rdr["ImgUrl"], String.Empty);
            info.TenCap = Utils.GetString(rdr["TenCap"], String.Empty);
            info.CapID = Utils.GetInt32(rdr["CapID"], 0);

            return info;
        }

        public int UpdateTenByUrl(string imgUrl, string tenQuyTrinh, string newUrl)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_URL, SqlDbType.VarChar,200),
                new SqlParameter(PARM_TENQUYTRINH, SqlDbType.NVarChar,200),
                new SqlParameter(PARM_NEWURL, SqlDbType.VarChar,200),
            };
            parameters[0].Value = imgUrl;
            parameters[1].Value = tenQuyTrinh;
            parameters[2].Value = newUrl;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE_TEN_BY_URL, parameters);
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


        public int Update(QuyTrinhInfo QuyTrinhInfo)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("QuyTrinhID", SqlDbType.Int),
                new SqlParameter("TenQuyTrinh", SqlDbType.NVarChar),
                new SqlParameter("CapID", SqlDbType.Int),
            };
            parameters[0].Value = QuyTrinhInfo.QuyTrinhID;
            parameters[1].Value = QuyTrinhInfo.TenQuyTrinh ?? Convert.DBNull;
            parameters[2].Value = QuyTrinhInfo.CapID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "DM_QuyTrinh_Update", parameters);
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

        public int Insert(QuyTrinhInfo QuyTrinhInfo)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("TenQuyTrinh", SqlDbType.NVarChar),
                new SqlParameter("CapID", SqlDbType.Int),
            };

            parameters[0].Value = QuyTrinhInfo.TenQuyTrinh ?? Convert.DBNull;
            parameters[1].Value = QuyTrinhInfo.CapID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "DM_QuyTrinh_Insert", parameters), 0);
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


        public QuyTrinhInfo GetByUrl(string imgUrl)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_URL, SqlDbType.VarChar,200),
            };
            parameters[0].Value = imgUrl;
            QuyTrinhInfo info = null;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_BY_URL, parameters))
            {
                if (dr.Read())
                {
                    info = GetData(dr);
                }
                dr.Close();
            }
            return info;
        }

        public IList<QuyTrinhInfo> GetAllTenCapByChaID(int pCapChaID)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_CAPCHAID, SqlDbType.Int),
            };
            parameters[0].Value = pCapChaID;
            IList<QuyTrinhInfo> list = new List<QuyTrinhInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_ALL_TENCAPBYCHAID, parameters))
            {
                while (dr.Read())
                {
                    QuyTrinhInfo info = new QuyTrinhInfo();
                    info.TenCap = Utils.GetString(dr["TenCap"], String.Empty);
                    info.CapID = Utils.GetInt32(dr["CapID"], 0);
                    list.Add(info);
                }
                dr.Close();
            }
            return list;
        }


        public IList<QuyTrinhInfo> GetByCapID(int pCapID)
        {
            IList<QuyTrinhInfo> list = new List<QuyTrinhInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_CAPID, SqlDbType.Int),
            };
            parameters[0].Value = pCapID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_BY_CAPID, parameters))
            {
                while (dr.Read())
                {
                    QuyTrinhInfo info = GetData(dr);
                    list.Add(info);
                }
                dr.Close();
            }
            return list;
        }

    }
}
