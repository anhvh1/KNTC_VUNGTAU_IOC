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
    public class PhanTPPhanGQ
    {
        #region Database query string
        private const string INSERT = @"PhanTPPhanGQ_Insert";
        private const string UPDATE = @"PhanTPPhanGQ_Update";
        private const string DELETE = @"PhanTPPhanGQ_Delete";
        private const string DEL_BY_XLDID_AND_CANBOID = @"PhanTPPhanGQ_DelByXLDID_And_CanBoID";
        private const string GET_BY_XLDID = "PhanTPPhanGQ_GetByXuLyDonID";
        #endregion

        #region constant parameter
        private const string PARAM_PHAN_TP_PHANGQID = @"PhanTPPhanGQID";
        private const string PARAM_XULYDONID = @"XuLyDonID";
        private const string PARAM_PHONGBANID = @"PhongBanID";
        private const string PARAM_CANBOID = @"CanBoID";
        private const string PARAM_NGAYPHANGQ = @"NgayPhanGQ";
        #endregion

        private PhanTPPhanGQModel GetData(SqlDataReader rdr)
        {
            PhanTPPhanGQModel tPPhanGQInfo = new PhanTPPhanGQModel();
            tPPhanGQInfo.PhanTPPhanGQID = Utils.GetInt32(rdr["PhanTPPhanGQID"], 0);
            tPPhanGQInfo.XuLyDonID = Utils.ConvertToInt32(rdr["XuLyDonID"], 0);
            tPPhanGQInfo.PhongBanID = Utils.ConvertToInt32(rdr["PhongBanID"], 0);
            tPPhanGQInfo.CanBoID = Utils.ConvertToInt32(rdr["CanBoID"], 0);
            tPPhanGQInfo.NgayPhanGQ = Utils.ConvertToDateTime(rdr["NgayPhanGQ"], DateTime.MinValue);
            return tPPhanGQInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAYPHANGQ, SqlDbType.DateTime)
            };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, PhanTPPhanGQModel tPPhanGQInfo)
        {
            parms[0].Value = tPPhanGQInfo.XuLyDonID;
            parms[1].Value = tPPhanGQInfo.PhongBanID;
            parms[2].Value = tPPhanGQInfo.CanBoID;
            parms[3].Value = tPPhanGQInfo.NgayPhanGQ;
            if (tPPhanGQInfo.NgayPhanGQ == DateTime.MinValue) parms[3].Value = DBNull.Value;

        }

        public IList<PhanTPPhanGQModel> GetByXuLyDonID(int xldID)
        {

            IList<PhanTPPhanGQModel> resultList = new List<PhanTPPhanGQModel>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID" , SqlDbType.Int)
            };
            parameters[0].Value = xldID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_XLDID, parameters))
                {
                    while (dr.Read())
                    {
                        PhanTPPhanGQModel info = GetData(dr);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        resultList.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return resultList;
        }

        public int Insert(PhanTPPhanGQModel tPPhanGQInfo)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, tPPhanGQInfo);
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
            return Convert.ToInt32(val);
        }

        public int DelByXLDIDAndCanBoID(int xldID, int canBoID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID,SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID,SqlDbType.Int)
            };
            parameters[0].Value = xldID;
            parameters[1].Value = canBoID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DEL_BY_XLDID_AND_CANBOID, parameters);
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
        public IList<PhanTPPhanGQModel> GetByXuLyDonID_Last(int xldID)
        {

            IList<PhanTPPhanGQModel> resultList = new List<PhanTPPhanGQModel>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID" , SqlDbType.Int)
            };
            parameters[0].Value = xldID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanTPPhanGQ_GetByXuLyDonID_Last", parameters))
                {
                    while (dr.Read())
                    {
                        PhanTPPhanGQModel info = GetData(dr);
                        resultList.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return resultList;
        }
    }
}
