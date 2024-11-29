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
    public class BieuMau
    {
        #region Database query string

        private const string SELECT_ALL = @"DM_BieuMau_GetAll";
        private const string SELECT_BY_ID = @"DM_BieuMau_GetByID";
        private const string SEARCH = @"DM_BieuMau_GetBySearch";
        private const string COUNT_SEARCH = @"DM_BieuMau_CountBySearch";
        private const string UPDATE_DUOCSUDUNG = @"DM_BieuMau_UpdateDuocSuDung";



        #endregion

        #region paramaters constant

        private const string PARM_BIEUMAUID = @"BieuMauID";
        private const string PARM_TENBIEUMAU = @"TenBieuMau";
        private const string PARM_MAPHIEUIN = @"MaPhieuIn";
        private const string PARM_DUOCSUDUNG = @"DuocSuDung";
        private const string PARM_KEYWORD = @"Keyword";
        private const string PARM_CAPID = @"CapID";
        private const string PARM_START = @"Start";
        private const string PARM_END = @"End";
        #endregion

        private BieuMauInfo GetData(SqlDataReader rdr)
        {
            BieuMauInfo info = new BieuMauInfo();
            info.BieuMauID = Utils.GetInt32(rdr["BieuMauID"], 0);
            info.TenBieuMau = Utils.GetString(rdr["TenBieuMau"], String.Empty);
            info.DuocSuDung = Utils.GetInt32(rdr["DuocSuDung"], 0);
            info.MaPhieuIn = Utils.GetString(rdr["MaPhieuIn"], String.Empty);
            info.CapID = Utils.GetInt32(rdr["CapID"], 0);

            return info;
        }

        public IList<BieuMauInfo> GetAll()
        {
            IList<BieuMauInfo> list = new List<BieuMauInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_ALL, null))
            {
                while (dr.Read())
                {
                    BieuMauInfo info = GetData(dr);
                    list.Add(info);
                }
                dr.Close();
            }
            return list;
        }

        public BieuMauInfo GetByID(int pBieuMauID)
        {
            BieuMauInfo info = null;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_BIEUMAUID, SqlDbType.Int)
            };
            parameters[0].Value = pBieuMauID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_BY_ID, parameters))
            {
                if (dr.Read())
                {
                    info = GetData(dr);
                }
                dr.Close();
            }
            return info;
        }

        public int CountBySearch(int pCapID, string pKeyword)
        {
            int result = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_CAPID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,500)

            };
            parameters[0].Value = pCapID;
            parameters[1].Value = pKeyword;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_SEARCH, parameters))
            {
                if (dr.Read())
                {
                    result = Utils.GetInt32(dr["CountNum"], 0);
                }
                dr.Close();
            }
            return result;
        }

        public int UpdateDuocSuDung(int pBieuMauID, int pDuocSuDung)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_BIEUMAUID, SqlDbType.Int),
                new SqlParameter(PARM_DUOCSUDUNG, SqlDbType.Int)
            };
            parameters[0].Value = pBieuMauID;
            parameters[1].Value = pDuocSuDung;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE_DUOCSUDUNG, parameters);
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

        public IList<BieuMauInfo> GetBySearch(int pStart, int pEnd, int pCapID, string pKeyword)
        {
            IList<BieuMauInfo> list = new List<BieuMauInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_CAPID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,500)
            };
            parameters[0].Value = pStart;
            parameters[1].Value = pEnd;
            parameters[2].Value = pCapID;
            parameters[3].Value = pKeyword;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SEARCH, parameters))
            {
                while (dr.Read())
                {
                    BieuMauInfo info = GetData(dr);
                    list.Add(info);
                }
                dr.Close();
            }
            return list;
        }

    }
}
