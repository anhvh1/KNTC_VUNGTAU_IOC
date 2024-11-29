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
    public class PhanXuLy
    {
        private const string INSERT = @"PhanXuLy_Insert";
        private const string UPDATE_DUEDATE = @"PhanXuLy_Update_DueDate";

        private const string PARAM_XULYDONID = "@XuLyDonID";
        private const string PARAM_PHONGBANID = "@PhongBanID";
        private const string PARAM_NGAYPHAN = "@NgayPhan";
        private const string PARAM_GHICHU = @"GhiChu";
        private const string PARAM_CANBOID = "@CanBoID";
        private const string PARAM_DUEDATE = @"DueDate";

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID, SqlDbType.Int),
               new SqlParameter(PARAM_PHONGBANID, SqlDbType.Int),
               new SqlParameter(PARAM_NGAYPHAN, SqlDbType.DateTime),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARAM_GHICHU, SqlDbType.NVarChar,300),
                };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, PhanXuLyInfo info)
        {

            parms[0].Value = info.XuLyDonID;
            parms[1].Value = info.PhongBanID;
            parms[2].Value = info.NgayPhan;
            parms[3].Value = info.CanBoID;
            parms[4].Value = !string.IsNullOrEmpty(info.GhiChu) ? info.GhiChu : string.Empty ;
            if (info.PhongBanID == 0)
            {
                parms[1].Value = DBNull.Value;
            }
            if (info.CanBoID == 0)
            {
                parms[3].Value = DBNull.Value;
            }
        }
        public int Insert(PhanXuLyInfo info)
        {

            int val = 0;

            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, info);

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
        public PhanXuLyInfo GetByID(int PhanXuLyID)
        {

            PhanXuLyInfo Info = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("PhanXuLyID",SqlDbType.Int)
            };
            parameters[0].Value = PhanXuLyID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_PhanXuLy_GetByID", parameters))
                {

                    if (dr.Read())
                    {
                        Info = new PhanXuLyInfo();
                        Info.PhanXuLyID = Utils.ConvertToInt32(dr["PhanXuLyID"], 0);
                        Info.NgayHetHan = Utils.ConvertToNullableDateTime(dr["NgayHetHan"], null);
                        //Info.HanXuLy = Format.FormatDate(Info.NgayQuaHan);
                        Info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        Info.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        Info.GhiChu = Utils.ConvertToString(dr["GhiChu"], String.Empty);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return Info;
        }
    }
}
