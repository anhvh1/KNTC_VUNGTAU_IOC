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
    public class CaNhanBiKN
    {
        //Su dung de goi StoreProcedure
        private const string GET_ALL = @"CaNhanBiKN_GetAll";
        private const string GET_BY_ID = @"CaNhanBiKN_GetByID";
        private const string INSERT = @"CaNhanBiKN_Insert";
        private const string UPDATE = @"CaNhanBiKN_Update";
        private const string DELETE = @"CaNhanBiKN_Delete";

        //TriNM :stored
        private const string GET_BY_DOI_TUONG_BI_KN = @"CaNhanBiKN_GetByDoiTuongBiKNID";

        //ten cac bien dau vao
        //Ten cac bien dau vao
        private const string PARAM_CA_NHAN_BIKN_ID = "@CaNhanBiKNID";
        private const string PARAM_NGHE_NGHIEP = "@NgheNghiep";
        private const string PARAM_NOI_CONG_TAC = "@NoiCongTac";
        private const string PARAM_CHUC_VU_ID = "@ChucVuID";
        private const string PARAM_QUOC_TICH_ID = "@QuocTichID";
        private const string PARAM_DAN_TOC_ID = "@DanTocID";
        private const string PARAM_DOI_TUONG_BIKN_ID = "@DoiTuongBiKNID";

        private CaNhanBiKNInfo GetData(SqlDataReader dr)
        {
            CaNhanBiKNInfo Info = new CaNhanBiKNInfo();
            Info.CaNhanBiKNID = Utils.GetInt32(dr["CaNhanBiKNID"], 0);
            Info.NgheNghiep = Utils.ConvertToString(dr["NgheNghiep"], String.Empty);
            Info.NoiCongTac = Utils.GetString(dr["NoiCongTac"], string.Empty);
            Info.ChucVuID = Utils.GetInt32(dr["ChucVuID"], 0);
            Info.QuocTichID = Utils.GetInt32(dr["QuocTichID"], 0);
            Info.DanTocID = Utils.GetInt32(dr["DanTocID"], 0);
            Info.DoiTuongBiKNID = Utils.GetInt32(dr["DoiTuongBiKNID"], 0);
            Info.TenDanToc = Utils.ConvertToString(dr["TenDanToc"], "");
            Info.TenQuocTich = Utils.ConvertToString(dr["TenQuocTich"], "");
            Info.TenChucVu = Utils.ConvertToString(dr["TenChucVu"], "");
            return Info;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                //new SqlParameter(PARAM_CA_NHAN_BIKN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGHE_NGHIEP,SqlDbType.NVarChar, 200),
                new SqlParameter(PARAM_NOI_CONG_TAC,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_CHUC_VU_ID,SqlDbType.Int),
                new SqlParameter(PARAM_QUOC_TICH_ID,SqlDbType.Int),
                new SqlParameter(PARAM_DAN_TOC_ID,SqlDbType.Int),
                new SqlParameter(PARAM_DOI_TUONG_BIKN_ID,SqlDbType.Int)
                };
            return parms;
        }
        private void SetInsertParms(SqlParameter[] parms, CaNhanBiKNInfo cInfo)
        {

            //parms[0].Value = cInfo.CaNhanBiKNID;
            parms[0].Value = cInfo.NgheNghiep ?? Convert.DBNull;
            parms[1].Value = cInfo.NoiCongTac ?? Convert.DBNull;
            parms[2].Value = cInfo.ChucVuID ?? Convert.DBNull;
            parms[3].Value = cInfo.QuocTichID ?? Convert.DBNull;
            parms[4].Value = cInfo.DanTocID ?? Convert.DBNull;
            parms[5].Value = cInfo.DoiTuongBiKNID;
        }
        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_CA_NHAN_BIKN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGHE_NGHIEP,SqlDbType.NVarChar, 200),
                new SqlParameter(PARAM_NOI_CONG_TAC,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_CHUC_VU_ID,SqlDbType.Int),
                new SqlParameter(PARAM_QUOC_TICH_ID,SqlDbType.Int),
                new SqlParameter(PARAM_DAN_TOC_ID,SqlDbType.Int),
                new SqlParameter(PARAM_DOI_TUONG_BIKN_ID,SqlDbType.Int)
            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, CaNhanBiKNInfo cInfo)
        {

            parms[0].Value = cInfo.CaNhanBiKNID;
            parms[1].Value = cInfo.NgheNghiep ?? Convert.DBNull;
            parms[2].Value = cInfo.NoiCongTac ?? Convert.DBNull;
            parms[3].Value = cInfo.ChucVuID ?? Convert.DBNull;
            parms[4].Value = cInfo.QuocTichID ?? Convert.DBNull;
            parms[5].Value = cInfo.DanTocID ?? Convert.DBNull;
            parms[6].Value = cInfo.DoiTuongBiKNID;
        }

        public int Delete(int cID)
        {

            object val = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CA_NHAN_BIKN_ID,SqlDbType.Int)
            };
            parameters[0].Value = cID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, DELETE, parameters);
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
        public int Update(CaNhanBiKNInfo cInfo)
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
        public int Insert(CaNhanBiKNInfo cInfo)
        {

            int val = 0;

            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, cInfo);

            //SqlParameter[] parms = new SqlParameter[]{
            //    //new SqlParameter(PARAM_CA_NHAN_BIKN_ID, SqlDbType.Int),
            //    new SqlParameter(PARAM_NGHE_NGHIEP,SqlDbType.NVarChar, 200),
            //    new SqlParameter(PARAM_NOI_CONG_TAC,SqlDbType.NVarChar,100),
            //    new SqlParameter(PARAM_CHUC_VU_ID,SqlDbType.Int),
            //    new SqlParameter(PARAM_QUOC_TICH_ID,SqlDbType.Int),
            //    new SqlParameter(PARAM_DAN_TOC_ID,SqlDbType.Int),
            //    new SqlParameter(PARAM_DOI_TUONG_BIKN_ID,SqlDbType.Int)
            //    };
            //parms[0].Value = cInfo.NgheNghiep ?? Convert.DBNull;
            //parms[1].Value = cInfo.NoiCongTac ?? Convert.DBNull;
            //parms[2].Value = cInfo.ChucVuID ?? Convert.DBNull;
            //parms[3].Value = cInfo.QuocTichID ?? Convert.DBNull;
            //parms[4].Value = cInfo.DanTocID ?? Convert.DBNull;
            //parms[5].Value = cInfo.DoiTuongBiKNID;


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

        public CaNhanBiKNInfo GetByID(int donthuID)
        {

            CaNhanBiKNInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CA_NHAN_BIKN_ID,SqlDbType.Int)
            };
            parameters[0].Value = donthuID;
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

        //lay gia tri trong bang CaNhanBiKN theo DoiTuongBiKNID
        //public CaNhanBiKNInfo getCaNhanBiKN(int DoiTuongBiKNID)
        //{
        //    object val = null;
        //    SqlParameter[] parameters = new SqlParameter[]{
        //        new SqlParameter(PARAM_DOI_TUONG_BIKN_ID,SqlDbType.Int)
        //    };
        //    parameters[0].Value = DoiTuongBiKNID;

        //    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
        //    {

        //        conn.Open();
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {

        //            try
        //            {
        //                val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, GET_ALL_BY_DOI_TUONG_BI_KN, parameters);
        //                trans.Commit();
        //            }
        //            catch
        //            {
        //                trans.Rollback();
        //                throw;
        //            }
        //        }
        //        conn.Close();
        //    }
        //    return Utils.ConvertToInt32(val, 0);
        //}
        public CaNhanBiKNInfo getCaNhanBiKN(int DoiTuongBiKNID)
        {

            CaNhanBiKNInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DOI_TUONG_BIKN_ID,SqlDbType.Int)
            };
            parameters[0].Value = DoiTuongBiKNID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_DOI_TUONG_BI_KN, parameters))
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

    }
}
